using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.UserRecords;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserRecordRepo> _userRecordRepoMock;
    private readonly UserRecordService _userRecordService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userRecordRepoMock = new();
        _userRecordService = new(_userRecordRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var userRecord = new UserRecord() { RecordId = 1, UserId = 1 };

        _userRecordRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(userRecord));

        //Act
        var result = await _userRecordService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetUserRecordDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _userRecordRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<UserRecord>(null));

        //Act
        var result = await _userRecordService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}