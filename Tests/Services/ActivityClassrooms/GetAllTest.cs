using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityClassrooms;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityClassroomRepo> _activityClassroomRepoMock;
    private readonly ActivityClassroomService _activityClassroomService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityClassroomRepoMock = new();
        _activityClassroomService = new(_activityClassroomRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var activityClassroomList = new List<ActivityClassroom>()
        {
            new ActivityClassroom(){ ActivityId = 1, ClassroomId = 1 }
        };

        _activityClassroomRepoMock.Setup(x => x.GetTableNoTracking()).Returns(activityClassroomList.AsQueryable());

        //Act
        var result = _activityClassroomService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetActivityClassroomDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var activityClassroomList = new List<ActivityClassroom>();

        _activityClassroomRepoMock.Setup(x => x.GetTableNoTracking()).Returns(activityClassroomList.AsQueryable());

        //Act
        var result = _activityClassroomService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetActivityClassroomDto>>();
    }
}