using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Schools;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISchoolRepo> _schoolRepoMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SchoolService _schoolService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _schoolRepoMock = new();
        _seasonRepoMock = new();
        _schoolService = new(_seasonRepoMock.Object, _schoolRepoMock.Object, null, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var school = new School() { Id = id, Name = "Cairo School" };
        _schoolRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(school));

        var schoolDto = new UpdateSchoolDto() { Id = id, Name = "Alex School" };

        //Act
        var result = await _schoolService.Update(schoolDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _schoolRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _schoolRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<School>(null));

        var schoolDto = new UpdateSchoolDto() { Id = id, Name = "Alex School" };

        //Act
        var result = await _schoolService.Update(schoolDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _schoolRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}