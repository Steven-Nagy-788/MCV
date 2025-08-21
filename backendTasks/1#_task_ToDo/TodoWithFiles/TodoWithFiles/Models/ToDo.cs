namespace TodoWithFiles.Models
{
    public record ToDo
    {
        public int Id { get; set; } 
        public string? Note { get; set; }
    }
}
