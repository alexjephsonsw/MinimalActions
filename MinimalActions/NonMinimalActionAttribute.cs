using System;

namespace MinimalActions
{
    /// <summary>
    /// Indicates that the type and any derived types that this attribute is applied to
    /// is not considered a minimal action by the default discovery mechanism.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class NonMinimalActionAttribute : Attribute
    {
    }
}