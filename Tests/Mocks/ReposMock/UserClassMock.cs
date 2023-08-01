namespace Test;

public static class UserClassMock
{
    public static Mock<IUserClassRepo> Get()
    {
        var mock = new Mock<IUserClassRepo>();

        // Setup the mock

        return mock;
    }
}
