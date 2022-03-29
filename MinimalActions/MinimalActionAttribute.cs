using System;

namespace MinimalActions
{
    /// <summary>
    /// Indicates that the type and any derived types that this attribute is applied to
    /// are considered a minimal action by the default discovery mechanism, unless
    /// <see cref="NonMinimalActionAttribute"/> is applied to any type in the hierarchy.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MinimalActionAttribute : Attribute
    {
    }
}