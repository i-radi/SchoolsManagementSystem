using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Courses;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ICourseRepo> _courseRepoMock;
    private readonly CourseService _courseService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _courseRepoMock = new();
        _courseService = new(_courseRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var courseDto = new AddCourseDto() { Name = "Cairo course" };
        var course = _mapperMock.Map<Course>(courseDto);

        _courseRepoMock.Setup(x => x.AddAsync(course)).Returns(Task.FromResult(course));

        //Act
        var result = await _courseService.Add(courseDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetCourseDto>();
        result.Data.Name.Should().BeEquivalentTo("Cairo course");
    }

}