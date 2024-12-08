using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SAPS_App.Areas.Identity.Pages.Account.Manage.RoleManagement
{
    public class ManageRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public bool ShowAssignRoleToUser { get; set; }
        [BindProperty]
        public bool ShowAddRoleToDb { get; set; }
        [BindProperty]
        public bool ShowRemoveRoleFromUser { get; set; }

        public void OnGet()
        {
            // Initialize visibility state
            ShowAssignRoleToUser = false;
            ShowAddRoleToDb = false;
            ShowRemoveRoleFromUser = false;
        }

        public IActionResult OnPostShowAssignRoleToUser()
        {
            ShowAssignRoleToUser = true;
            ShowAddRoleToDb = false;
            ShowRemoveRoleFromUser = false;
            return Page();
        }

        public IActionResult OnPostShowAddRoleToDb()
        {
            ShowAssignRoleToUser = false;
            ShowAddRoleToDb = true;
            ShowRemoveRoleFromUser = false;
            return Page();
        }

        public IActionResult OnPostShowRemoveRoleFromUser()
        {
            ShowAssignRoleToUser = false;
            ShowAddRoleToDb = false;
            ShowRemoveRoleFromUser = true;
            return Page();
        }

    }
}

