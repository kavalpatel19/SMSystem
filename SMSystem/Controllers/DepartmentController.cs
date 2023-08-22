 using Azure;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Students;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Controllers
{
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
            var response = new BaseResponseViewModel<DepartmentPaggedViewModel>();

            try
            {
                response = new BaseResponseViewModel<DepartmentPaggedViewModel>();
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
                    return PartialView("_DepartmentData", response);
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["ResCode"] = response.ResponseCode;
                    return PartialView("_DepartmentData", response);
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new DepartmentPaggedViewModel();
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return PartialView("_DepartmentData", response);
            }
        }

        //To Export Data
        public IActionResult ExportExcel()
        {
            var response = new BaseResponseViewModel<DepartmentViewModel>();
            try
            {
                response = DepRepo.GetAllDepartments();
                if (response.ResponseCode == 200)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ConvertDataTable.Convert(response.Results));
                        using (var mstream = new MemoryStream())
                        {
                            wb.SaveAs(mstream);
                            TempData["Message"] = "Data Downloaded successfully.";
                            TempData["ResCode"] = response.ResponseCode;
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
                return View("Index");
            }
        }

        // GET: DepartmentController/Create
        public async Task<IActionResult> Create()
        {
            var response = new BaseResponseViewModel<DepartmentViewModel>();

            try
            {
                response = DepRepo.GetAllDepartments();
                var department = new DepartmentViewModel();
                var departments = response.Results;
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
            var response = new BaseResponseViewModel<DepartmentViewModel>();

            try
            {
                response = await DepRepo.Add(department);
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
            var response = new BaseResponseViewModel<DepartmentViewModel>();

            try
            {
                response = await DepRepo.GetDepartment(id).ConfigureAwait(false);
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
            var response = new BaseResponseViewModel<DepartmentViewModel>();

            try
            {
                response = await DepRepo.Update(department);
                if (response.ResponseCode == 200)
                {
                    TempData["Message"] = "Record Modified Successfully.";
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
            var response = new BaseResponseViewModel<DepartmentViewModel>();

            try
            {
                var para = new SearchingParaModel()
                {
                    SId = string.Empty,
                    Name = string.Empty,
                    Year = string.Empty,
                    PageIndex = 1
                };

                response = await DepRepo.Delete(id);

                var departments = await DepRepo.GetDepartmnets(para).ConfigureAwait(false);
                return PartialView("_DepartmentData", departments); 
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["ResCode"] = 500;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
