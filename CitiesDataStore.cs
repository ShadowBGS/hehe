using asp2.Models;

namespace asp2

{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            // init dummy data
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Yaba",
                    Description = "The one with that big park.",
                    PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto()
                        {
                            Id = 1,
                              Name = "Unilag",
                            Description = "The most visited park in the United States."  },
                        new PointsOfInterestDto()
                        {
                            Id= 2,
                            Name = "Empire State Building",
                            Description = "A 102-story skyscrapper located in Midtown Manhattan."  },
                    }

                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the catherdral that was never really finished.",
                     PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto()
                        {
                            Id = 3,
                            Name = "Cathedral of Our Lady",
                            Description = "A Gothic Style Cathedral, conceived by architects Jan and Piete"  },
                        new PointsOfInterestDto()
                        {
                            Id= 4,
                            Name = "Antwerp Central Station",
                            Description = "The Finest exampe of railway architecture in Belgium."  },
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower.",
                     PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto()
                        {
                            Id = 5,
                            Name = "Eiffel Tower",
                            Description = "A wrought iron lattice tower onthe Champ de Mars, named after"  },
                        new PointsOfInterestDto()
                        {
                            Id= 6,
                            Name = "The Louvre",
                            Description = "The world's largest museum."  },
                    }
                },

              };
        }

    }
}
