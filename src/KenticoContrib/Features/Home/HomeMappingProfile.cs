using AutoMapper;
using KenticoContrib.Content.Home;

namespace KenticoContrib.Features.Home
{
    public class HomeMappingProfile : Profile
    {
        public HomeMappingProfile()
        {
            CreateMap<HomePage, HomeViewModel>();
        }
    }
}