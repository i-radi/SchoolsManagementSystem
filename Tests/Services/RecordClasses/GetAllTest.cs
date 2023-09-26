using FluentAssertions;
using Models.Entities;

namespace Tests.Services.RecordClasses;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IRecordClassRepo> _recordClassRepoMock;
    private readonly RecordClassService _recordClassService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _recordClassRepoMock = new();
        _recordClassService = new(_recordClassRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var recordClassList = new List<Models.Entities.RecordClass>()
        {
            new Models.Entities.RecordClass(){ RecordId = 1, ClassroomId = 1 }
        };

        _recordClassRepoMock.Setup(x => x.GetTableNoTracking()).Returns(recordClassList.AsQueryable());

        //Act
        var result = _recordClassService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetRecordClassDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var recordClassList = new List<RecordClass>();

        _recordClassRepoMock.Setup(x => x.GetTableNoTracking()).Returns(recordClassList.AsQueryable());

        //Act
        var result = _recordClassService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<List<GetRecordClassDto>>();
    }
}