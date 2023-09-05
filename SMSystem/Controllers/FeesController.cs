using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Exam;
using SMSystem.Models.Fees;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Controllers
{
    [Authorize]
    public class FeesController : Controller
    {
        private readonly IFeesRepository FeesRepo;

        public FeesController(IFeesRepository FeesRepo)
        {
            this.FeesRepo = FeesRepo;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = new FeesPaggedViewModel();
                return View(response);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            var response = new BaseResponseViewModel<FeesPaggedViewModel>();
            try
            {
                response = await FeesRepo.GetFees(para).ConfigureAwait(false);
                if (response.ResponseCode == 200)
                {
                    return PartialView("_FeesData", response.Result);
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return PartialView("_FeesData", response.Result);
                }
            }
            catch (Exception ex)
            {
                response.Result = new FeesPaggedViewModel();
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return PartialView("_FeesData", response.Result);
            }
        }

        public IActionResult ExportExcel()
        {
            try
            {
                var response = FeesRepo.GetAllFees();
                if (response.ResponseCode == 200)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ConvertDataTable.Convert(response.Results));
                        using (var mstream = new MemoryStream())
                        {
                            wb.SaveAs(mstream);
                            return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Fees.xlsx");
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
                return View("Index");
            }
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var fee = new FeesViewModel();
                var fees = FeesRepo.GetAllFees().Results;
                if (fees.Count > 0)
                {
                    var lastId = fees.OrderByDescending(x => x.FeesId).FirstOrDefault().FeesId;
                    char[] spearator = { '-', ' ' };
                    string[] feeId = lastId.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
                    int id = (Convert.ToInt32(feeId[1])) + 1;
                    fee.FeesId = "FEES-" + (id.ToString("0000"));
                }
                else
                {
                    fee.FeesId = "DEP-0001";
                }
                return View(fee);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult> Create(FeesViewModel fee)
        {
            try
            {
                var response = await FeesRepo.Add(fee);
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
                return RedirectToAction(nameof(Create));
            }
        }

        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await FeesRepo.GetFee(id).ConfigureAwait(false);
                if (response.ResponseCode == 200)
                {
                    return View(response.Result);
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(FeesViewModel fee)
        {
            try
            {
                var response = await FeesRepo.Update(fee);
                if (response.ResponseCode == 200)
                {
                    TempData["Message"] = "Data saved successfully.";
                    TempData["ResCode"] = response.ResponseCode;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return RedirectToAction(nameof(Edit));
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return View();
            }
        }

        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var para = new SearchingParaModel()
                {
                    PageIndex = 1
                };

                var response = await FeesRepo.Delete(id);

                var fees = await FeesRepo.GetFees(para).ConfigureAwait(false);
                return PartialView("_ExamData", fees.Result);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, responseCode = 500 });
            }
        }
    }
}
