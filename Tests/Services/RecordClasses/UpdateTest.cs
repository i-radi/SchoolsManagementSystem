using FluentAssertions;
using Models.Entities;

namespace Tests.Services.RecordClasses;

public class UpdateTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IRecordClassRepo> _recordClassRepoMock;
    private readonly RecordClassService _recordClassService;

    public UpdateTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _recordClassRepoMock = new();
        _recordClassService = new(_recordClassRepoMock.Object, _mapperMock);
    }


    [Theory]
    [InlineData(1)]
    public async Task Update_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var recordClass = new RecordClass() {Id = 1, RecordId = 1, ClassroomId = 1 };
        _recordClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(recordClass));

        var recordClassDto = new UpdateRecordClassDto() { Id = 1, RecordId = 1, ClassroomId = 2 };

        //Act
        var result = await _recordClassService.Update(recordClassDto);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Data.Should().BeTrue();
        _recordClassRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Update_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _recordClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<RecordClass>(null));

        var recordClassDto = new UpdateRecordClassDto() { RecordId = 1, ClassroomId = 2 };

        //Act
        var result = await _recordClassService.Update(recordClassDto);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Data.Should().BeFalse();
        _recordClassRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }
}