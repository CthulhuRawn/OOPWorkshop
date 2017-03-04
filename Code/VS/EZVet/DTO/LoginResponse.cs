namespace DTO
{
    public class LoginResponse
    {
        public int UserId { get; set; }
		
        public string Role { get; set; }

        public string AuthorizationKey { get; set; }
    }
}