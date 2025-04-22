using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Skadi.Models;
using Skadi.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skadi.ViewModels
{
    public partial class EditExerciseFormViewModel : ObservableObject
    {
        [ObservableProperty] private string _exerciseName = "";
        [ObservableProperty] private bool _durationVisible = false;
        [ObservableProperty] private bool _repetitionsVisible = true;
        [ObservableProperty] private int _repetitions = 0;
        [ObservableProperty] private int _minutesDuration = 0;
        [ObservableProperty] private int _secondsDuration = 0;
        [ObservableProperty] private int _exerciseTypeIndex = 0;
        [ObservableProperty] private int _laps = 0;
        [ObservableProperty] private int _id = 0;

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
        public async Task EditExercise()
        {
            ExerciseService exerciseService = new();
            Exercise exercise = new()
            {
                Id = Id,
                ExerciseName = ExerciseName,
                ExerciseType = GetExerciseTypeFromIndex(),
                Repetitions = Repetitions,
                Laps = Laps,
                DurationMinutes = MinutesDuration,
                DurationSeconds = SecondsDuration,
            };
            int changedRows = await exerciseService.UpdateExercise(exercise);
            if (changedRows > 0)
            {
                WeakReferenceMessenger.Default.Send("RefreshExercises");
                await Application.Current.MainPage.Navigation.PopAsync(true);
            }
        }

        public void LoadExercise(Exercise exercise)
        {
            Id = exercise.Id;
            ExerciseName = exercise.ExerciseName;
            Laps = exercise.Laps;
            ExerciseTypeIndex = GetExerciseTypeIndex(exercise.ExerciseType);

            if (exercise.DurationMinutes > 0 || exercise.DurationSeconds > 0)
            {
                DurationVisible = true;
                MinutesDuration = exercise.DurationMinutes;
                SecondsDuration = exercise.DurationSeconds;
            }
            else
            {
                RepetitionsVisible = true;
                Repetitions = exercise.Repetitions;
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
        private int GetExerciseTypeIndex(ExerciseType exerciseType)
        {
            switch (exerciseType)
            {
                case ExerciseType.Cardio:
                    return 0;
                case ExerciseType.Plyometrics:
                    return 1;
                case ExerciseType.Technique:
                    return 2;
                case ExerciseType.Strength:
                    return 3;
                case ExerciseType.Stretching:
                    return 4;
                case ExerciseType.Warmup:
                    return 5;
                default:
                    return 0;
            }
        }
    }
}
