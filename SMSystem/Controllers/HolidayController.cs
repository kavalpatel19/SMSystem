using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Holiday;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Controllers
{
    public class HolidayController : Controller
    {
        private readonly IHolidayRepository HoliRepo;

        public HolidayController(IHolidayRepository HoliRepo)
        {
            this.HoliRepo = HoliRepo;
        }

        // GET: HolidayController
        public async Task<IActionResult> Index()
        {
            HolidayPaggedViewModel holidays = new HolidayPaggedViewModel();

            return View(holidays);
        }

        // returns partial view of datatable.
        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            HolidayPaggedViewModel holidays = await HoliRepo.GetHolidays(para);
            return PartialView("_HolidayData", holidays);
        }

        // To Export Data
        public IActionResult ExportExcel()
        {
            var Data = HoliRepo.GetAllHolidays();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ConvertDataTable.Convert(Data.ToList()));
                using (MemoryStream mstream = new MemoryStream())
                {
                    wb.SaveAs(mstream);
                    return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Holidays.xlsx");
                }
            }
        }

        // GET: HolidayController/Create
        public async Task<IActionResult> Create()
        {
            HolidayViewModel holiday = new HolidayViewModel();
            var holidays = HoliRepo.GetAllHolidays();
            if (holidays.Count > 0)
            {
                var lastId = holidays.OrderByDescending(x => x.HolidayId).FirstOrDefault().HolidayId;
                char[] spearator = { '-', ' ' };
                string[] holiId = lastId.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
                int id = (Convert.ToInt32(holiId[1])) + 1;
                holiday.HolidayId = "HOLI-" + (id.ToString("0000"));
            }
            else
            {
                holiday.HolidayId = "HOLI-0001";
            }
            return View(holiday);
        }

        // POST: HolidayController/Create
        [HttpPost]
        public async Task<IActionResult> Create(HolidayViewModel holiday)
        {
            try
            {
                var response = HoliRepo.Add(holiday);
                if (response.IsCompletedSuccessfully)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Create));
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
