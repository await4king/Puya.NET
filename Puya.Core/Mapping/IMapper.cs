using System;
using System.Data;

namespace Puya.Mapping
{
    public interface IMapper
    {
        object Map(IDataReader reader, Type type);
        void Map(IDataReader reader, ref object target);
        object Map(Type type, object source);
        void Copy(object source, object target);
    }
}