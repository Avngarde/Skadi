using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Skadi.ViewModels;

public partial class TimerPageViewModel : ObservableObject
{
    [ObservableProperty] private int[] _minutes = Enumerable.Range(0, 60).ToArray();
    [ObservableProperty] private int[] _seconds = Enumerable.Range(0, 60).ToArray();
    [ObservableProperty] private int _minute = 0;
    [ObservableProperty] private int _second = 0;
    
    [RelayCommand]
    public async Task FrameTapped()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make($"ProgressBar tapped!", ToastDuration.Long, 12);
        await toast.Show(cancellationTokenSource.Token);
    }
}