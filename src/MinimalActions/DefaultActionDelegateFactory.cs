using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MinimalActions
{
    internal class DefaultActionDelegateFactory : IActionDelegateFactory
    {
        private static readonly Type _voidType = typeof(void);

        private readonly ConcurrentDictionary<Type, ObjectFactory> _objectFactoryCache = new ConcurrentDictionary<Type, ObjectFactory>();
        private readonly ConcurrentDictionary<Type, object> _actionCache = new ConcurrentDictionary<Type, object>();
        private readonly MinimalActionsOptions _options;

        public DefaultActionDelegateFactory(IOptions<MinimalActionsOptions> options)
        {
            _options = options.Value;
        }

        public Delegate Create(MinimalActionDescriptor descriptor, IServiceProvider serviceProvider)
        {
            object action;
            if (_options.InstanceCreationType == InstanceCreationType.InstancePerMethod)
            {
                var objectFactory = _objectFactoryCache.GetOrAdd(descriptor.ActionType, type => ActivatorUtilities.CreateFactory(type, Type.EmptyTypes));
                action = objectFactory(serviceProvider, arguments: null);
            }
            else if (_options.InstanceCreationType == InstanceCreationType.InstancePerType)
            {
                action = _actionCache.GetOrAdd(descriptor.ActionType, type => ActivatorUtilities.CreateInstance(serviceProvider, type));
            }
            else
            {
                throw new InvalidOperationException();
            }

            try
            {
                return CreateDelegate(descriptor.MethodInfo, action);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unable to create delegate for method {descriptor.MethodInfo.Name} of instance of {descriptor.ActionType.Name}.", ex);
            }
        }

        private static Delegate CreateDelegate(MethodInfo methodInfo, object target)
        {
            Func<Type[], Type> getType;
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);

            if (methodInfo.ReturnType == _voidType)
            {
                getType = Expression.GetActionType;
            }
            else
            {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }

            return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
        }
    }
}