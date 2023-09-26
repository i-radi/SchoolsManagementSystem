using FluentAssertions;
using Models.Entities;
using Models.Entities.Identity;

namespace Tests.Services.UserRoles;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserRoleRepo> _userRoleRepoMock;
    private readonly UserRoleService _userRoleService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userRoleRepoMock = new();
        _userRoleService = new(_userRoleRepoMock.Object, _mapperMock);
    }
}