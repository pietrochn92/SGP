using Application.DTOs.Pratica;
using AutoMapper;
using Domain.Models.Pratica;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.Pratica
{
    public class StoricoPraticaRepository : IStoricoPraticaRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public StoricoPraticaRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StoricoPraticaDto>?> GetStoricoPraticaById(int idPratica)
        {
            var storicoPratiche = await _dbContext.StoricoPratiche.Include(x => x.Pratica).Where(x => x.IdPratica == idPratica).OrderByDescending(x => x.Id).ToListAsync();
            var storicoPraticheDto = _mapper.Map<IEnumerable<StoricoPraticaDto>>(storicoPratiche);

            return storicoPraticheDto;
        }
    }
}
