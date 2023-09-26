using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Courses;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ICourseRepo> _courseRepoMock;
    private readonly CourseService _courseService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _courseRepoMock = new();
        _courseService = new(_courseRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var course = new Course() { Id = id, Name = "Cairo Course" };
        _courseRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(course));

        var courseDto = new UpdateCourseDto() { Id = id, Name = "Alex Course" };

        //Act
        var result = await _courseService.Update(courseDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _courseRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _courseRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Course>(null));

        var courseDto = new UpdateCourseDto() { Id = id, Name = "Alex Course" };

        //Act
        var result = await _courseService.Update(courseDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _courseRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}