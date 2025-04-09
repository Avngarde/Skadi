using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiIcons.Fluent;
using Skadi.Models;
using Skadi.Services;
using Skadi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

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

        [ObservableProperty] public string _repetitionsText = "";
        [ObservableProperty] public string _durationText = "";
        [ObservableProperty] public string _lapsText = "";
        [ObservableProperty] private string _currentExerciseName = "";
        [ObservableProperty] public bool _showNextExercise = false;
        [ObservableProperty] private bool _showDuration = true;
        [ObservableProperty] private bool _showRepetition = false;
        [ObservableProperty] private bool _durationPaused = false;
        [ObservableProperty] private int _durationProgress = 100;
        [ObservableProperty] private FluentIcons _playPauseIcon = FluentIcons.Pause16;

        [RelayCommand]
        public async Task RepetitionsOrDurationDone()
        {
            if (CurrentLap < Exercise.Laps)
            {
                CurrentLap += 1;
                LapsText = $"Laps: {CurrentLap}/{Exercise.Laps}";

                if (ShowDuration)
                {
                    DurationMinutes = Exercise.DurationMinutes;
                    DurationSeconds = Exercise.DurationSeconds;
                    DurationText = TimeHelper.TimeToDurationText(DurationMinutes, DurationSeconds);
                    DurationProgress = 0;
                    StartCounting();
                }
            }
            else 
            {
                if (ShowDuration)
                    await Task.Delay(1000); // Make sure it counts to 0 before moving to next exercise
                await LoadNextExercise();
            }
        }

        [RelayCommand]
        public void SetPauseOrPlay()
        {
            if (DurationPaused)
            {
                PlayPauseIcon = FluentIcons.Pause16;
                DurationPaused = false;
                StartCounting();
            }
            else
            {
                PlayPauseIcon = FluentIcons.Play16;
                DurationPaused = true;
            }
        }

        private async void StartCounting()
        {
            while (DurationMinutes > 0 || DurationSeconds > 0)
            {
                await Task.Delay(1000);
                if (DurationPaused)
                    return;

                if (DurationMinutes > 0 && DurationSeconds == 0)
                {
                    DurationMinutes--;
                    DurationSeconds = 59;
                }
                else
                {
                    DurationSeconds--;
                }

                DurationProgress = TimeHelper.TimeToProgress(DurationMinutes, DurationSeconds, Exercise.DurationMinutes, Exercise.DurationSeconds);
                DurationText = TimeHelper.TimeToDurationText(DurationMinutes, DurationSeconds);
            }

            await RepetitionsOrDurationDone();
        }

        public async Task LoadExercises()
        {
            ExerciseService exerciseService = new();
            Exercises = await exerciseService.GetAllExercises(WorkoutId);
            ExerciseIdx = 0;
            await LoadExercise();
        }

        public async Task LoadExerciseProperties()
        {
            CurrentExerciseName = Exercise.ExerciseName;
            LapsText = $"Laps: {CurrentLap}/{Exercise.Laps}";
            ShowDuration = Exercise.DurationMinutes > 0 || Exercise.DurationSeconds > 0 ? true : false;
            ShowRepetition = Exercise.Repetitions > 0 ? true : false;

            if (ShowRepetition)
                RepetitionsText = $"{Exercise.Repetitions} Repetitions";
            if (ShowDuration)
            {
                DurationText = TimeHelper.TimeToDurationText(Exercise.DurationMinutes, Exercise.DurationSeconds);
                DurationProgress = 100;
            }

            if (CurrentLap == Exercise.Laps) 
                ShowNextExercise = true;

            if (ShowDuration)
            {
                await Task.Delay(1000);
                StartCounting();
            }
        }

        public async Task LoadExercise()
        {
            if (ExerciseIdx == Exercises.Length)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                var toast = Toast.Make($"Workout completed!", ToastDuration.Long, 14);
                await toast.Show(cancellationTokenSource.Token);
                await Application.Current.MainPage.Navigation.PopAsync(true);
            }
            else if (Exercises != null)
            {
                Exercise = Exercises[ExerciseIdx];
                CurrentLap = 1;
                if (ShowDuration)
                {
                    DurationMinutes = Exercise.DurationMinutes;
                    DurationSeconds = Exercise.DurationSeconds + 1; // This ensures that timer will count exactly from set second
                }
                else
                    Repetitions = Exercise.Repetitions;

                await LoadExerciseProperties();
            }
        }

        public async Task LoadNextExercise()
        {
            ExerciseIdx++;
            await LoadExercise();
        }
    }
}
