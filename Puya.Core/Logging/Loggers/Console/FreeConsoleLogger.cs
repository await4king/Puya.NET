using System;

namespace Puya.Logging
{
    public enum ConsoleParseTemplate
    {
        Start = 1,
        Slash = 2,
        NewLine = 3,
        Color = 4,
        Interpolation = 5
    }
    public class FreeConsoleLoggerConfig: BaseLoggerConfig
    {
        public string Template { get; set; }
        public FreeConsoleLoggerConfig()
        {
            if (string.IsNullOrEmpty(Template))
            {
                Template = @"
\cDarkGreen{logdate}
\cBlue{logtype}

\?\kApp: \p{appid}
\?\cDarkGrayThreadId: \p{threadid}
\?\kCategory: \cYellow{category}
\kMemberName: \p{membername}
\kFile: \cDarkGray{file}, line: \cDarkGray{line}
\?\kMessage: \cDarkYellow{message}
\?\cDarkRedStackTrace: \cRed{stacktrace}
\?\kIp: \p{ip}
\?\kUser: \p{user}
\?\kData: \cDarkCyan{data}
\?\kMethod: \cMagenta{method}
\?\kUrl: \cCyan{url}
\?\kBrowserName: \p{browsername}
\?\kBrowserVersion: \p{browserversion}
\?\cDarkGrayReferrer: \cDarkGray{referrer}
\?\cDarkGrayHeaders: \cDarkGray{headers}
\?\cDarkGrayForm: \cDarkGray{form}
\?\cDarkGrayCookies: \cDarkGray{cookies}
\?\kBody: \p{body}
\?\cGrayContentType: \cDarkGray{contenttype}
\?\kOperationResult: \p{operationresult}
\cDarkGray" + new string('=', 100);
            }
        }
    }
    public class FreeConsoleLogger : BaseLogger<FreeConsoleLoggerConfig>
    {
        public FreeConsoleLogger() : this(null)
        { }
        public FreeConsoleLogger(FreeConsoleLoggerConfig config) : this(config, null)
        { }
        public FreeConsoleLogger(FreeConsoleLoggerConfig config, ILogger next) : base(config, next)
        { }
        void Write(string template, Log log)
        {
            var lines = template.Split('\n');
            var headerColor = ConsoleColor.White;
            var messageColor = ConsoleColor.Gray;
            var defaultColor = Console.ForegroundColor;

            foreach (var ln in lines)
            {
                var line = ln;

                if (ln.StartsWith("\\?"))
                {
                    if (log.Fill(ln) == log.Clear(ln))
                    {
                        continue;
                    }
                    else
                    {
                        line = ln.Substring(2);
                    }
                }

                var state = ConsoleParseTemplate.Start;
                var i = 0;
                var temp = "";
                char? lastCh = null;

                while (i < line.Length)
                {
                    char ch;

                    if (lastCh.HasValue)
                    {
                        ch = lastCh.Value;
                        lastCh = null;
                    }
                    else
                    {
                        ch = line[i];
                    }

                    switch (state)
                    {
                        case ConsoleParseTemplate.Start:
                            if (ch == '\\')
                            {
                                state = ConsoleParseTemplate.Slash;
                            }
                            else if (ch == '{')
                            {
                                state = ConsoleParseTemplate.Interpolation;
                            }
                            else
                            {
                                Console.Write(ch);
                            }
                            break;
                        case ConsoleParseTemplate.Slash:
                            if (ch == 'k' || ch == 'p' || ch == 'r' || ch == 'c')
                            {
                                switch (ch)
                                {
                                    case 'k':
                                        Console.ForegroundColor = headerColor;
                                        state = ConsoleParseTemplate.Start;
                                        break;
                                    case 'p':
                                        Console.ForegroundColor = messageColor;
                                        state = ConsoleParseTemplate.Start;
                                        break;
                                    case 'r':
                                        Console.ForegroundColor = defaultColor;
                                        state = ConsoleParseTemplate.Start;
                                        break;
                                    case 'c':
                                        state = ConsoleParseTemplate.Color;
                                        break;
                                }
                            }
                            else if (ch == '{' || ch == '}' || ch == '\\')
                            {
                                Console.Write(ch);
                                state = ConsoleParseTemplate.Start;
                            }
                            else
                            {
                                Console.Write('\\' + ch.ToString());
                                state = ConsoleParseTemplate.Start;
                            }
                            break;
                        case ConsoleParseTemplate.Color:
                            if (char.IsLetter(ch))
                            {
                                temp += ch;

                                if (Enum.TryParse(temp, true, out ConsoleColor c))
                                {
                                    Console.ForegroundColor = c;
                                    state = ConsoleParseTemplate.Start;
                                    temp = "";
                                }
                            }
                            else
                            {
                                if (Enum.TryParse(temp, true, out ConsoleColor c))
                                {
                                    Console.ForegroundColor = c;
                                }
                                else
                                {
                                    Console.Write("\\c" + temp);
                                }

                                temp = "";

                                lastCh = ch;

                                state = ConsoleParseTemplate.Start;
                            }
                            break;
                        case ConsoleParseTemplate.Interpolation:
                            if (ch == '}')
                            {
                                switch (temp.ToLower())
                                {
                                    case "id": temp = log.Id.ToString(); break;
                                    case "appid": temp = log.AppId?.ToString(); break;
                                    case "threadid": temp = log.ThreadId?.ToString(); break;
                                    case "type": temp = log.Type.ToString(); break;
                                    case "result": temp = log.Result.ToString(); break;
                                    case "category": temp = log.Category?.ToString(); break;
                                    case "file": temp = log.File?.ToString(); break;
                                    case "line": temp = log.Line?.ToString(); break;
                                    case "membername": temp = log.MemberName?.ToString(); break;
                                    case "message": temp = log.Message?.ToString(); break;
                                    case "stacktrace": temp = log.StackTrace?.ToString(); break;
                                    case "ip": temp = log.Ip?.ToString(); break;
                                    case "user": temp = log.User?.ToString(); break;
                                    case "logdate": temp = log.FormatDate(); break;
                                    case "data": temp = log.Data?.ToString(); break;
                                    case "method": temp = log.Method?.ToString(); break;
                                    case "url": temp = log.Url?.ToString(); break;
                                    case "browsername": temp = log.BrowserName?.ToString(); break;
                                    case "browserversion": temp = log.BrowserVersion?.ToString(); break;
                                    case "referrer": temp = log.Referrer?.ToString(); break;
                                    case "headers": temp = log.Headers?.ToString(); break;
                                    case "form": temp = log.Form?.ToString(); break;
                                    case "cookies": temp = log.Cookies?.ToString(); break;
                                    case "body": temp = log.Body?.ToString(); break;
                                    case "contenttype": temp = log.ContentType?.ToString(); break;
                                    case "operationresult": temp = log.OperationResult.ToString(); break;
                                    case "logtype": temp = log.LogType.ToString(); break;
                                    default:
                                        temp = "{" + temp + "}";
                                        break;
                                }

                                if (!string.IsNullOrEmpty(temp))
                                {
                                    Console.Write(temp);
                                }

                                temp = "";

                                state = ConsoleParseTemplate.Start;
                            }
                            else
                            {
                                temp += ch;
                            }

                            break;
                    }

                    if (!lastCh.HasValue)
                    {
                        i++;
                    }
                }

                Console.WriteLine();

                Console.ForegroundColor = defaultColor;
            }

            Console.ForegroundColor = defaultColor;
        }
        protected override void LogInternal(Log log)
        {
            Write(Config.Template, log);
        }
        protected override void ClearInternal()
        {
            Console.Clear();
        }
    }
}
