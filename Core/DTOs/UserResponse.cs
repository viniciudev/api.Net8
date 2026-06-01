namespace Core.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
