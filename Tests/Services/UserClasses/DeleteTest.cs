using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserClasses;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserClassRepo> _userClassRepoMock;
    private readonly UserClassService _userClassService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userClassRepoMock = new();
        _userClassService = new(_userClassRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var userClass = new UserClass() { ClassroomId = 1, UserId = 1 };
        var dto = new AddUserClassDto();

        _userClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(userClass));

        //Act
        var result = await _userClassService.Delete(dto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _userClassRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        var items = new List<UserClass>(); 
        _userClassRepoMock.Setup(x => x.GetTableNoTracking()).Returns(items.AsQueryable());
        var dto = new AddUserClassDto();    

        //Act
        var result = await _userClassService.Delete(dto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}