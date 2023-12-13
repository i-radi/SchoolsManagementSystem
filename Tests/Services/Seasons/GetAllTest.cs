using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Seasons;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SeasonService _seasonService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _seasonRepoMock = new();
        _seasonService = new(_seasonRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var seasonList = new List<Season>()
        {
            new Season(){ Id = 1, Name = "Cairo season"}
        };

        _seasonRepoMock.Setup(x => x.GetTableNoTracking()).Returns(seasonList.AsQueryable());

        //Act
        var result = _seasonService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetSeasonDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var seasonList = new List<Season>();

        _seasonRepoMock.Setup(x => x.GetTableNoTracking()).Returns(seasonList.AsQueryable());

        //Act
        var result = _seasonService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetSeasonDto>>();
    }
}