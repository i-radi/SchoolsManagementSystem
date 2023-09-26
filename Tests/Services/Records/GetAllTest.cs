using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Records;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IRecordRepo> _recordRepoMock;
    private readonly RecordService _recordService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _recordRepoMock = new();
        _recordService = new(_recordRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var recordList = new List<Models.Entities.Record>()
        {
            new Models.Entities.Record(){ Id = 1, Name = "Cairo record",Available = true}
        };

        _recordRepoMock.Setup(x => x.GetTableNoTracking()).Returns(recordList.AsQueryable());

        //Act
        var result = _recordService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetRecordDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var recordList = new List<Models.Entities.Record>();

        _recordRepoMock.Setup(x => x.GetTableNoTracking()).Returns(recordList.AsQueryable());

        //Act
        var result = _recordService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetRecordDto>>();
    }
}