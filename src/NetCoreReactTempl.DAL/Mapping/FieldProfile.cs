using AutoMapper;

namespace NetCoreReactTempl.DAL.Mapping
{
    public class FieldProfile : Profile
    {
        public FieldProfile()
        {
            CreateMap<Entities.Field, Domain.Models.Field>().ReverseMap();
        }
    }
}
