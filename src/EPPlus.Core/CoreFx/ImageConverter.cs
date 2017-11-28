#if NETSTANDARD2_0

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Drawing;

namespace OfficeOpenXml
{
    /// <summary>
    /// Summary description for ImageConverter.
    /// </summary>
    public class ImageConverter : TypeConverter
    {
        public ImageConverter()
        {
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(System.Byte[]))
                return true;
            else
                return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if ((destinationType == typeof(System.Byte[])) || (destinationType == typeof(System.String)))
                return true;
            else
                return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            byte[] bytes = value as byte[];
            if (bytes == null)
                return base.ConvertFrom(context, culture, value);

            MemoryStream ms = new MemoryStream(bytes);

            return Image.FromStream(ms);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
                return "(none)";

            if (value is System.Drawing.Image)
            {
                if (destinationType == typeof(string))
                {
                    return value.ToString();
                }
                else if (CanConvertTo(null, destinationType))
                {
                    //came here means destinationType is byte array ;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ((Image)value).Save(ms, ((Image)value).RawFormat);
                        return ms.ToArray();
                    }
                }
            }

            string msg = string.Format("ImageConverter can not convert from type '{0}'.", value.GetType());
            throw new NotSupportedException(msg);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(Image), attributes);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}

#endif