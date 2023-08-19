namespace Test;

public static class ClassMock
{
    public static Mock<IClassroomRepo> Get()
    {
        var mock = new Mock<IClassroomRepo>();

        // Setup the mock

        return mock;
    }
}
