using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Holiday;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Controllers
{
    [Authorize]
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
            try
            {
                var response = new HolidayPaggedViewModel();
                return View(response);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction("Index", "Home");
            }
        }

        // returns partial view of datatable.
        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            var response = new BaseResponseViewModel<HolidayPaggedViewModel>();
            try
            {
                response = await HoliRepo.GetHolidays(para).ConfigureAwait(false);
                if (response.ResponseCode == 200)
                {
                    return PartialView("_HolidayData", response.Result);
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return PartialView("_HolidayData", response.Result);
                }
            }
            catch (Exception ex)
            {
                response.Result = new HolidayPaggedViewModel();
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return PartialView("_HolidayData", response.Result);
            }
        }

        // To Export Data
        public IActionResult ExportExcel()
        {
            try
            {
                var response = HoliRepo.GetAllHolidays();
                if (response.ResponseCode == 200)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ConvertDataTable.Convert(response.Results));
                        using (var mstream = new MemoryStream())
                        {
                            wb.SaveAs(mstream);
                            return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Holidays.xlsx");
                        }
                    }
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction("Index");
            }
        }

        // GET: HolidayController/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var holiday = new HolidayViewModel();
                var holidays = HoliRepo.GetAllHolidays().Results;
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
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: HolidayController/Create
        [HttpPost]
        public async Task<IActionResult> Create(HolidayViewModel holiday)
        {
            try
            {
                var response = await HoliRepo.Add(holiday);
                if (response.ResponseCode == 200)
                {
                    TempData["Message"] = "Record Created Successfully.";
                    TempData["ResCode"] = response.ResponseCode;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return RedirectToAction(nameof(Create));
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return View();
            }
        }
    }
}
