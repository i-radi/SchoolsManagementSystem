namespace Tests.Services.Schools;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISchoolRepo> _schoolRepoMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SchoolService _schoolService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _schoolRepoMock = new();
        _seasonRepoMock = new();
        _schoolService = new(_seasonRepoMock.Object, _schoolRepoMock.Object, null, _mapperMock);
    }
}