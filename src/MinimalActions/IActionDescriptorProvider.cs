using System.Collections.Generic;

namespace MinimalActions
{
    public interface IActionDescriptorProvider
    {
        IReadOnlyCollection<MinimalActionDescriptor> Descriptors { get; }
    }
}