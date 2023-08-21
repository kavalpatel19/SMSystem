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
            try
            {
                var response = new BaseResponseViewModel<DepartmentPaggedViewModel>();

                return View(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponseViewModel<DepartmentPaggedViewModel>
                {
                    ResponseCode = 500,
                    Message = ex.Message,
                    Result = new DepartmentPaggedViewModel()
                };
                return View(response);
            }
            
        }

        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            try
            {
                var response = await DepRepo.GetDepartmnets(para).ConfigureAwait(false);
                return PartialView("_DepartmentData", response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponseViewModel<DepartmentPaggedViewModel>
                {
                    ResponseCode = 500,
                    Message = ex.Message,
                    Result = new DepartmentPaggedViewModel()
                };
                return View(response);
            }
        }

        public IActionResult ExportExcel()
        {
            var data = DepRepo.GetAllDepartments().Results;

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ConvertDataTable.Convert(data.ToList()));
                using (var mstream = new MemoryStream())
                {
                    wb.SaveAs(mstream);
                    return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Departments.xlsx");
                }
            }
        }

        // GET: DepartmentController/Create
        public async Task<IActionResult> Create()
        {
            var department = new DepartmentViewModel();
            var departments = DepRepo.GetAllDepartments().Results;
            if (departments.Count > 0)
            {
                var lastId = departments.OrderByDescending(x => x.DepartmentId).FirstOrDefault().DepartmentId;
                char[] spearator = { '-', ' ' };
                string[] depId = lastId.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
                int id = (Convert.ToInt32(depId[1]))+1;
                department.DepartmentId = "DEP-" + (id.ToString("0000"));
            }
            else
            {
                department.DepartmentId = "DEP-0001";
            }
            return View(department);
        }

        // POST: DepartmentController/Create
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel department)
        {
            try
            {
                var response = DepRepo.Add(department);
                if (response.IsCompletedSuccessfully)
                {
                    TempData["Msg"] = "Record Created Successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Msg"] = "Something Went Wrong!";
                    return RedirectToAction(nameof(Create));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var department = await DepRepo.GetDepartment(id).ConfigureAwait(false);
            return View(department);
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentViewModel department)
        {
            try
            {
                var response = DepRepo.Update(department);
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

                var response = DepRepo.Delete(id);

                if (response.IsCompletedSuccessfully)
                {
                    var departments = await DepRepo.GetDepartmnets(para).ConfigureAwait(false);
                    return PartialView("_DepartmentData", departments);
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
