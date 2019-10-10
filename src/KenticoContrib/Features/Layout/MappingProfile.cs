using AutoMapper;

namespace KenticoContrib.Features.Layout
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Content.Metadata.PageMetadata, PageMetadataViewModel>();
        }
    }
}