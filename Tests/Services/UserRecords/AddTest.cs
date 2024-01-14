using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserRecords;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserRecordRepo> _userRecordRepoMock;
    private readonly UserRecordService _userRecordService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userRecordRepoMock = new();
        _userRecordService = new(_userRecordRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var userRecordDto = new AddUserRecordDto() { RecordId = 1, UserId = 1 };
        var userRecord = _mapperMock.Map<UserRecord>(userRecordDto);

        _userRecordRepoMock.Setup(x => x.AddAsync(userRecord)).Returns(Task.FromResult(userRecord));

        //Act
        var result = await _userRecordService.Add(userRecordDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetUserRecordDto>();
    }

}