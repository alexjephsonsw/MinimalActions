using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MinimalActions
{
    internal class DefaultActionDescriptorGenerator : IActionDescriptorGenerator
    {
        private const string InvokeMethodName = "Invoke";
        private const string HttpDeleteMethodName = "Delete";
        private const string HttpGetMethodName = "Get";
        private const string HttpHeadMethodName = "Head";
        private const string HttpPostMethodName = "Post";
        private const string HttpPatchMethodName = "Patch";
        private const string HttpPutMethodName = "Put";
        private const string AsyncSuffix = "Async";

        private readonly ILogger<DefaultActionDescriptorGenerator> _logger;

        public DefaultActionDescriptorGenerator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DefaultActionDescriptorGenerator>();
        }

        public IReadOnlyList<MinimalActionDescriptor> Generate(Type type)
        {
            var methods = GetMethods(type);

            var descriptors = new List<MinimalActionDescriptor>();
            foreach (var methodInfo in methods)
            {
                var descriptor = CreateActionDescriptor(type, methodInfo, _logger);
                descriptors.Add(descriptor);
            }

            return descriptors.AsReadOnly();
        }

        private static MinimalActionDescriptor CreateActionDescriptor(Type type, MethodInfo methodInfo, ILogger logger)
        {
            AttributeRouteInfo? attributeRouteInfo = null;
            var httpMethods = new HashSet<string>();
            string? displayName = null;

            var attributes = methodInfo.GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                if (attribute is IActionHttpMethodProvider actionHttpMethodProvider)
                {
                    foreach (var httpMethod in actionHttpMethodProvider.HttpMethods)
                    {
                        httpMethods.Add(httpMethod);
                    }
                }

                if (attribute is IRouteTemplateProvider routeTemplateProvider)
                {
                    attributeRouteInfo = new AttributeRouteInfo
                    {
                        Name = routeTemplateProvider.Name,
                        Order = routeTemplateProvider.Order ?? 0,
                        Template = routeTemplateProvider.Template,
                    };
                }

                if (attribute is DisplayNameAttribute displayNameAttribute)
                {
                    displayName = displayNameAttribute.DisplayName;
                }
            }

            if (methodInfo.Name != InvokeMethodName && methodInfo.Name != InvokeMethodName + AsyncSuffix)
            {
                if (httpMethods.Count > 0)
                {
                    foreach (var method in httpMethods)
                    {
                        logger.LogWarning("HttpMethod {httpMethod} provided for method {method} was ignored.", method, methodInfo.Name);
                    }
                    httpMethods.Clear();
                }
                httpMethods.Add(GetHttpMethodFromMethodName(methodInfo.Name));
            }

            var routePattern = GeneratePattern(type, attributeRouteInfo);
            if (attributeRouteInfo is not null)
            {
                attributeRouteInfo.Template = routePattern;
            }

            var descriptor = new MinimalActionDescriptor(type, methodInfo)
            {
                AttributeRouteInfo = attributeRouteInfo,
                HttpMethods = httpMethods,
                RoutePattern = routePattern,
            };

            if (displayName is not null)
            {
                descriptor.DisplayName = displayName;
            }

            return descriptor;
        }

        private static string GetHttpMethodFromMethodName(string name)
        {
            return name switch
            {
                HttpDeleteMethodName or HttpDeleteMethodName + AsyncSuffix => HttpMethods.Delete,
                HttpGetMethodName or HttpGetMethodName + AsyncSuffix => HttpMethods.Get,
                HttpHeadMethodName or HttpHeadMethodName + AsyncSuffix => HttpMethods.Head,
                HttpPostMethodName or HttpPostMethodName + AsyncSuffix => HttpMethods.Post,
                HttpPatchMethodName or HttpPatchMethodName + AsyncSuffix => HttpMethods.Patch,
                HttpPutMethodName or HttpPutMethodName + AsyncSuffix => HttpMethods.Put,
                _ => throw new ArgumentOutOfRangeException(nameof(name)),
            };
        }

        private static string? GeneratePattern(Type type, AttributeRouteInfo? attributeRouteInfo)
        {
            string? actionRouteTemplate = null;
            var actionRoute = type.GetCustomAttributes().OfType<IRouteTemplateProvider>().FirstOrDefault(a => a.Template is not null);
            if (actionRoute is not null)
            {
                actionRouteTemplate = actionRoute.Template;
            }

            string? methodRouteTemplate = null;
            if (attributeRouteInfo is not null)
            {
                methodRouteTemplate = attributeRouteInfo.Template;
            }

            if (actionRouteTemplate is null && methodRouteTemplate is null)
            {
                return null;
            }

            return $"{actionRouteTemplate}{(actionRouteTemplate?.EndsWith('/') == true || methodRouteTemplate is null ? "" : "/")}{methodRouteTemplate}";
        }

        private static IEnumerable<MethodInfo> GetMethods(Type type)
        {
            var invokeMethod = type.GetMethod(InvokeMethodName);
            if (invokeMethod is not null)
            {
                yield return invokeMethod;
            }

            var invokeAsyncMethod = type.GetMethod(InvokeMethodName + AsyncSuffix);
            if (invokeAsyncMethod is not null)
            {
                yield return invokeAsyncMethod;
            }

            var getMethod = type.GetMethod(HttpGetMethodName);
            if (getMethod is not null)
            {
                yield return getMethod;
            }

            var getMethodAsync = type.GetMethod(HttpGetMethodName + AsyncSuffix);
            if (getMethodAsync is not null)
            {
                yield return getMethodAsync;
            }

            var postMethod = type.GetMethod(HttpPostMethodName);
            if (postMethod is not null)
            {
                yield return postMethod;
            }

            var postMethodAsync = type.GetMethod(HttpPostMethodName + AsyncSuffix);
            if (postMethodAsync is not null)
            {
                yield return postMethodAsync;
            }

            var patchMethod = type.GetMethod(HttpPatchMethodName);
            if (patchMethod is not null)
            {
                yield return patchMethod;
            }

            var patchMethodAsync = type.GetMethod(HttpPatchMethodName + AsyncSuffix);
            if (patchMethodAsync is not null)
            {
                yield return patchMethodAsync;
            }

            var putMethod = type.GetMethod(HttpPutMethodName);
            if (putMethod is not null)
            {
                yield return putMethod;
            }

            var putMethodAsync = type.GetMethod(HttpPutMethodName + AsyncSuffix);
            if (putMethodAsync is not null)
            {
                yield return putMethodAsync;
            }

            var deleteMethod = type.GetMethod(HttpDeleteMethodName);
            if (deleteMethod is not null)
            {
                yield return deleteMethod;
            }

            var deleteMethodAsync = type.GetMethod(HttpDeleteMethodName + AsyncSuffix);
            if (deleteMethodAsync is not null)
            {
                yield return deleteMethodAsync;
            }

            var headMethod = type.GetMethod(HttpHeadMethodName);
            if (headMethod is not null)
            {
                yield return headMethod;
            }

            var headMethodAsync = type.GetMethod(HttpHeadMethodName + AsyncSuffix);
            if (headMethodAsync is not null)
            {
                yield return headMethodAsync;
            }
        }
    }
}