using AutoMapper;

namespace NetCoreReactTempl.Web.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Dto.Data, DAL.Entities.Data>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<DAL.Entities.Data, Dto.Data>();
        }
    }
}
