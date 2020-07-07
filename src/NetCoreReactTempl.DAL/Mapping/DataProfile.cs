using AutoMapper;

namespace NetCoreReactTempl.DAL.Mapping
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<Entities.Data, Domain.Models.Data>().ReverseMap();
        }
    }
}
