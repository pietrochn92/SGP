using Application.DTOs.Pratica;

namespace Application.Repository.Pratica
{
    public interface IStatoPraticaRepository
    {
        //Task<StatoPraticaDto?> GetStatoPratica(int id);
        Task<PraticaDto?> UpdateStatoPratica(PraticaDto praticaDto);
    }
}