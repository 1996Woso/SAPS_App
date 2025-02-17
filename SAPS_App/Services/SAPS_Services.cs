using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SAPS_App.Context;
using SAPS_App.Models;

namespace SAPS_App.Services
{
    
    public class SAPS_Services
    {

        private readonly SAPS_Context _db;
        private readonly string con;
        private readonly IConfiguration _config;

        List<AspNetUsers> users = new List<AspNetUsers>();    
        public SAPS_Services(SAPS_Context db,IConfiguration confif)
        {
            _db = db;
            _config = confif;
            con = _config.GetConnectionString("Somee")??"";
        }

        public async Task <List<string?>> GetOffencesAsync()
        {
            return await _db.Offences.Select(x => x.Name).ToListAsync();
        }
        public async Task<List<string?>> GetStationsAsync()
        {
            return await  _db.PoliceStations.Select(x => x.Name).ToListAsync();
        }
        public async Task<ApplicationUser> GetUserAsync(string email)
        {
            //using (var connection = new SqlConnection(con))
            //{
            //    connection.Open();
            //    var sqlCommand = "Exec AspNetUser @Email";
            //    var param = new { Email = email };
            //    var user = await connection.QueryFirstOrDefaultAsync<AspNetUser>(sqlCommand, param);
            //    return user!;
            //}
            //var user = await _db.ApplicationUsers.Where(x => x.UserName == email).FirstOrDefaultAsync();
            
            return null!;

        }
    }
}
