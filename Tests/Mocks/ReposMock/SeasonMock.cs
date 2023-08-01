namespace Test;

public static class SeasonMock
{
    public static Mock<ISeasonRepo> Get()
    {
        var mock = new Mock<ISeasonRepo>();

        // Setup the mock

        return mock;
    }
}
