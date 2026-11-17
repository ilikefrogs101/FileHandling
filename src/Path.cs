using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ilikefrogs101.FileHandling;
public record Path(string location, PathOrigin origin = PathOrigin.NORMAL) {
    public string Resolve() {
        return origin switch {
            PathOrigin.CONFIG => _configPath(location),
            PathOrigin.DATA => _dataPath(location),
            _ => location,
        };
    }
    public PathType GetPathType() {
        if (!Exists()) return PathType.UNKNOWN;

        FileAttributes attr = File.GetAttributes(Resolve());
        return attr.HasFlag(FileAttributes.Directory) ? PathType.DIRECTORY : PathType.FILE;
    }
    public string GetFileName() {
        return GetPathType() switch
        {
            PathType.FILE => System.IO.Path.GetFileNameWithoutExtension(Resolve()),
            PathType.DIRECTORY => System.IO.Path.GetDirectoryName(Resolve()),
            _ => string.Empty,
        };
    }
    public string GetFileExtension() {
        if (GetPathType() != PathType.FILE) return string.Empty;

        return System.IO.Path.GetExtension(Resolve());
    }
    public Path GetDirectoryPath() {
        if (!Exists()) return null;

        return System.IO.Path.GetDirectoryName(Resolve()).ToPath();
    }
    public bool Exists() {
        string resolvedPath = Resolve();
        return File.Exists(resolvedPath) || Directory.Exists(resolvedPath);
    }
    public Path[] List(bool recursive = true, PathType type = PathType.UNKNOWN) {
        SearchOption option = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        string[] rawPaths = Directory.GetFileSystemEntries(Resolve(), "*", option);
        
        Path[] paths = new Path[rawPaths.Length];
        for (int i = 0; i < paths.Length; ++i) paths[i] = rawPaths[i].ToPath();

        if (type == PathType.UNKNOWN) return paths;

        return paths.Where(path => path.GetPathType() == type).ToArray();
    }
    public byte[] Read() {
        if (!Exists()) return null;

        return File.ReadAllBytes(Resolve());
    }
    public void Write(byte[] bytes) {
        File.WriteAllBytes(Resolve(), bytes);
    }
    public string ReadText() {
        if (!Exists()) return null;

        return File.ReadAllText(Resolve());
    }
    public void WriteText(string text) {
        File.WriteAllText(Resolve(), text);
    }
    public void Move(Path to) {
        if (!Exists()) return;

        switch (GetPathType()) {
            case PathType.FILE:
                File.Move(Resolve(), to.Resolve());
                return;
            case PathType.DIRECTORY:
                Directory.Move(Resolve(), to.Resolve());
                return;
        }
    }
    public void Copy(Path to) {
        if (!Exists()) return;

        switch (GetPathType()) {
            case PathType.FILE:
                File.Copy(Resolve(), to.Resolve());
                return;
            case PathType.DIRECTORY:
                // TODO: Create a function to copy directories
                return;
        }
    }
    public void Delete() {
        if (!Exists()) return;

        switch (GetPathType()) {
            case PathType.FILE:
                File.Delete(Resolve());
                return;
            case PathType.DIRECTORY:
                Directory.Delete(Resolve(), true);
                return;
        }    
    }
    public void CreateDirectory() {
        Directory.CreateDirectory(Resolve());
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
    UNKNOWN,
    FILE,
    DIRECTORY
}
public enum PathOrigin {
    NORMAL,
    CONFIG,
    DATA
}
