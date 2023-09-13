namespace VModels.ViewModels;
public class UserImages
{
    public string ProfilePictureUrl { get; set; }
    public string QRCodeUrl { get; set; }
    public bool IsProfilePictureEmpty => ProfilePictureUrl.Contains("emptyAvatar.png");
}

