using FluentAssertions;

namespace Tests.Services.Records;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IRecordRepo> _recordRepoMock;
    private readonly RecordService _recordService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _recordRepoMock = new();
        _recordService = new(_recordRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var record = new Models.Entities.Record() { Id = id, Name = "Cairo Record" };
        _recordRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(record));

        var recordDto = new UpdateRecordDto() { Id = id, Name = "Alex Record" };

        //Act
        var result = await _recordService.Update(recordDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _recordRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _recordRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Models.Entities.Record>(null));

        var recordDto = new UpdateRecordDto() { Id = id, Name = "Alex Record" };

        //Act
        var result = await _recordService.Update(recordDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _recordRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}