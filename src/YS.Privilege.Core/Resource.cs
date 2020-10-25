using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace YS.Privilege
{
    [TypeConverter(typeof(ResourceTypeConverter))]
    public class Resource
    {

        public string Code { get; set; }

        public string Category { get; set; }

        public override string ToString()
        {
            return $"{Category}:{Code}";
        }
    }
    public class ResourceTypeConverter : TypeConverter
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
                    return new Resource
                    {
                        Code = text
                    };
                }
                else
                {
                    return new Resource
                    {
                        Category = text.Substring(0, spliterIndex),
                        Code = text.Substring(spliterIndex + 1)
                    };
                };
            }
            return base.ConvertFrom(context, culture, value);
        }

    }

}
