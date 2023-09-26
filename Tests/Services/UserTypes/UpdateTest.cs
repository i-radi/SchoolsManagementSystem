using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserTypes;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserTypeRepo> _userTypeRepoMock;
    private readonly UserTypeService _userTypeService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userTypeRepoMock = new();
        _userTypeService = new(_userTypeRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var userType = new UserType() { Id = id, Name = "Cairo UserType" };
        _userTypeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(userType));

        var userTypeDto = new UpdateUserTypeDto() { Id = id, Name = "Alex UserType" };

        //Act
        var result = await _userTypeService.Update(userTypeDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _userTypeRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _userTypeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<UserType>(null));

        var userTypeDto = new UpdateUserTypeDto() { Id = id, Name = "Alex UserType" };

        //Act
        var result = await _userTypeService.Update(userTypeDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _userTypeRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}