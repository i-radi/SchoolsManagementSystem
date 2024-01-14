using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Courses;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ICourseRepo> _courseRepoMock;
    private readonly CourseService _courseService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _courseRepoMock = new();
        _courseService = new(_courseRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var courseList = new List<Course>()
        {
            new Course(){ Id = 1, Name = "Cairo course"}
        };

        _courseRepoMock.Setup(x => x.GetTableNoTracking()).Returns(courseList.AsQueryable());

        //Act
        var result = _courseService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetCourseDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var courseList = new List<Course>();

        _courseRepoMock.Setup(x => x.GetTableNoTracking()).Returns(courseList.AsQueryable());

        //Act
        var result = _courseService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetCourseDto>>();
    }
}