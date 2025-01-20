using Skadi.Interfaces;

namespace Skadi;

public class DbConfig
{
    private readonly IFileSystemHelper _fileSystemHelper;

    public DbConfig(IFileSystemHelper fileSystemHelper)
    {
        _fileSystemHelper = fileSystemHelper;
    }
    
    public string DbName => "Skadi.db3";
    
    public string DatabasePath => Path.Combine(_fileSystemHelper.AppDataDirectory, DbName);

    public readonly SQLite.SQLiteOpenFlags Flags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;
}