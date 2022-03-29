using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace MinimalActions.Sample
{
    /// <summary>
    ///
    /// </summary>
    [SwaggerTag("Static Action")]
    public static class StaticAction
    {
        /// <summary>
        /// Custom static thing
        /// </summary>
        /// <param name="id">Static id</param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static Task<IResult> CustomHandler(long id, HttpContext httpContext)
        {
            httpContext.RequestServices.GetService<IHttpContextAccessor>();
            return Task.FromResult(Results.Ok(id));
        }
    }
}