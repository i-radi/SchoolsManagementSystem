namespace Test.Services.ClassServiceTest;

public class AddTest
{
    #region Arrange

    private readonly ClassroomService _classesService;

    public AddTest()
    {
        var configuration = ConfigurationMock.GetConfiguration();
        var mapper = MapperMock.GetMapperMock(configuration);

        var repoMock = ClassMock.Get();

        _classesService = new ClassroomService(repoMock.Object, mapper);
    }

    #endregion Arrange

    #region Happy Scenario

    [Fact]
    public void AddClassroom_ValidInput_ReturnsSuccess()
    {
        // Arrange

        // Act

        // Assert
    }

    #endregion 
}
