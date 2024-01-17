using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Organizations;

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

    [Fact]
    public async Task GetAll_ExistItems_ReturnsSuccessMsg()
    {
        //Arrange
        var organizationList = new List<Organization>()
        {
            new Organization(){ Id = 1, Name = "Cairo Org."}
        };

        _organizationRepoMock.Setup(x => x.GetTableNoTracking()).Returns(organizationList.AsQueryable());

        //Act
        var result = await _organizationService.GetAll();

        //Assert
        result.Data.Should().NotBeNullOrEmpty();    
        result.Data.Should().BeOfType<List<GetOrganizationDto>>();
    }

    [Fact]
    public async Task GetAll_EmptyItems_ReturnsSuccessMsg()
    {
        //Arrange
        var organizationList = new List<Organization>();

        _organizationRepoMock.Setup(x => x.GetTableNoTracking()).Returns(organizationList.AsQueryable());

        //Act
        var result = await _organizationService.GetAll();

        //Assert
        result.Data.Should().HaveCount(0);
        result.Data.Should().BeOfType<List<GetOrganizationDto>>();
    }
}