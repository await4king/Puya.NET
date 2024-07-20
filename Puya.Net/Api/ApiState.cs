namespace Puya.Api
{
    public enum ApiState
    {
        Starting,

        CreatingApiRequest,
        ReadingRequestForm,
        ReadingRequestBody,
        ReadingRequestQueryString,
        DecryptingRequest,
        DeserializingRequest,
        DeserializingRequestData,
        ValidatingRawRequest,
        ValidatingRequest,
        FinalizingRequest,

        Loading,

        FindingApp,
        ValidatingApp,
        FindingApi,
        ValidatingApi,

        Locating,

        FindingService,
        ResolvingService,
        ValidatingService,
        FindingAction,
        ValidatingAction,
        ValidatingActionRequest,
        ConvertingActionRequest,

        Executing,

        RunningAction,
        CopyingActionResponse,
        CopyingActionResponseData,

        Serializing,

        SerializingResponse,
        EncryptingResponse,

        Ending
    }
}
