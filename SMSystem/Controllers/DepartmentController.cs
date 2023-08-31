 using Azure;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Students;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository DepRepo;
        public DepartmentController(IDepartmentRepository depRepo)
        {
            DepRepo = depRepo;
        }

        // GET: DepartmentController
        public async Task<IActionResult> Index(SearchingParaModel para)
        {
            try
            {
                //throw new Exception();

                var response = new DepartmentPaggedViewModel();
                return View(response);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction("Index","Home");
            }
        }

        // returning Partial view
        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            var response = new BaseResponseViewModel<DepartmentPaggedViewModel>();
            try
            {
                response = await DepRepo.GetDepartmnets(para).ConfigureAwait(false);
                if(response.ResponseCode == 200)
                {
                    return PartialView("_DepartmentData", response.Result);
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return PartialView("_DepartmentData", response.Result);
                }
            }
            catch (Exception ex)
            {
                response.Result = new DepartmentPaggedViewModel();
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return PartialView("_DepartmentData", response.Result);
            }
        }

        //To Export Data
        public IActionResult ExportExcel()
        {
            try
            {
                var response = DepRepo.GetAllDepartments();
                if (response.ResponseCode == 200)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ConvertDataTable.Convert(response.Results));
                        using (var mstream = new MemoryStream())
                        {
                            wb.SaveAs(mstream);
                            
                            return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Departments.xlsx");
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
            catch(Exception ex)
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
                var department = new DepartmentViewModel();
                var departments = DepRepo.GetAllDepartments().Results;
                if (departments.Count > 0)
                {
                    var lastId = departments.OrderByDescending(x => x.DepartmentId).FirstOrDefault().DepartmentId;
                    char[] spearator = { '-', ' ' };
                    string[] depId = lastId.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
                    int id = (Convert.ToInt32(depId[1])) + 1;
                    department.DepartmentId = "DEP-" + (id.ToString("0000"));
                }
                else
                {
                    department.DepartmentId = "DEP-0001";
                }

                return View(department);
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
        public async Task<IActionResult> Create(DepartmentViewModel department)
        {
            try
            {
                var response = await DepRepo.Add(department);
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
            catch(Exception ex)
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
                var response = await DepRepo.GetDepartment(id).ConfigureAwait(false);
                if(response.ResponseCode == 200)
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
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentViewModel department)
        {
            try
            {
                var response = await DepRepo.Update(department);
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

                var response = await DepRepo.Delete(id);

                var departments = await DepRepo.GetDepartmnets(para).ConfigureAwait(false);
                return PartialView("_DepartmentData", departments.Result); 
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message , responseCode = 500});
            }
        }
    }
}
