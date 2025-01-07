namespace Skadi.Models;

public class Workout
{
    public int Id { get; set; }
    public string? WorkoutName { get; set; }
    public Difficulty Difficulty { get; set; }
}

public enum Difficulty
{
    Hard,
    Medium,
    Easy
}