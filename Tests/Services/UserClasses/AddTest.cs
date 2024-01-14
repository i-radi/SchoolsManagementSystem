using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserClasses;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserClassRepo> _userClassRepoMock;
    private readonly UserClassService _userClassService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userClassRepoMock = new();
        _userClassService = new(_userClassRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var userClassDto = new AddUserClassDto() { ClassroomId = 1, UserId = 1 };
        var userClass = _mapperMock.Map<UserClass>(userClassDto);

        _userClassRepoMock.Setup(x => x.AddAsync(userClass)).Returns(Task.FromResult(userClass));

        //Act
        var result = await _userClassService.Add(userClassDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetUserClassDto>();
    }

}