

using System;
using Puya.Base;
using Puya.Conversion;

namespace Puya.Extensions
{
	public class EnumUndefinedException: Exception
	{
		public EnumUndefinedException(string message): base(message)
		{ }
	}
	public class NotEnumException: Exception
	{
		public NotEnumException(string message): base(message)
		{ }
	}
	public static class ToEnumExtensions
	{
		internal static T CheckType<T>(bool throwOnErrors, T defaultValue)
		{
			var result = defaultValue;

			if (!typeof(T).IsEnum)
			{
				if (throwOnErrors)
				{
					throw new NotEnumException($"{typeof(T).Name} is not an Enum type");
				}
			}

			return result;
		}
		internal static object CheckType(Type type, bool throwOnErrors, object defaultValue)
		{
			var result = defaultValue;

			if (!type.IsEnum)
			{
				if (throwOnErrors)
				{
					throw new NotEnumException($"{type} is not an Enum type");
				}
			}

			return result;
		}
			public static T ToEnum<T>(this Byte x, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, default(T));

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));
						var def = default(T);

						if (enumDefaultAttribute != null)
						{
							if (!Enum.TryParse<T>(enumDefaultAttribute.Value, out def))
							{
								def = default(T);
							}
						
						}

						result = def;
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static T ToEnum<T>(this Byte x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, defaultValue);

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
		public static object ToEnum(this Byte x, Type type, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, null);
			
			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));

						if (enumDefaultAttribute != null)
						{
							result = Enum.Parse(type, enumDefaultAttribute.Value);
						}
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static object ToEnum(this Byte x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, defaultValue);

			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
			public static T ToEnum<T>(this SByte x, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, default(T));

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));
						var def = default(T);

						if (enumDefaultAttribute != null)
						{
							if (!Enum.TryParse<T>(enumDefaultAttribute.Value, out def))
							{
								def = default(T);
							}
						
						}

						result = def;
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static T ToEnum<T>(this SByte x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, defaultValue);

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
		public static object ToEnum(this SByte x, Type type, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, null);
			
			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));

						if (enumDefaultAttribute != null)
						{
							result = Enum.Parse(type, enumDefaultAttribute.Value);
						}
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static object ToEnum(this SByte x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, defaultValue);

			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
			public static T ToEnum<T>(this Int64 x, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, default(T));

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));
						var def = default(T);

						if (enumDefaultAttribute != null)
						{
							if (!Enum.TryParse<T>(enumDefaultAttribute.Value, out def))
							{
								def = default(T);
							}
						
						}

						result = def;
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static T ToEnum<T>(this Int64 x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, defaultValue);

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
		public static object ToEnum(this Int64 x, Type type, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, null);
			
			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));

						if (enumDefaultAttribute != null)
						{
							result = Enum.Parse(type, enumDefaultAttribute.Value);
						}
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static object ToEnum(this Int64 x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, defaultValue);

			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
			public static T ToEnum<T>(this Int32 x, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, default(T));

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));
						var def = default(T);

						if (enumDefaultAttribute != null)
						{
							if (!Enum.TryParse<T>(enumDefaultAttribute.Value, out def))
							{
								def = default(T);
							}
						
						}

						result = def;
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static T ToEnum<T>(this Int32 x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, defaultValue);

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
		public static object ToEnum(this Int32 x, Type type, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, null);
			
			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));

						if (enumDefaultAttribute != null)
						{
							result = Enum.Parse(type, enumDefaultAttribute.Value);
						}
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static object ToEnum(this Int32 x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, defaultValue);

			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
			public static T ToEnum<T>(this Int16 x, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, default(T));

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));
						var def = default(T);

						if (enumDefaultAttribute != null)
						{
							if (!Enum.TryParse<T>(enumDefaultAttribute.Value, out def))
							{
								def = default(T);
							}
						
						}

						result = def;
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static T ToEnum<T>(this Int16 x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, defaultValue);

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
		public static object ToEnum(this Int16 x, Type type, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, null);
			
			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));

						if (enumDefaultAttribute != null)
						{
							result = Enum.Parse(type, enumDefaultAttribute.Value);
						}
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static object ToEnum(this Int16 x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, defaultValue);

			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
			public static T ToEnum<T>(this UInt64 x, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, default(T));

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));
						var def = default(T);

						if (enumDefaultAttribute != null)
						{
							if (!Enum.TryParse<T>(enumDefaultAttribute.Value, out def))
							{
								def = default(T);
							}
						
						}

						result = def;
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static T ToEnum<T>(this UInt64 x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, defaultValue);

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
		public static object ToEnum(this UInt64 x, Type type, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, null);
			
			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));

						if (enumDefaultAttribute != null)
						{
							result = Enum.Parse(type, enumDefaultAttribute.Value);
						}
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static object ToEnum(this UInt64 x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, defaultValue);

			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
			public static T ToEnum<T>(this UInt32 x, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, default(T));

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));
						var def = default(T);

						if (enumDefaultAttribute != null)
						{
							if (!Enum.TryParse<T>(enumDefaultAttribute.Value, out def))
							{
								def = default(T);
							}
						
						}

						result = def;
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static T ToEnum<T>(this UInt32 x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, defaultValue);

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
		public static object ToEnum(this UInt32 x, Type type, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, null);
			
			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));

						if (enumDefaultAttribute != null)
						{
							result = Enum.Parse(type, enumDefaultAttribute.Value);
						}
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static object ToEnum(this UInt32 x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, defaultValue);

			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
			public static T ToEnum<T>(this UInt16 x, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, default(T));

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));
						var def = default(T);

						if (enumDefaultAttribute != null)
						{
							if (!Enum.TryParse<T>(enumDefaultAttribute.Value, out def))
							{
								def = default(T);
							}
						
						}

						result = def;
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static T ToEnum<T>(this UInt16 x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			var type = typeof(T);
			var result = CheckType<T>(throwOnErrors, defaultValue);

			try
			{
				result = (T)Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
		public static object ToEnum(this UInt16 x, Type type, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, null);
			
			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
					{
						var enumDefaultAttribute = (EnumDefaultAttribute)Attribute.GetCustomAttribute(type, typeof(EnumDefaultAttribute));

						if (enumDefaultAttribute != null)
						{
							result = Enum.Parse(type, enumDefaultAttribute.Value);
						}
					}
				}
			}
			catch
			{
				if (throwOnErrors)
				{
					throw;
				}
			}

			return result;
		}
		public static object ToEnum(this UInt16 x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			var result = CheckType(type, throwOnErrors, defaultValue);

			try
			{
				result = Enum.ToObject(type, x);

				if (!Enum.IsDefined(type, result))
				{
					if (throwOnErrors)
						throw new EnumUndefinedException($"{type} enum does not have a {x} value");
					else
						result = defaultValue;
				}
			}
			catch
			{
				if (throwOnErrors)
					throw;

				result = defaultValue;
			}

			return result;
		}
			public static T ToEnum<T>(this Single x, bool throwOnErrors = false) where T : struct
		{
			return SafeClrConvert.ToULong(x).ToEnum<T>(throwOnErrors);
		}
		public static T ToEnum<T>(this Single x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			return SafeClrConvert.ToULong(x).ToEnum<T>(defaultValue, throwOnErrors);
		}
		public static object ToEnum(this Single x, Type type, bool throwOnErrors = false)
		{
			return SafeClrConvert.ToULong(x).ToEnum(type, throwOnErrors);
		}
		public static object ToEnum(this Single x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			return SafeClrConvert.ToULong(x).ToEnum(type, defaultValue, throwOnErrors);
		}
			public static T ToEnum<T>(this Double x, bool throwOnErrors = false) where T : struct
		{
			return SafeClrConvert.ToULong(x).ToEnum<T>(throwOnErrors);
		}
		public static T ToEnum<T>(this Double x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			return SafeClrConvert.ToULong(x).ToEnum<T>(defaultValue, throwOnErrors);
		}
		public static object ToEnum(this Double x, Type type, bool throwOnErrors = false)
		{
			return SafeClrConvert.ToULong(x).ToEnum(type, throwOnErrors);
		}
		public static object ToEnum(this Double x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			return SafeClrConvert.ToULong(x).ToEnum(type, defaultValue, throwOnErrors);
		}
			public static T ToEnum<T>(this Decimal x, bool throwOnErrors = false) where T : struct
		{
			return SafeClrConvert.ToULong(x).ToEnum<T>(throwOnErrors);
		}
		public static T ToEnum<T>(this Decimal x, T defaultValue, bool throwOnErrors = false) where T : struct
		{
			return SafeClrConvert.ToULong(x).ToEnum<T>(defaultValue, throwOnErrors);
		}
		public static object ToEnum(this Decimal x, Type type, bool throwOnErrors = false)
		{
			return SafeClrConvert.ToULong(x).ToEnum(type, throwOnErrors);
		}
		public static object ToEnum(this Decimal x, Type type, object defaultValue, bool throwOnErrors = false)
		{
			return SafeClrConvert.ToULong(x).ToEnum(type, defaultValue, throwOnErrors);
		}
		}
}