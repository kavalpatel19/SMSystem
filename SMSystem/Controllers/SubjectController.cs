﻿using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Subject;
using SMSystem.Repository;

namespace SMSystem.Controllers
{
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
            SubjectPaggedViewModel subjects = new SubjectPaggedViewModel();

            return View(subjects);
        }

        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            SubjectPaggedViewModel subjects = await SubRepo.GetSubjects(para);
            return PartialView("_SubjectData", subjects);
        }

        public IActionResult ExportExcel()
        {
            var Data = SubRepo.GetAllSubjects();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ConvertDataTable.Convert(Data.ToList()));
                using (MemoryStream mstream = new MemoryStream())
                {
                    wb.SaveAs(mstream);
                    return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Subjects.xlsx");
                }
            }
        }

        // GET: DepartmentController/Create
        public async Task<IActionResult> Create()
        {
            SubjectViewModel subject = new SubjectViewModel();
            var subjects = SubRepo.GetAllSubjects();
            var lastId = subjects.OrderByDescending(x => x.SubjectId).FirstOrDefault().SubjectId;
            char[] spearator = { '-', ' ' };
            string[] subId = lastId.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
            int id = (Convert.ToInt32(subId[1])) + 1;
            subject.SubjectId = "SUB-" + (id.ToString("0000"));
            return View(subject);
        }

        // POST: DepartmentController/Create
        [HttpPost]
        public async Task<IActionResult> Create(SubjectViewModel subject)
        {
            try
            {
                var response = SubRepo.Add(subject);
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

        // GET: DepartmentController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            SubjectViewModel subject = await SubRepo.GetSubject(id);
            return View(subject);
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(SubjectViewModel subject)
        {
            try
            {
                var response = SubRepo.Update(subject);
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

                var response = SubRepo.Delete(id);

                if (response.IsCompletedSuccessfully)
                {
                    SubjectPaggedViewModel subjects = await SubRepo.GetSubjects(para);
                    return PartialView("_SubjectData", subjects);
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
