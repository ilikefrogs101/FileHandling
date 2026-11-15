namespace ilikefrogs101.FileHandling;
public static class StringExtensions {
    public static Path ToPath(this string value, PathOrigin origin = PathOrigin.NORMAL) {
        return new Path(value, origin);
    }
}