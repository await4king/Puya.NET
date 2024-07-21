using System;
using System.Collections.Generic;
using System.Text;
using Puya.Collections;

namespace Puya.Logging
{
    public class CsvSerializer
    {
        public char RowSeparator { get; set; }
        public char ColSeparator { get; set; }
        public virtual string Deserialize(string x)
        {
            if (!string.IsNullOrEmpty(x))
            {
                var buff = new CharBuffer(32);
                char? last = null;
                char ch;
                int i = 0;
                var state = CsvEncodeStates.Start;

                while (i < x.Length)
                {
                    if (last.HasValue)
                    {
                        ch = last.Value;
                    }
                    else
                    {
                        ch = x[i++];
                    }

                    switch (state)
                    {
                        case CsvEncodeStates.Start:
                            if (ch == '\\')
                            {
                                state = CsvEncodeStates.Slash;
                            }
                            else
                            {
                                buff.Append(ch);
                            }
                            break;
                        case CsvEncodeStates.Slash:
                            switch (ch)
                            {
                                case 'n':
                                    buff.Append('\n');
                                    state = CsvEncodeStates.Start;
                                    break;
                                case 'r':
                                    buff.Append('\r');
                                    state = CsvEncodeStates.Start;
                                    break;
                                case '\\':
                                    buff.Append('\\');
                                    state = CsvEncodeStates.Start;
                                    break;
                                case ';':
                                    buff.Append(';');
                                    state = CsvEncodeStates.Start;
                                    break;
                                default:
                                    buff.Append(ch);
                                    state = CsvEncodeStates.Start;
                                    break;
                            }

                            break;
                    }
                }

                var result = buff.ToString();

                return result;
            }
            else
            {
                return "";
            }
        }
        public virtual string Serialize(string x)
        {
            if (!string.IsNullOrEmpty(x))
            {
                var buff = new CharBuffer(32);

                foreach (var ch in x)
                {
                    switch (ch)
                    {
                        case '\\':
                            buff.Append("\\\\");
                            break;
                        case '\n':
                            buff.Append("\\n");
                            break;
                        case '\r':
                            buff.Append("\\r");
                            break;
                        default:
                            if (ch == ColSeparator)
                            {
                                buff.Append("\\" + ColSeparator);
                            }
                            else
                            {
                                buff.Append(ch);
                            }
                            break;
                    }
                }

                return buff.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
