using AutoMapper;

namespace asp2.Profiles
{
    public class Cityprofile:Profile
    {
        public Cityprofile() 
        {
            CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
            CreateMap<Entities.City, Models.CityDto>();
        }    
    }
}
