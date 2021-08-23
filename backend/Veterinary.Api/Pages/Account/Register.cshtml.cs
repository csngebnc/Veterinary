using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Model.Entities;

namespace Veterinary.Api.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<VeterinaryUser> userManager;
        private readonly VeterinaryDbContext context;

        [Required(ErrorMessage = "K�telez�")]
        [BindProperty]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "K�telez�")]
        [MinLength(6, ErrorMessage = "A jelsz�nak legal�bb 6 karakterb�l kell �llnia")]
        [BindProperty]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "K�telez�")]
        [BindProperty]
        public string ConfirmPassword { get; set; } = "";

        [Required(ErrorMessage = "K�telez�")]
        [MinLength(3, ErrorMessage = "Az orsz�g nev�nek legal�bb 3 karakterb�l kell �llnia")]
        [BindProperty]
        public string CountryName { get; set; } = "";

        [BindProperty]
        public string ReturnUrl { get; set; } = "";

        public RegisterModel(UserManager<VeterinaryUser> userManager, VeterinaryDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "login")
            {
                return Redirect(ReturnUrl);
            }

            await ValidateFieldsAsync();

            if (ModelState.IsValid)
            {
                var user = new VeterinaryUser
                {
                    UserName = Username
                };

                var createResult = await userManager.CreateAsync(user, Password);
                if (createResult.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Subject, user.Id.ToString()));
                    return Redirect(ReturnUrl);
                }
                else
                {
                    AddModelErrorsForField(nameof(Username), createResult);
                    AddModelErrorsForField(nameof(Password), createResult);
                }
            }

            return Page();
        }

        private void AddModelErrorsForField(string fieldName, IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors.Where(x => x.Code.ToLower().Contains(fieldName.ToLower())))
            {
                ModelState.AddModelError(fieldName, error.Description);
            }
        }

        private async Task ValidateFieldsAsync()
        {
            var usernameTaken = Username != null && await context.Users.AnyAsync(x => x.UserName == Username);

            if (usernameTaken)
            {
                ModelState.AddModelError(nameof(Username), "A felhaszn�l�n�v m�r foglalt!");
            }

            if (ConfirmPassword != Password)
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "A megadott jelszavak nem egyeznek!");
            }
        }
    }
}
