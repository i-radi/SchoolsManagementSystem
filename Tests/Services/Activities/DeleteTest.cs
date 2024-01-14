using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Activities;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityRepo> _activityRepoMock;
    private readonly ActivityService _activityService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityRepoMock = new();
        _activityService = new(_activityRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var activity = new Activity() { Id = id, Name = "Cairo activity" };

        _activityRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activity));

        //Act
        var result = await _activityService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _activityRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _activityRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Activity>(null));

        //Act
        var result = await _activityService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}