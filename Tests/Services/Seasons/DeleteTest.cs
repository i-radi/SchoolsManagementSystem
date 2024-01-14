using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Seasons;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SeasonService _seasonService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _seasonRepoMock = new();
        _seasonService = new(_seasonRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var season = new Season() { Id = id, Name = "Cairo season" };

        _seasonRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(season));

        //Act
        var result = await _seasonService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _seasonRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _seasonRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Season>(null));

        //Act
        var result = await _seasonService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}