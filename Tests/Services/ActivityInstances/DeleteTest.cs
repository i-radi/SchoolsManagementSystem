using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityInstances;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityInstanceRepo> _activityInstanceRepoMock;
    private readonly ActivityInstanceService _activityInstanceService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityInstanceRepoMock = new();
        _activityInstanceService = new(_activityInstanceRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var activityInstance = new ActivityInstance() { ActivityId = 1, Name = "instance1" };

        _activityInstanceRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityInstance));

        //Act
        var result = await _activityInstanceService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _activityInstanceRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _activityInstanceRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityInstance>(null));

        //Act
        var result = await _activityInstanceService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}