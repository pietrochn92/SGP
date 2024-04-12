using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Application.Repository.Pratica;
using Application.DTOs.Pratica;

namespace PraticaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PraticaController : ControllerBase
    {
        private readonly IPraticaRepository _praticaRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PraticaController> _logger;
        private readonly string folderName;
        private readonly string pathToSave;

        public PraticaController(
            IPraticaRepository praticaRepository, 
            IConfiguration configuration, 
            ILogger<PraticaController> logger)
        {
            _praticaRepository = praticaRepository;
            _configuration = configuration;
            _logger = logger;
            folderName = _configuration.GetValue<string>("FileFolder:Default");
            pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        }

        /// <summary>
        /// Metodo GET per recuperare la lista di tutte le pratiche
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPratiche")]
        public async Task<IActionResult> GetPratiche()
        {
            _logger.LogInformation("Inizio metodo GetPratiche");

            try
            {
                var pratiche = await _praticaRepository.GetPratiche();

                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                _logger.LogInformation("Fine metodo GetPratiche");

                return Ok(pratiche);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Metodo GET per recuperare una pratica per Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPratica/{id}")]
        public async Task<IActionResult> GetPratica(int id)
        {
            _logger.LogInformation("Inizio metodo GetPratica");

            try
            {
                var pratica = await _praticaRepository.GetPratica(id);
                if (pratica == null)
                {
                    _logger.LogWarning("Pratica non trovata");
                    return NotFound();
                }

                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                _logger.LogInformation("Fine metodo GetPratica");

                return Ok(pratica);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Metodo POST per creare una pratica
        /// </summary>
        /// <param name="praticaDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreatePratica")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreatePratica([FromForm] PraticaDto praticaDto)
        {
            _logger.LogInformation("Inizio metodo CreatePratica");

            try
            {                
                praticaDto.IdStatoPratica = (int)StatoPratica.PraticaCreata;
                if (praticaDto.Allegato != null)
                {
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var fileName = $"{Guid.NewGuid()} - {praticaDto.Allegato?.FileName}";
                    var fullPath = Path.Combine(pathToSave, fileName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        praticaDto.Allegato?.CopyTo(stream);
                    }

                    praticaDto.AllegatoName = fileName;
                }

                var pratica = await _praticaRepository.CreatePratica(praticaDto);

                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                _logger.LogInformation("Fine metodo CreatePratica");

                return Ok(new
                {
                    message = "Pratica creata con successo",
                    id = pratica!.Id
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

        /// <summary>
        /// Metodo PUT per aggiornare una pratica per Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="praticaDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdatePratica/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdatePratica([FromRoute] int id, [FromForm] PraticaDto praticaDto)
        {
            _logger.LogInformation("Inizio metodo UpdatePratica");

            try
            {
                var pratica = await _praticaRepository.GetPratica(id);
                if (pratica == null)
                {
                    _logger.LogWarning("Pratica non trovata");
                    return NotFound();
                }

                if (pratica.IdStatoPratica != (int)StatoPratica.PraticaCreata)
                {
                    _logger.LogWarning("BadRequest - Pratica in stato diverso da 1");
                    return BadRequest("La Pratica può essere aggiornata esclusivamente in stato 1 - Pratica Creata.");
                }

                praticaDto.Id = praticaDto.Id != 0 ? praticaDto.Id : pratica.Id;
                praticaDto.IdStatoPratica = praticaDto.IdStatoPratica != 0 ? praticaDto.IdStatoPratica : pratica.IdStatoPratica;
                praticaDto.IdRisultatoPratica = (praticaDto.IdRisultatoPratica != null || praticaDto.IdRisultatoPratica != 0) ? praticaDto.IdRisultatoPratica : pratica.IdRisultatoPratica;
                praticaDto.NomeUtente = !string.IsNullOrEmpty(praticaDto.NomeUtente) ? praticaDto.NomeUtente : pratica.NomeUtente;
                praticaDto.CognomeUtente = !string.IsNullOrEmpty(praticaDto.CognomeUtente) ? praticaDto.CognomeUtente : pratica.CognomeUtente;
                praticaDto.CodiceFiscaleUtente = !string.IsNullOrEmpty(praticaDto.CodiceFiscaleUtente) ? praticaDto.CodiceFiscaleUtente : pratica.CodiceFiscaleUtente;
                praticaDto.DataNascitaUtente = praticaDto.DataNascitaUtente != null ? praticaDto.DataNascitaUtente : pratica.DataNascitaUtente;
                praticaDto.Allegato = praticaDto.Allegato ?? null;               
                if (praticaDto.Allegato != null)
                {
                    if (!string.IsNullOrEmpty(pratica.AllegatoName))
                    {
                        var fullPath = Path.Combine(pathToSave, pratica.AllegatoName);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    var fileName = $"{Guid.NewGuid()} - {praticaDto.Allegato?.FileName}";
                    var newFullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(newFullPath, FileMode.Create))
                    {
                        praticaDto.Allegato?.CopyTo(stream);
                    }

                    praticaDto.AllegatoName = fileName;
                }
                else
                {
                    praticaDto.AllegatoName = pratica.AllegatoName;
                }

                await _praticaRepository.UpdatePratica(praticaDto);

                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                _logger.LogInformation("Fine metodo UpdatePratica");

                return Ok(new
                {
                    message = "Pratica aggiornata con successo",
                    id = pratica!.Id
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

        /// <summary>
        /// Metodo DELETE per eliminare una pratica per Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeletePratica([FromRoute] int id)
        {
            _logger.LogInformation("Inizio metodo DeletePratica");

            try
            {
                var pratica = await _praticaRepository.GetPratica(id);
                if (pratica == null)
                {
                    _logger.LogWarning("Pratica non trovata");
                    return NotFound();
                }

                await _praticaRepository.DeletePratica(id);

                var fullPath = Path.Combine(pathToSave, pratica.AllegatoName ?? string.Empty);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                _logger.LogInformation("Fine metodo DeletePratica");

                return Ok(new
                {
                    message = "Pratica eliminata con successo",
                    id
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

        /// <summary>
        /// Metodo GET per il download del file by idPratica
        /// </summary>
        /// <param name="idPratica"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DownloadFileByIdPratica/{idPratica}")]
        public async Task<IActionResult> DownloadFileByIdPratica(int idPratica)
        {
            _logger.LogInformation("Inizio metodo DownloadFileByIdPratica");

            try
            {
                var pratica = await _praticaRepository.GetPratica(idPratica);
                if (pratica == null)
                {
                    _logger.LogWarning("Pratica non trovata");
                    return NotFound();
                }

                var fileName = pratica.AllegatoName ?? string.Empty;
                var fullPath = Path.Combine(pathToSave, fileName);
                if (!System.IO.File.Exists(fullPath))
                {
                    _logger.LogWarning("BadRequest - Il file non esiste");
                    return BadRequest("Il file non esiste.");
                }

                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                var fileContent = new FileContentResult(fileBytes, "application/octet-stream")
                {
                    FileDownloadName = fileName
                };

                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                _logger.LogInformation("Fine metodo DownloadFileByIdPratica");

                return fileContent;
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
