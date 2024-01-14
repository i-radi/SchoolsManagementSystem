using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityTimes;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityTimeRepo> _activityTimeRepoMock;
    private readonly ActivityTimeService _activityTimeService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityTimeRepoMock = new();
        _activityTimeService = new(_activityTimeRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var activityTime = new ActivityTime() { ActivityId = 1, Day = "day1" };
        _activityTimeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityTime));

        var activityTimeDto = new UpdateActivityTimeDto() { Id = 1, ActivityId = 1, Day = "day2" };

        //Act
        var result = await _activityTimeService.Update(activityTimeDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _activityTimeRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _activityTimeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityTime>(null));

        var activityTimeDto = new UpdateActivityTimeDto() { ActivityId = 1, Day = "day2" };

        //Act
        var result = await _activityTimeService.Update(activityTimeDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _activityTimeRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}