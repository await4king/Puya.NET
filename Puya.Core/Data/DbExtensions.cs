using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Puya.Extensions;
using Puya.Reflection;

namespace Puya.Data
{
	public static partial class DbExecuteExtensions
	{
		public static void SetContextInfo(this IDbContextInfoProvider contextInfo, IDictionary<string, object> data)
        {
			if (data != null && data.Count > 0)
			{
				var sb = new StringBuilder();

				foreach (var item in data)
				{
					sb.Append($"{(sb.Length == 0 ? "": ",")}{item.Key}={item.Value}");
				}

				contextInfo.SetContextInfo(sb.ToString());
			}
        }
		public static void SetContextInfoAsCsv(this IDbContextInfoProvider contextInfo, object data)
		{
			if (data != null)
			{
				var props = ReflectionHelper.GetPublicInstanceReadableProperties(data.GetType());

				if (props != null && props.Length > 0)
				{
					var sb = new StringBuilder();

					foreach (var prop in props)
					{
						sb.Append($"{(sb.Length == 0 ? "" : ",")}{prop.Name}={prop.GetValue(data)}");
					}

					contextInfo.SetContextInfo(sb.ToString());
				}
			}
		}
		public static void SetContextInfoAsJson(this IDbContextInfoProvider contextInfo, object data)
		{
			if (data != null)
			{
				contextInfo.SetContextInfo(JsonConvert.SerializeObject(data));
			}
		}
	}
}
