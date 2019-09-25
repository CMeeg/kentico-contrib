using AutoMapper;
using KenticoContrib.Content;

namespace KenticoContrib.Features.Layout
{
    public class LayoutMappingProfile : Profile
    {
        public LayoutMappingProfile()
        {
            CreateMap<PageMetadata, PageMetadataViewModel>();
        }
    }
}