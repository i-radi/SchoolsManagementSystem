using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.Activities;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityRepo> _activityRepoMock;
    private readonly ActivityService _activityService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityRepoMock = new();
        _activityService = new(_activityRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var activity = new Activity() { Id = id, Name = "Cairo activity" };

        _activityRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activity));

        //Act
        var result = await _activityService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetActivityDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _activityRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Activity>(null));

        //Act
        var result = await _activityService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}