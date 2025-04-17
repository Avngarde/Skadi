using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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

        int success = await service.CreateExercise(newExercise);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        if (success > 0)
        {
            WeakReferenceMessenger.Default.Send("RefreshExercises");

            var toast = Toast.Make($"Exercise {ExerciseName} added succesfully", ToastDuration.Long, 12);
            await toast.Show(cancellationTokenSource.Token);

            await Application.Current.MainPage.Navigation.PopAsync(true);
        }
        else
        {
            var toast = Toast.Make($"Failed to add exercise", ToastDuration.Long, 12);
            await toast.Show(cancellationTokenSource.Token);
        }
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