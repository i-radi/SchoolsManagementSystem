using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Records;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IRecordRepo> _recordRepoMock;
    private readonly RecordService _recordService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _recordRepoMock = new();
        _recordService = new(_recordRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var record = new Models.Entities.Record() { Id = id, Name = "Cairo record" };

        _recordRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(record));

        //Act
        var result = await _recordService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _recordRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _recordRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Models.Entities.Record>(null));

        //Act
        var result = await _recordService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}