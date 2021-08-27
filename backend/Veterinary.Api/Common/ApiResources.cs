﻿using Microsoft.AspNetCore.Mvc;

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
    }
}
