using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Veterinary.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum @enum)
        {
            var attribute = @enum.GetType()
                .GetMember(@enum.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            return attribute?.GetName() ?? throw new InvalidOperationException("Nincs megadva display attribútum.");
        }

        public static string GetDescription(this Enum @enum)
        {
            var attribute = @enum.GetType()
                .GetMember(@enum.ToString())
                .First()
                .GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? throw new InvalidOperationException("Nincs megadva description attribútum.");
        }

        public static string Value(this Enum @enum)
        {
            return @enum.ToString();
        }

        public static List<string> GetValues<T>()
        {
            return Enum.GetNames(typeof(T)).ToList();
        }
    }
}
