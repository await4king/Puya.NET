using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Service
{
    public class ServiceRequest
    {
    }
    public class ServiceRequest<T> : ServiceRequest
    {
        public T Model { get; set; }
    }
    public class ServiceRequestByPK<PK> : ServiceRequest
    {
        public PK Key { get; set; }
    }
    public class ServiceRequestByPK<PK1, PK2> : ServiceRequest
    {
        public PK1 Key1 { get; set; }
        public PK2 Key2 { get; set; }
    }
    public class ServiceRequestByPK<PK1, PK2, PK3> : ServiceRequest
    {
        public PK1 Key1 { get; set; }
        public PK2 Key2 { get; set; }
        public PK3 Key3 { get; set; }
    }
    public class ServiceFilteringRequest: ServiceRequest
    {
        public string Filter { get; set; }
        public string[] OrderBy { get; set; }
        public string[] OrderDir { get; set; }
    }
    public class ServicePagingRequest : ServiceFilteringRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    public enum OrderDirection {  asc, desc }
    public enum FilterComparisonType
    {
        eq,
        neq,
        gt,
        gte,
        lt,
        lte,
        contains,
        notcontains,
        @null,
        notnull,
        empty,
        notempty,
        startswith,
        notstartswith,
        endswith,
        notendswith,
        @in,
        notin
    }
    public enum FilterOperatorType
    {
        and,
        or,
        not,
        xor
    }
    public enum FilterArithmaticOperatorType
    {
        plus,
        minus,
        multiply,
        div,
        remain
    }
    public enum FilterComposition
    {
        openparentheses,
        closeparentheses
    }
    public enum ColumnType
    {
        @string,
        number,
        @bool,
        date,
        time,
        datetime,
        binary
    }
    public class OrderCriteria
    {
        public string By { get; set; }
        private string _dir;
        public string Dir
        {
            get
            {
                if (string.IsNullOrEmpty(_dir))
                {
                    _dir = "asc";
                }

                return _dir;
            }
            set
            {
                if (string.Compare(value, "asc", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(value, "desc", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _dir = value;
                }
                else
                {
                    _dir = "asc";
                }
            }
        }
        public OrderDirection Direction
        {
            get
            {
                return (OrderDirection)Enum.Parse(typeof(OrderDirection), Dir);
            }
            set
            {
                _dir = value.ToString();
            }
        }
    }
    public class ServiceSelectRecordsRequest
    {
        public List<string> Columns { get; set; }
        public List<OrderCriteria> Orders { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public ServiceSelectRecordsRequest()
        {
            Orders = new List<OrderCriteria>();
        }
    }
}
