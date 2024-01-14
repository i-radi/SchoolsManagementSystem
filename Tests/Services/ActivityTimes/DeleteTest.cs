using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityTimes;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityTimeRepo> _activityTimeRepoMock;
    private readonly ActivityTimeService _activityTimeService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityTimeRepoMock = new();
        _activityTimeService = new(_activityTimeRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var activityTime = new ActivityTime() { ActivityId = 1, Day = "day1" };

        _activityTimeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityTime));

        //Act
        var result = await _activityTimeService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _activityTimeRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _activityTimeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityTime>(null));

        //Act
        var result = await _activityTimeService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}