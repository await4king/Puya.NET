using Puya.Data;
using System.Data;

namespace Puya.Mapping
{
    public static class Extensions
    {
        public static T Map<T>(this IMapper mapper, IDataReader reader)
        {
            if (mapper == null)
            {
                return reader.ConvertTo<T>();
            }

            return (T)mapper.Map(reader, typeof(T));
        }
        public static T Map<T>(this IMapper mapper, object source)
        {
            if (source != null && source.GetType() == typeof(IDataReader))
            {
                return mapper.Map<T>(source as IDataReader);
            }

            return (T)mapper.Map(typeof(T), source);
        }
    }
}
