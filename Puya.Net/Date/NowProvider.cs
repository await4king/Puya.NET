using System;
using System.Collections.Generic;
using System.Text;
using Puya.Base;

namespace Puya.Date
{
    public class NowProvider : InstanceProvider<INow, DateTimeNow>
    { }
}
