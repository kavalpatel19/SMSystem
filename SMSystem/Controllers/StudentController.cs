using Azure;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Student;
using SMSystem.Models.Students;
using SMSystem.Repository.Interfaces;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Reflection;
using static Microsoft.VisualStudio.Services.Graph.GraphResourceIds;

namespace SMSystem.Controllers
{
    [Authorize]
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
            try
            {
                var response = new StudentPagedViewModel();
                return View(response);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> GridIndex()
        {
            try
            {
                var response = StdRepo.GetAllStudents();
                if (response.ResponseCode == 200)
                {
                    return View(response.Results);
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

        // Returning data to the view
        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            var response = new BaseResponseViewModel<StudentPagedViewModel>();
            try
            {
                response = await StdRepo.GetStudents(para).ConfigureAwait(false);
                if (response.ResponseCode == 200)
                {
                    return PartialView("_StudentData", response.Result);
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return PartialView("_StudentData", response.Result);
                }
            }
            catch (Exception ex)
            {
                response.Result = new StudentPagedViewModel();
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return PartialView("_StudentData", response.Result);
            }
        }

        [Authorize(Roles ="admin , teacher")]
        public IActionResult ExportExcel()
        {
            try
            {
                var response = StdRepo.GetAllStudents();
                if (response.ResponseCode == 200)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ConvertDataTable.Convert(response.Results));
                        using (var mstream = new MemoryStream())
                        {
                            wb.SaveAs(mstream);
                            return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students.xlsx");
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

        // GET: StudentController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await StdRepo.GetStudent(id).ConfigureAwait(false);
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

        // GET: StudentController/Create
        [Authorize(Roles ="admin , teacher")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var student = new StudentViewModel();
                var students = StdRepo.GetAllStudents().Results;
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
                var register = new StudentRegisterViewModel()
                {
                    StudentModel = student,
                    UserModel = new Models.Auth.ApplicationUser()
                };
                return View(register);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: StudentController/Create
        [Authorize(Roles ="admin , teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(StudentRegisterViewModel register)
        {
            try
            {
                var response = await StdRepo.Add(register);
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

        // GET: StudentController/Edit/5
        [Authorize(Roles ="admin , teacher")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await StdRepo.GetStudent(id).ConfigureAwait(false);
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

        // POST: StudentController/Edit/5
        [Authorize(Roles ="admin , teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentViewModel student)
        {
            try
            {
                var response = await StdRepo.Update(student);
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

        // POST: StudentController/Delete/5
        [Authorize(Roles ="admin , teacher")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var para = new SearchingParaModel()
                {
                    SId = string.Empty,
                    Name = string.Empty,
                    Year = string.Empty,
                    PageIndex = 1
                };

                var response = await StdRepo.Delete(id);

                var students = await StdRepo.GetStudents(para).ConfigureAwait(false);
                return PartialView("_StudentData", students.Result);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, responseCode = 500 });
            }
        }

    }
}


