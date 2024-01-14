using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Courses;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ICourseRepo> _courseRepoMock;
    private readonly CourseService _courseService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _courseRepoMock = new();
        _courseService = new(_courseRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var course = new Course() { Id = id, Name = "Cairo course" };

        _courseRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(course));

        //Act
        var result = await _courseService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _courseRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _courseRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Course>(null));

        //Act
        var result = await _courseService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}