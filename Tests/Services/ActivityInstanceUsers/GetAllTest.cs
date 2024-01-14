using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityInstanceUsers;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityInstanceUserRepo> _activityInstanceUserRepoMock;
    private readonly ActivityInstanceUserService _activityInstanceUserService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityInstanceUserRepoMock = new();
        _activityInstanceUserService = new(_activityInstanceUserRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var activityInstanceUserList = new List<ActivityInstanceUser>()
        {
            new ActivityInstanceUser(){ ActivityInstanceId = 1, UserId = 1 }
        };

        _activityInstanceUserRepoMock.Setup(x => x.GetTableNoTracking()).Returns(activityInstanceUserList.AsQueryable());

        //Act
        var result = _activityInstanceUserService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetActivityInstanceUserDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var activityInstanceUserList = new List<ActivityInstanceUser>();

        _activityInstanceUserRepoMock.Setup(x => x.GetTableNoTracking()).Returns(activityInstanceUserList.AsQueryable());

        //Act
        var result = _activityInstanceUserService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetActivityInstanceUserDto>>();
    }
}