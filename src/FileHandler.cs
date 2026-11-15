using System.IO;
using System.Linq;

namespace ilikefrogs101.FileHandling;
public static class FileHandler {
    public static bool Exists(Path path) {
        string resolvedPath = PathHandler.ResolvePathLocation(path);
        return File.Exists(resolvedPath) || Directory.Exists(resolvedPath);
    }
    public static Path[] ListIn(Path path, bool recursive = true, PathType type = PathType.BOTH) {
        SearchOption option = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        string[] rawPaths = Directory.GetFileSystemEntries(PathHandler.ResolvePathLocation(path), "*", option);
        
        Path[] paths = new Path[rawPaths.Length];
        for (int i = 0; i < paths.Length; ++i) paths[i] = new Path(rawPaths[i]);

        if (type == PathType.BOTH) return paths.ToArray();

        return paths.Where(path => PathHandler.ResolvePathType(path) == type).ToArray();
    }
    public static string Read(Path path) {
        if (!Exists(path)) return null;

        return File.ReadAllText(PathHandler.ResolvePathLocation(path));
    }
    public static void Write(string text, Path path) {
        File.WriteAllText(PathHandler.ResolvePathLocation(path), text);
    }
    public static void Move(Path from, Path to) {
        if (!Exists(from)) return;

        switch (PathHandler.ResolvePathType(from)) {
            case PathType.FILE:
                File.Move(PathHandler.ResolvePathLocation(from), PathHandler.ResolvePathLocation(to));
                return;
            case PathType.DIRECTORY:
                Directory.Move(PathHandler.ResolvePathLocation(from), PathHandler.ResolvePathLocation(to));
                return;
        }
    }
    public static void Copy(Path from, Path to) {
        if (!Exists(from)) return;

        switch (PathHandler.ResolvePathType(from)) {
            case PathType.FILE:
                File.Copy(PathHandler.ResolvePathLocation(from), PathHandler.ResolvePathLocation(to));
                return;
            case PathType.DIRECTORY:
                // TODO: Create a function to copy directories
                //Directory.Copy(PathHandler.ResolvePathLocation(from), PathHandler.ResolvePathLocation(to));
                return;
        }
    }
    public static void Delete(Path path) {
        if (!Exists(path)) return;

        switch (PathHandler.ResolvePathType(path)) {
            case PathType.FILE:
                File.Delete(PathHandler.ResolvePathLocation(path));
                return;
            case PathType.DIRECTORY:
                Directory.Delete(PathHandler.ResolvePathLocation(path), true);
                return;
        }    
    }
    public static void CreateDirectory(Path path) {
        Directory.CreateDirectory(PathHandler.ResolvePathLocation(path));
    }
}