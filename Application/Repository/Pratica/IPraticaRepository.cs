using Application.DTOs.Pratica;

namespace Application.Repository.Pratica
{
    public interface IPraticaRepository
    {
        Task<PraticaDto?> CreatePratica(PraticaDto praticaDto);
        Task<bool> DeletePratica(int id);
        Task<PraticaDto?> GetPratica(int id);
        Task<IEnumerable<PraticaDto>?> GetPratiche();
        Task<PraticaDto?> UpdatePratica(PraticaDto praticaDto);
    }
}