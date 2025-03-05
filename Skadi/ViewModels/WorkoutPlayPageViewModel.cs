using CommunityToolkit.Mvvm.ComponentModel;

using Skadi.Models;
using Skadi.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skadi.ViewModels
{
    partial class WorkoutPlayPageViewModel : ObservableObject
    {
        public int WorkoutId { get; set; }
        private int CurrentLap { get; set; }
        private Exercise[]? Exercises { get; set; }
        private Exercise? Exercise { get; set; }
        private int ExerciseIdx { get; set; }
        private int DurationMinutes { get; set; }
        private int DurationSeconds { get; set; }
        private int Repetitions { get; set; }

        //Observables
        [ObservableProperty] public string _repetitionsText = "";
        [ObservableProperty] public string _durationText = "";
        [ObservableProperty] public string _lapsText = "";
        [ObservableProperty] private string _currentExerciseName = "";
        [ObservableProperty] public bool _showNextExercise = false;
        [ObservableProperty] private bool _showDuration = true;
        [ObservableProperty] private bool _showRepetition = false;

        public async Task LoadExercises()
        {
            ExerciseService exerciseService = new();
            Exercises = await exerciseService.GetAllExercises(WorkoutId);
            ExerciseIdx = 0;
            InitExercise();
        }

        public void LoadExerciseProperties()
        {
            CurrentExerciseName = Exercise.ExerciseName;
            LapsText = $"Laps: {CurrentLap}/{Exercise.Laps}";
            ShowDuration = Exercise.DurationMinutes > 0 && Exercise.DurationSeconds > 0 ? true : false;
            ShowRepetition = Exercise.DurationMinutes > 0 && Exercise.DurationSeconds > 0 ? true : false;

            if (ShowRepetition)
                RepetitionsText = $"Do {Exercise.Repetitions} repetitions";
            if (ShowDuration)
                DurationText = $"{Exercise.DurationMinutes}:{Exercise.DurationSeconds}";

            if (CurrentLap == Exercise.Laps) 
                ShowNextExercise = true;
        }

        public void InitExercise()
        {
            if (Exercises != null)
            {
                Exercise = Exercises[ExerciseIdx];
                CurrentLap = 1;
                if (ShowDuration)
                {
                    DurationMinutes = Exercise.DurationMinutes;
                    DurationSeconds = Exercise.DurationSeconds;
                }
                else
                    Repetitions = Exercise.Repetitions;

                LoadExerciseProperties();
            }
        }
    }
}
