using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Skadi.Models;
using Skadi.Views;

namespace Skadi.ViewModels
{
    public partial class WorkoutPageViewModel : ObservableObject
    {
        public Workout Workout { get; set; }

        [ObservableProperty] public string _workoutName;

        public WorkoutPageViewModel()
        {
           
        }

        [RelayCommand]
        public async Task OpenAddExerciseForm()
        {
            AddExerciseForm addExerciseForm = new();
            await Application.Current.MainPage.Navigation.PushAsync(addExerciseForm);
        }
    }
}
