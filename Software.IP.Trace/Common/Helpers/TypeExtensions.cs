using System;

namespace Software.IP.Trace.Common.Helpers;

public static class TypeExtensions {
    public static bool CanCastTo<T>(this Type sourceType) =>
        sourceType.CanCastTo(typeof(T));

    public static bool CanCastTo(this Type sourceType, Type castType) =>
        castType.IsAssignableFrom(sourceType);
}