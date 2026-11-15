using System;
using System.IO;
using System.Reflection;

namespace ilikefrogs101.FileHandling;
public sealed record Path(string location, PathOrigin origin = PathOrigin.NORMAL) {
    public static string ResolveLocation(Path path) {
        return path.origin switch {
            PathOrigin.CONFIG => _configPath(path.location),
            PathOrigin.DATA => _dataPath(path.location),
            _ => path.location,
        };
    }
    public static PathType GetPathType(Path path) {
        if (!path.Exists()) return PathType.INVALID;

        FileAttributes attr = File.GetAttributes(ResolveLocation(path));
        return attr.HasFlag(FileAttributes.Directory) ? PathType.DIRECTORY : PathType.FILE;
    }
    public static string GetFileName(Path path) {
        return GetPathType(path) switch
        {
            PathType.FILE => System.IO.Path.GetFileNameWithoutExtension(path.Resolve()),
            PathType.DIRECTORY => System.IO.Path.GetDirectoryName(path.Resolve()),
            _ => string.Empty,
        };
    }
    public static string GetFileExtension(Path path) {
        if (path.GetPathType() != PathType.FILE) return string.Empty;

        return System.IO.Path.GetFileName(path.Resolve());
    }
    public static Path GetDirectoryPath(Path path) {
        if (!path.Exists()) return null;

        return System.IO.Path.GetDirectoryName(path.Resolve()).ToPath();
    }

    public static Path Combine(params string[] paths) {
        return System.IO.Path.Combine(paths).ToPath();
    }
    
    private static string _configPath(string path) {
        string directory = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Assembly.GetEntryAssembly().GetName().Name
        );

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        return System.IO.Path.Combine(directory, path);
    }
    private static string _dataPath(string path) {
        string directory = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Assembly.GetEntryAssembly().GetName().Name
        );

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        return System.IO.Path.Combine(directory, path);
    }
}
public enum PathType {
    INVALID,
    BOTH,
    FILE,
    DIRECTORY
}
public enum PathOrigin {
    NORMAL,
    CONFIG,
    DATA
}