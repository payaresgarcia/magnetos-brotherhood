namespace MagnetosBrotherhood.Api.Mappers
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.DnaDto, Domain.Models.DnaInfo>();
            CreateMap<Domain.Models.DnaEntryDo, DAL.Models.DnaEntry>()
                .ReverseMap();
            CreateMap<Domain.Models.DnaStatisticsDo, Models.DnaStatistics>();
        }
    }
}
