using Application.DTOs.Pratica;
using AutoMapper;
using Domain.Models.Pratica;

namespace Application.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<StoricoPratica, StoricoPraticaDto>()
               .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => src.LastUpdate.Equals(null) ? DateTime.Now : src.LastUpdate))
               .ForMember(dest => dest.Pratica, opt => opt.MapFrom(src => src.Pratica))
               .ForAllMembers(opts =>
               {
                   opts.AllowNull();
                   opts.Condition((src, dest, srcMember) => srcMember != null);
               });

            CreateMap<StoricoPraticaDto, StoricoPratica>()
                .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => src.LastUpdate.Equals(null) ? DateTime.Now : src.LastUpdate))
                .ForMember(dest => dest.Pratica, opt => opt.MapFrom(src => src.Pratica))
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<Pratica, StatoPraticaDto>()
                .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => src.LastUpdate.Equals(null) ? DateTime.Now : src.LastUpdate))
                .ForMember(dest => dest.StoricoPratiche, opt => opt.MapFrom(src => src.StoricoPratiche))
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<StatoPraticaDto, Pratica>()
                .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => src.LastUpdate.Equals(null) ? DateTime.Now : src.LastUpdate))
                .ForMember(dest => dest.StoricoPratiche, opt => opt.MapFrom(src => src.StoricoPratiche))
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<Pratica, PraticaDto>()
                .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => src.LastUpdate.Equals(null) ? DateTime.Now : src.LastUpdate))
                .ForMember(dest => dest.StoricoPratiche, opt => opt.MapFrom(src => src.StoricoPratiche))
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<PraticaDto, Pratica>()
                .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => src.LastUpdate.Equals(null) ? DateTime.Now : src.LastUpdate))
                .ForMember(dest => dest.StoricoPratiche, opt => opt.MapFrom(src => src.StoricoPratiche))
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });
        }
    }
}
