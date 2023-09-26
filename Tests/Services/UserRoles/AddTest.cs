using FluentAssertions;
using Models.Entities;
using Models.Entities.Identity;

namespace Tests.Services.UserRoles;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserRoleRepo> _userRoleRepoMock;
    private readonly UserRoleService _userRoleService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userRoleRepoMock = new();
        _userRoleService = new(_userRoleRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var userRoleDto = new AddUserRoleDto() { RoleId = 1, UserId = 1 };
        var userRole = _mapperMock.Map<UserRole>(userRoleDto);

        _userRoleRepoMock.Setup(x => x.AddAsync(userRole)).Returns(Task.FromResult(userRole));

        //Act
        var result = await _userRoleService.Add(userRoleDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetUserRoleDto>();
    }

}