using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Application.Repository.Pratica;

namespace SGP.Services.PraticaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoricoPraticaController : ControllerBase
    {
        private readonly IStoricoPraticaRepository _storicoPraticaRepository;
        private readonly ILogger<StoricoPraticaController> _logger;

        public StoricoPraticaController(IStoricoPraticaRepository storicoPraticaRepository,
            ILogger<StoricoPraticaController> logger)
        {
            _storicoPraticaRepository = storicoPraticaRepository;
            _logger = logger;
        }

        /// <summary>
        /// Metodo GET per recuperare lo storico pratica
        /// </summary>
        /// <param name="idPratica"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStoricoPratica/{idPratica}")]
        public async Task<IActionResult> GetStoricoPratica(int idPratica)
        {
            _logger.LogInformation("Inizio metodo GetStoricoPratica");

            try
            {
                var storicoPratica = await _storicoPraticaRepository.GetStoricoPraticaById(idPratica);

                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                _logger.LogInformation("Fine metodo GetStoricoPratica");

                return Ok(storicoPratica);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
