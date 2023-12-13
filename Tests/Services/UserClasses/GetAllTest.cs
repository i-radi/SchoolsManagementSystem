using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserClasses;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserClassRepo> _userClassRepoMock;
    private readonly UserClassService _userClassService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userClassRepoMock = new();
        _userClassService = new(_userClassRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var userClassList = new List<UserClass>()
        {
            new UserClass(){ ClassroomId = 1, UserId = 1 }
        };

        _userClassRepoMock.Setup(x => x.GetTableNoTracking()).Returns(userClassList.AsQueryable());

        //Act
        var result = _userClassService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetUserClassDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var userClassList = new List<UserClass>();

        _userClassRepoMock.Setup(x => x.GetTableNoTracking()).Returns(userClassList.AsQueryable());

        //Act
        var result = _userClassService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetUserClassDto>>();
    }
}