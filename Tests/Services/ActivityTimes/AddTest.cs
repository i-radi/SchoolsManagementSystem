using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityTimes;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityTimeRepo> _activityTimeRepoMock;
    private readonly ActivityTimeService _activityTimeService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityTimeRepoMock = new();
        _activityTimeService = new(_activityTimeRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var activityTimeDto = new AddActivityTimeDto() { ActivityId = 1, Day = "day1" };
        var activityTime = _mapperMock.Map<ActivityTime>(activityTimeDto);

        _activityTimeRepoMock.Setup(x => x.AddAsync(activityTime)).Returns(Task.FromResult(activityTime));

        //Act
        var result = await _activityTimeService.Add(activityTimeDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetActivityTimeDto>();
    }

}