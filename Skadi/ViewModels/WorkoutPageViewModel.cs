﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        [ObservableProperty] public ExerciseLayoutDto[] _exercises;

        public async Task LoadExercises()
        {
            ExerciseService exerciseService = new();
            Exercise[] exercisesList = await exerciseService.GetAllExercises(Workout.Id);
            List<ExerciseLayoutDto> dtoExercises = new List<ExerciseLayoutDto>();
            foreach(Exercise exercise in exercisesList)
            {
                dtoExercises.Add(
                    new ExerciseLayoutDto()
                    {
                        ExerciseName = exercise.ExerciseName,
                        DurationRepetitionText = CreateDurationRepetitionText(exercise),
                        ExerciseType = exercise.ExerciseType,
                        Laps = $"Laps: {exercise.Laps}"
                    }
                );
            }

            Exercises = dtoExercises.ToArray();
        }

        private string CreateDurationRepetitionText(Exercise exercise)
        {
            if(exercise.Repetitions > 0)
            {
                return $"Repetitions: {exercise.Repetitions}";
            }
            else
            {
                string durationMinutesText = exercise.DurationMinutes < 10 ? $"0{exercise.DurationMinutes}" : exercise.DurationMinutes.ToString();
                string durationSecondsText = exercise.DurationSeconds < 10 ? $"0{exercise.DurationSeconds}" : exercise.DurationSeconds.ToString();

                return $"Duration {durationMinutesText}:{durationSecondsText}";
            }
        }

        [RelayCommand]
        public async Task OpenAddExerciseForm()
        {
            AddExerciseForm addExerciseForm = new(Workout.Id);
            await Application.Current.MainPage.Navigation.PushAsync(addExerciseForm);
        }
    }


    public class ExerciseLayoutDto
    {
        public string ExerciseName { get; set; }
        public string DurationRepetitionText { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public string Laps { get; set; }
    }
}
