using Microsoft.EntityFrameworkCore;
using SAPS_App.Context;

namespace SAPS_App.Services
{
    
    public class SAPS_Services
    {
        private readonly SAPS_Context _db;
        public SAPS_Services(SAPS_Context db)
        {
            _db = db;
        }

        public async Task <List<string?>> GetOffencesAsync()
        {
            return await _db.Offences.Select(x => x.Name).ToListAsync();
        }
        public async Task<List<string?>> GetStationsAsync()
        {
            return await  _db.PoliceStations.Select(x => x.Name).ToListAsync();
        }
    }
}
