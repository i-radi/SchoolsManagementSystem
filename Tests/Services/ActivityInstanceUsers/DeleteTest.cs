namespace Tests.Services.ActivityInstanceUsers;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityInstanceUserRepo> _activityInstanceUserRepoMock;
    private readonly ActivityInstanceUserService _activityInstanceUserService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityInstanceUserRepoMock = new();
        _activityInstanceUserService = new(_activityInstanceUserRepoMock.Object, _mapperMock);
    }

    //[Theory]
    //[InlineData(1)]
    //public async Task Delete_ValidItem_ReturnsSuccess(int userid,int activityintanceid)
    //{
    //    //Arrange
    //    var activityInstanceUser = new ActivityInstanceUser() { ActivityInstanceId = 1, UserId = 1 };

    //    _activityInstanceUserRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityInstanceUser));

    //    //Act
    //    var result = await _activityInstanceUserService.Delete(id);

    //    //Assert
    //    result.Succeeded.Should().BeTrue();
    //    result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    //    _activityInstanceUserRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    //}

    //[Theory]
    //[InlineData(5)]
    //public async Task Delete_InValidItem_ReturnsNotFound(int id)
    //{
    //    //Arrange
    //    _activityInstanceUserRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityInstanceUser>(null));

    //    //Act
    //    var result = await _activityInstanceUserService.Delete(id);

    //    //Assert
    //    result.Succeeded.Should().BeFalse();
    //    result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    //}
}