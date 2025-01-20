using Skadi.Models;
using SQLite;

namespace Skadi.Services;

public class WorkoutService
{
    private SQLiteAsyncConnection con;

    public WorkoutService(DbConfig config)
    {
        con = new SQLiteAsyncConnection(config.DatabasePath, config.Flags);
    }
    
    public async Task InitTableIfDoesNotExist()
    { 
        await con.CreateTableAsync<Workout>();
    }

    public async Task<int> CreateWorkout(Workout workout)
    {
        return await con.InsertAsync(workout);
    }

    public async Task<int> UpdateWorkout(Workout editedWorkout)
    {
        return await con.UpdateAsync(editedWorkout);
    }

    public async Task<int> DeleteWorkout(Workout workout)
    {
        return await con.DeleteAsync(workout);
    }
    
    public async Task<Workout[]> GetAllWorkouts()
    {
        return await con.Table<Workout>().ToArrayAsync();
    }
}