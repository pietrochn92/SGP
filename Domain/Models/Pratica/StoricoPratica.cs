using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Pratica
{
    public class StoricoPratica
    {
        [Key]
        public int Id { get; set; }

        public required int IdPratica { get; set; }

        public required int IdStatoPratica { get; set; }

        public int? IdRisultatoPratica { get; set; }

        public DateTime? LastUpdate { get; set; }

        public Pratica Pratica { get; set; } = null!;
    }
}
