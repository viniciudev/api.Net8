namespace Core.DTOs
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public required string Message { get; set; }
        public string? Token { get; set; }
        public UserResponse? User { get; set; }
    }
}
