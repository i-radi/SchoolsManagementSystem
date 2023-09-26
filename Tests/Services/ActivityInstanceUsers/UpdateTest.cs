using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityInstanceUsers;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityInstanceUserRepo> _activityInstanceUserRepoMock;
    private readonly ActivityInstanceUserService _activityInstanceUserService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityInstanceUserRepoMock = new();
        _activityInstanceUserService = new(_activityInstanceUserRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var activityInstanceUser = new ActivityInstanceUser() { ActivityInstanceId = 1, UserId = 1 };
        _activityInstanceUserRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityInstanceUser));

        var activityInstanceUserDto = new UpdateActivityInstanceUserDto() { Id = 1, ActivityInstanceId = 1, UserId = 2 };

        //Act
        var result = await _activityInstanceUserService.Update(activityInstanceUserDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _activityInstanceUserRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _activityInstanceUserRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityInstanceUser>(null));

        var activityInstanceUserDto = new UpdateActivityInstanceUserDto() { ActivityInstanceId = 1, UserId = 2 };

        //Act
        var result = await _activityInstanceUserService.Update(activityInstanceUserDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _activityInstanceUserRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}