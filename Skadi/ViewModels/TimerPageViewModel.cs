using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Maui.Controls.PlatformConfiguration;

using Skadi.Helpers;
using Skadi.Models;

using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using MauiIcons.Fluent;

namespace Skadi.ViewModels;

public partial class TimerPageViewModel : ObservableObject
{
    [ObservableProperty] private int[] _minutes = Enumerable.Range(0, 60).ToArray();
    [ObservableProperty] private int[] _seconds = Enumerable.Range(0, 60).ToArray();
    [ObservableProperty] private int _minute = 0;
    [ObservableProperty] private int _second = 0;
    [ObservableProperty] private FluentIcons _playPauseSymbol = FluentIcons.Play48;
    [ObservableProperty] private bool _isPaused = true;
    [ObservableProperty] private int _timerProgress = 100;
    [ObservableProperty] private string _timePrint = "00:00";

    private bool _setOriginalTime = true;
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
        PlayPauseSymbol = FluentIcons.Play48;
        _setOriginalTime = true;
        Second = _originalSecond;
        Minute = _originalMinute;
        TimerProgress = 100;
        SetTimerProgressText();
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
        if (PlayPauseSymbol == FluentIcons.Play48)
        {
            if (Second > 0 || Minute > 0)
            {
                PlayPauseSymbol = FluentIcons.Pause48;
                IsPaused = false;
                if (_setOriginalTime)
                {
                    _originalSecond = Second;
                    _originalMinute = Minute;
                }
                _setOriginalTime = false;

                StartCounting();
            }
        }
        else
        {
            IsPaused = true;
            PlayPauseSymbol = FluentIcons.Play48;
        }
    }

    private void SetTimerProgressText()
    {
        string minuteString = Minute.ToString();
        string secondString = Second.ToString();
        if (Minute < 10) minuteString = "0" + minuteString;
        if (Second < 10) secondString = "0" + secondString;


        TimePrint = $"{minuteString}:{secondString}";
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
                        SetTimerProgressText();
                            
                    }
                });
            }
        }
        PlayPauseSymbol = FluentIcons.Play48;
        Second = _originalSecond;
        Minute = _originalMinute;
        TimerProgress = 100;
        _setOriginalTime = true;
        IsPaused = true;
        SetTimerProgressText();
       await ShowDoneMessage();
    }
}