using System;
using System.Reflection;
using System.IO;

namespace ilikefrogs101.FileHandling;
public static class PathHandler {
    public static string ResolvePathLocation(Path path) {
        return path.origin switch {
            PathOrigin.CONFIG => _configPath(path.location),
            PathOrigin.DATA => _dataPath(path.location),
            _ => path.location,
        };
    }
    public static PathType ResolvePathType(Path path) {
        if (!FileHandler.Exists(path)) return PathType.INVALID;

        FileAttributes attr = File.GetAttributes(ResolvePathLocation(path));
        return attr.HasFlag(FileAttributes.Directory) ? PathType.DIRECTORY : PathType.FILE;
    }
    public static string Combine(params string[] paths) {
        return System.IO.Path.Combine(paths);
    }
    
    private static string _configPath(string path) {
        string directory = Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Assembly.GetExecutingAssembly().GetName().Name
        );

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        return Combine(directory, path);
    }
    private static string _dataPath(string path) {
        string directory = Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Assembly.GetExecutingAssembly().GetName().Name
        );

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        return Combine(directory, path);
    }
}