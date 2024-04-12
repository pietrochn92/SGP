using Application.DTOs.Pratica;
using AutoMapper;
using Domain.Models.Pratica;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.Pratica
{
    public class PraticaRepository : IPraticaRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public PraticaRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PraticaDto>?> GetPratiche()
        {
            var pratiche = await _dbContext.Pratiche.AsNoTracking().OrderBy(x => x.Id).ToListAsync();
            var praticheDto = _mapper.Map<IEnumerable<PraticaDto>>(pratiche);

            return praticheDto;
        }

        public async Task<PraticaDto?> GetPratica(int id)
        {
            var pratica = await _dbContext.Pratiche.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            var praticaDto = _mapper.Map<PraticaDto>(pratica);

            return praticaDto;
        }

        public async Task<PraticaDto?> CreatePratica(PraticaDto praticaDto)
        {
            var pratica = _mapper.Map<Domain.Models.Pratica.Pratica>(praticaDto);
            pratica.StoricoPratiche?.Add(new StoricoPratica
            {
                IdPratica = pratica.Id,
                IdStatoPratica = pratica.IdStatoPratica,
                IdRisultatoPratica = pratica.IdRisultatoPratica,
                LastUpdate = pratica.LastUpdate
            });
            _dbContext.Pratiche.Add(pratica);

            var result = await _dbContext.SaveChangesAsync();

            var newPraticaDto = _mapper.Map<PraticaDto>(pratica);

            return result >= 0 ? newPraticaDto : null;
        }

        public async Task<PraticaDto?> UpdatePratica(PraticaDto praticaDto)
        {
            var pratica = _mapper.Map<Domain.Models.Pratica.Pratica>(praticaDto);
            _dbContext.Pratiche.Update(pratica);

            var result = await _dbContext.SaveChangesAsync();

            var newPraticaDto = _mapper.Map<PraticaDto>(pratica);

            return result >= 0 ? newPraticaDto : null;
        }

        public async Task<bool> DeletePratica(int id)
        {
            var pratica = await _dbContext.Pratiche.FirstOrDefaultAsync(x => x.Id == id);
            if (pratica != null)
            {
                _dbContext.Pratiche.Remove(pratica);
                var result = await _dbContext.SaveChangesAsync();

                return result >= 0;
            }

            return false;
        }
    }
}
