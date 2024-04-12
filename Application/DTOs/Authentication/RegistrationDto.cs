namespace Application.DTOs.Authentication
{
    public class RegistrationDto
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Password { get; set; }
        public string? Role { get; set; }
    }
}
