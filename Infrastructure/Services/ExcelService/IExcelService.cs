using System.Reflection;

namespace Infrastructure.Services;

public interface IExcelService
{
    Task<byte[]> Write<T>(IList<T> registers);
}
