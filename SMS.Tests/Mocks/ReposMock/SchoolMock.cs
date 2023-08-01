namespace SMS.Tests;

public static class SchoolMock
{
    public static Mock<ISchoolRepo> Get()
    {
        var mock = new Mock<ISchoolRepo>();

        // Setup the mock

        return mock;
    }
}
