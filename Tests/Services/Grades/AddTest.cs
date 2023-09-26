using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Grades;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IGradeRepo> _gradeRepoMock;
    private readonly GradeService _gradeService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _gradeRepoMock = new();
        _gradeService = new(_gradeRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var gradeDto = new AddGradeDto() { Name = "Cairo grade" };
        var grade = _mapperMock.Map<Grade>(gradeDto);

        _gradeRepoMock.Setup(x => x.AddAsync(grade)).Returns(Task.FromResult(grade));

        //Act
        var result = await _gradeService.Add(gradeDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetGradeDto>();
        result.Data.Name.Should().BeEquivalentTo("Cairo grade");
    }

}