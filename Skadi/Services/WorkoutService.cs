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

    // This constructor is only used for unit testing using Moq
    public WorkoutService(DbConfig mockDbConfig)
    {
        con = new SQLiteAsyncConnection(mockDbConfig.DatabasePath, mockDbConfig.Flags);
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

    public async Task<Workout> GetWorkout(int id)
    {
        await InitTableIfDoesNotExist();
        return await con.Table<Workout>().Where(wrkout => wrkout.Id == id).FirstAsync();
    }
}