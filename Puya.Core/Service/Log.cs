using System;

namespace Puya.Service
{
    public enum LogType
    {
        Info, Warning, Error, Debug, Message, Trace
    }
    public enum LogSource
    {
        System, Framework, App, Db, Service, Lib, Model, View
    }
    public class Log
    {
        public LogSource Source { get; set; }
        public LogType Type { get; set; }
        public DateTime Date { get; set; }
        public int Depth { get; set; }
        public object Data { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public string CallSite { get; set; }
        public string FilePath { get; set; }
        public int Line { get; set; }
        public Log()
        {
            Date = DateTime.Now;
        }
        public Log Clone()
        {
            var result = new Log
            {
                Source = Source,
                Type = Type,
                Date = Date,
                Depth = Depth,
                Data = Data,
                Category = Category,
                Message = Message,
                Exception = Exception,
                CallSite = CallSite,
                FilePath = FilePath,
                Line = Line,
            };

            if (Data != null)
            {
                var cloneable = Data as ICloneable;

                if (cloneable != null)
                {
                    result.Data = cloneable.Clone();
                }
                else
                {
                    result.Data = Data;
                }
            }

            return result;
        }
    }
}
