using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAPS_App.Areas.Identity.Pages;
using SAPS_App.Context;
using SAPS_App.Models;

namespace SAPS_App.Controllers
{
    public class CriminalRecordController : Controller
    {
        private readonly SAPS_Context _db;
        public CriminalRecordController(SAPS_Context db)
        {
            _db = db;
        }
        //CRIMINAL RECORDS
        public IActionResult Index()
        {
            IEnumerable<Models.CriminalRecord> objCriminalRecordList = _db.CriminalRecords;
            return View(objCriminalRecordList);
            //return View();
        }
        //EDIT BUTTON

        //GET
        public IActionResult EditRecord(int? id)//right-click to create a View
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var customerFromDb = _db.CriminalRecords.Find(id);

            if (customerFromDb == null)
            {
                return NotFound();
            }
            return View(customerFromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRecord(CriminalRecord obj)
        {
            try
            {
                _db.CriminalRecords.Update(obj);//Update new row to the database
                _db.SaveChanges();//Gos to the database and save the changes(store new row)
                TempData["succes"] = "Criminal record is successfully edited.";
            }
            catch (Exception ex) 
            {
                TempData["error"] = "An error occured while editing criminal record.";
            }
            return View();

        }
        //ADD CRIMINAL RECORD
            
        //GET
        public IActionResult AddRecords(int id)//right-click to create a View
        {
            TempData["SuspectNumber"] = id;
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRecords(CriminalRecord obj)
        {
            obj.Id = 0;//To fix the error (SqlException: Cannot insert explicit value for identity column in table 'CriminalRecords' when IDENTITY_INSERT is set to OFF.)
            // Check if the SuspectNumber exists in the Suspects table
            var existingSuspect = _db.Suspects.FirstOrDefault(s => s.SuspectNumber == obj.SuspectNumber);

            if (existingSuspect == null)
            {
                // If the SuspectNumber doesn't exist, return an error message
                TempData["error"] = "The specified suspect number does not exist.";
                return View();
            }

            // Proceed with adding the record

            var managers = _db.Case_Managers.Include(c => c.CriminalRecords).ToList();

            if (managers.Count > 0)
            {
                // Find the minimum number of cases
                var minCases = managers.Min(m => m.TotalCases);

                // Filter managers with the minimum number of cases
                var managersWithMinCases = managers.Where(m => m.TotalCases == minCases).ToList();

                if (managersWithMinCases.Count > 0)
                {
                    // If there are multiple managers with the same minimum number of cases, randomly select one
                    var random = new Random();
                    var randomManager = managersWithMinCases[random.Next(managersWithMinCases.Count)];

                    // Assign ManagerId and IssuedBy to the CriminalRecord
                    obj.CaseManagerNo = randomManager.CaseManagerNo;
                    obj.CaseManagerId = randomManager.CaseManagerId;
                    obj.CaseManagerName = $"{randomManager.Name} {randomManager.Surname}";

                    _db.CriminalRecords.Add(obj);
                    _db.SaveChanges();

                    TempData["success"] = "Criminal record is successfully added to the database.";
                    return View();
                }
                else
                {
                    TempData["error"] = "No managers available to assign the criminal record.";
                    return View();
                }
            }
            else
            {
                TempData["error"] = "No managers available to assign the criminal record.";
                return View();
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult AddRecords(CriminalRecord obj)
        //{

        //    var managers = _db.Case_Managers.Include(c => c.CriminalRecords).ToList();
        //    //var manager = _db.ApplicationUsers.Include(s=>s.CriminalRecords).ToList();

        //    if (managers.Count > 0)
        //    {
        //        // Find the minimum number of cases
        //        var minCases = managers.Min(m => m.TotalCases);

        //        // Filter managers with the minimum number of cases
        //        var managersWithMinCases = managers.Where(m => m.TotalCases == minCases).ToList();

        //        if (managersWithMinCases.Count > 0)
        //        {
        //            // If there are multiple managers with the same minimum number of cases, randomly select one
        //            var random = new Random();
        //            var randomManager = managersWithMinCases[random.Next(managersWithMinCases.Count)];

        //            // Assign ManagerId and IssuedBy to the CriminalRecord
        //            obj.CaseManagerNo = randomManager.CaseManagerNo;
        //            obj.CaseManagerId = randomManager.CaseManagerId;
        //            obj.CaseManagerName = $"{randomManager.Name} {randomManager.Surname}";

        //            _db.CriminalRecords.Add(obj);
        //            _db.SaveChanges();

        //            TempData["SuccessMessage"] = "Criminal record is successfully added to the database.";
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "No managers available to assign the criminal record.";
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "No managers available to assign the criminal record.";
        //        return RedirectToAction("Index");
        //    }
        //}
        public int TotalOffences(int suspectNumber)
        {
            var records = _db.CriminalRecords.Where(cr => cr.SuspectNumber == suspectNumber).ToList();
            return records.Count;
        }
        public IActionResult DownloadCases()
        {


            //var employees = connection.Query<dynamic>("SELECT * FROM [Admin-Valuer] WHERE Sector = @UserSector", new { UserSector = userSector });
            var cases = _db.CriminalRecords.ToList();
            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Cases");//creates an Excel package using the EPPlus
                worksheet.Cells.LoadFromCollection(cases, true);//loads the data from the fetched records into the workshee
                byte[] fileContents = package.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Cases.xlsx");
            }
        }
        //Action to download all the tables (Suspects,CriminalRecords, and Case_Managers
        public IActionResult DownloadCombinedReport()
        {
            // Fetch the data
            var caseManagers = _db.Case_Managers.Include(s => s.CriminalRecords).ToList();
            var criminalRecords = _db.CriminalRecords.ToList();
            var suspects = _db.Suspects.Include(s => s.CriminalRecords).ToList();

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                // Create a worksheet
                var worksheet = package.Workbook.Worksheets.Add("CombinedReport");

                // Load the Case Managers data
                worksheet.Cells["A1"].LoadFromCollection(caseManagers, true);

                // Find the row number where the Criminal Records data should start
                int caseManagersRowCount = caseManagers.Count + 3; // +1 for header, +1 for an empty row
                worksheet.Cells[$"A{caseManagersRowCount}"].LoadFromCollection(criminalRecords, true);

                // Find the row number where the Suspects data should start
                int criminalRecordsRowCount = caseManagersRowCount + criminalRecords.Count + 2; // +1 for header, +1 for an empty row
                worksheet.Cells[$"A{criminalRecordsRowCount}"].LoadFromCollection(suspects, true);

                // Get the combined file as a byte array
                byte[] fileContents = package.GetAsByteArray();

                // Return the combined Excel file
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CombinedReport.xlsx");
            }
        }



    }
}
