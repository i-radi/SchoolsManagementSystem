namespace SMS.Models.Results;

public class JwtAuthResult
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiryDate { get; set; }
    public Guid RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDate { get; set; }
}
