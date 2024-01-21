using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Activities;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityRepo> _activityRepoMock;
    private readonly ActivityService _activityService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityRepoMock = new();
        _activityService = new(_activityRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var activity = new Activity() { Id = id, Name = "Cairo Activity" };
        _activityRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activity));

        var activityDto = new UpdateActivityDto() { Id = id, Name = "Alex Activity" };

        //Act
        var result = await _activityService.Update(activityDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _activityRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _activityRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Activity>(null));

        var activityDto = new UpdateActivityDto() { Id = id, Name = "Alex Activity" };

        //Act
        var result = await _activityService.Update(activityDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        _activityRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}