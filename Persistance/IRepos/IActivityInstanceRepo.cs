namespace Persistance.IRepos;

public interface IActivityInstanceRepo : IGenericRepoAsync<ActivityInstance>
{
    Task<ActivityInstance> GetActivityInstanceById(int id);
}
