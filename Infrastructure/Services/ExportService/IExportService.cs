namespace Infrastructure.Services
{
    public interface IExportService<T>
    {
        Task<byte[]> ExportToExcel(List<T> users);
        Task<string> ExportToExcelAndSave(List<T> registers, string path);

        byte[] ExportToCsv(List<T> users);

        byte[] ExportToHtml(List<T> users);

        byte[] ExportToJson(List<T> users);

        byte[] ExportToXml(List<T> users);

        byte[] ExportToYaml(List<T> users);
    }
}
