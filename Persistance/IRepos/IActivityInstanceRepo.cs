using Microsoft.EntityFrameworkCore;

namespace Persistance.IRepos;

public interface IActivityInstanceRepo : IGenericRepoAsync<ActivityInstance>
{
    Task<ActivityInstance> AddActivityInstanceAsync(ActivityInstance activityInstance);


}
