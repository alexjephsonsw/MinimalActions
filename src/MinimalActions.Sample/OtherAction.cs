using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MinimalActions.Sample
{
    /// <summary>
    ///
    /// </summary>
    public class OtherAction
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet("cheese")]
        public async Task<IResult> InvokeAsync()
        {
            await Task.Delay(0);
            return Results.Ok();
        }
    }
}