using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Skadi.ViewModels;

public partial class TimerPageViewModel : ObservableObject
{
    [ObservableProperty] private int[] _minutes = Enumerable.Range(0, 59).ToArray();
    [ObservableProperty] private int[] _seconds = Enumerable.Range(1, 59).ToArray();
    
    [RelayCommand]
    public async Task FrameTapped()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make($"ProgressBar tapped!", ToastDuration.Long, 12);
        await toast.Show(cancellationTokenSource.Token);
    }
}