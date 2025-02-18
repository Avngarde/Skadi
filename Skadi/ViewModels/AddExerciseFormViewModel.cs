using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Layouts;
using Skadi.Models;
using Skadi.Services;

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
    [ObservableProperty] private int _laps = 0;

    public int WorkoutId { get; set; }
    
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

    [RelayCommand]
    public async Task AddExercise()
    {
        ExerciseService service = new();
        Exercise newExercise = new()
        {
            ExerciseName = ExerciseName,
            Repetitions = Repetitions,
            DurationMinutes = MinutesDuration,
            DurationSeconds = SecondsDuration,
            ExerciseType = GetExerciseTypeFromIndex(),
            Laps = Laps,
            WorkoutId = WorkoutId
        };

        await service.CreateExercise(newExercise);
    }
    
    private ExerciseType GetExerciseTypeFromIndex()
    {
        ExerciseType exerType = new();
        switch (ExerciseTypeIndex)
        {
            case 0:
                exerType = ExerciseType.Cardio;
                break;
            case 1:
                exerType = ExerciseType.Plyometrics;
                break;   
            case 2:
                exerType = ExerciseType.Technique;
                break;
            case 3:
                exerType = ExerciseType.Strength;
                break;      
            case 4:
                exerType = ExerciseType.Stretching;
                break;
            case 5:
                exerType = ExerciseType.Warmup;
                break;
        }

        return exerType;
    }
}