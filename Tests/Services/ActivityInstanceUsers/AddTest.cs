using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityInstanceUsers;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityInstanceUserRepo> _activityInstanceUserRepoMock;
    private readonly ActivityInstanceUserService _activityInstanceUserService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityInstanceUserRepoMock = new();
        _activityInstanceUserService = new(_activityInstanceUserRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var activityInstanceUserDto = new AddActivityInstanceUserDto() { ActivityInstanceId = 1, UserId = 1 };
        var activityInstanceUser = _mapperMock.Map<ActivityInstanceUser>(activityInstanceUserDto);

        _activityInstanceUserRepoMock.Setup(x => x.AddAsync(activityInstanceUser)).Returns(Task.FromResult(activityInstanceUser));

        //Act
        var result = await _activityInstanceUserService.Add(activityInstanceUserDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetActivityInstanceUserDto>();
    }

}