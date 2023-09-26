using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserRecords;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserRecordRepo> _userRecordRepoMock;
    private readonly UserRecordService _userRecordService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userRecordRepoMock = new();
        _userRecordService = new(_userRecordRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var userRecordList = new List<UserRecord>()
        {
            new UserRecord(){ RecordId = 1, UserId = 1 }
        };

        _userRecordRepoMock.Setup(x => x.GetTableNoTracking()).Returns(userRecordList.AsQueryable());

        //Act
        var result = _userRecordService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetUserRecordDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var userRecordList = new List<UserRecord>();

        _userRecordRepoMock.Setup(x => x.GetTableNoTracking()).Returns(userRecordList.AsQueryable());

        //Act
        var result = _userRecordService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetUserRecordDto>>();
    }
}