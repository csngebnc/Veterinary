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

        public static List<LabelValuePair<T>> GetLabelValuePairs<T>() where T : Enum
        {
            var list = new List<LabelValuePair<T>>();
            var values = (T[])Enum.GetValues(typeof(T));
            foreach (var value in values)
            {
                list.Add(new LabelValuePair<T>
                {
                    Label = value.GetDisplayName(),
                    Value = value
                });
            }

            return list;
        }

        public static T GetValueFromName<T>(string name)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null)
                {
                    if (attribute.Name == name)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == name)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentOutOfRangeException("name");
        }

        public static List<string> GetValues<T>()
        {
            return Enum.GetNames(typeof(T)).ToList();
        }
    }
}
