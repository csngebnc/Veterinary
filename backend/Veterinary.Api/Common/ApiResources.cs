using Microsoft.AspNetCore.Mvc;

namespace Veterinary.Api.Common
{
    public static class ApiResources
    {
        public const string BasePath = "api";

        public static class Animals
        {
            public const string BasePath = ApiResources.BasePath + "/animals";
        }

        public static class AnimalSpecies
        {
            public const string BasePath = ApiResources.BasePath + "/species";
        }

        public static class Vaccine
        {
            public const string BasePath = ApiResources.BasePath + "/vaccines";
        }

        public static class VeterinaryUser
        {
            public const string BasePath = ApiResources.BasePath + "/users";
        }

        public static class Medication
        {
            public const string BasePath = ApiResources.BasePath + "/medications";
        }

        public static class Therapia
        {
            public const string BasePath = ApiResources.BasePath + "/therapias";
        }

        public static class Treatment
        {
            public const string BasePath = ApiResources.BasePath + "/treatments";
            public const string Intervals = ApiResources.BasePath + "/treatment-intervals";
        }

        public static class Holiday
        {
            public const string BasePath = ApiResources.BasePath + "/holidays";
        }

        public static class Appointment
        {
            public const string BasePath = ApiResources.BasePath + "/appointments";
        }

        public static class MedicalRecord
        {
            public const string BasePath = ApiResources.BasePath + "/records";
        }

        public static class MedicalRecordTextTemplate
        {
            public const string BasePath = ApiResources.BasePath + "/text-templates";
        }
    }
}
