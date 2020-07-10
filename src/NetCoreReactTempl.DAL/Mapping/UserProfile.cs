using AutoMapper;

namespace NetCoreReactTempl.DAL.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.User, Domain.Models.User>().ReverseMap();
        }
    }
}
