using AutoMapper;
using KenticoContrib.Content.Home;

namespace KenticoContrib.Features.Home
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HomePage, HomeViewModel>();
        }
    }
}