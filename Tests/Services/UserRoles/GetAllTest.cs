using FluentAssertions;
using Models.Entities.Identity;

namespace Tests.Services.UserRoles;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserRoleRepo> _userRoleRepoMock;
    private readonly UserRoleService _userRoleService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userRoleRepoMock = new();
        _userRoleService = new(_userRoleRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var userRoleList = new List<UserRole>()
        {
            new UserRole(){ RoleId = 1, UserId = 1 }
        };

        _userRoleRepoMock.Setup(x => x.GetTableNoTracking()).Returns(userRoleList.AsQueryable());

        //Act
        var result = _userRoleService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetUserRoleDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var userRoleList = new List<UserRole>();

        _userRoleRepoMock.Setup(x => x.GetTableNoTracking()).Returns(userRoleList.AsQueryable());

        //Act
        var result = _userRoleService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetUserRoleDto>>();
    }
}