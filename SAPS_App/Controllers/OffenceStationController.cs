using Microsoft.AspNetCore.Mvc;
using SAPS_App.Context;
using SAPS_App.Models;

namespace SAPS_App.Controllers
{
    public class OffenceStationController : Controller
    {
        public readonly SAPS_Context _db;
        public OffenceStationController(SAPS_Context db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddOffence()
        {
            return View();
        }
        //POST
        [HttpPost]
        public IActionResult AddOffence(Offences obj)
        {
            if (_db.Offences.Any(s => s.Name == obj.Name))
            {
                return BadRequest(new { message = $"{obj.Name} already exists in the database." });
            }
            try
            {
                _db.Offences.Add(obj);
                _db.SaveChanges();
                return Ok(new
                {
                    message = $"{obj.Name} is successfully added to the database.",
                    redirectUrl = Url.Action("AddOffence", "OffenceStation")
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }

        public IActionResult AddStation()
        {
            return View();
        }
        //POST
        [HttpPost]
        public IActionResult AddStation(PoliceStations obj)
        {
            if (_db.Offences.Any(s => s.Name == obj.Name))
            {
                return BadRequest(new { message = $"{obj.Name} already exists in the database." });
            }
            try
            {
                _db.PoliceStations.Add(obj);
                _db.SaveChanges();
                return Ok(new
                {
                    message = $"{obj.Name} is successfully added to the database.",
                    redirectUrl = Url.Action("AddStation", "OffenceStation")
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }
    }
}
