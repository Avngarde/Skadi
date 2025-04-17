using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiIcons.Fluent;
using Skadi.Models;
using Skadi.Services;
using Skadi.Helpers;
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

        // Setting it to 101 triggers CircularProgressBar.AnimateProgress once its set to 100, making the timer progress bar to appear at the same time as the rest of UI
        [ObservableProperty] private int _durationProgress = 101; 
        [ObservableProperty] private FluentIcons _playPauseIcon = FluentIcons.Pause16;
        [ObservableProperty] private Color _exerciseColor = Colors.White;

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
                    DurationProgress = 100;
                    await StartCounting();
                }
            }
            else 
            {
                if (ShowDuration)
                    await Task.Delay(1000);
                await LoadNextExercise();
            }
        }

        [RelayCommand]
        public async Task SetPauseOrPlay()
        {
            if (DurationPaused)
            {
                PlayPauseIcon = FluentIcons.Pause16;
                DurationPaused = false;
                //await StartCounting();
            }
            else
            {
                PlayPauseIcon = FluentIcons.Play16;
                DurationPaused = true;
            }
        }

        [RelayCommand]
        public async void SkipExercise()
        {
            if (ShowDuration)
            {
                CurrentLap = Exercise.Laps;
                ResetTimer();
            }
            else await LoadNextExercise();
        }

        [RelayCommand]
        public async void SkipLap()
        {
            if (ShowDuration)
                ResetTimer();
            else
                await RepetitionsOrDurationDone();
            
        }

        private void ResetTimer()
        {
            if (ShowDuration)
            {
                DurationSeconds = -1;
                DurationMinutes = 0;
            }
        }

        private async Task StartCounting()
        {
            while (DurationMinutes > 0 || DurationSeconds >= 0)
            {
                if (DurationPaused)
                {
                    await Task.Delay(1000);
                    continue;
                }

                DurationProgress = TimeHelper.TimeToProgress(DurationMinutes, DurationSeconds, Exercise.DurationMinutes, Exercise.DurationSeconds);
                DurationText = TimeHelper.TimeToDurationText(DurationMinutes, DurationSeconds);

                if (DurationMinutes > 0 && DurationSeconds == 0)
                {
                    DurationMinutes--;
                    DurationSeconds = 59;
                }
                else if (DurationSeconds >= 0)
                {
                    DurationSeconds--;
                }
                await Task.Delay(1000);
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
            ExerciseColor = ColoursHelper.GetExerciseTypeColor(Exercise.ExerciseType);

            ShowDuration = Exercise.DurationMinutes > 0 || Exercise.DurationSeconds > 0 ? true : false;
            ShowRepetition = Exercise.Repetitions > 0 ? true : false;

            CurrentLap = 1;
            LapsText = $"Laps: {CurrentLap}/{Exercise.Laps}";

            if (ShowDuration)
            {
                DurationMinutes = Exercise.DurationMinutes;
                DurationSeconds = Exercise.DurationSeconds;
                DurationText = TimeHelper.TimeToDurationText(Exercise.DurationMinutes, Exercise.DurationSeconds);
                DurationProgress = 100;
                await Task.Delay(100);
                await StartCounting();
            }
            else
            {
                Repetitions = Exercise.Repetitions;
                RepetitionsText = $"{Exercise.Repetitions} Repetitions";
            }
        }

        public async Task LoadExercise()
        {
            if (ExerciseIdx >= Exercises.Length)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                var toast = Toast.Make($"Workout completed!", ToastDuration.Long, 14);
                await toast.Show(cancellationTokenSource.Token);
                await Application.Current.MainPage.Navigation.PopAsync(true);
            }
            else if (Exercises != null)
            {
                Exercise = Exercises[ExerciseIdx];
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
