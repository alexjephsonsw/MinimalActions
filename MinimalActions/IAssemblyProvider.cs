using System.Collections.Generic;
using System.Reflection;

namespace MinimalActions
{
    internal interface IAssemblyProvider
    {
        Assembly Assembly { get; }
    }
}