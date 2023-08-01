namespace Test.Services.ClassServiceTest;

public class AddTest
{
    #region Arrange

    private readonly ClassRoomService _classesService;

    public AddTest()
    {
        var configuration = ConfigurationMock.GetConfiguration();
        var mapper = MapperMock.GetMapperMock(configuration);

        var repoMock = ClassMock.Get();

        _classesService = new ClassRoomService(repoMock.Object, mapper);
    }

    #endregion Arrange

    #region Happy Scenario

    [Fact]
    public void AddClassRoom_ValidInput_ReturnsSuccess()
    {
        // Arrange

        // Act

        // Assert
    }

    #endregion 
}
