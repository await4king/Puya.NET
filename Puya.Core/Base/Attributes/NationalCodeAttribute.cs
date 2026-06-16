using System;

namespace Puya.Base
{
    // requires a prop to be a string containing an iranian national code.
    // validating a national code is done using a Validation.IsNationalCode()
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NationalCodeAttribute : RegExpAttribute   // requires a prop to be a string containing a national code
    {
        public NationalCodeAttribute() : base("")
        { }
    }
}
