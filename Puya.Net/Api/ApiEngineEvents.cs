namespace Puya.Api
{
    public enum ApiEngineEvents
    {
        None,
        /// <summary>
        /// Triggers as soon as engine intends to serve the request, before doing any operation like processing the request, finding application and api in the registry, etc.
        /// </summary>
        Starting,
        /// <summary>
        /// The request is successfully processed. This event triggers before finding application and api info corresponding to the incomming request.
        /// </summary>
        Loading,
        /// <summary>
        /// The api was found successfully. This event triggers before locating api service and action
        /// </summary>
        Locating,
        /// <summary>
        /// The service and action are successfully located. This event triggers before executing service action.
        /// </summary>
        Executing,
        /// <summary>
        /// The action was executed successfully. This event triggers before serializing the response.
        /// </summary>
        Serializing,
        /// <summary>
        /// This is the last step in serving the request. It's the last hook in order to apply any custom business logic or operation before sending back the response. It's usefull for finalization, cleanup or overriding the response.
        /// </summary>
        Ending
    }
}
