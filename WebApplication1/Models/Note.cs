namespace SafeScribe.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
