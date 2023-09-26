using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserClasses;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserClassRepo> _userClassRepoMock;
    private readonly UserClassService _userClassService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userClassRepoMock = new();
        _userClassService = new(_userClassRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var userClass = new UserClass() { ClassroomId = 1, UserId = 1 };
        _userClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(userClass));

        var userClassDto = new UpdateUserClassDto() { Id = 1, ClassroomId = 1, UserId = 2 };

        //Act
        var result = await _userClassService.Update(userClassDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _userClassRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _userClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<UserClass>(null));

        var userClassDto = new UpdateUserClassDto() { Id = 1, ClassroomId = 1, UserId = 2 };

        //Act
        var result = await _userClassService.Update(userClassDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _userClassRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}