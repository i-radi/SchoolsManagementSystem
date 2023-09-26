using FluentAssertions;
using Models.Entities;
using Models.Entities.Identity;
using Tests.PassData;

namespace Tests.Services.UserRoles;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserRoleRepo> _userRoleRepoMock;
    private readonly UserRoleService _userRoleService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userRoleRepoMock = new();
        _userRoleService = new(_userRoleRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var userRole = new UserRole() { RoleId = 1, UserId = 1 };

        _userRoleRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(userRole));

        //Act
        var result = await _userRoleService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetUserRoleDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _userRoleRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<UserRole>(null));

        //Act
        var result = await _userRoleService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}