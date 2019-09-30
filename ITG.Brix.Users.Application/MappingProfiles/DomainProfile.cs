using AutoMapper;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using ITG.Brix.Users.Domain;

namespace ITG.Brix.Users.Application.MappingProfiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<User, UserModel>(MemberList.Destination)
                .ForMember(dest => dest.Login, m => m.MapFrom(src => src.Login.Value))
                .ForMember(dest => dest.FirstName, m => m.MapFrom(src => src.FullName.FirstName.Value))
                .ForMember(dest => dest.LastName, m => m.MapFrom(src => src.FullName.LastName.Value));
        }
    }
}
