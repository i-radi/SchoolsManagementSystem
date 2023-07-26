namespace SMS.Models.Results;

public class JwtAuthResult
{
    public string Message { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public RefreshToken refreshToken { get; set; } = null!;
}
public class RefreshToken
{
    public string UserName { get; set; } = string.Empty;
    public string TokenString { get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; }
}
