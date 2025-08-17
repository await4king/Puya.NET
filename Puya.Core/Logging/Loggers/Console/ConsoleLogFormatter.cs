using System;

namespace Puya.Logging
{
    public class ConsoleLogFormatter : StringLogFormatter
    {
        void Write(ConsoleColor headerColor, string header, ConsoleColor messageColor, string message)
        {
            if (!string.IsNullOrEmpty(header))
            {
                Console.ForegroundColor = headerColor;

                Console.Write(header);
            }

            if (!string.IsNullOrEmpty(message))
            {
                Console.ForegroundColor = messageColor;

                Console.Write(message);

                Console.WriteLine();
            }
        }
        protected override void OnFormatPart(Log log, string part, string value, string format, string formattedValue)
        {
            var forColor = Console.ForegroundColor;

            if (!string.IsNullOrEmpty(part) && !part.StartsWith("mixed", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(format))
            {
                var headerColor = ConsoleColor.White;
                var messageColor = ConsoleColor.Gray;
                var header = "";
                var message = "";
                var key = "{" + part.ToLower() + "}";
                var index = format.IndexOf(key);

                if (index >= 0)
                {
                    header = format.Substring(0, index);
                    message = value + format.Substring(index + key.Length);
                }
                else
                {
                    message = formattedValue;
                }

                if (Equals(part, "logtype") || Equals(part, "message"))
                {
                    switch (log.LogType)
                    {
                        case LogType.Error:
                            messageColor = ConsoleColor.Red;
                            break;
                        case LogType.Warning:
                            messageColor = ConsoleColor.Magenta;
                            break;
                        case LogType.Info:
                            messageColor = ConsoleColor.Blue;
                            break;
                        case LogType.Suggestion:
                            messageColor = ConsoleColor.DarkBlue;
                            break;
                        case LogType.Debug:
                            messageColor = ConsoleColor.Yellow;
                            break;
                        case LogType.Trace:
                            messageColor = ConsoleColor.Cyan;
                            break;
                        case LogType.Alert:
                            messageColor = ConsoleColor.DarkYellow;
                            break;
                    }
                }
                else if (Equals(part, "id") || Equals(part, "threadid"))
                {
                    headerColor = ConsoleColor.DarkGray;
                }
                else if (Equals(part, "user") || Equals(part, "url"))
                {
                    messageColor = ConsoleColor.Cyan;
                }
                else if (Equals(part, "logdate"))
                {
                    messageColor = ConsoleColor.DarkGreen;
                }
                else if (Equals(part, "method"))
                {
                    messageColor = ConsoleColor.Magenta;
                }
                else if (Equals(part, "category"))
                {
                    messageColor = ConsoleColor.DarkCyan;
                }
                else if (Equals(part, "referrer") || Equals(part, "form") || Equals(part, "cookies") || Equals(part, "headers"))
                {
                    headerColor = ConsoleColor.DarkGray;
                    messageColor = ConsoleColor.DarkGray;
                }
                else if (Equals(part, "contenttype"))
                {
                    headerColor = ConsoleColor.Gray;
                    messageColor = ConsoleColor.DarkGray;
                }
                else if (Equals(part, "stacktrace"))
                {
                    headerColor = ConsoleColor.DarkRed;
                    messageColor = ConsoleColor.Red;
                }

                Write(headerColor, header, messageColor, message);
            }
            else
            {
                if (!string.IsNullOrEmpty(part) && part.StartsWith("mixed", StringComparison.OrdinalIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                
                Console.Write(formattedValue);

                if (!string.IsNullOrEmpty(part) && !part.StartsWith("mixed", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine();
                }
            }

            Console.ForegroundColor = forColor;
        }
        protected override void OnEndFormat(Log log)
        {
        }
    }
}
