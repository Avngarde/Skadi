using SQLite;

namespace Skadi.Models;

public class Workout
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? WorkoutName { get; set; }
    public int Rounds { get; set; }
    public Difficulty Difficulty { get; set; }
}

public enum Difficulty
{
    Hard,
    Medium,
    Easy
}