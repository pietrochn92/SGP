using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Application.DTOs.Pratica
{
    public class PraticaDto
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public required int IdStatoPratica { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public int? IdRisultatoPratica { get; set; }

        public string? NomeUtente { get; set; }

        public string? CognomeUtente { get; set; }

        public string? CodiceFiscaleUtente { get; set; }

        public DateTime? DataNascitaUtente { get; set; }

        public IFormFile? Allegato { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public string? AllegatoName { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public DateTime? LastUpdate { get; set; }

        [JsonIgnore]
        [SwaggerSchema(ReadOnly = true)]
        public ICollection<StoricoPraticaDto>? StoricoPratiche { get; } = new List<StoricoPraticaDto>();
    }
}
