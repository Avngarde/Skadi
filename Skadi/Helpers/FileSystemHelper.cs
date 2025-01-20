using Skadi.Interfaces;

namespace Skadi.Helpers;

public class FileSystemHelper : IFileSystemHelper
{
    public string AppDataDirectory => FileSystem.AppDataDirectory;
}