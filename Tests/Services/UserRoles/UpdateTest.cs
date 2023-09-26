using FluentAssertions;
using Models.Entities;
using Models.Entities.Identity;

namespace Tests.Services.UserRoles;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserRoleRepo> _userRoleRepoMock;
    private readonly UserRoleService _userRoleService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userRoleRepoMock = new();
        _userRoleService = new(_userRoleRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var userRole = new UserRole() { RoleId = 1, UserId = 1 };
        _userRoleRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(userRole));

        var userRoleDto = new UpdateUserRoleDto() { Id = 1, RoleId = 1, UserId = 2 };

        //Act
        var result = await _userRoleService.Update(userRoleDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _userRoleRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _userRoleRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<UserRole>(null));

        var userRoleDto = new UpdateUserRoleDto() { Id = 1, RoleId = 1, UserId = 2};

        //Act
        var result = await _userRoleService.Update(userRoleDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _userRoleRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}