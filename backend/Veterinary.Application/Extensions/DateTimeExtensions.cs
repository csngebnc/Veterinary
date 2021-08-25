﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinary.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static string CalculateAge(this DateTime dobDate)
        {
            var todayDate = DateTime.Today;
            var ageyear = todayDate.Year - dobDate.Year;
            var agemonth = todayDate.Month - dobDate.Month;
            var ageday = todayDate.Day - dobDate.Day;

            if (agemonth <= 0)
            {
                ageyear--;
                agemonth = (12 + agemonth);
            }
            if (todayDate.Day < dobDate.Day)
            {
                agemonth--;
                ageday = 30 + ageday;
            }
            if (agemonth == 12)
            {
                ageyear = ageyear + 1;
                agemonth = 0;
            }

            if (ageday == 0)
                ageday = 1;

            string age = "";
            if (ageyear > 0)
                age += ageyear + " év ";
            if (agemonth > 0)
                age += agemonth + " hónap ";
            if (agemonth == 0)
                age += ageday + " nap";

            return age;

        }

        public static int GetMinutes(this DateTime date)
        {
            return (date.Hour * 60 + date.Minute);
        }

        public static int GetMinutesLocalTime(this DateTime date)
        {
            return (date.ToLocalTime().Hour * 60 + date.ToLocalTime().Minute);
        }

    }
}
