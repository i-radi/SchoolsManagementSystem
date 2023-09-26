using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Grades;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IGradeRepo> _gradeRepoMock;
    private readonly GradeService _gradeService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _gradeRepoMock = new();
        _gradeService = new(_gradeRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var grade = new Grade() { Id = id, Name = "Cairo Grade" };
        _gradeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(grade));

        var gradeDto = new UpdateGradeDto() { Id = id, Name = "Alex Grade" };

        //Act
        var result = await _gradeService.Update(gradeDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _gradeRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _gradeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Grade>(null));

        var gradeDto = new UpdateGradeDto() { Id = id, Name = "Alex Grade" };

        //Act
        var result = await _gradeService.Update(gradeDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _gradeRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}