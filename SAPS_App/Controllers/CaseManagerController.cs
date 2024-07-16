using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAPS_App.Areas.Identity.Pages;
using SAPS_App.Context;
using SAPS_App.Models;

namespace SAPS_App.Controllers
{
    public class CaseManagerController : Controller
    {
        private readonly SAPS_Context _db;
        public CaseManagerController(SAPS_Context db)
        {
            _db = db;
        }
        //VIEW MANAGERS
        public IActionResult Index()
        {
            var managers = _db.Case_Managers.Include(c => c.CriminalRecords).ToList();
            var TotalCases = _db.CriminalRecords.Count();
            var totalCriminalRecords = _db.CriminalRecords.Count();
            ViewBag.TotalCriminalRecords = totalCriminalRecords;
            ViewBag.TotalCases = TotalCases;
            return View(managers);
        }
        //SEARCH MANAGER
        public IActionResult SearchManager()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchManager(string email)
        {
            ViewBag.Input = email;
            var managers = _db.Case_Managers.ToList();
            var managerNo = 0;
            foreach (var manager in managers)
            {
                if (manager.Email == email)
                {
                    managerNo = manager.CaseManagerNo;
                }
            }
            ViewBag.ManagerNumber = managerNo;
            var foundManager = _db.Case_Managers.Find(managerNo);
            TempData["SuspectNumber"] = managerNo;
            //ViewBag.SuspectNumber = foundSuspect.SuspectNumber;

            var records = _db.CriminalRecords.ToList();
            List<CriminalRecord> criminalRecords = new List<CriminalRecord>();
            foreach (var record in records)
            {
                if (record.CaseManagerNo == managerNo)
                {
                    criminalRecords.Add(record);
                }
            }
            var viewModel = new SearchManager
            {
                Managers = foundManager,
                CriminalRecords = criminalRecords,

            };
            return View(viewModel);

        }
        //EDIT CASE MANAGER
        public IActionResult EditManager(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var managerFromDb = _db.Case_Managers.Find(id);
            if (managerFromDb == null)
            {
                return NotFound();
            }
            return View(managerFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditManager(CaseManager obj)
        {
            _db.Case_Managers.Update(obj);
            _db.SaveChanges();
            TempData["SuccessMessage"] = "Case Manager is successfully edited.";
            return RedirectToAction("ViewManagers");

        }
        public IActionResult ShowCaseManagerDetails(int id)
        {
            var manager = _db.Case_Managers.FirstOrDefault(s => s.CaseManagerNo == id);
            if (manager == null)
            {
                return NotFound(); // Return 404 if suspect is not found
            }

            var criminalRecords = _db.CriminalRecords.Where(cr => cr.CaseManagerNo == manager.CaseManagerNo).ToList();
            // Retrieve suspects associated with the criminal records and the manager
            var suspectNumbers = criminalRecords.Select(cr => cr.SuspectNumber).ToList();
            var suspects = _db.Suspects.Where(s => suspectNumbers.Contains(s.SuspectNumber)).ToList();
            var viewModel = new CaseManagerDetailsViewModel
            {
                CaseManager = manager,
                CriminalRecords = criminalRecords,
                Suspects = suspects,
            };

            return View(viewModel);
        }
        public IActionResult DownloadCaseManagers()
        {


            //var employees = connection.Query<dynamic>("SELECT * FROM [Admin-Valuer] WHERE Sector = @UserSector", new { UserSector = userSector });
            var managers = _db.Case_Managers.Include(s => s.CriminalRecords).ToList();
            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("CaseManagers");//creates an Excel package using the EPPlus
                worksheet.Cells.LoadFromCollection(managers, true);//loads the data from the fetched records into the workshee
                byte[] fileContents = package.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CaseManagers.xlsx");
            }
        }
    }
}
