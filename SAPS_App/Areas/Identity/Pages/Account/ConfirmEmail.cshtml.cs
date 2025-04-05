// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SAPS_App.Services;

namespace SAPS_App.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailSender emailSender;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager
            ,Services.EmailSender emailSender)
        {
            _userManager = userManager;
            this.emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                var body1 = $@"Good day Eviwe Khohlombe, <br><br>
                                   A new user has created account on SAPS application, 
                                   user's email is {user.Email}.<br> Please assign a role the user:
                                   <a href = 'http://eviwekhohlombe-001-site1.ltempurl.com/Identity/Account/Login'> Login</a> <br><br>
                                   Kind Regards,<br> SAPS Management Admin.
                ";

                await emailSender.SendEmailAsync("eviwe.khohlombe@gmail.com", "Assign role to new user", body1);
            }
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}
