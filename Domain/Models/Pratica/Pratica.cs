using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Pratica
{
    public class Pratica
    {
        [Key]
        public int Id { get; set; }

        public required int IdStatoPratica { get; set; }

        public int? IdRisultatoPratica { get; set; }

        public required string NomeUtente { get; set; }

        public required string CognomeUtente { get; set; }

        public required string CodiceFiscaleUtente { get; set; }

        public required DateTime DataNascitaUtente { get; set; }

        public string? AllegatoName { get; set; }

        public DateTime? LastUpdate { get; set; }

        public ICollection<StoricoPratica>? StoricoPratiche { get; } = new List<StoricoPratica>();
    }
}
