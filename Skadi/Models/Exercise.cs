using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace Skadi.Models;

public class Exercise
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? ExerciseName { get; set; }
    public int Repetitions { get; set; }
    public int Duration { get; set; } // To be used in place of repetitions if needed
    public int Laps { get; set; }
    public int WorkoutId { get; set; }
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