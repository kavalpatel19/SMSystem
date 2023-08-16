using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models;
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
            var exams = new ExamPaggedViewModel();
            return View(exams);
        }

        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            var exams = await ExamRepo.GetExams(para).ConfigureAwait(false);
            return PartialView("_ExamData", exams);
        }

        public IActionResult ExportExcel()
        {
            var data = ExamRepo.GetAllExams();
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ConvertDataTable.Convert(data.ToList()));
                using (var mstream = new MemoryStream())
                {
                    wb.SaveAs(mstream);
                    return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Exams.xlsx");
                }
            }
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExamViewModel exam)
        {
            try
            {
                var response = ExamRepo.Add(exam);
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

        public async Task<IActionResult> Edit(int id)
        {
            var exam = await ExamRepo.GetExam(id).ConfigureAwait(false);
            return View(exam);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExamViewModel exam)
        {
            try
            {
                var response = ExamRepo.Update(exam);
                if (response.IsCompletedSuccessfully)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Edit));
                }
            }
            catch
            {
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

                var response = ExamRepo.Delete(id);

                if (response.IsCompletedSuccessfully)
                {
                    var exams = await ExamRepo.GetExams(para).ConfigureAwait(false);
                    return PartialView("_ExamData", exams);
                }
                return PartialView();
            }
            catch(Exception ex)
            {
                return View(ex);
            }
        }
        
    }
}
