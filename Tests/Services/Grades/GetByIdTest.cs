using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.Grades;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IGradeRepo> _gradeRepoMock;
    private readonly GradeService _gradeService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _gradeRepoMock = new();
        _gradeService = new(_gradeRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var grade = new Grade() { Id = id, Name = "Cairo grade" };

        _gradeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(grade));

        //Act
        var result = await _gradeService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetGradeDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _gradeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Grade>(null));

        //Act
        var result = await _gradeService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}