using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using SAPS_App.Models;

namespace SAPS_App.Services
{
    public interface ISAPSService
    {
        Task<IdentityUser> GetIdentityUserAsync(ClaimsPrincipal user);
        Task<ApplicationUser> GetAplicationUserUserByIdAsync(string id);
        Task<List<string>> GetOffencesAsync();
        Task<List<string>> GetStationsAsync();
        Task<ApplicationUser> GetAplicationUserUserByUsernameAsync(string username);
        Task<Dictionary<string,string>> GetCaseStatusesAsync();
        Task<List<OffenceCountDto>> GetOffenceStatisticsAsync();
    }
}
