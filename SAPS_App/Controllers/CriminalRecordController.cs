using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAPS_App.Areas.Identity.Pages;
using SAPS_App.Context;
using SAPS_App.Models;
using SAPS_App.Services;
using System.Runtime.ConstrainedExecution;

namespace SAPS_App.Controllers
{
    public class CriminalRecordController : Controller
    {
        private readonly SAPS_Context _db;
        private readonly SAPS_Services _services;
        private readonly EmailSender emailSender;
        private readonly ISAPSService sapsService;
        private readonly UserManager<IdentityUser> userManager;

        public CriminalRecordController(SAPS_Context db
            ,SAPS_Services services
            ,EmailSender emailSender,
            ISAPSService sapsService,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _services = services;
            this.emailSender = emailSender;
            this.sapsService = sapsService;
            this.userManager = userManager;
        }
        //CRIMINAL RECORDS
        public IActionResult Index()
        {
            IEnumerable<CriminalRecord> objCriminalRecordList = _db.CriminalRecords;
            return View(objCriminalRecordList);
            //return View();
        }
        //ADD CRIMINAL RECORD

        //GET
        [Authorize(Roles ="Police Officer")]
        public async Task<IActionResult> AddRecordsAsync(int id)//right-click to create a View
        {
            TempData["SuspectNumber"] = id;
			return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRecordsAsync(CriminalRecord obj)
        {
			//if (!ModelState.IsValid)
			//{
			//	return BadRequest(new { message = "Validation failed." });
			//}
            //obj.IssueDate = DateTime.Now;
            //obj.Status = "Opened";
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

            var managers = _db.Case_Managers.Where(c => c.IsActive == true).Include(c => c.CriminalRecords).ToList();
            try
            {
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
                        var appUser = await sapsService.GetAplicationUserUserByIdAsync(obj.CaseManagerId);

                        //Send Email 
                        var body = $@"Good day {obj.CaseManagerName}, <br><br>
                                   A new case has been assigned to you, Case Id is {obj.Id}.
                                   <br><br>
                                   Kind Regards,<br> SAPS Management Admin.";

                        await emailSender.SendEmailAsync(appUser.UserName, "Case Assignment", body);
                        _db.CriminalRecords.Add(obj);
                        _db.SaveChanges();
                        return Ok(new
                        {
                            message = "Criminal record is successfully added to the database.",
                            redirectUrl = Url.Action("Index", "CriminalRecord")
                        });
                        //TempData["success"] = "Criminal record is successfully added to the database.";
                    }
                    else
                    {
                        return BadRequest(new { message = "No managers available to assign the criminal record." });
                    }
                }
                else
                {
                    //TempData["error"] = "No managers available to assign the criminal record.";
                    return BadRequest(new { message = "No managers available to assign the criminal record." });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
          
        }
        //EDIT BUTTON

        //GET
        [Authorize(Roles = "Police Officer,Case Manager,Station Manager")]
        public async Task<IActionResult> EditRecordAsync(int? id)//right-click to create a View
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
		public async Task<IActionResult> EditRecordAsync(CriminalRecord obj)
		{
			try
			{
				_db.CriminalRecords.Update(obj);//Update new row to the database
				_db.SaveChanges();//Goes to the database and save the changes(store new row)
								  //TempData["succes"] = "Criminal record is successfully edited.";
				return Ok(new
				{
					message = "Criminal record is successfully edited.",
					redirectUrl = Url.Action("Index", "CriminalRecord")
				});
			}
			catch (Exception ex)
			{
				//TempData["error"] = "An error occured while editing criminal record.";
				return BadRequest(new { message = ex.Message.ToString() });
			}
		}
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRecords(CriminalRecord obj)
        {

            var managers = _db.Case_Managers.Include(c => c.CriminalRecords).ToList();
            //var manager = _db.ApplicationUsers.Include(s=>s.CriminalRecords).ToList();

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

                    TempData["SuccessMessage"] = "Criminal record is successfully added to the database.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "No managers available to assign the criminal record.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No managers available to assign the criminal record.";
                return RedirectToAction("Index");
            }
        }
        */
        [HttpGet]
        public async Task<IActionResult> ReAssignCase(int? id,string? managerId)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var customerFromDb = await  _db.CriminalRecords.FirstOrDefaultAsync(x => x.Id == id);

            if (customerFromDb == null)
            {
                return NotFound();
            }
            ViewBag.ManagerId = managerId;
            return View(customerFromDb);
        }
        [HttpPost]
        public async Task<IActionResult> ReAssignCase(CriminalRecord obj, string email)
        {
            try
            {
                var newManager = await sapsService.GetAplicationUserUserByUsernameAsync(email);
                var currentManagerEmail = await userManager.Users.Where(x => x.Id == obj.CaseManagerId).Select(x => x.UserName).FirstOrDefaultAsync();
                obj.CaseManagerId = newManager.Id;
                obj.CaseManagerName = $"{newManager.Name} {newManager.Surname}";
                obj.CaseManagerNo = await _db.Case_Managers.Where(x => x.Email == email).Select(x => x.CaseManagerNo).FirstOrDefaultAsync();
                _db.CriminalRecords.Update(obj);//Update new row to the database
                _db.SaveChanges();//Goes to the database and save the changes(store new row)
                                  //Send Email 
                var body = $@"Good day {obj.CaseManagerName}, <br><br>
                                   A new case has been assigned to you, Case Id is {obj.Id}.
                                   <br><br>
                                   Kind Regards,<br> SAPS Management Admin.";

                await emailSender.SendEmailAsync(email, "Case Assignment", body);
                var body1 = $@"Good day {obj.CaseManagerName}, <br><br>
                                   A case has been un-assigned to you, Case Id is {obj.Id}.
                                   <br><br>
                                   Kind Regards,<br> SAPS Management Admin.";

                await emailSender.SendEmailAsync(currentManagerEmail??"", "Un-assignment", body1);
                return Ok(new
                {
                    message = "Case record is successfully re-assigned.",
                    redirectUrl = Url.Action("Index", "CriminalRecord")
                });
            }
            catch (Exception ex)
            {
                //TempData["error"] = "An error occured while editing criminal record.";
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }
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
