using ilikefrogs101.FileHandling;

namespace ilikefrogs101.FileHandling;
public static class PathExtensions {
    public static string Resolve(this Path value) {
        return Path.ResolveLocation(value);
    }
    public static PathType GetPathType(this Path value) {
        return Path.GetPathType(value);
    }
    public static string GetName(this Path value) {
        return Path.GetFileName(value);
    }
    public static string GetExtension(this Path value) {
        return Path.GetFileExtension(value);
    }
    public static Path GetDirectoryPath(this Path value) {
        return Path.GetDirectoryPath(value);
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