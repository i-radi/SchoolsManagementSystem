namespace Infrastructure.Services;

public interface IExcelService
{
    Task<byte[]> Write<T>(IList<T> registers);
    Task<string> WriteAndSave<T>(IList<T> registers, string path);
}
