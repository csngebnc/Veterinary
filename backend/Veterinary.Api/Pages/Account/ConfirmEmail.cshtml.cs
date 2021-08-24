using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Veterinary.Dal.Data;
using Veterinary.Model.Entities;

namespace Veterinary.Api.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<VeterinaryUser> _userManager;
        private readonly VeterinaryDbContext _context;
        public ConfirmEmailModel(UserManager<VeterinaryUser> userManager, VeterinaryDbContext context)
        {
            _userManager = userManager;
            _context = context;
           }

        public async Task<IActionResult> OnGetAsync(string userId, string code, string returnUrl)
        {
            var ReturnUrl = returnUrl;
            if (userId == null || code == null)
            {
                return Redirect(ReturnUrl);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Redirect(ReturnUrl);
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                /*
                var medicalRecords = await _context.MedicalRecords.Where(m => m.OwnerEmail == user.Email).ToListAsync();
                foreach (var record in medicalRecords)
                {
                    record.OwnerId = userId;
                }
                await _context.SaveChangesAsync();
                */
            }
            return Redirect(ReturnUrl);
        }
    }
}
