using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Skadi.Models;
using Skadi.Services;

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
    
    [RelayCommand]
    public async Task CreateNewWorkout()
    {
        Difficulty difficulty = GetDifficultyFromIndex();
        WorkoutService workoutService = new();
        Workout workout = new()
        {
            WorkoutName = WorkoutName,
            Rounds = Rounds,
            Difficulty = difficulty
        };
        
       bool isDuplicate = await workoutService.WorkoutAlreadyExist(workout);

       if (!isDuplicate)
       {
           await workoutService.CreateWorkout(workout);
           CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
           var toast = Toast.Make($"{WorkoutName} successfully created!", ToastDuration.Long, 12);
           await toast.Show(cancellationTokenSource.Token);
           WeakReferenceMessenger.Default.Send("RefreshWorkouts");
           await Application.Current.MainPage.Navigation.PopAsync(true);

       }
       else
       {
           CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
           var toast = Toast.Make($"{WorkoutName} already exists!", ToastDuration.Long, 12);
           await toast.Show(cancellationTokenSource.Token);           
       }
    }
}