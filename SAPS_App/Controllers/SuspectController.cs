using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SAPS_App.Areas.Identity.Pages;
using SAPS_App.Context;
using SAPS_App.Models;
using System.Security.Claims;

namespace SAPS_App.Controllers
{
    public class SuspectController : Controller
    {
        public readonly SAPS_Context _db;
        public SuspectController(SAPS_Context db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            //IEnumerable<Suspect> suspects = _db.Suspects;
            var suspects = _db.Suspects.Include(s => s.CriminalRecords).ToList();

            //Calculate total criminal records in the database
            var totalCriminalRecords = _db.CriminalRecords.Count();
            ViewBag.TotalCriminalRecords = totalCriminalRecords;
            return View(suspects);
        }
        //ADD SUSPECTS
        //GET
        public IActionResult AddSuspects()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSuspects(Suspect obj)
        {
            if (_db.Suspects.Any(s => s.SuspectId == obj.SuspectId))
            {
                //TempData["duplicate"] = "A suspect with " + obj.SuspectId + " already exists in the database";
                return BadRequest(new {message = "A suspect with " + obj.SuspectId + " already exists in the database." });
            }
            try
            {
                _db.Suspects.Add(obj);
                _db.SaveChanges();
                //TempData["success"] = $"{obj.FirstName} {obj.LastName} is successfully added to the database.";
                return Ok(new
                {
                    message = $"{obj.FirstName} {obj.LastName} is successfully added to the database.",
                    redirectUrl = Url.Action("Index","Suspect")
            });
            }
            catch (Exception ex)
            {
                //TempData["error"] = $"An error occured while adding {obj.FirstName} {obj.LastName} .";
                return BadRequest(new {message = $"An error occured while adding {obj.FirstName} {obj.LastName} ." });
            }
            return RedirectToAction("Index");
        }
        //SEARCH SUSPECT
        public IActionResult SearchSuspect()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchSuspect(string suspectId)
        {
            ViewBag.Input = suspectId;
            var suspects = _db.Suspects.ToList();
            var suspectNo = 0;
            foreach (var suspect in suspects)
            {
                if (suspect.SuspectId == suspectId)
                {
                    suspectNo = suspect.SuspectNumber;
                }
            }
            ViewBag.SuspectNumber = suspectNo;
            var foundSuspect = _db.Suspects.Find(suspectNo);
            TempData["SuspectNumber"] = suspectNo;
            //ViewBag.SuspectNumber = foundSuspect.SuspectNumber;

            var records = _db.CriminalRecords.ToList();
            List<CriminalRecord> criminalRecords = new List<CriminalRecord>();
            foreach (var record in records)
            {
                if (record.SuspectNumber == suspectNo)
                {
                    criminalRecords.Add(record);
                }
            }
            var viewModel = new SuspectSearch
            {
                Suspect = foundSuspect,
                CriminalRecords = criminalRecords,

            };
            return View(viewModel);

        }
        //EDIT SUSPECT
        public IActionResult EditSuspect(int? id)//right-click to create a View
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var suspectFromDb = _db.Suspects.Find(id);

            if (suspectFromDb == null)
            {
                return NotFound();
            }
            return View(suspectFromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSuspect(Suspect obj)
        {
            try
            {
                _db.Suspects.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Suspect information is successfully edited.";
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occured while editing suspect information .";
            }
            // Redirect to another action or view after successful submission
            return RedirectToAction("Index");

        }
        public IActionResult DownloadSuspects()
        {


            //var employees = connection.Query<dynamic>("SELECT * FROM [Admin-Valuer] WHERE Sector = @UserSector", new { UserSector = userSector });
            var suspects = _db.Suspects.Include(s=>s.CriminalRecords).ToList();
            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Suspects");//creates an Excel package using the EPPlus
                worksheet.Cells.LoadFromCollection(suspects, true);//loads the data from the fetched records into the workshee
                byte[] fileContents = package.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Suspects.xlsx");
            }
        }
        public IActionResult ShowSuspectDetails(int id)
        {
            var suspect = _db.Suspects.FirstOrDefault(s => s.SuspectNumber == id);
            if (suspect == null)
            {
                return NotFound(); // Return 404 if suspect is not found
            }

            var criminalRecords = _db.CriminalRecords.Where(cr => cr.SuspectNumber == id).ToList();

            var viewModel = new SuspectDetailsViewModel
            {
                Suspect = suspect,
                CriminalRecords = criminalRecords
            };

            return View(viewModel);
        }

    }
    ////  MANAGER CASES
    //public IActionResult ManagerCases()
    //{
    //    return View();
    //}


    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult ManagerCases(string email)
    //{
    //    ViewBag.Input = email;
    //    var managers = _db.Case_Managers.ToList();
    //    var managerNo = 0;
    //    foreach (var manager in managers)
    //    {
    //        if (manager.Email == email)
    //        {
    //            managerNo = manager.CaseManagerNo;
    //        }
    //    }
    //    ViewBag.ManagerNumber = managerNo;
    //    var foundManager = _db.Case_Managers.Find(managerNo);
    //    TempData["SuspectNumber"] = managerNo;
    //    //ViewBag.SuspectNumber = foundSuspect.SuspectNumber;

    //    var records = _db.CriminalRecords.ToList();
    //    List<CriminalRecord> criminalRecords = new List<CriminalRecord>();
    //    foreach (var record in records)
    //    {
    //        if (record.CaseManagerNo == managerNo)
    //        {
    //            criminalRecords.Add(record);
    //        }
    //    }
    //    var viewModel = new ManagerCases
    //    {
    //        Managers = foundManager,
    //        CriminalRecords = criminalRecords,

    //    };
    //    return View(viewModel);

    //}


}

