using FluentAssertions;
using Models.Entities;
using Tests.PassData;

namespace Tests.Services.ActivityInstances;

public class GetByIdTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityInstanceRepo> _activityInstanceRepoMock;
    private readonly ActivityInstanceService _activityInstanceService;

    public GetByIdTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityInstanceRepoMock = new();
        _activityInstanceService = new(_activityInstanceRepoMock.Object, _mapperMock);
    }

    [Theory]
    [MemberData(nameof(PassDataToParamUsingMemberData.GetParamData), MemberType = typeof(PassDataToParamUsingMemberData))]
    public async void GetById_where_Found_Return_StatusCode200(int id)
    {
        //Arrange
        var activityInstance = new ActivityInstance() { ActivityId = 1, Name = "instance1" };

        _activityInstanceRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(activityInstance));

        //Act
        var result = await _activityInstanceService.GetById(id);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetActivityInstanceDto>();
    }

    [Theory]
    [InlineData(5)]
    public async void GetById_where_NotFound_Return_Null(int id)
    {
        //Arrange
        _activityInstanceRepoMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult<ActivityInstance>(null));

        //Act
        var result = await _activityInstanceService.GetById(id);

        //Assert
        result.Should().BeNull();
    }
}