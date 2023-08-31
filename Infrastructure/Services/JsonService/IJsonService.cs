namespace Infrastructure.Services;

public interface IJsonService
{
    byte[] Write<T>(IList<T> registers);
}
