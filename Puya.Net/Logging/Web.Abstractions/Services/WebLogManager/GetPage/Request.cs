using Puya.Service;
using System;
using Puya.Logging.Models;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class TapWebLogManagerGetPageRequest : ServiceRequest
    {
		public int Page { get; set; }
		public int PageSize { get; set; }
		public Puya.Data.CommandParameter RecordCount { get; set; }
		public Puya.Data.CommandParameter PageCount { get; set; }
		public int AppId { get; set; }
		public Logging.Models.LogType LogType { get; set; }
		public OperationResult OperationResult { get; set; }
		public string Category { get; set; }
		public string MemberName { get; set; }
		public string User { get; set; }
		public string Ip { get; set; }
		public string Message { get; set; }
		public string Data { get; set; }
		public string Method { get; set; }
		public string Url { get; set; }
		public string BrowserName { get; set; }
		public string BrowserVersion { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public string OrderBy { get; set; }
		public string OrderDir { get; set; }
	}
}
