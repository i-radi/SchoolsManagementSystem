using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.UserClasses;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserClassRepo> _userClassRepoMock;
    private readonly UserClassService _userClassService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userClassRepoMock = new();
        _userClassService = new(_userClassRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var userClass = new UserClass() { ClassroomId = 1, UserId = 1 };

        _userClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(userClass));

        //Act
        var result = await _userClassService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetUserClassDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _userClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<UserClass>(null));

        //Act
        var result = await _userClassService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}