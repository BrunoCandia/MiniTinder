namespace API.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string DisplayName { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
        public string? ImageUrl { get; set; }
    }
}
