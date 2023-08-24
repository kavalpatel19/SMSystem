using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Exam;
using SMSystem.Models.Fees;
using SMSystem.Repository.Interfaces;
using System.ComponentModel;
using System.Diagnostics;

namespace SMSystem.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExamRepository ExamRepo;

        public ExamController(IExamRepository ExamRepo)
        {
            this.ExamRepo = ExamRepo;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = new ExamPaggedViewModel();
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
            var response = new BaseResponseViewModel<ExamPaggedViewModel>();
            try
            {
                response = await ExamRepo.GetExams(para).ConfigureAwait(false);
                if (response.ResponseCode == 200)
                {
                    return PartialView("_ExamData", response.Result);
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return PartialView("_ExamData", response.Result);
                }
            }
            catch (Exception ex)
            {
                response.Result = new ExamPaggedViewModel();
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return PartialView("_ExamData", response.Result);
            }
        }

        public IActionResult ExportExcel()
        {
            try
            {
                var response = ExamRepo.GetAllExams();
                if (response.ResponseCode == 200)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ConvertDataTable.Convert(response.Results));
                        using (var mstream = new MemoryStream())
                        {
                            wb.SaveAs(mstream);
                            return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Exams.xlsx");
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
        public async Task<IActionResult> Create()
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExamViewModel exam)
        {
            try
            {
                var response = await ExamRepo.Add(exam);
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

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await ExamRepo.GetExam(id).ConfigureAwait(false);
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

        [HttpPost]
        public async Task<IActionResult> Edit(ExamViewModel exam)
        {
            try
            {
                var response = await ExamRepo.Update(exam);
                if (response.ResponseCode == 200)
                {
                    TempData["Message"] = "Exam has been saved successfully.";
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

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var para = new SearchingParaModel()
                {
                    PageIndex = 1
                };

                var response = await ExamRepo.Delete(id);

                var exams = await ExamRepo.GetExams(para).ConfigureAwait(false);
                return PartialView("_ExamData", exams.Result);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, responseCode = 500 });
            }
        }
        
    }
}
