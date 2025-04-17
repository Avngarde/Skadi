using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Core;
using Skadi.Services;
using Skadi.Models;
using Skadi.Views;
using Skadi.Helpers;

namespace Skadi.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty] private WorkoutMainPage[] _workouts;
    
    [RelayCommand]
    public async Task OpenAddWorkoutForm()
    {
        AddWorkoutForm addWorkoutForm = new();
        await Application.Current.MainPage.Navigation.PushAsync(addWorkoutForm);
        await LoadWorkoutsAsync();
    }

    [RelayCommand]
    public async Task OpenTimer()
    {
        TimerPage timerPage = new();
        await Application.Current.MainPage.Navigation.PushAsync(timerPage);
    }

    [RelayCommand]
    public async Task OpenWorkout(WorkoutMainPage workout)
    {
        WorkoutPage workoutPage = new WorkoutPage();
        if (workoutPage.BindingContext is WorkoutPageViewModel viewModel)
        {
            viewModel.WorkoutName = workout.WorkoutName;
            viewModel.Workout = new Workout()
            {
                Id = workout.Id,
                WorkoutName = workout.WorkoutName,
                Difficulty = workout.Difficulty,
            };
            await viewModel.LoadExercises();
        }
        await Application.Current.MainPage.Navigation.PushAsync(workoutPage);
    }

    [RelayCommand]
    public async Task WorkoutTap(WorkoutMainPage tappedWorkout)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make($"You tapped workout, name: {tappedWorkout.WorkoutName}", ToastDuration.Long, 12);
        await toast.Show(cancellationTokenSource.Token);
    }

    public MainPageViewModel()
    {
        _ = LoadWorkoutsAsync();
        WeakReferenceMessenger.Default.Register<string>(this, (r, m) =>
        {
            if (m == "RefreshWorkouts")
                _ = LoadWorkoutsAsync();
        });
    }
    
    private async Task LoadWorkoutsAsync()
    {
        List<WorkoutMainPage> workoutsMainPageList = new();
        Workout[] workoutsDb = await new WorkoutService().GetAllWorkouts();
        foreach (var wrkout in workoutsDb)
        {
            Exercise[] exercises = await new ExerciseService().GetAllExercises(wrkout.Id);
            (Color, Color) colors = ColoursHelper.GetWorkoutColours(wrkout.Difficulty);
            workoutsMainPageList.Add(
                new WorkoutMainPage()
                {
                    Difficulty = wrkout.Difficulty,
                    DifficultyColor = colors.Item1,
                    DifficultyTextColor = colors.Item2,
                    Id = wrkout.Id,
                    WorkoutName = wrkout.WorkoutName,
                    ExerciseCount = $"Exercises: {exercises.Count()}"
                }
            );
        }

        Workouts = workoutsMainPageList.ToArray();
    }
    
    public class WorkoutMainPage
    {
        public int Id { get; set; }
        public string? WorkoutName { get; set; }
        public Difficulty Difficulty { get; set; }
        public Color DifficultyColor { get; set; }
        public Color DifficultyTextColor { get; set; }
        public string? ExerciseCount { get; set; }
    }
}