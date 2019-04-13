using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FRSkinTester.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SmartRequiredAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var properties = value.GetType().GetProperties();

            foreach(var property in properties.Where(x => !x.GetCustomAttributes(typeof(IgnoreRequiredAttribute), true).Any()))
                if (property.GetValue(value, null) != null)
                    return true;

            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreRequiredAttribute : Attribute
    {
        public IgnoreRequiredAttribute()
        {
        }
    }
}