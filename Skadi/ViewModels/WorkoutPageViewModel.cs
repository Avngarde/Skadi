using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Skadi.Models;
using Skadi.Views;
using Skadi.Services;
using Skadi.Helpers;
using CommunityToolkit.Mvvm.Messaging;

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
                Color textColor = ColoursHelper.GetExerciseTypeColor(exercise.ExerciseType);
                dtoExercises.Add(
                    new ExerciseLayoutDto()
                    {
                        Id = exercise.Id,
                        ExerciseName = exercise.ExerciseName,
                        DurationRepetitionText = CreateDurationRepetitionText(exercise),
                        ExerciseType = exercise.ExerciseType,
                        Laps = $"Laps: {exercise.Laps}",
                        TextColor = textColor
                    }
                );
            }

            Exercises = dtoExercises.ToArray();
        }

        public WorkoutPageViewModel()
        {
            WeakReferenceMessenger.Default.Register<string>(this, async (r, m) =>
            {
                if (m == "RefreshExercises")
                    await LoadExercises();
            });
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

        [RelayCommand]
        public async Task OpenWorkoutPlayPage()
        {
            WorkoutPlayPage workoutPlayPage = new();
            await Application.Current.MainPage.Navigation.PushAsync(workoutPlayPage);
            if (workoutPlayPage.BindingContext is WorkoutPlayPageViewModel viewModel)
            {
                viewModel.WorkoutId = Workout.Id;
                await viewModel.LoadExercises();
            }
        }

        [RelayCommand]
        public async Task EditExercise(ExerciseLayoutDto exerciseElem)
        {
            ExerciseService exerciseService = new();
            Exercise exercise = await exerciseService.GetExercise(exerciseElem.Id);
            EditExerciseForm editExerciseForm = new();
            if (editExerciseForm.BindingContext is EditExerciseFormViewModel viewModel)
            {
                viewModel.LoadExercise(exercise);
            }
            await Application.Current.MainPage.Navigation.PushAsync(editExerciseForm);
        }

        [RelayCommand]
        public async Task DeleteExercise(ExerciseLayoutDto exerciseElem)
        {
            bool delete = await Application.Current.MainPage.DisplayAlert("Delete", $"Are you sure you want to delete {exerciseElem.ExerciseName}", "Yes", "No");
            if (delete)
            {
                ExerciseService exerciseService = new();
                Exercise exercise = await exerciseService.GetExercise(exerciseElem.Id);
                int deletedRows = await exerciseService.DeleteExercise(exercise);
                if (deletedRows > 0)
                {
                    await LoadExercises();
                    WeakReferenceMessenger.Default.Send("RefreshWorkouts"); // Refresh exercise count at workouts
                }
            }
        }
    }
 

    public class ExerciseLayoutDto
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public string DurationRepetitionText { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public string Laps { get; set; }
        public Color TextColor { get; set; }
    }
}
