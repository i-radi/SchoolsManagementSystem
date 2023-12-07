namespace Persistance.IRepos;

public interface IActivityRepo : IGenericRepoAsync<Activity>
{
    Task<List<UserClass>?> GetUserClassesByActivityId(int id);
    Task<List<ActivityInstance>?> GetActivityInstancesWithUserByActivityId(int id);
}
