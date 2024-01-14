using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.Classrooms;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IClassroomRepo> _classroomRepoMock;
    private readonly ClassroomService _classroomService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _classroomRepoMock = new();
        _classroomService = new(_classroomRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var classroom = new Classroom() { Id = id, Name = "Cairo classroom" };

        _classroomRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(classroom));

        //Act
        var result = await _classroomService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetClassroomDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _classroomRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<Classroom>(null));

        //Act
        var result = await _classroomService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}