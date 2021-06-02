namespace MagnetosBrotherhood.Api.Controllers
{
    using MagnetosBrotherhood.Api.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Endpoint for system services.
    /// </summary>
    [Route("api/v1/system")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SystemController : Controller
    {
        /// <summary>
        /// Return the version of the Api.
        /// </summary>
        /// <returns>A string version message.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet()]
        public string Version()
            => "Version 1.0";

        /// <summary>
        /// Get info related to the program.
        /// </summary>
        /// <returns>A string message.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("info")]
        public string Info([FromBody] InfoDto info)
            => string.IsNullOrWhiteSpace(info?.ApplicantName) ? "You better provide a name!" : $"I'm already watching you {info.ApplicantName}, but we need to test you dna first!";
    }
}
