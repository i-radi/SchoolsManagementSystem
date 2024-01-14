namespace Infrastructure.Services;

public interface IYamlService
{
    byte[] Write<T>(IList<T> registers);
}
