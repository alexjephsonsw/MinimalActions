using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MinimalActions
{
    /// <summary>
    /// Describes an Minimal Action.
    /// </summary>
    public class MinimalActionDescriptor
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MinimalActionDescriptor"/>.
        /// </summary>
        public MinimalActionDescriptor(Type actionType, MethodInfo methodInfo)
        {
            Id = Guid.NewGuid().ToString();
            ActionType = actionType;
            MethodInfo = methodInfo;
            DisplayName = actionType.Name;
        }

        /// <summary>
        /// Gets an id which uniquely identifies the action.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the <see cref="AttributeRouteInfo"/>.
        /// </summary>
        public AttributeRouteInfo? AttributeRouteInfo { get; init; }

        /// <summary>
        /// A friendly name for this action.
        /// </summary>
        public string DisplayName { get; init; }

        /// <summary>
        ///
        /// </summary>
        public Type ActionType { get; }

        /// <summary>
        ///
        /// </summary>
        public MethodInfo MethodInfo { get; }

        /// <summary>
        ///
        /// </summary>
        public string? RoutePattern { get; init; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> HttpMethods { get; init; } = Enumerable.Empty<string>();

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<Attribute> MethodAttributes { get; init; } = Enumerable.Empty<Attribute>();
    }
}