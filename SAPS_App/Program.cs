using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SAPS_App;
using Microsoft.AspNetCore.Identity.UI.Services;
using SAPS_App.Areas.Identity.Pages;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//This is done after connecting MsServer and creating a table on  (appsettings.json and ApplictationsDbContext.cs
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
            ));/* after writing this code , install Microsoft.EntityFrameworkCore.SqlServer
                * Then Add a migration (first install Microsoft.EntityFrameworkCore.Tools)
                * to add migration go to toolS>Nuget Package manager>package manager console
                * The after PM> write add-migration AddCustomerToDatabase(migration name) 
                * then write and run update-database (to push migrations to database.
                * After all these steps , the project will be connected to MSSQL
                */
//This  Code did not add roles
/*builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();*/ //I removed this:options => options.SignIn.RequireConfirmedAccount = true: on <IdentityUser>()
//To add roles to Identity
//After adding roles on on views, now add 'AddDefaultTokenProviders 
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();//added AddDefaultTokenProviders()
//Add this so that register and login can work
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailSender,EmailSender>();//Registering what is in EmailSender class
// Set EPPlus license context
ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Or LicenseContext.Commercial if you have a commercial license
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();//Added after adding Idenity
app.UseAuthorization();
//Add this after adding RazorPages
app.MapRazorPages();//To map the pipeline(Now the Register and Login pages should work perfectly)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
