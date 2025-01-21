using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Skadi.Models;

namespace Skadi.ViewModels;

public partial class AddWorkoutFormViewModel : ObservableObject
{
    [ObservableProperty] private string _workoutName;

    [ObservableProperty] private int _rounds;

    [ObservableProperty] private int _difficultyIndex;
    
    public List<string> Difficulties
    {
        get { return Enum.GetNames(typeof(Difficulty)).ToList(); }
    }
    
    [RelayCommand]
    public async Task CreateNewWorkout()
    {
        await Application.Current.MainPage.DisplayAlert("_workoutName value", WorkoutName, "Ok");
    }
}