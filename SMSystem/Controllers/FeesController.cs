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
            var fees = new FeesPaggedViewModel();

            return View(fees);
        }

        public async Task<IActionResult> GetAll(SearchingParaModel para)
        {
            var fees = await FeesRepo.GetFees(para).ConfigureAwait(false);
            return PartialView("_FeesData", fees);
        }

        public IActionResult ExportExcel()
        {
            var data = FeesRepo.GetAllFees();

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ConvertDataTable.Convert(data.ToList()));
                using (var mstream = new MemoryStream())
                {
                    wb.SaveAs(mstream);
                    return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Fees.xlsx");
                }
            }
        }
        public async Task<IActionResult> Create()
        {
            var fee = new FeesViewModel();
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
            var fee = await FeesRepo.GetFee(id).ConfigureAwait(false);
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
                var para = new SearchingParaModel()
                {
                    PageIndex = 1
                };

                var response = FeesRepo.Delete(id);

                if (response.IsCompletedSuccessfully)
                {
                    var fees = await FeesRepo.GetFees(para).ConfigureAwait(false);
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
