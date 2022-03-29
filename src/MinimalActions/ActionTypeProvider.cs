using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MinimalActions
{
    internal class ActionTypeProvider : IActionTypeProvider
    {
        private const string ActionTypeNameSuffix = "Action";

        private readonly IAssemblyProvider _assemblyProvider;
        private readonly Lazy<IReadOnlyCollection<TypeInfo>> _actions;

        public ActionTypeProvider(IAssemblyProvider assemblyProvider)
        {
            _assemblyProvider = assemblyProvider;

            _actions = new Lazy<IReadOnlyCollection<TypeInfo>>(Factory);
        }

        private IReadOnlyCollection<TypeInfo> Factory()
        {
            return GenerateTypes().ToList().AsReadOnly();
        }

        private IEnumerable<TypeInfo> GenerateTypes()
        {
            var types = _assemblyProvider.Assembly.GetTypes().Select(t => t.GetTypeInfo());

            foreach (var type in types)
            {
                if (IsMinimalAction(type))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Determines if a given <paramref name="typeInfo"/> is a controller.
        /// </summary>
        /// <param name="typeInfo">The <see cref="TypeInfo"/> candidate.</param>
        /// <returns><see langword="true" /> if the type is a controller; otherwise <see langword="false" />.</returns>
        protected virtual bool IsMinimalAction(TypeInfo typeInfo)
        {
            if (typeof(IAction).IsAssignableFrom(typeInfo))
            {
                return true;
            }

            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            // We only consider public top-level classes as actions. IsPublic returns false for nested
            // classes, regardless of visibility modifiers
            if (!typeInfo.IsPublic)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (typeInfo.IsDefined(typeof(NonMinimalActionAttribute)))
            {
                return false;
            }

            if (!typeInfo.Name.EndsWith(ActionTypeNameSuffix, StringComparison.OrdinalIgnoreCase) &&
                !typeInfo.IsDefined(typeof(MinimalActionAttribute)))
            {
                return false;
            }

            return true;
        }

        public IReadOnlyCollection<TypeInfo> Actions => _actions.Value;
    }
}