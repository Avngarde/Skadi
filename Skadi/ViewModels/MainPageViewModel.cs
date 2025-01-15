using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Skadi.ViewModels;

public class MainPageViewModel : INotifyPropertyChanged
{
    public ICommand OpenAddWorkoutFormCommand { get; }

    public MainPageViewModel()
    {
        OpenAddWorkoutFormCommand = new Command(async () => await ExecuteOpenAddWorkoutFormCommand());
    }

    private async Task ExecuteOpenAddWorkoutFormCommand()
    {
        await Application.Current.MainPage.DisplayAlert("Alert", "You just got alerted!", "Ugh fine");
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }    
}