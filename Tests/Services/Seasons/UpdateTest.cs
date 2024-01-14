using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Seasons;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SeasonService _seasonService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _seasonRepoMock = new();
        _seasonService = new(_seasonRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var season = new Season() { Id = id, Name = "Cairo Season" };
        _seasonRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(season));

        var seasonDto = new UpdateSeasonDto() { Id = id, Name = "Alex Season" };

        //Act
        var result = await _seasonService.Update(seasonDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _seasonRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _seasonRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Season>(null));

        var seasonDto = new UpdateSeasonDto() { Id = id, Name = "Alex Season" };

        //Act
        var result = await _seasonService.Update(seasonDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _seasonRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}