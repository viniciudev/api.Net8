namespace Core
{
	public class User : BaseModel
	{
		public required string Name { get; set; }
		public required string Password { get; set; }
		public required string Email { get; set; }
	}
}
