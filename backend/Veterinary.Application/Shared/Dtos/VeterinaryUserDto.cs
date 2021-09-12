using System;

namespace Veterinary.Application.Shared.Dtos
{
    public class VeterinaryUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
    }
}
