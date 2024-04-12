using Swashbuckle.AspNetCore.Annotations;

namespace Application.DTOs.Pratica
{
    public class StatoPraticaDto
    {
        public required int IdStatoPratica { get; set; }

        public int? IdRisultatoPratica { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public DateTime? LastUpdate { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public ICollection<StoricoPraticaDto>? StoricoPratiche { get; } = new List<StoricoPraticaDto>();
    }
}
