using System;
using System.IO;
using System.Reflection;

namespace ilikefrogs101.FileHandling;
public record Path(string location, PathOrigin origin = PathOrigin.NORMAL);
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
