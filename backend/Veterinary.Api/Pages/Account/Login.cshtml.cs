using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Veterinary.Model.Entities;

namespace Veterinary.Api.Pages
{
    public class LoginModel : PageModel
    {
        public LoginModel(
            IIdentityServerInteractionService interactionService,
            IUserClaimsPrincipalFactory<VeterinaryUser> claimsPrincipalFactory,
            UserManager<VeterinaryUser> userManager)
        {
            this.interactionService = interactionService;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
            this.userManager = userManager;
        }

        private readonly IIdentityServerInteractionService interactionService;
        private readonly IUserClaimsPrincipalFactory<VeterinaryUser> claimsPrincipalFactory;
        private readonly UserManager<VeterinaryUser> userManager;

        [Required(ErrorMessage = "Kötelező")]
        [BindProperty]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Kötelező")]
        [BindProperty]
        public string Password { get; set; } = "";

        [BindProperty]
        public string ReturnUrl { get; set; } = "/";

        public List<string> Errors { get; set; } = new List<string>();

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(Username);
                if (user != null)
                {
                    if((await userManager.CheckPasswordAsync(user, Password)))
                    {
                        var signInProperties = new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                            AllowRefresh = true,
                            RedirectUri = ReturnUrl,
                            IsPersistent = false
                        };

                        var claimsPrincipal = await claimsPrincipalFactory.CreateAsync(user);
                        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal, signInProperties);
                        HttpContext.User = claimsPrincipal;

                        if (interactionService.IsValidReturnUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                    }
                }

            }
            Errors.Add("Hibás felhasználónév vagy jelszó!");

            return Page();
        }
    }
}
