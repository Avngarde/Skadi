using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Skadi.Views;

namespace Skadi.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [RelayCommand]
    public async Task OpenAddWorkoutForm()
    {
        AddWorkoutForm addWorkoutForm = new();
        await Application.Current.MainPage.Navigation.PushAsync(addWorkoutForm);
    }
}