using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;
using Application.Repository.Pratica;
using Application.DTOs.Pratica;

namespace StatoPraticaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatoPraticaController : ControllerBase
    {
        private readonly IPraticaRepository _praticaRepository;
        private readonly IStatoPraticaRepository _statoPraticaRepository;
        private readonly ILogger<StatoPraticaController> _logger;
        private readonly HttpClient _httpClient;

        public StatoPraticaController(
            IPraticaRepository praticaRepository,
            IStatoPraticaRepository statoPraticaRepository,
            IConfiguration configuration,
            ILogger<StatoPraticaController> logger,
            HttpClient httpClient)
        {
            _praticaRepository = praticaRepository;
            _statoPraticaRepository = statoPraticaRepository;
            _logger = logger;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Metodo GET per recuperare lo stato di una pratica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStatoPratica/{id}")]
        public async Task<IActionResult> GetStatoPratica(int id)
        {
            _logger.LogInformation("Inizio metodo GetStatoPratica");

            try
            {
                var pratica = await _praticaRepository.GetPratica(id);
                if (pratica == null)
                {
                    _logger.LogWarning("Pratica non trovata");
                    return NotFound();
                }

                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                _logger.LogInformation("Fine metodo GetStatoPratica");

                return Ok(pratica.IdStatoPratica);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Metodo PUT per aggiornare lo stato di una pratica
        /// </summary>
        /// <param name="id"></param>
        /// <param name="praticaDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateStatoPratica/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateStatoPratica([FromRoute] int id, [FromBody] StatoPraticaDto statoPraticaDto)
        {
            _logger.LogInformation("Inizio metodo UpdateStatoPratica");

            try
            {
                var pratica = await _praticaRepository.GetPratica(id);
                if (pratica == null)
                {
                    _logger.LogWarning("Pratica non trovata");
                    return NotFound();
                }

                if (statoPraticaDto.IdStatoPratica < pratica.IdStatoPratica)
                {
                    _logger.LogWarning("BadRequest - La Stato Pratica inserito è inferiore allo Stato Pratica corrente");
                    return BadRequest("La Stato Pratica inserito è inferiore allo Stato Pratica corrente");
                }

                if (statoPraticaDto.IdStatoPratica == (int)StatoPratica.PraticaCompletata
                    && (statoPraticaDto.IdRisultatoPratica == null || statoPraticaDto.IdRisultatoPratica == 0 || statoPraticaDto.IdRisultatoPratica > 2))
                {
                    _logger.LogWarning("BadRequest - Inserire un Risultato Pratica corretto: 1 = Approvata, 2 = Rifiutata");
                    return BadRequest("Inserire un Risultato Pratica corretto: 1 = Approvata, 2 = Rifiutata");
                }

                if (pratica.IdStatoPratica == (int)StatoPratica.PraticaCompletata)
                {
                    _logger.LogWarning("BadRequest - La Stato Pratica non può essere aggiornata in quanto è in Stato Completata");
                    return BadRequest("La Stato Pratica non può essere aggiornata in quanto è in Stato Completata");
                }

                pratica.IdStatoPratica = statoPraticaDto.IdStatoPratica != 0 ? statoPraticaDto.IdStatoPratica : pratica.IdStatoPratica;
                pratica.IdRisultatoPratica = (statoPraticaDto.IdRisultatoPratica != null || statoPraticaDto.IdRisultatoPratica != 0) ? statoPraticaDto.IdRisultatoPratica : pratica.IdRisultatoPratica;

                await _statoPraticaRepository.UpdateStatoPratica(pratica);

                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                _logger.LogInformation("Fine metodo UpdateStatoPratica");

                var url = "https://localhost:7004/api/AggiornamentoStato/PostAggiornamentoStatoPratica";
                var aggiornamentoStatoDto = new AggiornamentoStatoDto()
                {
                    IdPratica = id,
                    IdStatoPratica = pratica.IdStatoPratica,
                    IdRisultatoPratica = pratica.IdRisultatoPratica
                };
                var content = new StringContent(JsonSerializer.Serialize(pratica), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                return Ok(new
                {
                    message = "Stato Pratica aggiornato con successo",
                    idStatoPratica = pratica!.IdStatoPratica
                });
            }
            catch (IOException ioExp)
            {
                _logger.LogError($"Errore IOException: {ioExp.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ioExp.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
