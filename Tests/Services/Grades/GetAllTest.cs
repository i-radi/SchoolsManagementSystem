using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Grades;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IGradeRepo> _gradeRepoMock;
    private readonly GradeService _gradeService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _gradeRepoMock = new();
        _gradeService = new(_gradeRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var gradeList = new List<Grade>()
        {
            new Grade(){ Id = 1, Name = "Cairo grade"}
        };

        _gradeRepoMock.Setup(x => x.GetTableNoTracking()).Returns(gradeList.AsQueryable());

        //Act
        var result = _gradeService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetGradeDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var gradeList = new List<Grade>();

        _gradeRepoMock.Setup(x => x.GetTableNoTracking()).Returns(gradeList.AsQueryable());

        //Act
        var result = _gradeService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetGradeDto>>();
    }
}