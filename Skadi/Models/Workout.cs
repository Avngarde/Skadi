namespace Skadi.Models;

public class Workout
{
    public int Id { get; set; }
    public string? WorkoutName { get; set; }
    public int Rounds { get; set; }
    public Difficulty Difficulty { get; set; }
    public List<Exercise> Exercises { get; set; }
}

public enum Difficulty
{
    Hard,
    Medium,
    Easy
}