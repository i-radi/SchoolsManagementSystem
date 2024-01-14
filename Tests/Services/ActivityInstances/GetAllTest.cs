using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityInstances;

public class GetAllTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityInstanceRepo> _activityInstanceRepoMock;
    private readonly ActivityInstanceService _activityInstanceService;

    public GetAllTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityInstanceRepoMock = new();
        _activityInstanceService = new(_activityInstanceRepoMock.Object, _mapperMock);
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_ExistItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var activityInstanceList = new List<ActivityInstance>()
        {
            new ActivityInstance(){ ActivityId = 1,Name = "instance1" }
        };

        _activityInstanceRepoMock.Setup(x => x.GetTableNoTracking()).Returns(activityInstanceList.AsQueryable());

        //Act
        var result = _activityInstanceService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetActivityInstanceDto>>();
    }

    [Theory]
    [InlineData(1, 10)]
    public void GetAll_EmptyItems_ReturnsSuccessMsg(int pageNumber, int pageSize)
    {
        //Arrange
        var activityInstanceList = new List<ActivityInstance>();

        _activityInstanceRepoMock.Setup(x => x.GetTableNoTracking()).Returns(activityInstanceList.AsQueryable());

        //Act
        var result = _activityInstanceService.GetAll(pageNumber, pageSize);

        //Assert
        result.Data.Should().HaveCount(0);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<PaginatedList<GetActivityInstanceDto>>();
    }
}