using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Organizations;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IOrganizationRepo> _organizationRepoMock;
    private readonly OrganizationService _organizationService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _organizationRepoMock = new();
        _organizationService = new(_organizationRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var organizationDto = new AddOrganizationDto() { Name = "Cairo Org." };
        var organization = _mapperMock.Map<Organization>(organizationDto);

        _organizationRepoMock.Setup(x => x.AddAsync(organization)).Returns(Task.FromResult(organization));

        //Act
        var result = await _organizationService.Add(organizationDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetOrganizationDto>();
        result.Data.Name.Should().BeEquivalentTo("Cairo Org.");
    }

}