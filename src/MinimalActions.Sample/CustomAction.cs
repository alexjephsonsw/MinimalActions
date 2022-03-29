using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace MinimalActions.Sample
{
    /// <summary>
    /// Actionss
    /// </summary>
    [Route("action")]
    [SwaggerTag("Actions")]
    public class CustomAction : IAction
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        ///
        /// </summary>
        public CustomAction(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Booma
        /// </summary>
        /// <param name="id">The Id</param>
        /// <param name="key">The key</param>
        /// <param name="bank">The query</param>
        /// <returns></returns>
        [HttpGet("thing/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Boo hee", Description = "Whompoos")]
        public Task<IResult> InvokeAsync(int id, [FromHeader] string key, [FromQuery] string bank)
        {
            if (key != _configuration.GetValue<string>("ApiKey"))
            {
                return Task.FromResult(Results.Unauthorized());
            }
            return Task.FromResult(Results.Ok(id.ToString()));
        }

        /// <summary>
        /// Ho get
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Boo hoo", Description = "Whompas", Tags = new[] { "ActionFromOperation2" })]
        public Task<IResult> GetAsync()
        {
            return Task.FromResult(Results.Ok());
        }

        /// <summary>
        /// Post DDD
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="input"></param>
        /// <param name="apiKey"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Route("{id}")]
        [Route("temp")]
        public Task<IResult> PostAsync(string id, TestClass input, [FromHeader] string apiKey, [FromQuery] int limit = 500)
        {
            return Task.FromResult(Results.Ok());
        }

        /// <summary>
        ///
        /// </summary>
        public class TestClass
        {
            /// <summary>
            /// The value under test.
            /// </summary>
            public string? Value { get; set; }

            /// <summary>
            /// The site to test.
            /// </summary>
            public string? Site { get; set; }
        }
    }
}