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

        [Required(ErrorMessage = "E-mail c�m megad�sa k�telez�")]
        [EmailAddress(ErrorMessage = "K�rlek adj meg egy �rv�nyes e-mail c�met.")]
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
                            "Jelsz�-eml�keztet�",
                            $"Kedves Felhaszn�l�nk!<br>" +
                            $"<br>" +
                            $"Az al�bbi �zenetet az�rt k�ldt�k, mert jelsz�-eml�keztet�t k�rt�l. Amennyiben nem te volt�l, akkor nyugodtan hagyd figyelmen k�v�l ezt az e-mailt.<br>" +
                            $"<br>" +
                            $"�j jelsz� be�ll�t�s�hoz: <a href = '{HtmlEncoder.Default.Encode(callbackUrl)}' > kattints ide </a >.<br>" +
                            $"<br>" +
                            $"Tov�bbi sz�p napot k�v�nunk! <br>" +
                            $"Veterinary csapata"
                        );

                }
            }

            return Redirect(ReturnUrl);
        }
    }
}
