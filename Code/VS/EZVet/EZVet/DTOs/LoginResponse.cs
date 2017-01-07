namespace EZVet.DTOs
{
    public class LoginResponse
    {
        public int UserId { get; set; }
		
		public bool IsUserFrozen { get; set; }

        public string Role { get; set; }

        public string AuthorizationKey { get; set; }
    }
}