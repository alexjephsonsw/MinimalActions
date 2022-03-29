using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimalActions
{
    internal class DefaultActionDescriptorProvider : IActionDescriptorProvider
    {
        private readonly Lazy<IReadOnlyCollection<MinimalActionDescriptor>> _descriptors;
        private readonly IActionTypeProvider _actionTypeProvider;
        private readonly IEnumerable<IActionDescriptorGenerator> _actionDescriptorGenerators;

        public DefaultActionDescriptorProvider(
            IActionTypeProvider actionTypeProvider,
            IEnumerable<IActionDescriptorGenerator> actionDescriptorGenerators)
        {
            _descriptors = new Lazy<IReadOnlyCollection<MinimalActionDescriptor>>(Factory);
            _actionTypeProvider = actionTypeProvider;
            _actionDescriptorGenerators = actionDescriptorGenerators;
        }

        private IReadOnlyCollection<MinimalActionDescriptor> Factory()
        {
            return GetDescriptors().ToList().AsReadOnly();
        }

        private IEnumerable<MinimalActionDescriptor> GetDescriptors()
        {
            foreach (var type in _actionTypeProvider.Actions)
            {
                foreach (var actionDescriptorGenerator in _actionDescriptorGenerators)
                {
                    var descriptors = actionDescriptorGenerator.Generate(type);
                    foreach (var descriptor in descriptors)
                    {
                        yield return descriptor;
                    }
                }
            }
        }

        public IReadOnlyCollection<MinimalActionDescriptor> Descriptors => _descriptors.Value;
    }
}