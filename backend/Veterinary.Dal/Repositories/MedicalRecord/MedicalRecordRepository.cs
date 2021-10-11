using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Dal.Repositories.MedicalRecordRepository
{
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task<MedicalRecord> GetMedicalRecordWithDetailsAsync(Guid recordId)
        {
            var record = await GetAllAsQueryable()
                .Include(record => record.MedicationRecords)
                    .ThenInclude(medicationRecord => medicationRecord.Medication)
                .Include(record => record.TherapiaRecords)
                    .ThenInclude(therapiaRecord => therapiaRecord.Therapia)
                .Include(record => record.Photos)
                .SingleAsync(record => record.Id == recordId);
            return record ?? throw new EntityNotFoundException();
        }

        public async Task<MedicalRecord> GetMedicalRecordForPDFAsync(Guid recordId)
        {
            var record = await GetAllAsQueryable()
                .Include(record => record.MedicationRecords)
                    .ThenInclude(medicationRecord => medicationRecord.Medication)
                .Include(record => record.TherapiaRecords)
                    .ThenInclude(therapiaRecord => therapiaRecord.Therapia)
                .Include(record => record.Owner)
                .Include(record => record.Animal)
                    .ThenInclude(animal => animal.Species)
                .Include(record => record.Photos)
                .SingleAsync(record => record.Id == recordId);
            return record ?? throw new EntityNotFoundException();
        }

        public IQueryable<MedicalRecord> GetMedicalRecordsByAnimalIdQueryable(Guid animalId)
        {
            return Table
                .Include(record => record.Owner)
                .Include(record => record.Doctor)
                .Include(record => record.Animal)
                .Include(record => record.MedicationRecords)
                    .ThenInclude(medicationRecord => medicationRecord.Medication)
                .Include(record => record.TherapiaRecords)
                    .ThenInclude(therapiaRecord => therapiaRecord.Therapia)
                .Include(record => record.Photos)
                .Where(record => record.AnimalId == animalId)
                .OrderByDescending(record => record.Date)
                .AsQueryable();
        }

        public IQueryable<MedicalRecord> GetMedicalRecordsByUserIdQueryable(Guid userId)
        {
            return Table
                .Include(record => record.Owner)
                .Include(record => record.Doctor)
                .Include(record => record.Animal)
                .Include(record => record.MedicationRecords)
                    .ThenInclude(medicationRecord => medicationRecord.Medication)
                .Include(record => record.TherapiaRecords)
                    .ThenInclude(therapiaRecord => therapiaRecord.Therapia)
                .Include(record => record.Photos)
                .Where(record => record.OwnerId == userId)
                .OrderByDescending(record => record.Date)
                .AsQueryable();
        }
    }
}
