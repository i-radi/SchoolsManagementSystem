using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Records;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IRecordRepo> _recordRepoMock;
    private readonly RecordService _recordService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _recordRepoMock = new();
        _recordService = new(_recordRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var recordDto = new AddRecordDto() { Name = "Cairo record" };
        var record = _mapperMock.Map<Models.Entities.Record>(recordDto);

        _recordRepoMock.Setup(x => x.AddAsync(record)).Returns(Task.FromResult(record));

        //Act
        var result = await _recordService.Add(recordDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetRecordDto>();
        result.Data.Name.Should().BeEquivalentTo("Cairo record");
    }

}