using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Organizations;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IOrganizationRepo> _organizationRepoMock;
    private readonly OrganizationService _organizationService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _organizationRepoMock = new();
        _organizationService = new(_organizationRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var organization = new Organization() { Id = id, Name = "Cairo Org." };
        _organizationRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(organization));

        var organizationDto = new UpdateOrganizationDto() { Id = id, Name = "Alex Org." };

        //Act
        var result = await _organizationService.Update(organizationDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _organizationRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Organization>(null));

        var organizationDto = new UpdateOrganizationDto() { Id = id, Name = "Alex Org." };

        //Act
        var result = await _organizationService.Update(organizationDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
    }
}