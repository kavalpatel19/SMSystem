using Azure;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models.Students;
using SMSystem.Repository.Interfaces;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Reflection;
using static Microsoft.VisualStudio.Services.Graph.GraphResourceIds;

namespace SMSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository StdRepo;

        public StudentController(IStudentRepository repository)
        {
            StdRepo = repository;
        }

        // Index
        public async Task<IActionResult> Index(SearchingParaModel para)
        {
            var students = new StudentPagedViewModel();
            return View(students);
        }

        public async Task<IActionResult> GridIndex()
        {
            var Students = StdRepo.GetAllStudents();
            return View(Students);
        }

        // Returning data to the view
        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            var students = await StdRepo.GetStudents(para).ConfigureAwait(false);

            return PartialView("_StudentData", students);
        }

        public IActionResult ExportExcel()
        {
            var data = StdRepo.GetAllStudents();

                using (var wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ConvertDataTable.Convert(data.ToList()));
                    using (var mstream = new MemoryStream())
                    {
                        wb.SaveAs(mstream);
                        return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students.xlsx");
                    }
                }

        }

        // GET: StudentController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var student = await StdRepo.GetStudent(id).ConfigureAwait(false);

            return View(student);
        }

        // GET: StudentController/Create
        public async Task<IActionResult> Create()
        {
            var student = new StudentViewModel();
            var students = StdRepo.GetAllStudents();
            if (students.Count > 0)
            {
                var lastId = students.OrderByDescending(x => x.StudentId).FirstOrDefault().StudentId;
                char[] spearator = { '-', ' ' };
                string[] stdId = lastId.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
                int id = (Convert.ToInt32(stdId[1])) + 1;
                student.StudentId = "STD-" + (id.ToString("0000"));
            }
            else
            {
                student.StudentId = "STD-0001";
            }
            
            return View(student);
        }

        // POST: StudentController/Create
        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel student)
        {
            var res = StdRepo.Add(student);

            if (res.IsCompletedSuccessfully)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        // GET: StudentController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var student = await StdRepo.GetStudent(id).ConfigureAwait(false);

            return View(student);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentViewModel student)
        {
            var response = StdRepo.Update(student);

            if (response.IsCompletedSuccessfully)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        // POST: StudentController/Delete/5
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

            var response = StdRepo.Delete(id);

            if (response.IsCompletedSuccessfully)
            {
                var students = await StdRepo.GetStudents(para).ConfigureAwait(false);
                return PartialView("_StudentData", students);
            }
            return PartialView();
        }

        public IActionResult EmailExist(string email ,int id)
        {
            var Students = StdRepo.GetAllStudents().Where(x => x.Email == email).FirstOrDefault();
            

            if(Students != null)
            {
                if (id > 0 && id == Students.Id)
                {
                    return Json(true);
                }
                return Json("Email Already Exist!");
            }
            return Json(true);
            
        }
    }
}


