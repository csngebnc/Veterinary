using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinary.Dal.Data;
using Veterinary.Model.Entities;

namespace Veterinary.Api.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<VeterinaryUser> userManager;
        private readonly VeterinaryDbContext context;

        [Required(ErrorMessage = "A jelszó megadása kötelezõ.")]
        [MinLength(6, ErrorMessage = "A jelszónak legalább 6 karakterbõl kell állnia, tartalmaznia kell kis- és nagybetût, valamint legalább egy speciális karaktert.")]
        [BindProperty]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "A jelszó ismételt megadása kötelezõ.")]
        [BindProperty]
        public string ConfirmPassword { get; set; } = "";

        [BindProperty]
        public string ReturnUrl { get; set; } = "";
        [BindProperty]
        public string ResetToken { get; set; } = "";
        [BindProperty]
        public string UserId { get; set; } = "";

        public VeterinaryUser VeterinaryUser { get; set; }

        public ResetPasswordModel(UserManager<VeterinaryUser> userManager, VeterinaryDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string resetToken, string returnUrl)
        {
            ReturnUrl = returnUrl;
            ResetToken = resetToken;
            UserId = userId;

            if(userId == null || resetToken == null)
            {
                return Redirect(ReturnUrl);
            }

            VeterinaryUser = await userManager.FindByIdAsync(userId);
            if (VeterinaryUser == null)
            {
                return Redirect(ReturnUrl);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (ConfirmPassword == Password)
                {
                    var user = await userManager.FindByIdAsync(UserId);
                    var result = await userManager.ResetPasswordAsync(user, ResetToken, Password);
                    if (result.Succeeded)
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return Redirect(ReturnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(ConfirmPassword), "A megadott jelszavak nem egyeznek!");
                }
            }

            return Page();
        }
    }
}
