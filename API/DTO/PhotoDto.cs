namespace API.DTO
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public string? PublicId { get; set; }
        public bool IsApproved { get; set; }

        // Navigation properties        
        public Guid MemberId { get; set; }
    }
}
