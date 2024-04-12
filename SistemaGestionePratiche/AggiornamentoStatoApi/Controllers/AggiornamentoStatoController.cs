using Application.DTOs.Pratica;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AggiornamentoStatoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggiornamentoStatoController : ControllerBase
    {
        private readonly ILogger<AggiornamentoStatoController> _logger;

        public AggiornamentoStatoController(ILogger<AggiornamentoStatoController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Metodo POST per recuperare l'aggiornamento di stato di una pratica
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("PostAggiornamentoStatoPratica")]
        public async Task<IActionResult> PostAggiornamentoStatoPratica(AggiornamentoStatoDto aggiornamentoStatoDto)
        {
            _logger.LogInformation("Inizio metodo GetAggiornamentoStatoPratica");

            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                var text = $"Lo stato della Pratica {aggiornamentoStatoDto.IdPratica} è stato aggiornato con successo a: {aggiornamentoStatoDto.IdStatoPratica} - {(StatoPratica)aggiornamentoStatoDto.IdStatoPratica}";

                _logger.LogInformation("Fine metodo GetAggiornamentoStatoPratica");

                return Ok(new
                {
                    message = text
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
