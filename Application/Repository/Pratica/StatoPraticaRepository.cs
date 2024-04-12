using Application.DTOs.Pratica;
using AutoMapper;
using Domain.Models.Pratica;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.Pratica
{
    public class StatoPraticaRepository : IStatoPraticaRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public StatoPraticaRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<StatoPraticaDto?> GetStatoPratica(int id)
        {
            var pratica = await _dbContext.Pratiche.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            var statoPraticaDto = _mapper.Map<StatoPraticaDto>(pratica);

            return statoPraticaDto;
        }

        public async Task<PraticaDto?> UpdateStatoPratica(PraticaDto praticaDto)
        {
            var pratica = _mapper.Map<Domain.Models.Pratica.Pratica>(praticaDto);
            pratica.StoricoPratiche?.Add(new StoricoPratica
            {
                IdPratica = pratica.Id,
                IdStatoPratica = pratica.IdStatoPratica,
                IdRisultatoPratica = pratica.IdRisultatoPratica,
                LastUpdate = pratica.LastUpdate
            });
            _dbContext.Pratiche.Update(pratica);

            var result = await _dbContext.SaveChangesAsync();

            var newPraticaDto = _mapper.Map<PraticaDto>(pratica);

            return result >= 0 ? newPraticaDto : null;
        }
    }
}
