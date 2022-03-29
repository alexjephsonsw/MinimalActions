using System;

namespace MinimalActions
{
    internal interface IActionDelegateFactory
    {
        Delegate Create(MinimalActionDescriptor descriptor, IServiceProvider serviceProvider);
    }
}