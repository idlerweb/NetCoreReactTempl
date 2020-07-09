using AutoMapper;
using NetCoreReactTempl.DAL.Entities;
using System.Linq;

namespace NetCoreReactTempl.DAL.Mapping
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<Data, Domain.Models.Data>()
                .ForMember(dest => dest.Fields, opt => opt.MapFrom(src => src.Fields.ToDictionary(k => k.Name, v => v.Value)));

            CreateMap<Domain.Models.Data, Data>()
                .ForMember(dest => dest.Fields, opt => opt.MapFrom(src => src.Fields.Select(f => new Field { 
                    Name = f.Key,
                    Value = f.Value.ToString()
                })));
        }
    }
}
