using Moq;
using Skadi;
using Skadi.Models;
using Skadi.Interfaces;

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
        // Arrange: Create a temporary directory
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDirectory);
        
        var mockFileSystemHelper = new Mock<IFileSystemHelper>();
        mockFileSystemHelper.Setup(f => f.AppDataDirectory).Returns(tempDirectory);
        DbConfig dbConfig = new(mockFileSystemHelper.Object);
        
        Skadi.Services.WorkoutService workoutService = new(dbConfig);
        await workoutService.InitTableIfDoesNotExist();
        Workout[] workouts = await workoutService.GetAllWorkouts();
        int count = workouts.Length;

        Skadi.Models.Workout workout = new()
        {
            Difficulty = Difficulty.Easy,
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