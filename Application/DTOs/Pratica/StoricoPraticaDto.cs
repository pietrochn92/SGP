using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs.Pratica
{
    public class StoricoPraticaDto
    {
        [Key]
        public int Id { get; set; }

        public required int IdPratica { get; set; }

        public required int IdStatoPratica { get; set; }

        public int? IdRisultatoPratica { get; set; }

        public DateTime? LastUpdate { get; set; }

        public PraticaDto Pratica { get; set; } = null!;
    }
}
