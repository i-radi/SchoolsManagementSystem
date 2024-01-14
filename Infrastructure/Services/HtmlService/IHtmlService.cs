namespace Infrastructure.Services;

public interface IHtmlService
{
    byte[] Write<T>(IList<T> registers);
}
