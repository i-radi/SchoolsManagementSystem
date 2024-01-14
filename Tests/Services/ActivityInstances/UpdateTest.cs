using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityInstances;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityInstanceRepo> _activityInstanceRepoMock;
    private readonly ActivityInstanceService _activityInstanceService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityInstanceRepoMock = new();
        _activityInstanceService = new(_activityInstanceRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var activityInstance = new ActivityInstance() { ActivityId = 1, Name = "instance1" };
        _activityInstanceRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityInstance));

        var activityInstanceDto = new UpdateActivityInstanceDto() { Id = 1, ActivityId = 1, Name = "instance2" };

        //Act
        var result = await _activityInstanceService.Update(activityInstanceDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _activityInstanceRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _activityInstanceRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityInstance>(null));

        var activityInstanceDto = new UpdateActivityInstanceDto() { ActivityId = 1, Name = "instance2" };

        //Act
        var result = await _activityInstanceService.Update(activityInstanceDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _activityInstanceRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}