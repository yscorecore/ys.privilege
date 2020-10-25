using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace YS.Privilege
{
    [TypeConverter(typeof(LogicRoleTypeConverter))]
    public class LogicRole
    {
        public string Provider { get; set; }

        public string Id { get; set; }

        public override string ToString()
        {
            return $"{Provider}:{Id}";
        }
    }
    public class LogicRoleTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return base.CanConvertFrom(context, sourceType) || sourceType == typeof(string);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string text)
            {
                var spliterIndex = text.IndexOf(':');
                if (spliterIndex < 0)
                {
                    return new LogicRole
                    {
                        Provider = string.Empty,
                        Id = text
                    };
                }
                else
                {
                    return new LogicRole
                    {
                        Provider = text.Substring(0, spliterIndex),
                        Id = text.Substring(spliterIndex + 1)
                    };
                };
            }
            return base.ConvertFrom(context, culture, value);
        }

    }
}
