using FluentAssertions;
using Models.Entities;

namespace Tests.Services.RecordClasses;

public class DeleteTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IRecordClassRepo> _recordClassRepoMock;
    private readonly RecordClassService _recordClassService;

    public DeleteTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _recordClassRepoMock = new();
        _recordClassService = new(_recordClassRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1)]
    public async Task Delete_ValidItem_ReturnsSuccess(int id)
    {
        //Arrange
        var recordClass = new RecordClass() { RecordId = 1, ClassroomId = 1 };

        _recordClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(recordClass));

        //Act
        var result = await _recordClassService.Delete(id);

        //Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _recordClassRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once, "Not Called");
    }

    [Theory]
    [InlineData(5)]
    public async Task Delete_InValidItem_ReturnsNotFound(int id)
    {
        //Arrange
        _recordClassRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<RecordClass>(null));

        //Act
        var result = await _recordClassService.Delete(id);

        //Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}