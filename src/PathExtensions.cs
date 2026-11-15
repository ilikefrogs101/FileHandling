using ilikefrogs101.FileHandling;

public static class PathExtensions {
    public static string ResolveLocation(this Path value) {
        return PathUtils.ResolvePathLocation(value);
    }
    public static PathType ResolveType(this Path value) {
        return PathUtils.ResolvePathType(value);
    }
    public static string ResolveName(this Path value) {
        return PathUtils.ResolvePathName(value);
    }
    public static string ResolveExtension(this Path value) {
        return PathUtils.ResolvePathExtension(value);
    }
    public static Path ResolveDirectory(this Path value) {
        return PathUtils.ResolveDirectory(value);
    }

    public static bool Exists(this Path value) {
        return FileSystem.Exists(value);
    }
    public static Path[] List(this Path value, bool recursive = true, PathType type = PathType.BOTH) {
        return FileSystem.ListIn(value, recursive, type);
    }
    public static string Read(this Path value) {
        return FileSystem.Read(value);
    }
    public static void Write(this Path value, string text) {
        FileSystem.Write(text, value);
    }
    public static void MoveTo(this Path value, Path to) {
        FileSystem.Move(value, to);
    }
    public static void Copy(this Path value, Path to) {
        FileSystem.Copy(value, to);
    }
    public static void Delete(this Path value) {
        FileSystem.Delete(value);
    }
}