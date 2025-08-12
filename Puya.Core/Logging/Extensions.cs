using Puya.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public static class Extensions
    {
        #region Danger
        public static void Danger(this ILogger logger,
                                Exception e,
                                string category,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Error,
                OperationResult = OperationResult.Danger,
                Category = category,
                Message = e.ToString("\n"),
                StackTrace = e.StackTrace,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Danger(this ILogger logger,
                                Exception e,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            logger.Danger(e, "", data, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task DangerAsync(this ILogger logger,
                                Exception e,
                                string category,
                                object data,
                                CancellationToken cancellation,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                LogType = LogType.Error,
                OperationResult = OperationResult.Danger,
                Category = category,
                Message = e.ToString("\n"),
                StackTrace = e.StackTrace,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task DangerAsync(this ILogger logger,
                                Exception e,
                                string category,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return logger.DangerAsync(e, category, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task DangerAsync(this ILogger logger,
                                Exception e,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return logger.DangerAsync(e, "", data, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Fatal
        public static void Fatal(this ILogger logger,
                                Exception e,
                                string category,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Error,
                OperationResult = OperationResult.Fatal,
                Category = category,
                Message = e.ToString("\n"),
                StackTrace = e.StackTrace,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Fatal(this ILogger logger,
                                Exception e,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            logger.Fatal(e, "", data, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task FatalAsync(this ILogger logger,
                                Exception e,
                                string category,
                                object data,
                                CancellationToken cancellation,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                LogType = LogType.Error,
                OperationResult = OperationResult.Fatal,
                Category = category,
                Message = e.ToString("\n"),
                StackTrace = e.StackTrace,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task FatalAsync(this ILogger logger,
                                Exception e,
                                string category,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return logger.FatalAsync(e, category, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task FatalAsync(this ILogger logger,
                                Exception e,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return logger.FatalAsync(e, "", data, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Trace
        public static void Trace(this ILogger logger,
                                string category = "",
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Trace,
                OperationResult = OperationResult.Normal,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Trace(this ILogger logger,
                                string category = "",
                                Func<object> fnGetData = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Trace,
                OperationResult = OperationResult.Normal,
                GetData = fnGetData,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Trace(this ILogger logger,
                                string category,
                                string message = "",
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Trace,
                OperationResult = OperationResult.Normal,
                Data = data,
                Message = message,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Trace(this ILogger logger,
                                string category,
                                string message = "",
                                Func<object> fnGetData = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            logger.Trace(category, message, fnGetData, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task TraceAsync(this ILogger logger,
                                string category,
                                string message,
                                object data,
                                CancellationToken cancellation,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Trace,
                OperationResult = OperationResult.Normal,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath,
                Message = message,
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task TraceAsync(this ILogger logger,
                                string category,
                                string message,
                                Func<object> fnGetData,
                                CancellationToken cancellation,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Trace,
                OperationResult = OperationResult.Normal,
                GetData = fnGetData,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath,
                Message = message,
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task TraceAsync(this ILogger logger,
                                string category,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return logger.TraceAsync(category, "", data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task TraceAsync(this ILogger logger,
                                string category,
                                Func<object> fnGetData,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return logger.TraceAsync(category, "", fnGetData, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task TraceAsync(this ILogger logger,
                                string category,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return logger.TraceAsync(category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task TraceAsync(this ILogger logger,
                                string category,
                                string message,
                                Func<object> fnGetData,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return logger.TraceAsync(category, message, fnGetData, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Cancel
        public static void Cancel(this ILogger logger,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                OperationResult = OperationResult.Cancel,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Cancel(this ILogger logger,
                                    string category,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                OperationResult = OperationResult.Cancel,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task CancelAsync(this ILogger logger,
                                    string category,
                                    string message,
                                    object data,
                                    CancellationToken cancellation,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                OperationResult = OperationResult.Cancel,
                Category = category,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        
        public static Task CancelAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return CancelAsync(logger, category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task CancelAsync(this ILogger logger,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return CancelAsync(logger, "", message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Success
        public static void Success(this ILogger logger,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                OperationResult = OperationResult.Success,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Success(this ILogger logger,
                                    string category,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                OperationResult = OperationResult.Success,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task SuccessAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data,
                                        CancellationToken cancellation,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                OperationResult = OperationResult.Success,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task SuccessAsync(this ILogger logger,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return SuccessAsync(logger, "", message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        
        public static Task SuccessAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return SuccessAsync(logger, category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Fault
        public static void Fault(this ILogger logger,
                                string category,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Error,
                OperationResult = OperationResult.Fault,
                Category = category,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Fault(this ILogger logger,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Error,
                OperationResult = OperationResult.Fault,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task FaultAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data,
                                        CancellationToken cancellation,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Error,
                OperationResult = OperationResult.Fault,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task FaultAsync(this ILogger logger,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return FaultAsync(logger, "", message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task FaultAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return FaultAsync(logger, category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Fail
        public static void Fail(this ILogger logger,
                                string category,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Error,
                OperationResult = OperationResult.Failure,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Fail(this ILogger logger,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Error,
                OperationResult = OperationResult.Failure,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task FailAsync(this ILogger logger,
                                    string category,
                                    string message, object data,
                                    CancellationToken cancellation,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Error,
                OperationResult = OperationResult.Failure,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task FailAsync(this ILogger logger,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return FailAsync(logger, "", message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task FailAsync(this ILogger logger,
                                    string category,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return FailAsync(logger, category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Error
        public static void Error(this ILogger logger,
                                string category,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Error,
                OperationResult = OperationResult.Error,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Error(this ILogger logger,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Error,
                OperationResult = OperationResult.Error,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task ErrorAsync(this ILogger logger,
                                string category,
                                string message,
                                object data,
                                CancellationToken cancellation,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Error,
                OperationResult = OperationResult.Error,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task ErrorAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return ErrorAsync(logger, category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task ErrorAsync(this ILogger logger,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return ErrorAsync(logger, "", message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Abort
        public static void Abort(this ILogger logger,
                                string category,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                OperationResult = OperationResult.Abort,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Abort(this ILogger logger,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                OperationResult = OperationResult.Abort,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task AbortAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data,
                                        CancellationToken cancellation,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                OperationResult = OperationResult.Abort,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        
        public static Task AbortAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return AbortAsync(logger, category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task AbortAsync(this ILogger logger,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return AbortAsync(logger, "", message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Alert
        public static void Alert(this ILogger logger,
                                string category,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Alert,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Alert(this ILogger logger,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0
                                )
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Alert,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task AlertAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data,
                                        CancellationToken cancellation,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0
                                        )
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Alert,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log,cancellation);
        }
        public static Task AlertAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return AlertAsync(logger, category, message, data, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task AlertAsync(this ILogger logger,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return AlertAsync(logger, "", message, data, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Debug
        public static void Debug(this ILogger logger,
                                    string category,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Debug,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Debug(this ILogger logger,
                                    string category,
                                    string message,
                                    Func<object> fnGetData,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Debug,
                Message = message,
                GetData = fnGetData,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Debug(this ILogger logger,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Debug,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Debug(this ILogger logger,
                                    string message,
                                    Func<object> fnGetData,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Debug,
                Message = message,
                GetData = fnGetData,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task DebugAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data,
                                        CancellationToken cancellation,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Debug,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task DebugAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        Func<object> fnGetData,
                                        CancellationToken cancellation,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Debug,
                Message = message,
                GetData = fnGetData,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task DebugAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return DebugAsync(logger, category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task DebugAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        Func<object> fnGetData,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return DebugAsync(logger, category, message, fnGetData, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task DebugAsync(this ILogger logger,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return DebugAsync(logger, "", message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task DebugAsync(this ILogger logger,
                                        string message,
                                        Func<object> fnGetData,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return DebugAsync(logger, "", message, fnGetData, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Info
        public static void Info(this ILogger logger,
                                string category,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Info(this ILogger logger,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task InfoAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data,
                                        CancellationToken cancellation,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task InfoAsync(this ILogger logger,
                                    string category,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return InfoAsync(logger, category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task InfoAsync(this ILogger logger,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return InfoAsync(logger, "", message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Warn
        public static void Warn(this ILogger logger,
                                string category,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Warning,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Warn(this ILogger logger,
                                string message,
                                object data = null,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Warning,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task WarnAsync(this ILogger logger,
                                    string category,
                                    string message,
                                    object data,
                                    CancellationToken cancellation,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Warning,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task WarnAsync(this ILogger logger,
                                    string category,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return WarnAsync(logger, category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task WarnAsync(this ILogger logger,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return WarnAsync(logger, "", message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        #region Suggest
        public static void Suggest(this ILogger logger,
                                    string category,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Suggestion,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static void Suggest(this ILogger logger,
                                    string message,
                                    object data = null,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string sourceFilePath = "",
                                    [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return;

            var log = new Log
            {
                LogType = LogType.Suggestion,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            logger.Log(log);
        }
        public static Task SuggestAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data,
                                        CancellationToken cancellation,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            if (logger == null || logger is NullLogger)
                return Task.CompletedTask;

            var log = new Log
            {
                Category = category,
                LogType = LogType.Suggestion,
                Message = message,
                Data = data,
                MemberName = memberName,
                Line = sourceLineNumber,
                File = sourceFilePath
            };

            return logger.LogAsync(log, cancellation);
        }
        public static Task SuggestAsync(this ILogger logger,
                                        string category,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return SuggestAsync(logger, category, message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        public static Task SuggestAsync(this ILogger logger,
                                        string message,
                                        object data = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string sourceFilePath = "",
                                        [CallerLineNumber] int sourceLineNumber = 0)
            
        {
            return SuggestAsync(logger, "", message, data, CancellationToken.None, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion
        public static string SerializeData(this ILogFormatter formatter, Log log)
        {
            var data = string.Empty;

            if (formatter != null && log != null && formatter.DataConverter != null)
            {
                var obj = log.GetData == null ? log.Data : log.GetData();

                if (obj != null)
                {
                    try
                    {
                        data = formatter.DataConverter.Serialize(obj);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            data = new JsonLogDataConverter().Serialize(obj);
                        }
                        catch (Exception e2)
                        {
                            data = e.ToString("\n") + "\n\n" + e2.ToString("\n");
                        }
                    }

                    var _formatter = formatter as BaseLogFormatter;

                    if (_formatter != null && _formatter.EncodeData && !string.IsNullOrEmpty(data))
                    {
                        try
                        {
                            data = _formatter.Encode(data);
                        }
                        catch (Exception e)
                        {
                            data += ", encode error: " + e.ToString("\n");
                        }
                    }
                }

                if (string.IsNullOrEmpty(data))
                {
                    data = null;
                }
            }

            return data;
        }
    }
}
