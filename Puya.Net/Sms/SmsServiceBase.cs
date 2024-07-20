using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Extensions;
using Puya.Service;

namespace Puya.Sms
{
    public abstract class SmsServiceBase<TConfig> : ISmsService
        where TConfig : SmsConfigItem, new()
    {
        TConfig _config;
        ISmsLogger _logger;
        public SmsServiceBase(TConfig config, ISmsLogger logger)
        {
            _config = config;
            _logger = logger;
        }
        public SmsServiceBase(ISmsLogger logger) : this(null, logger)
        { }
        public SmsServiceBase(TConfig config) : this(config, new SmsLoggerNull())
        { }
        public SmsServiceBase(): this(null, new SmsLoggerNull())
        { }
        public virtual TConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = new TConfig();
                }

                return _config;
            }
            set { _config = value; }
        }
        public virtual ISmsLogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }
        protected abstract SendResponse SendInternal(string mobile, string message);
        protected abstract Task<SendResponse> SendAsyncInternal(string mobile, string message, CancellationToken cancellation);
        public SendResponse Send(string mobile, string message)
        {
            var result = new SendResponse();

            try
            {
                var sr = SendInternal(mobile, message);

                if (sr != null)
                {
                    result.Copy(sr);
                }
                else
                {
                    result.Succeeded();
                }

                Logger?.Log(new SmsLog { Topic = "Sent", MobileNo = mobile, Message = message, Success = result.Success, Response = sr?.Data?.Response, Data = sr?.Data?.Data, Error = sr?.Data?.Error });
            }
            catch (Exception e)
            {
                Logger?.Log(new SmsLog { Topic = "SendError", MobileNo = mobile, Message = message, Success = false, Error = e });

                result.Failed(e);
            }

            return result;
        }

        public async Task<SendResponse> SendAsync(string mobile, string message, CancellationToken cancellation)
        {
            var result = new SendResponse();

            try
            {
                var sr = await SendAsyncInternal(mobile, message, cancellation);

                if (sr != null)
                {
                    result.Copy(sr);
                }
                else
                {
                    result.Succeeded();
                }

                await Logger?.LogAsync(new SmsLog { MobileNo = mobile, Message = message, Success = result.Success, Response = sr?.Data?.Response, Data = sr?.Data?.Data }, cancellation);
            }
            catch (Exception e)
            {
                await Logger?.LogAsync(new SmsLog { MobileNo = mobile, Message = message, Success = false, Error = e }, cancellation);

                result.Failed(e);
            }

            return result;
        }
        public void Debug(string message, object data = null)
        {
            if (Config?.Debug ?? false)
            {
                Logger?.Log(new SmsLog { Message = message, Data = data });
            }
        }
        public void Warn(string message, object data = null)
        {
            if (Config?.Debug ?? false)
            {
                Logger?.Log(new SmsLog { Message = message, Data = data });
            }
        }
        public void Danger(Exception e, string message, object data = null)
        {
            if (Config?.Debug ?? false)
            {
                Logger?.Log(new SmsLog { Message = message, Data = data, Error = e });
            }
        }
    }
}
