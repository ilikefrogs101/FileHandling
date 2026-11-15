using System.IO;
using System.Linq;

namespace ilikefrogs101.FileHandling;
public static class FileSystem {
    public static bool Exists(Path path) {
        string resolvedPath = path.ResolveLocation();
        return File.Exists(resolvedPath) || Directory.Exists(resolvedPath);
    }
    public static Path[] ListIn(Path path, bool recursive = true, PathType type = PathType.BOTH) {
        SearchOption option = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        string[] rawPaths = Directory.GetFileSystemEntries(path.ResolveLocation(), "*", option);
        
        Path[] paths = new Path[rawPaths.Length];
        for (int i = 0; i < paths.Length; ++i) paths[i] = rawPaths[i].ToPath();

        if (type == PathType.BOTH) return paths.ToArray();

        return paths.Where(path => path.ResolveType() == type).ToArray();
    }
    public static string Read(Path path) {
        if (!Exists(path)) return null;

        return File.ReadAllText(path.ResolveLocation());
    }
    public static void Write(string text, Path path) {
        File.WriteAllText(path.ResolveLocation(), text);
    }
    public static void Move(Path from, Path to) {
        if (!from.Exists()) return;

        switch (from.ResolveType()) {
            case PathType.FILE:
                File.Move(from.ResolveLocation(), to.ResolveLocation());
                return;
            case PathType.DIRECTORY:
                Directory.Move(from.ResolveLocation(), to.ResolveLocation());
                return;
        }
    }
    public static void Copy(Path from, Path to) {
        if (!from.Exists()) return;

        switch (from.ResolveType()) {
            case PathType.FILE:
                File.Copy(from.ResolveLocation(), to.ResolveLocation());
                return;
            case PathType.DIRECTORY:
                // TODO: Create a function to copy directories
                //Directory.Copy(from.ResolveLocation(), to.ResolveLocation());
                return;
        }
    }
    public static void Delete(Path path) {
        if (!Exists(path)) return;

        switch (path.ResolveType()) {
            case PathType.FILE:
                File.Delete(path.ResolveLocation());
                return;
            case PathType.DIRECTORY:
                Directory.Delete(path.ResolveLocation(), true);
                return;
        }    
    }
    public static void CreateDirectory(Path path) {
        Directory.CreateDirectory(path.ResolveLocation());
    }
}