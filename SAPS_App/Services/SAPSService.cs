using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAPS_App.Context;
using SAPS_App.Models;
using System.Security.Claims;

namespace SAPS_App.Services
{
    public class SAPSService: ISAPSService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IdentityContext identityContext;
        private readonly SAPS_Context sapsContext;

        public SAPSService(UserManager<IdentityUser> userManager
            , IdentityContext identityContext, SAPS_Context sapsContext)
        {
            this.userManager = userManager;
            this.identityContext = identityContext;
            this.sapsContext = sapsContext;
        }

        public async Task<ApplicationUser> GetAplicationUserUserByIdAsync(string id)
        {
            return await identityContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ApplicationUser> GetAplicationUserUserByUsernameAsync(string username)
        {
            return await identityContext.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IdentityUser> GetIdentityUserAsync(ClaimsPrincipal User)
        {

            return await userManager.GetUserAsync(User);
        }

        public async Task<List<string>> GetOffencesAsync()
        {
            return await sapsContext.Offences.OrderBy(x => x.Name).Select(x => x.Name).ToListAsync();
        }

        public async Task<List<string>> GetStationsAsync()
        {
            return await sapsContext.PoliceStations.OrderBy(x => x.Name).Select(x => x.Name).ToListAsync();
        }
        public async Task<Dictionary<string,string>> GetCaseStatusesAsync()
        {
            var statuses = new Dictionary<string, string>
            {
                {"Opened", "Open"},
                {"Re-Opened","Re-Open" },
                {"Closed","Close" }
            };
            return await Task.FromResult(statuses.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value));
        }

        public async Task<List<OffenceCountDto>> GetOffenceStatisticsAsync()
        {
            return sapsContext.CriminalRecords
           .GroupBy(c => c.OffenceCommited)
           .Select(g => new OffenceCountDto { Offence = g.Key, Count = g.Count() })
           .Where(g => g.Count > 0) // Ensure zero counts are excluded
           .ToList();
        }
    }
}
