using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
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

        [Required(ErrorMessage = "A teljes neved megad�sa k�telez�.")]
        [BindProperty]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "E-mail c�m megad�sa k�telez�")]
        [EmailAddress(ErrorMessage = "K�rlek adj meg egy �rv�nyes e-mail c�met.")]
        [BindProperty]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "A jelsz� megad�sa k�telez�.")]
        [MinLength(6, ErrorMessage = "A jelsz�nak legal�bb 6 karakterb�l kell �llnia, tartalmaznia kell kis- �s nagybet�t, valamint legal�bb egy speci�lis karaktert.")]
        [BindProperty]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "A jelsz� ism�telt megad�sa k�telez�.")]
        [BindProperty]
        public string ConfirmPassword { get; set; } = "";

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
                    Name = Name,
                    UserName = Username,
                    Email = Username,
                    NormalizedUserName = Username.ToUpper(),
                    NormalizedEmail = Username.ToUpper()
                };

                var createResult = await userManager.CreateAsync(user, Password);
                if (createResult.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Subject, user.Id.ToString()));
                    await userManager.AddToRoleAsync(user, "User");
                    ReturnUrl += "&Message=Sikeres+regisztr%C3%A1ci%C3%B3%21+Er%C5%91s%C3%ADtsd+meg+az+e-mail+c%C3%ADmed%2C+hogy+bejelentkezhess%21";
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
                ModelState.AddModelError(nameof(Username), "Az e-mail c�m m�r foglalt!");
            }

            if (ConfirmPassword != Password)
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "A megadott jelszavak nem egyeznek!");
            }
        }
    }
}
