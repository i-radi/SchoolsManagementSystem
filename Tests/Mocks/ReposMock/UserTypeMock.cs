namespace Test;

public static class UserTypeMock
{
    public static Mock<IUserTypeRepo> Get()
    {
        var mock = new Mock<IUserTypeRepo>();

        // Setup the mock

        return mock;
    }
}
