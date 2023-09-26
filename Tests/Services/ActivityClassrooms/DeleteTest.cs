using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityClassrooms;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityClassroomRepo> _activityClassroomRepoMock;
    private readonly ActivityClassroomService _activityClassroomService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityClassroomRepoMock = new();
        _activityClassroomService = new(_activityClassroomRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var activityClassroom = new ActivityClassroom() { ActivityId = 1, ClassroomId = 1 };

        _activityClassroomRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityClassroom));

        //Act
        var result = await _activityClassroomService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _activityClassroomRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _activityClassroomRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityClassroom>(null));

        //Act
        var result = await _activityClassroomService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}