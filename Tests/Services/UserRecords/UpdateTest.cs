using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserRecords;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserRecordRepo> _userRecordRepoMock;
    private readonly UserRecordService _userRecordService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userRecordRepoMock = new();
        _userRecordService = new(_userRecordRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var userRecord = new UserRecord() { RecordId = 1, UserId = 1 };
        _userRecordRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(userRecord));

        var userRecordDto = new UpdateUserRecordDto() { Id = 1, RecordId = 1, UserId = 2 };

        //Act
        var result = await _userRecordService.Update(userRecordDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _userRecordRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _userRecordRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<UserRecord>(null));

        var userRecordDto = new UpdateUserRecordDto() { Id = 1, RecordId = 1, UserId = 2 };

        //Act
        var result = await _userRecordService.Update(userRecordDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _userRecordRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}