namespace SMS.VModels.DTOS.Auth;

public class RefreshTokenInputDto
{
    public string AccessToken { get; set; } = string.Empty;
    public Guid RefreshToken { get; set; }
}
