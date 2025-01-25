using Skadi.Helpers;
using Skadi.Models;
using SQLite;

namespace Skadi.Services;

public class WorkoutService
{
    private SQLiteAsyncConnection con;

    public WorkoutService()
    {
        DbConfig config = new DbConfig(new FileSystemHelper());
        con = new SQLiteAsyncConnection(config.DatabasePath, config.Flags);
    }
    
    public async Task InitTableIfDoesNotExist()
    { 
        await con.CreateTableAsync<Workout>();
    }

    public async Task<int> CreateWorkout(Workout workout)
    {
        await InitTableIfDoesNotExist();
        return await con.InsertAsync(workout);
    }

    public async Task<int> UpdateWorkout(Workout editedWorkout)
    {
        await InitTableIfDoesNotExist();
        return await con.UpdateAsync(editedWorkout);
    }

    public async Task<int> DeleteWorkout(Workout workout)
    {
        await InitTableIfDoesNotExist();
        return await con.DeleteAsync(workout);
    }

    public async Task<bool> WorkoutAlreadyExist(Workout workout)
    {
        await InitTableIfDoesNotExist();
        Workout[] workouts = await con.Table<Workout>().ToArrayAsync();
        int duplicateSize = workouts.Where(wrk => wrk.WorkoutName == workout.WorkoutName).Count();
        return duplicateSize > 0 ? true : false;
    }
    
    public async Task<Workout[]> GetAllWorkouts()
    {
        await InitTableIfDoesNotExist();
        return await con.Table<Workout>().ToArrayAsync();
    }
}