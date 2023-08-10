namespace Models.Results;

public class JwtAuthResult
{
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();
    public bool IsAuthenticated { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiryDate { get; set; }
    public Guid RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDate { get; set; }
}
