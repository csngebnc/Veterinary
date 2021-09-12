using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities;

namespace Veterinary.Application.Features.Manager.Queries
{
    public class GetDoctorsWithRoleQuery : IRequest<List<DoctorWithRoleDto>>
    {
    }

    public class DoctorWithRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class GetDoctorsQueryHandler : IRequestHandler<GetDoctorsWithRoleQuery, List<DoctorWithRoleDto>>
    {
        private readonly IDoctorRepository doctorManager;

        public GetDoctorsQueryHandler(IDoctorRepository doctorManager)
        {
            this.doctorManager = doctorManager;
        }

        public async Task<List<DoctorWithRoleDto>> Handle(GetDoctorsWithRoleQuery request, CancellationToken cancellationToken)
        {
            var result =  await doctorManager.GetDoctors();

            return result.Select(tuple => new DoctorWithRoleDto
            {
                Id = tuple.User.Id,
                Name = tuple.User.Name,
                Email = tuple.User.Email,
                Role = tuple.RoleName
            }).OrderBy(u => u.Role).ToList();
        }
    }
}
