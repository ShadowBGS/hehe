namespace asp2.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NumberOfPointOfInterest
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }
        public List<PointsOfInterestDto> PointsOfInterest { get; set; }
            = new List<PointsOfInterestDto>();
    

    }
}
