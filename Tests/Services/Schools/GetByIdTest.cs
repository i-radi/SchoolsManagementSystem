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
        _schoolService = new(_seasonRepoMock.Object, _schoolRepoMock.Object, null, _mapperMock);
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