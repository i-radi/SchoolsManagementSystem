namespace Infrastructure.Services;

public class ExportService<T> : IExportService<T>
{
    private readonly IExcelService _excelService;
    private readonly ICsvService _csvService;
    private readonly IHtmlService _htmlService;
    private readonly IJsonService _jsonService;
    private readonly IXmlService _xmlService;
    private readonly IYamlService _yamlService;

    public ExportService(IExcelService excelService, ICsvService csvService, IHtmlService htmlService, IJsonService jsonService, IXmlService xmlService, IYamlService yamlService)
    {
        _excelService = excelService;
        _csvService = csvService;
        _htmlService = htmlService;
        _jsonService = jsonService;
        _xmlService = xmlService;
        _yamlService = yamlService;
    }

    public async Task<byte[]> ExportToExcel(List<T> registers)
    {
        return await _excelService.Write(registers);
    }

    public async Task<string> ExportToExcelAndSave(List<T> registers, string path)
    {
        return await _excelService.WriteAndSave(registers, path);
    }

    public byte[] ExportToCsv(List<T> registers)
    {
        return _csvService.Write(registers);
    }

    public byte[] ExportToHtml(List<T> registers)
    {
        return _htmlService.Write(registers);
    }

    public byte[] ExportToJson(List<T> registers)
    {
        return _jsonService.Write(registers);
    }

    public byte[] ExportToXml(List<T> registers)
    {
        return _xmlService.Write(registers);
    }

    public byte[] ExportToYaml(List<T> registers)
    {
        return _yamlService.Write(registers);
    }
}
