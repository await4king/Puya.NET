using Puya.Service;

namespace Puya.Service
{
    public class ReportResponse : ServiceResponse<ReportData>
    {
    }
    public class ReportResponse<T> : ServiceResponse<ReportData<T>>
    {
    }
}
