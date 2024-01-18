using System.ComponentModel;
using System.Reflection;

namespace AuditLog.API.AuditTrail.Enums
{
    // This enum is used to identify the names of applications that is being audited.
    public static class EnumExtensions
    {
        public static string GetApplication(this Enum value)
        {
            FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute? attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();

            return attribute != null ? attribute.Description : value.ToString();
        }

        public static string GetEntity(this Enum value)
        {
            FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute? attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();

            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}
