using AutoMapper;

namespace NetCoreReactTempl.Web.API.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Dto.AuthInfo, Domain.Models.AuthInfo>().ReverseMap();
        }
    }
}
