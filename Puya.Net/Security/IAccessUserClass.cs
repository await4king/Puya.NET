using System;

namespace Puya.Security
{
    public enum TBrowseAccess : long
    {
        bsacView = 0x00000003, //private, public, sub   مشاهده
        bsacEdit = 0x0000000C, //private, public, sub   ويرايش
        bsacCreate = 0x00000030, //grant, deny  ايجاد/ثبت
        bsacDelete = 0x000000C0, //grant, deny, sub حذف
        bsacConfirm = 0x00000300, //private, public, deny, sub  تاييد
        bsacDecide = 0x00000C00, //private, public, deny, sub      تصويب
        bsacRegister = 0x00003000, //private, public, deny, sub    ثبت در دفاتر
        bsacCheckout = 0x0000C000, //private, public, deny, sub  ابطال
        bsacPrint = 0x00030000, //private, public, deny, sub    چاپ
        bsacResort = 0x000C0000, //private, public, deny, sub   مرتب سازي
        bsacDeleteBook = 0x00300000, //grant, deny  
        bsacLock = 0x00C00000, //grant, deny    قفل
        bsacSign = 0x03000000,  // امضا
        bsacExeclExport = 0x0C000000,   // گزارش اکسل
        bsacCopy = 0x30000000,  // کپي
        bsacPaste = 0xC0000000  // پيست
    } //private, public, deny, sub
    public enum TCatalogAccess
    {
        cgacView = 0x0001,   //grant, deny
        cgacEdit = 0x0004,   //grant, deny
        cgacCreate = 0x0010,   //grant, deny
        cgacDelete = 0x0040    //grant, deny
    }
    public interface IAccessUserClass
    {
        string CurrentSystemId { get; set; }
        string UserName { get; set; }
        long GetAccessBs(string DialogName, string Section);
        long GetAccessBs(string DialogName, string Section, TBrowseAccess Access);
        long GetAccessCg(string DialogName);
        long GetAccessCb(string DialogName);
        long GetAccessCb(string DialogName, TCatalogAccess Access);
        long GetAccessOp(string Operation, string Section);
        bool IsAdmin(string username = "");
        bool IsSubordinate(string Subordinate, string Head);
        long StripZeroes(long I);
        bool HasAccessBs(string DialogName, string Section, string CreatedBy, TBrowseAccess Access);
        bool HasAccessCg(string DialogName, TCatalogAccess Access);
        bool HasAccessCb(string DialogName, TCatalogAccess Access);
        bool HasAccessOp(string Operation, string Section);
        bool VerifyAccess(bool HasAccess);
        bool VerifyAccessCb(string DialogName, TCatalogAccess Access);
        bool VerifyAccessBs(string DialogName, string Section, string CreatedBy, TBrowseAccess Access);
        bool VerifyAccessCg(string DialogName, TCatalogAccess Access);
        bool VerifyAccessOp(string Operation, string Section);
    }
}
