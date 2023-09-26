using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserTypes;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserTypeRepo> _userTypeRepoMock;
    private readonly UserTypeService _userTypeService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userTypeRepoMock = new();
        _userTypeService = new(_userTypeRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var userTypeDto = new AddUserTypeDto() { Name = "Cairo userType" };
        var userType = _mapperMock.Map<UserType>(userTypeDto);

        _userTypeRepoMock.Setup(x => x.AddAsync(userType)).Returns(Task.FromResult(userType));

        //Act
        var result = await _userTypeService.Add(userTypeDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetUserTypeDto>();
        result.Data.Name.Should().BeEquivalentTo("Cairo userType");
    }

}