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
}