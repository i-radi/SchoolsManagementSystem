using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.RecordClasses;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IRecordClassRepo> _recordClassRepoMock;
    private readonly RecordClassService _recordClassService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _recordClassRepoMock = new();
        _recordClassService = new(_recordClassRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var recordClass = new RecordClass() { RecordId = 1, ClassroomId = 1 };

        _recordClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(recordClass));

        //Act
        var result = await _recordClassService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetRecordClassDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _recordClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<RecordClass>(null));

        //Act
        var result = await _recordClassService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}