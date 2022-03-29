using System;
using System.Collections.Generic;

namespace MinimalActions
{
    public interface IActionDescriptorGenerator
    {
        IReadOnlyList<MinimalActionDescriptor> Generate(Type type);
    }
}