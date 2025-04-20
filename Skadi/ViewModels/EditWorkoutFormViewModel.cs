using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Skadi.Models;
using Skadi.Services;

namespace Skadi.ViewModels;

public partial class EditWorkoutFormViewModel : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _workoutName;
    [ObservableProperty] private int _difficultyIndex;
    
    public List<string> Difficulties
    {
        get { return Enum.GetNames(typeof(Difficulty)).ToList(); }
    }

    private Difficulty GetDifficultyFromIndex()
    {
        Difficulty difficulty = new();
        switch (DifficultyIndex)
        {
            case 0:
                difficulty = Difficulty.Hard;
                break;
            case 1:
                difficulty = Difficulty.Medium;
                break;
            case 2:
                difficulty = Difficulty.Easy;
                break;
        }

        return difficulty;
    }

    private int GetIndexFromDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Hard:
                return 0;
            case Difficulty.Medium:
                return 1;
            case Difficulty.Easy:
                return 2;
            default:
                return 0;
        }
    }
    
    [RelayCommand]
    public async Task EditWorkout()
    {
        Difficulty difficulty = GetDifficultyFromIndex();
        WorkoutService workoutService = new();
        Workout workout = new()
        {
            Id = Id,
            WorkoutName = WorkoutName,
            Difficulty = difficulty
        };
        
        await workoutService.UpdateWorkout(workout);
        WeakReferenceMessenger.Default.Send("RefreshWorkouts");
        await Application.Current.MainPage.Navigation.PopAsync(true);
    }

    public void LoadData(Workout workout) 
    {
        Id = workout.Id;
        DifficultyIndex = GetIndexFromDifficulty(workout.Difficulty);
        WorkoutName = workout.WorkoutName;
    }
}
