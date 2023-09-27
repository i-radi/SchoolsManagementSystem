using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Schools;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISchoolRepo> _schoolRepoMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SchoolService _schoolService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _schoolRepoMock = new();
        _seasonRepoMock = new();
        _schoolService = new(_seasonRepoMock.Object, _schoolRepoMock.Object, null, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var school = new School() { Id = id, Name = "Cairo school" };

        _schoolRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(school));

        //Act
        var result = await _schoolService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _schoolRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _schoolRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<School>(null));

        //Act
        var result = await _schoolService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}