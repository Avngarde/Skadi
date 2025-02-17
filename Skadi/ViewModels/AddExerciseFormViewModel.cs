using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Layouts;
using Skadi.Models;

namespace Skadi.ViewModels;

public partial class AddExerciseFormViewModel : ObservableObject
{
    [ObservableProperty] private string _exerciseName = "";
    [ObservableProperty] private bool _durationVisible = false;
    [ObservableProperty] private bool _repetitionsVisible = true;
    [ObservableProperty] private int _repetitions = 0;
    [ObservableProperty] private int _minutesDuration = 0;
    [ObservableProperty] private int _secondsDuration = 0;
    [ObservableProperty] private int _exerciseTypeIndex = 0;
    
    public List<string> ExerciseTypes
    {
        get { return Enum.GetNames(typeof(ExerciseType)).ToList(); }
    }

    [RelayCommand]
    public void DurationOrRepetitionsToggle()
    {
        if (RepetitionsVisible)
        {
            DurationVisible = true;
            RepetitionsVisible = false;
        }
        else
        {
            DurationVisible = false;
            RepetitionsVisible = true;
        }
    }
}