using System;
using System.IO;
using System.Reflection;

namespace ilikefrogs101.FileHandling;
public class Path(string location, PathOrigin origin = PathOrigin.NORMAL) {
    public override string ToString() {
        return origin switch {
            PathOrigin.CONFIG => _configPath(location),
            PathOrigin.DATA => _dataPath(location),
            _ => location,
        };
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
