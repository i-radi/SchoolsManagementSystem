namespace Persistance.IRepos;

public interface IRecordRepo : IGenericRepoAsync<Record>
{
    Task<List<UserClass>?> GetUserClassesByRecordId(int id);
    Task<Record?> GetRecordWithUsersByRecordId(int id);
}
