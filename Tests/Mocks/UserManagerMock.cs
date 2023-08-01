using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Entities.Identity;

namespace Test;

public static class UserManagerMock
{
    public static Mock<UserManager<User>> GetUserManagerMock()
    {
        var userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object);

        userManagerMock
            .Setup(userManager => userManager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Success));
        userManagerMock
            .Setup(userManager => userManager.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

        return userManagerMock;
    }
}
