using Puya.Base;
using Puya.Date;

namespace Puya.Service
{
    public static class ServiceConstants
    {
        public static class ServiceResponse
        {
            public static string[] SuccessKeys { get; set; }
            public static string Success { get; set; }
            public static string Failed { get; set; }
            public static string Faulted { get; set; }
            public static string Errored { get; set; }
            public static string Flawed { get; set; }
            public static string Halted { get; set; }
            public static string NotFound { get; set; }
            public static string Deleted { get; set; }
            public static string AlreadyExists { get; set; }
            public static string AccessDenied { get; set; }
            public static string NotAuthenticated { get; set; }
            public static string NotAuthorized { get; set; }
            static ServiceResponse()
            {
                SuccessKeys = new string[] { "success", "succeed" };
                Success = "Success";
                Failed = "Failed";
                Faulted = "Faulted";
                Errored = "Errored";
                Flawed = "Flawed";
                Halted = "Halted";
                NotFound = "NotFound";
                Deleted = "Deleted";
                AlreadyExists = "AlreadyExists";
                AccessDenied = "AccessDenied";
                NotAuthenticated = "NotAuthenticated";
                NotAuthorized = "NotAuthorized";
            }
        }
        public static IObjectActivator ObjectActivator { get; set; }
        public static INow Now { get; set; }
        static ServiceConstants()
        {
            Now = new DateTimeNow();
            ObjectActivator = new ObjectActivatorDefault();
        }
    }
}
