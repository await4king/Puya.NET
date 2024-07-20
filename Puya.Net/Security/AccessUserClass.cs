using Puya.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Puya.Conversion;

namespace Puya.Security
{
    public class AccessUserClass : IAccessUserClass
    {
        private readonly IDb _db;
        public AccessUserClass(IDb db)
        {
            this._db = db;
        }
        public bool IsAdmin(string username = "")
        {
            var isAdmin = _db.ExecuteScalerSql($"SELECT dbo.UDF_GetIsAdmin(N'{(string.IsNullOrEmpty(username) ? FUserName: username)}')");

            return SafeClrConvert.ToBoolean(isAdmin);
        }
        public string CurrentSystemId { get; set; }
        private string FUserName;
        public string UserName
        {
            get { return FUserName; }
            set { FUserName = value; }
        }
        public long GetAccessBs(string DialogName, string Section)
        {
            long result = 0;

            if (IsAdmin())
            {
                unchecked
                {
                    result = 0xAAAAAAAA;
                }
            }
            else
            {
                var access = _db.ExecuteScalerSql($"SELECT dbo.UDF_GetAccessBs(N'{CurrentSystemId}', N'{FUserName}', N'{DialogName}', N'{Section}')");
                
                result = SafeClrConvert.ToLong(access);
            }

            return result;
        }
        public long GetAccessBs(string DialogName, string Section, TBrowseAccess Access)
        {
            long Result = 0;

            if (IsAdmin())
            {
                switch (Access)
                {
                    case TBrowseAccess.bsacView:
                    case TBrowseAccess.bsacEdit:
                    case TBrowseAccess.bsacDelete:
                    case TBrowseAccess.bsacDeleteBook:
                    case TBrowseAccess.bsacConfirm:
                    case TBrowseAccess.bsacDecide:
                    case TBrowseAccess.bsacRegister:
                    case TBrowseAccess.bsacCheckout:
                    case TBrowseAccess.bsacLock:
                        Result = 2;
                        break;
                    case TBrowseAccess.bsacCreate:
                    case TBrowseAccess.bsacPrint:
                    case TBrowseAccess.bsacResort:
                        Result = 1;
                        break;
                }
            }

            Result = StripZeroes(GetAccessBs(DialogName, Section) & (long)(Access));

            return Result;
        }
        public long GetAccessCg(string DialogName)
        {
            long result = 0;

            if (IsAdmin())
            {
                result = 0x5555;
            }
            else
            {
                var access = _db.ExecuteScalerSql($"SELECT dbo.UDF_GetAccessCg(N'{CurrentSystemId}', N'{FUserName}', N'{DialogName}')");

                result = SafeClrConvert.ToLong(access);
            }

            return result;
        }
        public long GetAccessCb(string DialogName)
        {
            long result = 0;

            if (IsAdmin())
            {
                result = 0x5555;
            }
            else
            {
                var access = _db.ExecuteScalerSql($"SELECT dbo.UDF_GetAccessCb(N'{CurrentSystemId}', N'{FUserName}', N'{DialogName}')");

                result = SafeClrConvert.ToLong(access);
            }

            return result;
        }
        public long GetAccessOp(string Operation, string Section)
        {
            long result = 0;

            if (IsAdmin())
            {
                result = 1;
            }
            else
            {
                var access = _db.ExecuteScalerSql($"SELECT dbo.UDF_GetAccessOp(N'{CurrentSystemId}', N'{FUserName}', N'{Operation}', N'{Section}')");

                result = SafeClrConvert.ToLong(access);
            }

            return result;
        }
        public bool IsSubordinate(string Subordinate, string Head)
        {
            var result = _db.ExecuteScalerSql($@" SELECT 1 FROM VW_SecuritySubordinates 
                                                  WHERE Head = N'{Head}' AND Subordinate = N'{Subordinate}''");
            return result != null;
        }
        public long StripZeroes(long I)
        {
            while ((I > 0) && ((I & 0x00000003) == 0)) I >>= 2;

            return I;
        }
        public bool HasAccessBs(string DialogName, string Section, string CreatedBy, TBrowseAccess Access)
        {
            var result = false;
            long IntAcc;

            if (IsAdmin(CreatedBy))
            {
                result = true;
            }
            else
            {
                IntAcc = StripZeroes(GetAccessBs(DialogName, Section) & (long)(Access));

                switch (Access)
                {
                    case TBrowseAccess.bsacView:
                    case TBrowseAccess.bsacEdit:
                    case TBrowseAccess.bsacDelete:
                    case TBrowseAccess.bsacDeleteBook:
                    case TBrowseAccess.bsacConfirm:
                    case TBrowseAccess.bsacDecide:
                    case TBrowseAccess.bsacRegister:
                    case TBrowseAccess.bsacCheckout:
                    case TBrowseAccess.bsacLock:
                        result = ((IntAcc & 3) == 2) ||
                                 ((IntAcc & 1) == 1) && (string.IsNullOrEmpty(CreatedBy) || (FUserName == CreatedBy)) ||
                                 ((IntAcc & 3) == 3) && IsSubordinate(CreatedBy, FUserName);
                        break;
                    case TBrowseAccess.bsacCreate:
                    case TBrowseAccess.bsacPrint:
                    case TBrowseAccess.bsacResort:
                    case TBrowseAccess.bsacExeclExport:
                    case TBrowseAccess.bsacCopy:
                    case TBrowseAccess.bsacPaste:
                        result = (IntAcc & 1) == 1;
                        break;
                }
            }

            return result;
        }

        public bool HasAccessCg(string DialogName, TCatalogAccess Access)
        {
            return (GetAccessCg(DialogName) & (long)(Access)) == (long)(Access);
        }
        public bool HasAccessCb(string DialogName, TCatalogAccess Access)
        {
            return (GetAccessCb(DialogName) & (long)(Access)) == (long)(Access);
        }

        public long GetAccessCb(string DialogName, TCatalogAccess Access)
        {
            return StripZeroes(GetAccessCb(DialogName) & (long)Access);
        }

        public bool VerifyAccess(bool HasAccess)
        {
            var result = HasAccess;

            //if (!HasAccess)
            //    throw new Exception("سطح دسترسي شما کافي نمي باشد");

            return result;
        }

        public bool VerifyAccessCb(string DialogName, TCatalogAccess Access)
        {
            return VerifyAccess(HasAccessCb(DialogName, Access));
        }

        public bool VerifyAccessBs(string DialogName, string Section, string CreatedBy, TBrowseAccess Access)
        {
            return VerifyAccess(HasAccessBs(DialogName, Section, CreatedBy, Access));
        }

        public bool VerifyAccessCg(string DialogName, TCatalogAccess Access)
        {
            return VerifyAccess(HasAccessCg(DialogName, Access));
        }

        public bool VerifyAccessOp(string Operation, string Section)
        {
            return VerifyAccess(HasAccessOp(Operation, Section));
        }

        public bool HasAccessOp(string Operation, string Section)
        {
            return GetAccessOp(Operation, Section) == 1;
        }
    }
}
