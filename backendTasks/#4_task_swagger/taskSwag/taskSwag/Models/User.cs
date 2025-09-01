namespace taskSwag.Models
{
    public class UserDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public int? LocationId { get; set; } 
    }
    public class User: UserDto
    {
        public int? Id { get; set; }
        public Location? Location { get; set; } 
    }
}
