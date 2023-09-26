using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Schools;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<ISchoolRepo> _schoolRepoMock;
    private readonly Mock<ISeasonRepo> _seasonRepoMock;
    private readonly SchoolService _Schoolservice;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _schoolRepoMock = new();
        _seasonRepoMock = new();
        _Schoolservice = new(_seasonRepoMock.Object, _schoolRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var schoolDto = new AddSchoolDto() { Name = "Cairo school" };
        var school = _mapperMock.Map<School>(schoolDto);

        _schoolRepoMock.Setup(x => x.AddAsync(school)).Returns(Task.FromResult(school));

        //Act
        var result = await _Schoolservice.Add(schoolDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetSchoolDto>();
        result.Data.Name.Should().BeEquivalentTo("Cairo school");
    }

}