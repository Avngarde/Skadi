using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Maui.Controls.PlatformConfiguration;

using Skadi.Helpers;
using Skadi.Models;

using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Skadi.ViewModels;

public partial class TimerPageViewModel : ObservableObject
{
    [ObservableProperty] private int[] _minutes = Enumerable.Range(0, 60).ToArray();
    [ObservableProperty] private int[] _seconds = Enumerable.Range(0, 60).ToArray();
    [ObservableProperty] private int _minute = 0;
    [ObservableProperty] private int _second = 0;
    [ObservableProperty] private string _playPauseSymbol = "▶️";
    [ObservableProperty] private bool _isPaused = true;
    [ObservableProperty] private int _timerProgress = 100;

    private int _originalMinute = 0;
    private int _originalSecond = 0;

    [RelayCommand]
    public async Task FrameTapped()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make($"ProgressBar tapped!", ToastDuration.Long, 12);
        await toast.Show(cancellationTokenSource.Token);
    }

    [RelayCommand]
    public async Task ResetButton()
    {
        IsPaused = true;
        PlayPauseSymbol = "▶️";
        Second = _originalSecond;
        Minute = _originalMinute;
        TimerProgress = 100;
        await ShowDoneMessage();
    }

    private async Task ShowDoneMessage()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make($"Done!", ToastDuration.Long, 14);
        await toast.Show(cancellationTokenSource.Token);
    }

    [RelayCommand]
    public void PlayPauseClicked()
    {
        if (PlayPauseSymbol == "▶️")
        {
            if (Second > 0 || Minute > 0)
            {
                PlayPauseSymbol = "⏸️";
                IsPaused = false;
                _originalSecond = Second;
                _originalMinute = Minute;
                StartCounting();
            }
        }
        else
        {
            IsPaused = true;
            PlayPauseSymbol = "▶️";
        }
    }

    private void SetProgress()
    {
        
        int newProgress = TimeHelper.TimeToProgress(Minute, Second, _originalMinute, _originalSecond);
        TimerProgress = newProgress;
    }

    private async void StartCounting()
    {
        while (Second > 0 || Minute > 0)
        {
            if (IsPaused)
            {
                return;
            }
            else
            {
                await Task.Delay(1000);
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (!IsPaused) // Avoid seconds decreasing by 1 after the pause button was clicked
                    {
                        if (Minute > 0 && Second == 0)
                        {
                            Minute = Minute - 1;
                            Second = 59;
                        }
                        Second = Second - 1;
                        SetProgress();
                            
                    }
                });
            }
        }
        IsPaused = true;
        PlayPauseSymbol = "▶️";
        Second = _originalSecond;
        Minute = _originalMinute;
        TimerProgress = 100;
        await ShowDoneMessage();
    }
}