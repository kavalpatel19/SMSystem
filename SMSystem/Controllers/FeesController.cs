using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Fees;
using SMSystem.Repository.Interfaces;

namespace SMSystem.Controllers
{
    public class FeesController : Controller
    {
        private readonly IFeesRepository FeesRepo;

        public FeesController(IFeesRepository FeesRepo)
        {
            this.FeesRepo = FeesRepo;
        }

        public async Task<IActionResult> Index()
        {
            FeesPaggedViewModel fees = new FeesPaggedViewModel();

            return View(fees);
        }

        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            FeesPaggedViewModel fees = await FeesRepo.GetFees(para);
            return PartialView("_FeesData", fees);
        }

        public IActionResult ExportExcel()
        {
            var Data = FeesRepo.GetAllFees();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ConvertDataTable.Convert(Data.ToList()));
                using (MemoryStream mstream = new MemoryStream())
                {
                    wb.SaveAs(mstream);
                    return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Fees.xlsx");
                }
            }
        }
        public async Task<IActionResult> Create()
        {
            FeesViewModel fee = new FeesViewModel();
            var fees = FeesRepo.GetAllFees();
            if (fees.Count > 0)
            {
                var lastId = fees.OrderByDescending(x => x.FeesId).FirstOrDefault().FeesId;
                char[] spearator = { '-', ' ' };
                string[] feeId = lastId.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
                int id = (Convert.ToInt32(feeId[1])) + 1;
                fee.FeesId = "FEES-" + (id.ToString("0000"));
            }
            else
            {
                fee.FeesId = "DEP-0001";
            }
            return View(fee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeesViewModel fee)
        {
            try
            {
                var response = FeesRepo.Add(fee);
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

        public async Task<IActionResult> Edit(int id)
        {
            FeesViewModel fee = await FeesRepo.GetFee(id);
            return View(fee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FeesViewModel fee)
        {
            try
            {
                var response = FeesRepo.Update(fee);
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

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                SearchingParaModel para = new SearchingParaModel()
                {
                    PageIndex = 1
                };

                var response = FeesRepo.Delete(id);

                if (response.IsCompletedSuccessfully)
                {
                    FeesPaggedViewModel fees = await FeesRepo.GetFees(para);
                    return PartialView("_FeesData", fees);
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
