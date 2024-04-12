namespace Application.DTOs.Authentication
{
    public class UserDto
    {
        public string Id { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
