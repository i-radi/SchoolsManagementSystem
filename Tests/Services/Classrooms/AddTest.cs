using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Classrooms;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IClassroomRepo> _classroomRepoMock;
    private readonly ClassroomService _classroomService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _classroomRepoMock = new();
        _classroomService = new(_classroomRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var classroomDto = new AddClassroomDto() { Name = "Cairo classroom" };
        var classroom = _mapperMock.Map<Classroom>(classroomDto);

        _classroomRepoMock.Setup(x => x.AddAsync(classroom)).Returns(Task.FromResult(classroom));

        //Act
        var result = await _classroomService.Add(classroomDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetClassroomDto>();
        result.Data.Name.Should().BeEquivalentTo("Cairo classroom");
    }

}