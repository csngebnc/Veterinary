using System.ComponentModel.DataAnnotations;

namespace Veterinary.Shared.Enums
{
    public enum AppointmentStatusEnum
    {
        [Display(Name = "Új")]
        New = 0,

        [Display(Name = "Megérkezett")]
        Arrived = 1,

        [Display(Name = "Kórlappal lezárva")]
        Closed = 2,

        [Display(Name = "Lemondva")]
        Resigned = 3,

        [Display(Name = "Lemondva orvos által")]
        ResignedByDoctor = 4,

        [Display(Name = "Egyéb indokkal lezárva")]
        Other = 5
    }
}