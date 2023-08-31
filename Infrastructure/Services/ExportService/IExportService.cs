using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IExportService
    {
        Task<byte[]> ExportToExcel(List<UserViewModel> users);

        byte[] ExportToCsv(List<UserViewModel> users);

        byte[] ExportToHtml(List<UserViewModel> users);

        byte[] ExportToJson(List<UserViewModel> users);

        byte[] ExportToXml(List<UserViewModel> users);

        byte[] ExportToYaml(List<UserViewModel> users);
    }
}
