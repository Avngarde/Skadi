using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Skadi.Services;
using Skadi.Models;
using Skadi.Views;

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

    public MainPageViewModel()
    {
        LoadWorkoutsAsync();
        WeakReferenceMessenger.Default.Register<RefreshWorkoutsMessage>(this, (r, m) =>
        {
            LoadWorkoutsAsync();
        });        
    }
    
    private async Task LoadWorkoutsAsync()
    {
        List<WorkoutMainPage> workoutsMainPageList = new();
        Workout[] workoutsDb = await new WorkoutService().GetAllWorkouts();
        foreach (var wrkout in workoutsDb)
        {
            Color colorBackground;
            Color colorText;
            if (wrkout.Difficulty == Difficulty.Hard)
            {
                colorBackground = Color.FromRgba(255, 230, 226, 255);
                colorText = Color.FromRgba(230,166,160,255);
            }
            else if (wrkout.Difficulty == Difficulty.Medium)
            {
                colorBackground = Color.FromRgba(254, 248, 221, 255);
                colorText = Color.FromRgba(227,180,99,255);
            }
            else
            {
                colorBackground = Color.FromRgba(219, 254, 227, 255);
                colorText = Color.FromRgba(80,202,83,255);
            }

            workoutsMainPageList.Add(
                new WorkoutMainPage()
                {
                    Difficulty = wrkout.Difficulty,
                    DifficultyColor = colorBackground,
                    DifficultyTextColor = colorText,
                    Id = wrkout.Id,
                    RoundsText = $"Rounds: {wrkout.Rounds}",
                    WorkoutName = wrkout.WorkoutName
                }
            );
        }

        Workouts = workoutsMainPageList.ToArray();
    }
    
    public class WorkoutMainPage
    {
        public int Id { get; set; }
        public string? WorkoutName { get; set; }
        public string RoundsText { get; set; }
        public Difficulty Difficulty { get; set; }
        public Color DifficultyColor { get; set; }
        public Color DifficultyTextColor { get; set; }
    }
    
    public class RefreshWorkoutsMessage
    {
        // You can add properties if needed to pass data with the message
    }
}