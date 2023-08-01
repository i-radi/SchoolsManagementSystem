namespace Test;

public static class ClassMock
{
    public static Mock<IClassRoomRepo> Get()
    {
        var mock = new Mock<IClassRoomRepo>();

        // Setup the mock

        return mock;
    }
}
