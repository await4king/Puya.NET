namespace Puya.Service
{
    public partial class ServiceResponse
    {
        public static ServiceResponse Succeeded()
        {
            var result = new ServiceResponse();

            result.Succeeded();

            return result;
        }
        public static ServiceResponse Succeeded<T>(T data)
        {
            var result = new ServiceResponse();

            result.SetData(data);
            result.Succeeded();

            return result;
        }
        public static ServiceResponse FromStatus(string status, string message = null)
        {
            var result = new ServiceResponse();

            result.SetStatus(status, null, message);

            return result;
        }
        public static ServiceResponse Failed()
        {
            return FromStatus(ServiceConstants.ServiceResponse.Failed);
        }
        public static ServiceResponse Faulted()
        {
            return FromStatus(ServiceConstants.ServiceResponse.Faulted);
        }
        public static ServiceResponse NotFound()
        {
            return FromStatus(ServiceConstants.ServiceResponse.NotFound);
        }
        public static ServiceResponse Deleted()
        {
            return FromStatus(ServiceConstants.ServiceResponse.Deleted);
        }
        public static ServiceResponse AlreadyExists()
        {
            return FromStatus(ServiceConstants.ServiceResponse.AlreadyExists);
        }
        public static ServiceResponse AccessDenied()
        {
            return FromStatus(ServiceConstants.ServiceResponse.AccessDenied);
        }
        public static ServiceResponse NotAuthenticated()
        {
            return FromStatus(ServiceConstants.ServiceResponse.NotAuthenticated);
        }
        public static ServiceResponse NotAuthorized()
        {
            return FromStatus(ServiceConstants.ServiceResponse.NotAuthorized);
        }
        public static ServiceResponse Broke()
        {
            return FromStatus(ServiceConstants.ServiceResponse.Broke);
        }
        public static ServiceResponse Closed()
        {
            return FromStatus(ServiceConstants.ServiceResponse.Closed);
        }
        public static ServiceResponse Aborted()
        {
            return FromStatus(ServiceConstants.ServiceResponse.Aborted);
        }
        public static ServiceResponse Crashed()
        {
            return FromStatus(ServiceConstants.ServiceResponse.Crashed);
        }
    }
}
