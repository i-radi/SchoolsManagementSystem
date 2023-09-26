using FluentAssertions;
using Models.Entities;

namespace Test.Services.OrganizationServiceTest;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IOrganizationRepo> _organizationRepoMock;
    private readonly OrganizationService _organizationService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _organizationRepoMock = new();
        _organizationService = new(_organizationRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var organization = new Organization() { Id = id, Name = "Cairo Org." };

        _organizationRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(organization));

        //Act
        var result = await _organizationService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _organizationRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Organization>(null));

        //Act
        var result = await _organizationService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}