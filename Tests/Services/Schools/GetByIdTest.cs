using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.Schools;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISchoolRepo> _schoolRepoMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SchoolService _schoolService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _schoolRepoMock = new();
        _seasonRepoMock = new();
        _schoolService = new(_seasonRepoMock.Object, _schoolRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var school = new School() { Id = id, Name = "Cairo school" };

        _schoolRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(school));

        //Act
        var result = await _schoolService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetSchoolDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _schoolRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<School>(null));

        //Act
        var result = await _schoolService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}