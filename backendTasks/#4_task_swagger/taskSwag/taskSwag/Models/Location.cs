namespace taskSwag.Models
{
    public class LocationDto
    {
        public string? City { get; set; }
        public string? Countery { get; set; }
    }
    public class Location : LocationDto
    {
        public int? Id { get; set; }
        public List<User>? Users { get; set; }
    }
}
