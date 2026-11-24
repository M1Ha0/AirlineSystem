namespace AirlineClient.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Диспетчер";
    }

    public class AuthResponse
    {
        public string access_token { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string fullName { get; set; } = string.Empty;
    }

    public static class SessionState
    {
        public static string AccessToken { get; set; } = string.Empty;
        public static string UserEmail { get; set; } = string.Empty;
        public static string UserRole { get; set; } = string.Empty;
        public static string UserFullName { get; set; } = string.Empty;
        public static string ApiBaseUrl { get; set; } = "http://localhost:5050";
    }
}
