using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Skadi.Models;
using Skadi.Views;
using Skadi.Services;

namespace Skadi.ViewModels
{
    public partial class WorkoutPageViewModel : ObservableObject
    {
        public Workout Workout { get; set; }

        [ObservableProperty] public string _workoutName;
        [ObservableProperty] public Exercise[] exercises;

        public async Task LoadExercises()
        {
            ExerciseService exerciseService = new();
            Exercise[] exercisesList = await exerciseService.GetAllExercises(Workout.Id);
            Exercises = exercisesList;
        }

        [RelayCommand]
        public async Task OpenAddExerciseForm()
        {
            AddExerciseForm addExerciseForm = new(Workout.Id);
            await Application.Current.MainPage.Navigation.PushAsync(addExerciseForm);
        }
    }
}
