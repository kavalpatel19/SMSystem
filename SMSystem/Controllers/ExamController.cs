using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models.Exam;
using SMSystem.Models.Fees;
using SMSystem.Repository.Interfaces;

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
            ExamPaggedViewModel exams = new ExamPaggedViewModel();

            return View(exams);
        }

        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            ExamPaggedViewModel exams = await ExamRepo.GetExams(para);
            return PartialView("_ExamData", exams);
        }

        public IActionResult ExportExcel()
        {
            var Data = ExamRepo.GetAllExams();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ConvertDataTable.Convert(Data.ToList()));
                using (MemoryStream mstream = new MemoryStream())
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
            ExamViewModel exam = await ExamRepo.GetExam(id);
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
                SearchingParaModel para = new SearchingParaModel()
                {
                    PageIndex = 1
                };

                var response = ExamRepo.Delete(id);

                if (response.IsCompletedSuccessfully)
                {
                    ExamPaggedViewModel exams = await ExamRepo.GetExams(para);
                    return PartialView("_ExamData", exams);
                }
                return PartialView();
            }
            catch
            {
                return View();
            }
        }
    }
}
