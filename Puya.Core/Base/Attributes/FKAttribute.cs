using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FKAttribute : Attribute
    {
        public string ReferredTable { get; set; }
        public string ReferredPk { get; }
        public string ConstraintName { get; }

        public FKAttribute(string referredTable, string referredPk = "", string constraint_name = "")
        {
            ReferredTable = referredTable;
            ReferredPk = referredPk;
            ConstraintName = constraint_name;
        }
    }
}
