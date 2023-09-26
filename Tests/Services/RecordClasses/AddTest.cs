using FluentAssertions;
using Models.Entities;

namespace Tests.Services.RecordClasses;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IRecordClassRepo> _recordClassRepoMock;
    private readonly RecordClassService _recordClassService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _recordClassRepoMock = new();
        _recordClassService = new(_recordClassRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var recordClassDto = new AddRecordClassDto() { RecordId = 1, ClassroomId = 1 };
        var recordClass = _mapperMock.Map<RecordClass>(recordClassDto);

        _recordClassRepoMock.Setup(x => x.AddAsync(recordClass)).Returns(Task.FromResult(recordClass));

        //Act
        var result = await _recordClassService.Add(recordClassDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetRecordClassDto>();
    }

}