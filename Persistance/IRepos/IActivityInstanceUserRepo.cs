namespace Persistance.IRepos;

public interface IActivityInstanceUserRepo : IGenericRepoAsync<ActivityInstanceUser>
{
    Task<List<int>> GetUserIdsByActivityInstanceId(int acivityInstanceId);
}
