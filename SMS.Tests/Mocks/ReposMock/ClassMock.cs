namespace SMS.Tests;

public static class ClassMock
{
    public static Mock<IClassesRepo> Get()
    {
        var mock = new Mock<IClassesRepo>();

        // Setup the mock

        return mock;
    }
}
