using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.ActivityInstanceUsers;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityInstanceUserRepo> _activityInstanceUserRepoMock;
    private readonly ActivityInstanceUserService _activityInstanceUserService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityInstanceUserRepoMock = new();
        _activityInstanceUserService = new(_activityInstanceUserRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var activityInstanceUser = new ActivityInstanceUser() { ActivityInstanceId = 1, UserId = 1 };

        _activityInstanceUserRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityInstanceUser));

        //Act
        var result = await _activityInstanceUserService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetActivityInstanceUserDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _activityInstanceUserRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityInstanceUser>(null));

        //Act
        var result = await _activityInstanceUserService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}