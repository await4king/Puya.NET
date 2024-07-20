namespace Puya.Api
{
    public static class ApiEngineConstants
    {
        private static int bodyStreamBufferSize;
        public static int BodyStreamBufferSize
        {
            get
            {
                if (bodyStreamBufferSize <= 0)
                {
                    bodyStreamBufferSize = 10240;
                }

                return bodyStreamBufferSize;
            }
            set { bodyStreamBufferSize = value; }
        }
        private static string encryptedRequestHeaderName;
        public static string EncryptedRequestHeaderName
        {
            get
            {
                if (string.IsNullOrEmpty(encryptedRequestHeaderName))
                {
                    encryptedRequestHeaderName = "x-request-encrypted";
                }

                return encryptedRequestHeaderName;
            }
            set { encryptedRequestHeaderName = value; }
        }
        private static string encryptedResponseHeaderName;
        public static string EncryptedResponseHeaderName
        {
            get
            {
                if (string.IsNullOrEmpty(encryptedResponseHeaderName))
                {
                    encryptedResponseHeaderName = "x-response-encrypted";
                }

                return encryptedResponseHeaderName;
            }
            set { encryptedResponseHeaderName = value; }
        }
        public static bool ShowDetailedEnginePipeline { get; set; }
        public static bool RevealExceptions { get; set; }
        private static string schemaListResponseHeader;
        public static string SchemaListResponseHeader
        {
            get
            {
                if (string.IsNullOrEmpty(schemaListResponseHeader))
                {
                    schemaListResponseHeader = "x-schema-list";
                }

                return schemaListResponseHeader;
            }
            set { schemaListResponseHeader = value; }
        }
        private static string apiSettingsEncryptedRequestName;
        public static string ApiSettingsEncryptedRequestName
        {
            get
            {
                if (string.IsNullOrEmpty(apiSettingsEncryptedRequestName))
                {
                    apiSettingsEncryptedRequestName = "EncryptedRequest";
                }

                return apiSettingsEncryptedRequestName;
            }
            set { apiSettingsEncryptedRequestName = value; }
        }
        private static string apiSettingsEncryptedResponseName;
        public static string ApiSettingsEncryptedResponseName
        {
            get
            {
                if (string.IsNullOrEmpty(apiSettingsEncryptedResponseName))
                {
                    apiSettingsEncryptedResponseName = "EncryptedResponse";
                }

                return apiSettingsEncryptedResponseName;
            }
            set { apiSettingsEncryptedResponseName = value; }
        }
    }
}
