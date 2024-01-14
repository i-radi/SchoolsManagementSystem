using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.Seasons;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SeasonService _seasonService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _seasonRepoMock = new();
        _seasonService = new(_seasonRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var season = new Season() { Id = id, Name = "Cairo season" };

        _seasonRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(season));

        //Act
        var result = await _seasonService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetSeasonDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _seasonRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Season>(null));

        //Act
        var result = await _seasonService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}