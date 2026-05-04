namespace API.DTO
{
    public class PhotoResponseDto
    {
        public int Id { get; set; }
        public Guid MemberId { get; set; }
        public required string Url { get; set; }
        public required string FileName { get; set; }   // This is the public ID used for deletion in Azure Blob Storage
    }
}
