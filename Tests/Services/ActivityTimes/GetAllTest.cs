using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityTimes;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityTimeRepo> _activityTimeRepoMock;
    private readonly ActivityTimeService _activityTimeService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityTimeRepoMock = new();
        _activityTimeService = new(_activityTimeRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var activityTimeList = new List<ActivityTime>()
        {
            new ActivityTime(){ ActivityId = 1,Day = "day1" }
        };

        _activityTimeRepoMock.Setup(x => x.GetTableNoTracking()).Returns(activityTimeList.AsQueryable());

        //Act
        var result = _activityTimeService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetActivityTimeDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var activityTimeList = new List<ActivityTime>();

        _activityTimeRepoMock.Setup(x => x.GetTableNoTracking()).Returns(activityTimeList.AsQueryable());

        //Act
        var result = _activityTimeService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetActivityTimeDto>>();
    }
}