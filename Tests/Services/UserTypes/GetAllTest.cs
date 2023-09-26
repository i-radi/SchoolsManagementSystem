using FluentAssertions;
using Models.Entities;

namespace Tests.Services.UserTypes;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IUserTypeRepo> _userTypeRepoMock;
    private readonly UserTypeService _userTypeService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _userTypeRepoMock = new();
        _userTypeService = new(_userTypeRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var userTypeList = new List<UserType>()
        {
            new UserType(){ Id = 1, Name = "Cairo userType"}
        };

        _userTypeRepoMock.Setup(x => x.GetTableNoTracking()).Returns(userTypeList.AsQueryable());

        //Act
        var result = _userTypeService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetUserTypeDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var userTypeList = new List<UserType>();

        _userTypeRepoMock.Setup(x => x.GetTableNoTracking()).Returns(userTypeList.AsQueryable());

        //Act
        var result = _userTypeService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetUserTypeDto>>();
    }
}