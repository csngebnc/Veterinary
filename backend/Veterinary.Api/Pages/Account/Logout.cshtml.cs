using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Veterinary.Api.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IConfiguration configuration;

        [BindProperty]
        public string LogoutId { get; set; } = "";

        public LogoutModel(
            IIdentityServerInteractionService interactionService,
            IConfiguration configuration)
        {
            this.interactionService = interactionService;
            this.configuration = configuration;
        }

        public void OnGet(string logoutId)
        {
            LogoutId = logoutId;
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "cancel")
            {
                return Redirect(configuration.GetValue<string>("Redirects:AfterAbortedLogout"));
            }
            else
            {
                var context = await interactionService.GetLogoutContextAsync(LogoutId);
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return Redirect(context.PostLogoutRedirectUri);
            }
        }
    }
}
