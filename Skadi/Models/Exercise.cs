namespace Skadi.Models;

public class Exercise
{
    public int Id { get; set; }
    public string? ExerciseName { get; set; }
    public int Repetitions { get; set; }
    public Workout Workout { get; set; }
    public ExerciseType ExerciseType { get; set; }
}

public enum ExerciseType
{
    Cardio,
    Plyometrics,
    Technical,
    Strength,
    Stretching
}