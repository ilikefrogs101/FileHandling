using System;
using System.Reflection;
using System.IO;

namespace ilikefrogs101.FileHandling;
public static class PathUtils {
    public static string ResolvePathLocation(Path path) {
        return path.origin switch {
            PathOrigin.CONFIG => _configPath(path.location),
            PathOrigin.DATA => _dataPath(path.location),
            _ => path.location,
        };
    }
    public static PathType ResolvePathType(Path path) {
        if (!path.Exists()) return PathType.INVALID;

        FileAttributes attr = File.GetAttributes(ResolvePathLocation(path));
        return attr.HasFlag(FileAttributes.Directory) ? PathType.DIRECTORY : PathType.FILE;
    }
    public static string ResolvePathName(Path path) {
        return ResolvePathType(path) switch
        {
            PathType.FILE => System.IO.Path.GetFileNameWithoutExtension(path.ResolveLocation()),
            PathType.DIRECTORY => System.IO.Path.GetDirectoryName(path.ResolveLocation()),
            _ => string.Empty,
        };
    }
    public static string ResolvePathExtension(Path path) {
        if (path.ResolveType() != PathType.FILE) return string.Empty;

        return System.IO.Path.GetFileName(path.ResolveLocation());
    }
    public static string ResolveDirectory(Path path) {
        if (!path.Exists()) return string.Empty;

        return System.IO.Path.GetDirectoryName(path.ResolveLocation());
    }

    public static Path Combine(params string[] paths) {
        return new Path(System.IO.Path.Combine(paths));
    }
    
    private static string _configPath(string path) {
        string directory = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Assembly.GetExecutingAssembly().GetName().Name
        );

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        return System.IO.Path.Combine(directory, path);
    }
    private static string _dataPath(string path) {
        string directory = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Assembly.GetExecutingAssembly().GetName().Name
        );

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        return System.IO.Path.Combine(directory, path);
    }
}