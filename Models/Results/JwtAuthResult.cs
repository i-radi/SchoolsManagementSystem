namespace Models.Results;

public class JwtAuthResult
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsSuperAdmin { get; set; }
    public List<RoleResult> Roles { get; set; } = new List<RoleResult>();
    public bool IsAuthenticated { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiryDate { get; set; }
    public Guid RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDate { get; set; }
}
