using Moq;
using Skadi;
using Skadi.Models;
using Skadi.Helpers;
using Skadi.Interfaces;
using System.Security.Cryptography.X509Certificates;

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

    [Test]
    public void TimeToProgressTest()
    {
        int progress1 = TimeHelper.TimeToProgress(0, 30, 1, 30);
        int progress2 = TimeHelper.TimeToProgress(2, 30, 2, 40);
        Assert.Pass();
    }

    [Test]
    public async Task AddingNewExerciseTest()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDirectory);

        var mockFileSystemHelper = new Mock<IFileSystemHelper>();
        mockFileSystemHelper.Setup(f => f.AppDataDirectory).Returns(tempDirectory);
        DbConfig dbConfig = new(mockFileSystemHelper.Object);

        Skadi.Services.ExerciseService exerciseService = new(dbConfig);
        await exerciseService.InitTableIfDoesNotExist();
        Exercise[] exercises = await exerciseService.GetAllExercises(1);
        int count = exercises.Length;

        Skadi.Models.Exercise exercise = new()
        {
            ExerciseName = "Test1",
            Repetitions = 3,
            DurationMinutes = 0,
            DurationSeconds = 30,
            ExerciseType = ExerciseType.Strength,
            WorkoutId = 1
        };

        await exerciseService.CreateExercise(exercise);
        exercises = await exerciseService.GetAllExercises(1);
        if (exercises.Length > count)
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }

    [Test]
    public void ColourHelperTest()
    {
        (Color,Color) coloursWorkout = ColoursHelper.GetWorkoutColours(Difficulty.Hard);
        Color exerciseTypeColor = ColoursHelper.GetExerciseTypeColor(ExerciseType.Technique);
        if (exerciseTypeColor.Red != 255 && exerciseTypeColor.Green != 180 && exerciseTypeColor.Blue != 67)
        {
            Assert.Fail();
        }

        else
        {
            Assert.Pass();
        }
    }

    [Test]
    public void TimeToDurationTest()
    {
        int minutes = 5;
        int seconds = 34;

        string durText = TimeHelper.TimeToDurationText(minutes, seconds);

        if (durText == "05:34")
            Assert.Pass();
        else
            Assert.Fail();
    }
}