using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.ActivityTimes;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityTimeRepo> _activityTimeRepoMock;
    private readonly ActivityTimeService _activityTimeService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityTimeRepoMock = new();
        _activityTimeService = new(_activityTimeRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var activityTime = new ActivityTime() { ActivityId = 1, Day = "day1" };

        _activityTimeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityTime));

        //Act
        var result = await _activityTimeService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetActivityTimeDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _activityTimeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityTime>(null));

        //Act
        var result = await _activityTimeService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}