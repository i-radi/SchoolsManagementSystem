using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.Records;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IRecordRepo> _recordRepoMock;
    private readonly RecordService _recordService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _recordRepoMock = new();
        _recordService = new(_recordRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var record = new Models.Entities.Record() { Id = id, Name = "Cairo record" };

        _recordRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(record));

        //Act
        var result = await _recordService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetRecordDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _recordRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Models.Entities.Record>(null));

        //Act
        var result = await _recordService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}