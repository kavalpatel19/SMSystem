using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models;
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
            try
            {
                var response = new TeacherPagedViewModel();
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
                var response = TeachRepo.GetAllTeachers();
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
            var response = new BaseResponseViewModel<TeacherPagedViewModel>();
            try
            {
                response = await TeachRepo.GetTeachers(para).ConfigureAwait(false);
                if (response.ResponseCode == 200)
                {
                    return PartialView("_TeacherData", response.Result);
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return PartialView("_TeacherData", response.Result);
                }
            }
            catch (Exception ex)
            {
                response.Result = new TeacherPagedViewModel();
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return PartialView("_TeacherData", response.Result);
            }
        }

        public IActionResult ExportExcel()
        {
            try
            {
                var response = TeachRepo.GetAllTeachers();
                if (response.ResponseCode == 200)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ConvertDataTable.Convert(response.Results));
                        using (var mstream = new MemoryStream())
                        {
                            wb.SaveAs(mstream);
                            return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Teachers.xlsx");
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

        // GET: TeacherController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await TeachRepo.GetTeacher(id).ConfigureAwait(false);
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

        // GET: TeacherController/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var teacher = new TeacherViewModel();
                var teachers = TeachRepo.GetAllTeachers().Results;
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
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: TeacherController/Create
        [HttpPost]
        public async Task<IActionResult> Create(TeacherViewModel teacher)
        {
            try
            {
                var response = await TeachRepo.Add(teacher);
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

        // GET: TeacherController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await TeachRepo.GetTeacher(id).ConfigureAwait(false);
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

        // POST: TeacherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeacherViewModel teacher)
        {
            try
            {
                var response = await TeachRepo.Update(teacher);
                if (response.ResponseCode == 200)
                {
                    TempData["Message"] = "Department has been saved successfully.";
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

        // POST: TeacherController/Delete/5
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

                var response = await TeachRepo.Delete(id);

                var teachers = await TeachRepo.GetTeachers(para).ConfigureAwait(false);
                return PartialView("_TeacherData", teachers.Result);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, responseCode = 500 });
            }
        }
    }
}
