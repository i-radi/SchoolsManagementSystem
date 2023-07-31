namespace SMS.Tests;

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

        _authService = new AuthService(jwtSettings, userManagerMock.Object, userContext, mapper);
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
            UserName = "Test",
            Password = "password",
            PhoneNumber = "1234567890",
            Role = "SuperAdmin"
        };

        // Act
        var result = await _authService.RegisterAsync(model);

        // Assert
        Assert.Equal("Test created successfully", result.Data);
    }

    #endregion 
}