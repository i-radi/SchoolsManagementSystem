using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Reflection;
using VModels.ViewModels.Attendances;

namespace Presentation.Controllers.MVC
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public async Task<IActionResult> Activity(int activityId = 0)
        {
            if (activityId <= 0)
            {
                return RedirectToAction("Index", "Activities");
            }

            var viewmodel = await _attendanceService.GetByActivityId(activityId);

            if (viewmodel is null)
            {
                return RedirectToAction("Index", "Activities");
            }

            return View(viewmodel);
        }

        public async Task<IActionResult> DownloadActivityAttendance(int activityId = 0)
        {
            if (activityId <= 0)
            {
                return RedirectToAction("Index", "Activities");
            }

            var viewmodel = await _attendanceService.GetByActivityId(activityId);

            if (viewmodel is null)
            {
                return RedirectToAction("Index", "Activities");
            }

            var data = new List<Dictionary<string, string>>();
            foreach (var user in viewmodel.Users)
            {
                var item = new Dictionary<string, string>();
                item["ActivityId"] = viewmodel.ActivityId.ToString();
                item["ActivityName"] = viewmodel.ActivityName!;
                item["ClassId"] = user.ClassId.ToString();
                item["ClassName"] = user.ClassName!;
                item["UserId"] = user.UserId.ToString();
                item["UserName"] = user.UserName!;
                item["UserTypeId"] = user.UserTypeId.ToString();
                item["UserType"] = user.UserType!;

                foreach (var instance in viewmodel.ActivityInstances)
                {
                    item[instance.InstanceDate.Date.ToString("d")] = (user.InstanceIds.Contains(instance.InstanceId) ? "Yes" : "No");
                }
                item["Total"] = $"{user.InstanceIds.Count}/ {viewmodel.ActivityInstances.Count}";

                data.Add(item);
            }

            return File(
                        await WriteExcel(data),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"ActivityAttendance-{activityId}-{DateTime.Now:f}.xlsx");
        }

        private static async Task<byte[]> WriteExcel(IList<Dictionary<string, string>> registers)
        {
            var registersTotalRows = registers.Count;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excelPackage = new ExcelPackage();
            var excelWorksheet = excelPackage.Workbook.Worksheets.Add("data");

            List<List<string>> values = new();
            foreach (var item in registers)
            {
                values.Add(new(item.Values));
            }

            List<string> keys = new(registers[0].Keys);
            for (var i = 0; i < keys.Count; i++)
            {
                string value = keys[i];
                excelWorksheet.Cells[1, i + 1].Value = value;
            }

            int index = 0;
            for (int row = 2; row <= registersTotalRows + 1; row++)
            {
                for (var i = 0; i < keys.Count; i++)
                {
                    var columnIndex = i + 1;
                    excelWorksheet.Cells[row, columnIndex].Value = values[index][i];
                }
                index++;
            }
            excelWorksheet.Cells.AutoFitColumns();
            return await excelPackage.GetAsByteArrayAsync();
        }

        public async Task<IActionResult> Record(int recordId = 0)
        {
            if (recordId <= 0)
            {
                return RedirectToAction("Index", "Records");
            }

            var viewmodel = await _attendanceService.GetByRecordId(recordId);

            if (viewmodel is null)
            {
                return RedirectToAction("Index", "Records");
            }

            return View(viewmodel);
        }
    }
}
