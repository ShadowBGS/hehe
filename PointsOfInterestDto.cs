namespace asp2.Models
{
    public class PointsOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class PointsOfInterestContainerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int NumberOfPointsOfInterest
        {
            get => PointsOfInterest.Count;
        }

        public ICollection<PointsOfInterestDto> PointsOfInterest { get; set; } = new List<PointsOfInterestDto>();
    }
}
