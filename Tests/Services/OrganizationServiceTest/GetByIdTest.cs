using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Test.Services.OrganizationServiceTest;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IOrganizationRepo> _organizationRepoMock;
    private readonly OrganizationService _organizationService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _organizationRepoMock = new();
        _organizationService = new(_organizationRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var organization = new Organization() { Id = id, Name = "Cairo Org." };

        _organizationRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(organization));

        //Act
        var result = await _organizationService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetOrganizationDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _organizationRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Organization>(null));

        //Act
        var result = await _organizationService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}