using Skadi.Helpers;
using Skadi.Models;
using SQLite;

namespace Skadi.Services
{
    public class ExerciseService
    {
        private SQLiteAsyncConnection con;

        public ExerciseService()
        {
            DbConfig config = new DbConfig(new FileSystemHelper());
            con = new SQLiteAsyncConnection(config.DatabasePath, config.Flags);
        }

        // Moq testing constructor
        public ExerciseService(DbConfig mockDbConfig)
        {
            con = new SQLiteAsyncConnection(mockDbConfig.DatabasePath, mockDbConfig.Flags);
        }

        public async Task InitTableIfDoesNotExist()
        {
            await con.CreateTableAsync<Exercise>();
        }

        public async Task<int> CreateExercise(Exercise exercise)
        {
            await InitTableIfDoesNotExist();
            return await con.InsertAsync(exercise);
        }

        public async Task<int> UpdateExercise(Exercise editedExercise)
        {
            await InitTableIfDoesNotExist();
            return await con.UpdateAsync(editedExercise);
        }

        public async Task<int> DeleteExercise(Exercise exercise)
        {
            await InitTableIfDoesNotExist();
            return await con.DeleteAsync(exercise);
        }

        public async Task<bool> ExerciseAlreadyExist(Exercise exercise)
        {
            await InitTableIfDoesNotExist();
            Exercise[] exercises = await con.Table<Exercise>().Where(ex => ex.WorkoutId == exercise.WorkoutId).ToArrayAsync();
            int duplicateSize = exercises.Where(exc => exc.ExerciseName == exercise.ExerciseName).Count();
            return duplicateSize > 0 ? true : false;
        }

        public async Task<Exercise[]> GetAllExercises(int workoutId)
        {
            await InitTableIfDoesNotExist();
            return await con.Table<Exercise>().Where(ex => ex.WorkoutId == workoutId).ToArrayAsync();
        }
    }
}
