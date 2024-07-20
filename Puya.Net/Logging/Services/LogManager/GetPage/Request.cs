using Puya.Service;
using System;
using System.Collections.Generic;
using Puya.Logging.Models;

namespace Puya.Logging.Services.LogManager
{
	public partial class TapLogManagerGetPageRequest : ServiceRequest
    {
		public int Page { get; set; }
		public int PageSize { get; set; }
		public Puya.Data.CommandParameter RecordCount { get; set; }
		public Puya.Data.CommandParameter PageCount { get; set; }
		public int AppId { get; set; }
		public Models.LogType LogType { get; set; }
		public OperationResult OperationResult { get; set; }
		public string Category { get; set; }
		public string MemberName { get; set; }
		public string User { get; set; }
		public string Ip { get; set; }
		public string Message { get; set; }
		public string Data { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public string OrderBy { get; set; }
		public string OrderDir { get; set; }
	}
}
