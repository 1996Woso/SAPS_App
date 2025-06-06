﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SAPS_App.Context;
using SAPS_App.Models;

namespace SAPS_App.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;//Added when adding roles
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;

        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;//After adding roles you get an error
                                                   // Add this line
                                                   //The error is
        /*An unhandled exception occurred while processing the request.
        InvalidOperationException: Unable to resolve service for type 'Microsoft.AspNetCore.Identity
        .UI.Services.IEmailSender' while attempting to activate 'SAPS_App.Areas.Identity.Pages.Account.RegisterModel'.*/

        //To Solve it, create a class EmailSender(not in the models just a class under projrect)
        private readonly Services.EmailSender emailSender;
        private readonly SAPS_Context _db;//Added because we want to add users who are managers to Managers(table)
        public RegisterModel(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,//Added after add RoleManager above
    IUserStore<IdentityUser> userStore,
    SignInManager<IdentityUser> signInManager,
    ILogger<RegisterModel> logger,
       Services.EmailSender emailSender,
    SAPS_Context db
            )
        {
            _roleManager = roleManager;//Added after adding RoleManager
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            this.emailSender = emailSender;
            _db = db;//Added because we want to add users who are managers to Managers(table)
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [Display(Name = "First Name")]
            [StringLength(40, MinimumLength = 2, ErrorMessage = "The First Name must be between 2 and 40 characters.")]
            public string Name { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            [StringLength(40, MinimumLength = 2, ErrorMessage = "The Last Name must be between 2 and 40 characters.")]
            public string Surname { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            //Now we give a user an option to choose his role
            public string? Role { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            //Populating role options
            Input = new()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            //returnUrl ??= Url.Action("Suspect", "ViewManagers");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Name = Input.Name;
                user.Surname = Input.Surname;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    ////Assign role to a user
                    //if (!String.IsNullOrEmpty(Input.Role))
                    //{
                    //    await _userManager.AddToRoleAsync(user, Input.Role);
                    //}
                    //else//Default role
                    //{
                    //    await _userManager.AddToRoleAsync(user, "CaseManager");
                    //}
                    ////Add managers to Managers(Table I created myself)
                    //if (Input.Role == "CaseManager" || String.IsNullOrEmpty(Input.Role))
                    //{
                    //    var caseManager = new CaseManager
                    //    {
                    //        Name = Input.Name,
                    //        Surname = Input.Surname,
                    //        Email = Input.Email,
                    //        CaseManagerId = user.Id,
                    //        CriminalRecords = new List<CriminalRecord>()
                    //    };
                    //    _db.Case_Managers.Add(caseManager);
                    //    await _db.SaveChangesAsync();
                    //    //return RedirectToPage("Suspect","ViewManagers");

                    //}

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //Send email 
                    var body = $@"Good day {user.Name} {user.Surname}, <br><br>
                                   Please click this link to confirm your email:
                                   <a href = '{callbackUrl}'> Confrim Email </a><br><br>
                                   Kind Regards,<br> SAPS Management Admin.";

                    await emailSender.SendEmailAsync(user.Email, "Confirm Email", body);
                    //var body1 = $@"Good day Eviwe Khohlombe, <br><br>
                    //               A new user has created account on SAPS application, 
                    //               user's email is {Input.Email}.<br> Please assign a role the user:
                    //               <a href = 'http://eviwekhohlombe-001-site1.ltempurl.com/Identity/Account/Login'> Login</a> <br><br>
                    //               Kind Regards,<br> SAPS Management Admin.";

                    //await emailSender.SendEmailAsync("eviwe.khohlombe@gmail.com", "Assign role to new user", body1);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()//Change IdentityUser to ApplicationUser
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();//Change IdentityUser to ApplicationUser
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
