using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Activities;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityRepo> _activityRepoMock;
    private readonly ActivityService _activityService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityRepoMock = new();
        _activityService = new(_activityRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var activityList = new List<Activity>()
        {
            new Activity(){ Id = 1, Name = "Cairo activity"}
        };

        _activityRepoMock.Setup(x => x.GetTableNoTracking()).Returns(activityList.AsQueryable());

        //Act
        var result = _activityService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetActivityDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var activityList = new List<Activity>();

        _activityRepoMock.Setup(x => x.GetTableNoTracking()).Returns(activityList.AsQueryable());

        //Act
        var result = _activityService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetActivityDto>>();
    }
}