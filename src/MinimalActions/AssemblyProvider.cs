using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace MinimalActions
{
    internal class AssemblyProvider : IAssemblyProvider
    {
        private readonly AssemblyName _assemblyName;

        public AssemblyProvider(IWebHostEnvironment webHostEnvironment)
        {
            _assemblyName = new AssemblyName(webHostEnvironment.ApplicationName);
        }

        public Assembly Assembly
        {
            get
            {
                return Assembly.Load(_assemblyName);
            }
        }
    }
}