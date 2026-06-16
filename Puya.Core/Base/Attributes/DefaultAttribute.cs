using Puya.Conversion;
using Puya.Extensions;
using System;
using System.Linq;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultAttribute : Attribute
    {
        public object Value { get; set; }
        public DefaultAttribute(object value = null, Type type = null)
        {
            Value = value;

            do
            {
                if (type == null)
                {
                    break;
                }

                if (type == TypeHelper.TypeOfBool || type == TypeHelper.TypeOfNullableBool)
                {
                    if (value != null)
                    {
                        Value = SafeClrConvert.ToBoolean(value);
                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = false;
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfDouble || type == TypeHelper.TypeOfNullableDouble)
                {
                    if (value != null)
                    {
                        Value = SafeClrConvert.ToDouble(value);
                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = 0;
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfSingle || type == TypeHelper.TypeOfNullableSingle)
                {
                    if (value != null)
                    {
                        Value = SafeClrConvert.ToSingle(value);
                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = 0;
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfDecimal || type == TypeHelper.TypeOfNullableDecimal)
                {
                    if (value != null)
                    {
                        Value = SafeClrConvert.ToDecimal(value);
                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = 0;
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfLong || type == TypeHelper.TypeOfNullableLong)
                {
                    if (value != null)
                    {
                        Value = SafeClrConvert.ToLong(value);
                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = 0;
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfInt || type == TypeHelper.TypeOfNullableInt)
                {
                    if (value != null)
                    {
                        Value = SafeClrConvert.ToInt(value);
                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = 0;
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfShort || type == TypeHelper.TypeOfNullableShort)
                {
                    if (value != null)
                    {
                        Value = SafeClrConvert.ToShort(value);
                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = 0;
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfByte || type == TypeHelper.TypeOfNullableByte)
                {
                    if (value != null)
                    {
                        Value = SafeClrConvert.ToByte(value);
                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = 0;
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfDateTime || type == TypeHelper.TypeOfNullableDateTime)
                {
                    if (value != null)
                    {
                        if (value.GetType() == TypeHelper.TypeOfString)
                        {
                            var _value = value.ToString();

                            if (_value.ToCharArray().Any(char.IsLetter))
                            {
                                Value = DateTime.Now.ToString(_value);
                            }
                            else
                            {
                                Value = SafeClrConvert.ToDateTime(value);
                            }
                        }
                        else
                        {
                            Value = SafeClrConvert.ToDateTime(value);
                        }

                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = DateTime.Now;
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfTimeSpan || type == TypeHelper.TypeOfNullableTimeSpan)
                {
                    if (value != null)
                    {
                        if (value.GetType() == TypeHelper.TypeOfString)
                        {
                            var _value = value.ToString();

                            if (_value.ToCharArray().Any(ch => char.IsLetter(ch)))
                            {
                                Value = DateTime.Now.ToString(_value);
                            }
                            else
                            {
                                Value = SafeClrConvert.ToTimeSpan(value);
                            }
                        }
                        else
                        {
                            Value = SafeClrConvert.ToTimeSpan(value);
                        }

                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = DateTime.Now;
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfGuid || type == typeof(Guid?))
                {
                    if (value != null)
                    {
                        Value = SafeClrConvert.ToGuid(value);
                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = Guid.NewGuid();
                    }

                    break;
                }

                if (type == TypeHelper.TypeOfByteArray)
                {
                    var _type = value.GetType();

                    if (value != null)
                    {
                        if (_type == TypeHelper.TypeOfString)
                        {
                            Value = System.Convert.FromBase64String(value.ToString());
                        }

                        break;
                    }

                    if (!type.IsNullable())
                    {
                        Value = new byte[] { };
                    }
                }
            } while (false);
        }
    }
}
