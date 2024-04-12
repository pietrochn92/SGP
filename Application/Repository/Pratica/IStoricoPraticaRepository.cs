using Application.DTOs.Pratica;

namespace Application.Repository.Pratica
{
    public interface IStoricoPraticaRepository
    {
        Task<IEnumerable<StoricoPraticaDto>?> GetStoricoPraticaById(int idPratica);
    }
}