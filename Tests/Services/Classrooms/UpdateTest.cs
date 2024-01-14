using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Classrooms;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IClassroomRepo> _classroomRepoMock;
    private readonly ClassroomService _classroomService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _classroomRepoMock = new();
        _classroomService = new(_classroomRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var classroom = new Classroom() { Id = id, Name = "Cairo Classroom" };
        _classroomRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(classroom));

        var classroomDto = new UpdateClassroomDto() { Id = id, Name = "Alex Classroom" };

        //Act
        var result = await _classroomService.Update(classroomDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _classroomRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _classroomRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Classroom>(null));

        var classroomDto = new UpdateClassroomDto() { Id = id, Name = "Alex Classroom" };

        //Act
        var result = await _classroomService.Update(classroomDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _classroomRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}