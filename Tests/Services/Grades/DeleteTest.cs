using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Grades;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IGradeRepo> _gradeRepoMock;
    private readonly GradeService _gradeService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _gradeRepoMock = new();
        _gradeService = new(_gradeRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var grade = new Grade() { Id = id, Name = "Cairo grade" };

        _gradeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(grade));

        //Act
        var result = await _gradeService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _gradeRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _gradeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Grade>(null));

        //Act
        var result = await _gradeService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}