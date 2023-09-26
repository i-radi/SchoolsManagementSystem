using FluentAssertions;
using Models.Entities;

namespace Tests.Services.ActivityInstances;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityInstanceRepo> _activityInstanceRepoMock;
    private readonly ActivityInstanceService _activityInstanceService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityInstanceRepoMock = new();
        _activityInstanceService = new(_activityInstanceRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var activityInstanceDto = new AddActivityInstanceDto() { ActivityId = 1, Name = "instance1" };
        var activityInstance = _mapperMock.Map<ActivityInstance>(activityInstanceDto);

        _activityInstanceRepoMock.Setup(x => x.AddAsync(activityInstance)).Returns(Task.FromResult(activityInstance));

        //Act
        var result = await _activityInstanceService.Add(activityInstanceDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetActivityInstanceDto>();
    }

}