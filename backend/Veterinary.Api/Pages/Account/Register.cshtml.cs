using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Model.Entities;

namespace Veterinary.Api.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<VeterinaryUser> userManager;
        private readonly VeterinaryDbContext context;
        private readonly IEmailSender emailSender;

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

        public RegisterModel(UserManager<VeterinaryUser> userManager, VeterinaryDbContext context, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.context = context;
            this.emailSender = emailSender;
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

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page
                        (
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new
                            {
                                userId = user.Id,
                                code = code,
                                returnUrl = ReturnUrl
                            },
                            protocol: Request.Scheme
                        );

                    await emailSender.SendEmailAsync
                        (
                            Username, 
                            "E-mail c�m meger�s�t�se",
                            $"Kedves Felhaszn�l�nk!<br>" +
                            $"Az al�bbi emailt az�rt k�ldt�k, mert regisztr�lt�l �llatorvosi rendszer�nkbe.<br>" +
                            $"<br>" +
                            $"A regisztr�ci� v�gleges�t�s�hez k�r�nk er�s�tsd meg az e-mail c�med a k�vetkez� link seg�ts�g�vel.<br>" +
                            $"Az e-mail c�m meger�s�t�s�hez: <a href = '{HtmlEncoder.Default.Encode(callbackUrl)}' > kattints ide </a >.<br>" +
                            $"<br>" +
                            $"Tov�bbi sz�p napot k�v�nunk! <br>" +
                            $"Veterinary csapata"
                        );

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
