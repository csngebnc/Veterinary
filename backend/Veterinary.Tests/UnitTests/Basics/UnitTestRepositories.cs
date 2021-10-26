using Veterinary.Dal.Data;
using Veterinary.Dal.Repositories;
using Veterinary.Dal.Repositories.AnimalRepository;
using Veterinary.Dal.Repositories.AnimalSpeciesRepository;
using Veterinary.Dal.Repositories.AppointmentRepository;
using Veterinary.Dal.Repositories.Doctor;
using Veterinary.Dal.Repositories.MedicalRecordRepository;
using Veterinary.Dal.Repositories.MedicalRecordTextTemplateRepository;
using Veterinary.Dal.Repositories.MedicationRepository;
using Veterinary.Dal.Repositories.TherapiaRepository;
using Veterinary.Dal.Repositories.Vaccination;
using Veterinary.Domain.Entities;
using Veterinary.Domain.Entities.AnimalRepository;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;
using Veterinary.Domain.Entities.AppointmentEntities;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Domain.Entities.MedicalRecordEntities;
using Veterinary.Domain.Entities.MedicationEntities;
using Veterinary.Domain.Entities.TherapiaEntities;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Tests.UnitTests.Basics
{
    public class UnitTestRepositories
    {
        public IAnimalRepository AnimalRepository { get; private set; }
        public IAnimalSpeciesRepository AnimalSpeciesRepository { get; private set; }
        public IAppointmentRepository AppointmentRepository { get; private set; }
        public IHolidayRepository HolidayRepository { get; private set; }
        public IMedicationRepository MedicationRepository { get; private set; }
        public ITherapiaRepository TherapiaRepository { get; private set; }
        public ITreatmentRepository TreatmentRepository { get; private set; }
        public ITreatmentIntervalRepository TreatmentIntervalRepository { get; private set; }
        public IMedicalRecordRepository MedicalRecordRepository { get; private set; }
        public IMedicalRecordTextTemplateRepository MedicalRecordTextTemplateRepository { get; private set; }
        public IVaccineRepository VaccineRepository { get; private set; }
        public IVaccineRecordRepository VaccineRecordRepository { get; private set; }
        public IDoctorRepository DoctorRepository { get; set; }
        public IVeterinaryUserRepository VeterinaryUserRepository { get; set; }

        public UnitTestRepositories(VeterinaryDbContext context)
        {
            AnimalRepository = new AnimalRepository(context);
            AnimalSpeciesRepository = new AnimalSpeciesRepository(context);
            AppointmentRepository = new AppointmentRepository(context);
            HolidayRepository = new HolidayRepository(context);
            TreatmentIntervalRepository = new TreatmentIntervalRepository(context);
            TreatmentRepository = new TreatmentRepository(context);
            MedicalRecordRepository = new MedicalRecordRepository(context);
            MedicalRecordTextTemplateRepository = new MedicalRecordTextTemplateRepository(context);
            MedicationRepository = new MedicationRepository(context);
            TherapiaRepository = new TherapiaRepository(context);
            VaccineRecordRepository = new VaccineRecordRepository(context);
            VaccineRepository = new VaccineRepository(context);
            DoctorRepository = new DoctorRepository(context);
            VeterinaryUserRepository = new VeterinaryUserRepository(context);
        }
    }
}
