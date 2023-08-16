using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models.Students;
using SMSystem.Models.Teacher;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository TeachRepo;

        public TeacherController(ITeacherRepository TeachRepo)
        {
            this.TeachRepo = TeachRepo;
        }

        // GET: TeacherController
        public async Task<IActionResult> Index(SearchingParaModel para)
        {
            var teacherPagedViewModel = new TeacherPagedViewModel();

            return View(teacherPagedViewModel);
        }

        public async Task<IActionResult> GridIndex()
        {
            var teachers = TeachRepo.GetAllTeachers();
            return View(teachers);
        }

        // Returning data to the view
        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            var teachers = await TeachRepo.GetTeachers(para).ConfigureAwait(false);

            return PartialView("_TeacherData", teachers);
        }

        public IActionResult ExportExcel()
        {
            var data = TeachRepo.GetAllTeachers();

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ConvertDataTable.Convert(data.ToList()));
                using (var mstream = new MemoryStream())
                {
                    wb.SaveAs(mstream);
                    return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Teachers.xlsx");
                }
            }
        }

        // GET: TeacherController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var teacher = await TeachRepo.GetTeacher(id).ConfigureAwait(false);

            return View(teacher);
        }

        // GET: TeacherController/Create
        public async Task<IActionResult> Create()
        {
            var teacher = new TeacherViewModel();
            var teachers = TeachRepo.GetAllTeachers();
            if (teachers.Count > 0)
            {
                var lastId = teachers.OrderByDescending(x => x.TeacherId).FirstOrDefault().TeacherId;
                char[] spearator = { '-', ' ' };
                string[] tchId = lastId.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
                int id = (Convert.ToInt32(tchId[1])) + 1;
                teacher.TeacherId = "TCHR-" + (id.ToString("0000"));
            }
            else
            {
                teacher.TeacherId = "TCHR-0001";
            }
            return View(teacher);
        }

        // POST: TeacherController/Create
        [HttpPost]
        public async Task<IActionResult> Create(TeacherViewModel teacher)
        {
            var res = TeachRepo.Add(teacher);

            if (res.IsCompletedSuccessfully)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: TeacherController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await TeachRepo.GetTeacher(id).ConfigureAwait(false);

            return View(teacher);
        }

        // POST: TeacherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeacherViewModel teacher)
        {
            var response = TeachRepo.Update(teacher);

            if (response.IsCompletedSuccessfully)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Edit));
            }
        }

        // POST: TeacherController/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var para = new SearchingParaModel()
            {
                SId = string.Empty,
                Name = string.Empty,
                Phone = string.Empty,
                PageIndex = 1
            };

            var response = TeachRepo.Delete(id);

            if (response.IsCompletedSuccessfully)
            {
                var teachers = await TeachRepo.GetTeachers(para).ConfigureAwait(false);
                return PartialView("_TeacherData", teachers);
            }
            return PartialView();
        }
    }
}
