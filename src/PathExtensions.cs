using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ilikefrogs101.FileHandling;
public static class PathExtensions {
    public static PathType GetPathType(this Path path) {
        if (!path.Exists()) return PathType.INVALID;

        FileAttributes attr = File.GetAttributes(path.ToString());
        return attr.HasFlag(FileAttributes.Directory) ? PathType.DIRECTORY : PathType.FILE;
    }
    public static string GetFileName(this Path path) {
        return GetPathType(path) switch
        {
            PathType.FILE => System.IO.Path.GetFileNameWithoutExtension(path.ToString()),
            PathType.DIRECTORY => System.IO.Path.GetDirectoryName(path.ToString()),
            _ => string.Empty,
        };
    }
    public static string GetFileExtension(this Path path) {
        if (path.GetPathType() != PathType.FILE) return string.Empty;

        return System.IO.Path.GetExtension(path.ToString());
    }
    public static Path GetDirectoryPath(this Path path) {
        if (!path.Exists()) return null;

        return System.IO.Path.GetDirectoryName(path.ToString()).ToPath();
    }
    public static bool Exists(this Path path) {
        string resolvedPath = path.ToString();
        return File.Exists(resolvedPath) || Directory.Exists(resolvedPath);
    }
    public static Path[] ListIn(this Path path, bool recursive = true, PathType type = PathType.BOTH) {
        SearchOption option = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        string[] rawPaths = Directory.GetFileSystemEntries(path.ToString(), "*", option);
        
        Path[] paths = new Path[rawPaths.Length];
        for (int i = 0; i < paths.Length; ++i) paths[i] = rawPaths[i].ToPath();

        if (type == PathType.BOTH) return paths;

        return paths.Where(path => path.GetPathType() == type).ToArray();
    }
    public static byte[] Read(this Path path) {
        if (!Exists(path)) return null;

        return File.ReadAllBytes(path.ToString());
    }
    public static void Write(this Path path, byte[] bytes) {
        File.WriteAllBytes(path.ToString(), bytes);
    }
    public static string ReadText(this Path path) {
        if (!Exists(path)) return null;

        return File.ReadAllText(path.ToString());
    }
    public static void WriteText(this Path path, string text) {
        File.WriteAllText(path.ToString(), text);
    }
    public static void Move(this Path from, Path to) {
        if (!from.Exists()) return;

        switch (from.GetPathType()) {
            case PathType.FILE:
                File.Move(from.ToString(), to.ToString());
                return;
            case PathType.DIRECTORY:
                Directory.Move(from.ToString(), to.ToString());
                return;
        }
    }
    public static void Copy(this Path from, Path to) {
        if (!from.Exists()) return;

        switch (from.GetPathType()) {
            case PathType.FILE:
                File.Copy(from.ToString(), to.ToString());
                return;
            case PathType.DIRECTORY:
                // TODO: Create a function to copy directories
                return;
        }
    }
    public static void Delete(this Path path) {
        if (!Exists(path)) return;

        switch (path.GetPathType()) {
            case PathType.FILE:
                File.Delete(path.ToString());
                return;
            case PathType.DIRECTORY:
                Directory.Delete(path.ToString(), true);
                return;
        }    
    }
    public static void CreateDirectory(this Path path) {
        Directory.CreateDirectory(path.ToString());
    }
}
