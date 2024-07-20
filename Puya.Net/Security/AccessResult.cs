using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Security
{
    public class AccessFormResult
    {
        public bool View { get; set; }
        public bool Edit { get; set; }
        public bool Create { get; set; }
        public bool Delete { get; set; }
        public bool Confirm { get; set; }
        public bool Decide { get; set; }
        public bool Register { get; set; }
        public bool Checkout { get; set; }
        public bool Print { get; set; }
        public bool Resort { get; set; }
        public bool DeleteBook { get; set; }
        public bool Lock { get; set; }
        public bool Sign { get; set; }
        public bool ExeclExport { get; set; }
        public bool Copy { get; set; }
        public bool Paste { get; set; }
    }
    public class AccessCatalogResult
    {
        public bool View { get; set; }
        public bool Edit { get; set; }
        public bool Create { get; set; }
        public bool Delete { get; set; }
    }
}
