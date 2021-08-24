using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Veterinary.Domain.Entities;

namespace Veterinary.Api.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<VeterinaryUser> userManager;
        private readonly IEmailSender emailSender;

        [Required(ErrorMessage = "E-mail cím megadása kötelezõ")]
        [EmailAddress(ErrorMessage = "Kérlek adj meg egy érvényes e-mail címet.")]
        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string ReturnUrl { get; set; } = "";

        public ForgotPasswordModel(UserManager<VeterinaryUser> userManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(Email);
                if (user != null)
                {
                    var callbackUrl = Url.Page
                        (
                            "/Account/ResetPassword",
                            pageHandler: null,
                            values: new
                            {
                                userId = user.Id,
                                resetToken = await userManager.GeneratePasswordResetTokenAsync(user),
                                returnUrl = ReturnUrl
                            },
                            protocol: Request.Scheme
                        );

                    await emailSender.SendEmailAsync
                        (
                            Email,
                            "Jelszó-emlékeztetõ",
                            $"Kedves Felhasználónk!<br>" +
                            $"<br>" +
                            $"Az alábbi üzenetet azért küldtük, mert jelszó-emlékeztetõt kértél. Amennyiben nem te voltál, akkor nyugodtan hagyd figyelmen kívül ezt az e-mailt.<br>" +
                            $"<br>" +
                            $"Új jelszó beállításához: <a href = '{HtmlEncoder.Default.Encode(callbackUrl)}' > kattints ide </a >.<br>" +
                            $"<br>" +
                            $"További szép napot kívánunk! <br>" +
                            $"Veterinary csapata"
                        );

                }
            }

            return Redirect(ReturnUrl);
        }
    }
}
