using System;

namespace Puya.Data
{
    public static class DataHelper
    {
        public static Type TypeOfCommandParameter { get; private set; }
        static DataHelper()
        {
            TypeOfCommandParameter = typeof(CommandParameter);
        }
    }
}
