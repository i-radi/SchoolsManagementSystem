namespace Infrastructure.Services;

public interface IXmlService
{
    byte[] Write<T>(IList<T> registers);
}
