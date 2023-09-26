using FluentAssertions;
using Models.Entities;

namespace Test.Services.OrganizationServiceTest;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IOrganizationRepo> _organizationRepoMock;
    private readonly OrganizationService _organizationService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _organizationRepoMock = new();
        _organizationService = new(_organizationRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var organizationList = new List<Organization>()
        {
            new Organization(){ Id = 1, Name = "Cairo Org."}
        };

        _organizationRepoMock.Setup(x => x.GetTableNoTracking()).Returns(organizationList.AsQueryable());

        //Act
        var result = _organizationService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetOrganizationDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var organizationList = new List<Organization>();

        _organizationRepoMock.Setup(x => x.GetTableNoTracking()).Returns(organizationList.AsQueryable());

        //Act
        var result = _organizationService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetOrganizationDto>>();
    }
}