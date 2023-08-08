using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Students;
using SMSystem.Repository;

namespace SMSystem.Controllers
{
    public class DepartmentController : Controller
    {
        IDepartmentRepository DepRepo;
        public DepartmentController(IDepartmentRepository depRepo)
        {
            DepRepo = depRepo;
        }


        //https://localhost:7199/api/DepartmentApi?pageIndex=1
        // GET: DepartmentController
        public async Task<IActionResult> Index(SearchingParaModel para)
        {
            DepartmentPaggedViewModel departments = new DepartmentPaggedViewModel();

            return View(departments);
        }

        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            DepartmentPaggedViewModel departments = await DepRepo.GetDepartmnets(para);
            return PartialView("_DepartmentData", departments);
        }
        public IActionResult ExportExcel()
        {
            var Data = DepRepo.GetAllDepartments();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ConvertDataTable.Convert(Data.ToList()));
                using (MemoryStream mstream = new MemoryStream())
                {
                    wb.SaveAs(mstream);
                    return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Departments.xlsx");
                }
            }
        }

        // GET: DepartmentController/Create
        public async Task<IActionResult> Create()
        {
            DepartmentViewModel department = new DepartmentViewModel();
            var departments = DepRepo.GetAllDepartments();
            var lastId = departments.OrderByDescending(x => x.DepartmentId).FirstOrDefault().DepartmentId;
            char[] spearator = { '-', ' ' };
            string[] depId = lastId.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
            int id = (Convert.ToInt32(depId[1]))+1;
            department.DepartmentId = "DEP-" + (id.ToString("0000"));
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
                    return  RedirectToAction(nameof(Index));
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

        // GET: DepartmentController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            DepartmentViewModel department = await DepRepo.GetDepartment(id);
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
                SearchingParaModel para = new SearchingParaModel()
                {
                    SId = string.Empty,
                    Name = string.Empty,
                    Year = string.Empty,
                    PageIndex = 1
                };

                var response = DepRepo.Delete(id);

                if (response.IsCompletedSuccessfully)
                {
                    DepartmentPaggedViewModel departments = await DepRepo.GetDepartmnets(para);
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
