using AutoMapper;

namespace asp2.Profiles
{
    public class PointOfInterestProfile:Profile
    {
        public PointOfInterestProfile() 
        {
            CreateMap<Entities.PointOfInterest,Models.PointsOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
        }

    }
}
 