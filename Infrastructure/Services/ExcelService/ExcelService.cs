using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Infrastructure.Services;

public class ExcelService : IExcelService
{
    public async Task<byte[]> Write<T>(IList<T> registers)
    {
        var registersTotalRows = registers.Count;

        ExcelPackage.LicenseContext = LicenseContext.Commercial;
        using var excelPackage = new ExcelPackage();
        var excelWorksheet = excelPackage.Workbook.Worksheets.Add("data");

        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();

        for (var i = 0; i < properties.Length; i++)
        {
            string value = properties[i].Name;

            var attribute = properties[i].GetCustomAttribute(typeof(DisplayAttribute));
            if (attribute != null)
            {
                value = (attribute as DisplayAttribute)!.Name!;
            }

            excelWorksheet.Cells[1, i + 1].Value = value;
        }

        int index = 0;
        for (int row = 2; row <= registersTotalRows + 1; row++)
        {
            for (var i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(registers[index], null);

                Type propertyType = properties[i].PropertyType;
                TypeCode typeCode = Type.GetTypeCode(propertyType);

                var columnIndex = i + 1;

                excelWorksheet.Cells[row, columnIndex].Value = typeCode switch
                {
                    TypeCode.String => value,
                    TypeCode.Int32 or TypeCode.Double or TypeCode.Decimal or TypeCode.Single => value?.ToString(),
                    TypeCode.Boolean => ((bool)value!) ? "Yes" : "No",
                    TypeCode.DateTime => ((DateTime)value!).ToString("dd/MM/yyyy HH:mm:ss"),
                    _ => string.Empty,
                };
            }

            index++;
        }
        excelWorksheet.Cells.AutoFitColumns();

        var excelTable = excelWorksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: registersTotalRows, toColumn: properties.Length), "TestRegisters");
        excelTable.ShowHeader = true;

        var excelRange = excelWorksheet.Cells[1, 1, excelWorksheet.Dimension.End.Row, excelWorksheet.Dimension.End.Column];
        excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        excelTable.TableStyle = TableStyles.Light21;
        excelTable.ShowTotal = false;

        return await excelPackage.GetAsByteArrayAsync();
    }

    public Task<string> WriteAndSave<T>(IList<T> registers, string filePath)
    {
        var registersTotalRows = registers.Count;

        ExcelPackage.LicenseContext = LicenseContext.Commercial;
        using var excelPackage = new ExcelPackage();
        var excelWorksheet = excelPackage.Workbook.Worksheets.Add("data");

        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();

        for (var i = 0; i < properties.Length; i++)
        {
            string value = properties[i].Name;

            var attribute = properties[i].GetCustomAttribute(typeof(DisplayAttribute));
            if (attribute != null)
            {
                value = (attribute as DisplayAttribute)!.Name!;
            }

            excelWorksheet.Cells[1, i + 1].Value = value;
        }

        int index = 0;
        for (int row = 2; row <= registersTotalRows + 1; row++)
        {
            for (var i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(registers[index], null);

                Type propertyType = properties[i].PropertyType;
                TypeCode typeCode = Type.GetTypeCode(propertyType);

                var columnIndex = i + 1;

                excelWorksheet.Cells[row, columnIndex].Value = typeCode switch
                {
                    TypeCode.String => value,
                    TypeCode.Int32 or TypeCode.Double or TypeCode.Decimal or TypeCode.Single => value?.ToString(),
                    TypeCode.Boolean => ((bool)value!) ? "Yes" : "No",
                    TypeCode.DateTime => ((DateTime)value!).ToString("dd/MM/yyyy HH:mm:ss"),
                    _ => string.Empty,
                };
            }

            index++;
        }
        excelWorksheet.Cells.AutoFitColumns();

        var excelTable = excelWorksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: registersTotalRows, toColumn: properties.Length), "TestRegisters");
        excelTable.ShowHeader = true;

        var excelRange = excelWorksheet.Cells[1, 1, excelWorksheet.Dimension.End.Row, excelWorksheet.Dimension.End.Column];
        excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        excelTable.TableStyle = TableStyles.Light21;
        excelTable.ShowTotal = false;

        excelPackage.SaveAs(new FileInfo(filePath));

        return Task.FromResult(filePath);
    }
}
