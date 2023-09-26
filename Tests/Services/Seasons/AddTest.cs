using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Seasons;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SeasonService _seasonService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _seasonRepoMock = new();
        _seasonService = new(_seasonRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var seasonDto = new AddSeasonDto() { Name = "Cairo season" };
        var season = _mapperMock.Map<Season>(seasonDto);

        _seasonRepoMock.Setup(x => x.AddAsync(season)).Returns(Task.FromResult(season));

        //Act
        var result = await _seasonService.Add(seasonDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetSeasonDto>();
        result.Data.Name.Should().BeEquivalentTo("Cairo season");
    }

}