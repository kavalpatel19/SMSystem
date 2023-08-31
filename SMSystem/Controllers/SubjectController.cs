using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Subject;
using SMSystem.Repository.Interfaces;
using static Microsoft.VisualStudio.Services.Graph.GraphResourceIds;

namespace SMSystem.Controllers
{
    [Authorize]
    public class SubjectController : Controller
    {
        private readonly ISubjectRepository SubRepo;

        public SubjectController(ISubjectRepository SubRepo)
        {
            this.SubRepo = SubRepo;
        }

        // GET: DepartmentController
        public async Task<IActionResult> Index(SearchingParaModel para)
        {
            try
            {
                var response = new SubjectPaggedViewModel();
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
            var response = new BaseResponseViewModel<SubjectPaggedViewModel>();
            try
            {
                response = await SubRepo.GetSubjects(para).ConfigureAwait(false);
                if (response.ResponseCode == 200)
                {
                    return PartialView("_SubjectData", response.Result);
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return PartialView("_SubjectData", response.Result);
                }
            }
            catch (Exception ex)
            {
                response.Result = new SubjectPaggedViewModel();
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return PartialView("_SubjectData", response.Result);
            }
        }

        public IActionResult ExportExcel()
        {
            try
            {
                var response = SubRepo.GetAllSubjects();
                if (response.ResponseCode == 200)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ConvertDataTable.Convert(response.Results));
                        using (var mstream = new MemoryStream())
                        {
                            wb.SaveAs(mstream);
                            return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Subjects.xlsx");
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

        // GET: DepartmentController/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var subject = new SubjectViewModel();
                var subjects = SubRepo.GetAllSubjects().Results;
                if (subjects.Count > 0)
                {
                    var lastId = subjects.OrderByDescending(x => x.SubjectId).FirstOrDefault().SubjectId;
                    char[] spearator = { '-', ' ' };
                    string[] subId = lastId.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
                    int id = (Convert.ToInt32(subId[1])) + 1;
                    subject.SubjectId = "SUB-" + (id.ToString("0000"));
                }
                else
                {
                    subject.SubjectId = "SUB-0001";
                }

                return View(subject);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DepartmentController/Create
        [HttpPost]
        public async Task<IActionResult> Create(SubjectViewModel subject)
        {
            try
            {
                var response = await SubRepo.Add(subject);
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

        // GET: DepartmentController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await SubRepo.GetSubject(id).ConfigureAwait(false);
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

        // POST: DepartmentController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(SubjectViewModel subject)
        {
            try
            {
                var response = await SubRepo.Update(subject);
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

        // POST: DepartmentController/Delete/5
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

                var response = await SubRepo.Delete(id);

                var subjects = await SubRepo.GetSubjects(para).ConfigureAwait(false);
                return PartialView("_SubjectData", subjects.Result);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, responseCode = 500 });
            }
        }
    }
}
