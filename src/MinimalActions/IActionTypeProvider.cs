using System.Collections.Generic;
using System.Reflection;

namespace MinimalActions
{
    internal interface IActionTypeProvider
    {
        IReadOnlyCollection<TypeInfo> Actions { get; }
    }
}