using Microsoft.AspNetCore.Identity.UI.Services;

namespace SAPS_App
{
    public class EmailSender : IEmailSender//Install ASPNETCore.Identity.UI
    {
        public Task SendEmailAsync(string email,string subject,string htmlMessage)
        {
            //logic to send email
            return Task.CompletedTask;
        }
        //Now go and register the implementation on Program.cs
    }
}
