using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Classrooms;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IClassroomRepo> _classroomRepoMock;
    private readonly ClassroomService _classroomService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _classroomRepoMock = new();
        _classroomService = new(_classroomRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var classroomList = new List<Classroom>()
        {
            new Classroom(){ Id = 1, Name = "Cairo classroom"}
        };

        _classroomRepoMock.Setup(x => x.GetTableNoTracking()).Returns(classroomList.AsQueryable());

        //Act
        var result = _classroomService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetClassroomDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var classroomList = new List<Classroom>();

        _classroomRepoMock.Setup(x => x.GetTableNoTracking()).Returns(classroomList.AsQueryable());

        //Act
        var result = _classroomService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetClassroomDto>>();
    }
}