namespace MagnetosBrotherhood.Api.Controllers
{
    using AutoMapper;
    using MagnetosBrotherhood.Domain.Services;
    using MagnetosBrotherhood.Api.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// Endpoint for mutants services.
    /// </summary>
    [Route("api/v1/mutants")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MutantController : Controller
    {
        /// <summary>
        /// Auto mapper instance.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Domain mutant service instance.
        /// </summary>
        private readonly IMutantService _mutantService;

        /// <summary>
        /// Initialize a new instance of <see cref="MutantController"/> class.
        /// </summary>
        public MutantController(IMapper mapper, IMutantService mutantService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mutantService = mutantService ?? throw new ArgumentNullException(nameof(mutantService));
        }

        /// <summary>
        /// Process a sequence of dna and return ok or forbidden based on whether is human or mutant.
        /// </summary>
        /// <param name="dnaCheckRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>An action result response with Ok ro Forbidden.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost("mutant")]
        public async Task<IActionResult> ProcessDnaSample([FromBody]DnaDto dnaCheckRequest, CancellationToken cancellationToken)
        {
            if (dnaCheckRequest?.Dna == null)
                return BadRequest(dnaCheckRequest?.Dna);

            var dna = _mapper.Map<DnaDto, Domain.Models.DnaInfo>(dnaCheckRequest);
            if (await _mutantService.ProcessDnaSampleAsync(dna, cancellationToken).ConfigureAwait(false))
                return Ok();

            return StatusCode(403); //Forbidden
        }

        /// <summary>
        /// Get dnas statistics.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A DnaStatistics object.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("stats")]
        public async Task<IActionResult> GetDnaStatistics(CancellationToken cancellationToken)
        {
            var result = await _mutantService.GetDnaStatisticsAsync(cancellationToken).ConfigureAwait(false);

            if (result != null)
                return Ok(JsonConvert.SerializeObject(_mapper.Map<Models.DnaStatistics>(result), Formatting.Indented));

            return NotFound();
        }
    }
}
