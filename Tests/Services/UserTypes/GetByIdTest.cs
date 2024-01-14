using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.UserTypes;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserTypeRepo> _userTypeRepoMock;
    private readonly UserTypeService _userTypeService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userTypeRepoMock = new();
        _userTypeService = new(_userTypeRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var userType = new UserType() { Id = id, Name = "Cairo userType" };

        _userTypeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(userType));

        //Act
        var result = await _userTypeService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetUserTypeDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _userTypeRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<UserType>(null));

        //Act
        var result = await _userTypeService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}