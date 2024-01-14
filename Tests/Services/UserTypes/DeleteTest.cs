using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserTypes;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserTypeRepo> _userTypeRepoMock;
    private readonly UserTypeService _userTypeService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userTypeRepoMock = new();
        _userTypeService = new(_userTypeRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var userType = new UserType() { Id = id, Name = "Cairo userType" };

        _userTypeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(userType));

        //Act
        var result = await _userTypeService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _userTypeRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _userTypeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<UserType>(null));

        //Act
        var result = await _userTypeService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}