using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.ActivityClassrooms;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityClassroomRepo> _activityClassroomRepoMock;
    private readonly ActivityClassroomService _activityClassroomService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityClassroomRepoMock = new();
        _activityClassroomService = new(_activityClassroomRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var activityClassroom = new ActivityClassroom() { ActivityId = 1, ClassroomId = 1 };

        _activityClassroomRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityClassroom));

        //Act
        var result = await _activityClassroomService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetActivityClassroomDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _activityClassroomRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityClassroom>(null));

        //Act
        var result = await _activityClassroomService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}