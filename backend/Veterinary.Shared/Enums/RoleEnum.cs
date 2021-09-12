using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Veterinary.Shared.Enums
{
    public enum RoleEnum
    {
        [Display(Name = "Felhasználó")]
        [Description("User")]
        User,

        [Display(Name = "Doktor")]
        [Description("Doctor")]
        NormalDoctor,

        [Display(Name = "Főorvos")]
        [Description("Manager")]
        ManagerDoctor

    }
}
