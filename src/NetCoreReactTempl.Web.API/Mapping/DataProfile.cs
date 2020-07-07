using AutoMapper;

namespace NetCoreReactTempl.Web.API.Mapping
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<Dto.Data, Domain.Models.Data>().ReverseMap();
        }
    }
}
