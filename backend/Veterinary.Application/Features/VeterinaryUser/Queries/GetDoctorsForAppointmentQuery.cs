using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Microsoft.EntityFrameworkCore;

namespace Veterinary.Application.Features.VeterinaryUserFeatures.Queries
{
    public class GetDoctorsForAppointmentQuery : IRequest<List<DoctorForAppointmentDto>>
    {
    }

    public class DoctorForAppointmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class GetDoctorsForAppointmentQueryHandler : IRequestHandler<GetDoctorsForAppointmentQuery, List<DoctorForAppointmentDto>>
    {
        private readonly IDoctorRepository doctorRepository;
        private readonly ITreatmentRepository treatmentRepository;

        public GetDoctorsForAppointmentQueryHandler(IDoctorRepository doctorRepository, ITreatmentRepository treatmentRepository)
        {
            this.doctorRepository = doctorRepository;
            this.treatmentRepository = treatmentRepository;
        }

        public async Task<List<DoctorForAppointmentDto>> Handle(GetDoctorsForAppointmentQuery request, CancellationToken cancellationToken)
        {
            var doctors = (await doctorRepository.GetDoctors())
                                .Select(tuple => tuple.User).ToList();
            var availableTreatmentDoctorIds = await treatmentRepository.GetAllAsQueryable()
                .Where(treatment => !treatment.IsInactive)
                .Select(treatment => treatment.DoctorId)
                .Distinct()
                .ToListAsync();

            doctors = doctors.Where(doctor => availableTreatmentDoctorIds.Contains(doctor.Id)).ToList();

            return doctors.Select(doctor => new DoctorForAppointmentDto
            {
                Id = doctor.Id,
                Name = doctor.Name
            }).ToList();
        }
    }
}
