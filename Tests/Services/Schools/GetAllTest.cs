using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Schools;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISchoolRepo> _schoolRepoMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SchoolService _schoolService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _schoolRepoMock = new();
        _seasonRepoMock = new();
        _schoolService = new(_seasonRepoMock.Object, _schoolRepoMock.Object, null, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var schoolList = new List<School>()
        {
            new School(){ Id = 1, Name = "Cairo school"}
        };

        _schoolRepoMock.Setup(x => x.GetTableNoTracking()).Returns(schoolList.AsQueryable());

        //Act
        var result = _schoolService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetSchoolDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var schoolList = new List<School>();

        _schoolRepoMock.Setup(x => x.GetTableNoTracking()).Returns(schoolList.AsQueryable());

        //Act
        var result = _schoolService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetSchoolDto>>();
    }
}