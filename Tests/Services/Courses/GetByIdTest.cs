using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.Courses;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ICourseRepo> _courseRepoMock;
    private readonly CourseService _courseService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _courseRepoMock = new();
        _courseService = new(_courseRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var course = new Course() { Id = id, Name = "Cairo course" };

        _courseRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(course));

        //Act
        var result = await _courseService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetCourseDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _courseRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Course>(null));

        //Act
        var result = await _courseService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}