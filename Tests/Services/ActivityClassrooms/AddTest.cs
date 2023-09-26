using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityClassrooms;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityClassroomRepo> _activityClassroomRepoMock;
    private readonly ActivityClassroomService _activityClassroomService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityClassroomRepoMock = new();
        _activityClassroomService = new(_activityClassroomRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var activityClassroomDto = new AddActivityClassroomDto() { ActivityId = 1, ClassroomId = 1 };
        var activityClassroom = _mapperMock.Map<ActivityClassroom>(activityClassroomDto);

        _activityClassroomRepoMock.Setup(x => x.AddAsync(activityClassroom)).Returns(Task.FromResult(activityClassroom));

        //Act
        var result = await _activityClassroomService.Add(activityClassroomDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetActivityClassroomDto>();
    }

}