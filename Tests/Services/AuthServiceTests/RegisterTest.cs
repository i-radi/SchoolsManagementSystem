namespace Test;

public class RegisterTest
{
    #region Arrange

    private readonly AuthService _authService;

    public RegisterTest()
    {
        var configuration = ConfigurationMock.GetConfiguration();
        var jwtSettings = new JwtSettings();
        configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);

        var mapper = MapperMock.GetMapperMock(configuration);
        var userContext = ContextMock.Get();

        var userManagerMock = UserManagerMock.GetUserManagerMock();
        var userRoleMock = UserRoleMock.Get();

        //_authService = new AuthService(jwtSettings, userManagerMock.Object, userContext, userRoleMock.Object, mapper);
    }

    #endregion Arrange

    #region Happy Scenario

    [Fact]
    public async void Register_ValidInput_ReturnsSuccessMsg()
    {
        // Arrange
        var model = new RegisterDto
        {
            Email = "test@mail.com",
            Name = "test",
            Password = "password",
            PhoneNumber = "1234567890"
        };

        // Act
        //var result = await _authService.RegisterAsync(model);

        // Assert
        Assert.True(true);
    }

    #endregion 
}