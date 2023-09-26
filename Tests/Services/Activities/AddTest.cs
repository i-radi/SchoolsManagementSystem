using FluentAssertions;
using Models.Entities;

namespace Tests.Services.Activities;

public class AddTest
{
    private readonly IMapper _mapperMock;
    private readonly Mock<IActivityRepo> _activityRepoMock;
    private readonly ActivityService _activityService;

    public AddTest()
    {
        _mapperMock = MapperMock.GetAllProfile();
        _activityRepoMock = new();
        _activityService = new(_activityRepoMock.Object, _mapperMock);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItem()
    {
        //Arrange
        var activityDto = new AddActivityDto() { Name = "Cairo activity" };
        var activity = _mapperMock.Map<Activity>(activityDto);

        _activityRepoMock.Setup(x => x.AddAsync(activity)).Returns(Task.FromResult(activity));

        //Act
        var result = await _activityService.Add(activityDto);

        //Assert
        result.Data.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeOfType<GetActivityDto>();
        result.Data.Name.Should().BeEquivalentTo("Cairo activity");
    }

}