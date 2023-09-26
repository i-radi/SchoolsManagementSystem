using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Classrooms;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IClassroomRepo> _classroomRepoMock;
    private readonly ClassroomService _classroomService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _classroomRepoMock = new();
        _classroomService = new(_classroomRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var classroom = new Classroom() { Id = id, Name = "Cairo classroom" };

        _classroomRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(classroom));

        //Act
        var result = await _classroomService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _classroomRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _classroomRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Classroom>(null));

        //Act
        var result = await _classroomService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}