using Skadi;
using Skadi.Models;

namespace SkadiTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task AddingNewWorkoutTest()
    {
        Skadi.Services.WorkoutService workoutService = new();
        await workoutService.InitTableIfDoesNotExist();
        Workout[] workouts = await workoutService.GetAllWorkouts();
        int count = workouts.Length;

        Skadi.Models.Workout workout = new()
        {
            Difficulty = Difficulty.Easy,
            Exercises = new List<Exercise>(),
            Rounds = 1,
            WorkoutName = "TestWorkout"
        };

        await workoutService.CreateWorkout(workout);
        workouts = await workoutService.GetAllWorkouts();
        if (workouts.Length > count)
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }
}