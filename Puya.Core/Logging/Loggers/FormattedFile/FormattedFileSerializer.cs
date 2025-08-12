using Puya.Collections;
using System.Collections.Generic;

namespace Puya.Logging
{
    internal enum FormattedFileStates
    {
        Start,
        Slash
    }
    public class FormattedFileSerializer
    {
        public char ColSeparator { get; set; }
        public string RowSeparator { get; set; }
        public FormattedFileSerializer(char colSeparator, string rowSeparator)
        {
            ColSeparator = colSeparator;
            RowSeparator = rowSeparator;
        }
        bool RowSeparatorAtPosition(string stream, int i)
        {
            return i + RowSeparator.Length < stream.Length && string.Equals(stream.Substring(i, RowSeparator.Length), RowSeparator, System.StringComparison.Ordinal);
        }
        public virtual string Deserialize(string x)
        {
            if (!string.IsNullOrEmpty(x))
            {
                var buff = new CharBuffer(64);
                char ch;
                int i = 0;
                var state = FormattedFileStates.Start;

                while (i < x.Length)
                {
                    ch = x[i];

                    switch (state)
                    {
                        case FormattedFileStates.Start:
                            if (ch == '\\')
                            {
                                state = FormattedFileStates.Slash;
                            }
                            else
                            {
                                buff.Append(ch);
                            }

                            i++;

                            break;
                        case FormattedFileStates.Slash:
                            if (ch == '\\')
                            {
                                buff.Append('\\');
                                i++;
                            }
                            else if (ch == ColSeparator)
                            {
                                buff.Append(ColSeparator);
                                i++;
                            }
                            else if (RowSeparatorAtPosition(x, i))
                            {
                                buff.Append(RowSeparator);
                                i += RowSeparator.Length;
                            }
                            else
                            {
                                buff.Append("\\" + ch);
                                i++;
                            }

                            state = FormattedFileStates.Start;

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
        public List<List<string>> DeserializeAll(string content)
        {
            var result = new List<List<string>>();

            if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(RowSeparator))
            {
                var temp = new CharBuffer(64);
                var state = FormattedFileStates.Start;
                var row = new List<string>();
                var i = 0;

                while (i < content.Length)
                {
                    var ch = content[i];

                    switch (state)
                    {
                        case FormattedFileStates.Start:
                            if (ch == ColSeparator)
                            {
                                row.Add(temp.ToString());

                                temp.Reset();

                                i++;
                            }
                            else if (ch == '\\')
                            {
                                state = FormattedFileStates.Slash;
                                i++;
                            }
                            else if (RowSeparatorAtPosition(content, i))
                            {
                                row.Add(temp.ToString());

                                temp.Reset();

                                result.Add(row);

                                row = new List<string>();

                                i += RowSeparator.Length;
                            }
                            else
                            {
                                temp.Append(ch);
                                i++;
                            }

                            break;
                        case FormattedFileStates.Slash:
                            if (ch == ColSeparator)
                            {
                                temp.Append(ColSeparator);
                                i++;
                            }
                            else
                            if (ch == '\\')
                            {
                                temp.Append('\\');
                                i++;
                            }
                            else
                            if (RowSeparatorAtPosition(content, i))
                            {
                                temp.Append(RowSeparator);
                                i += RowSeparator.Length;
                            }
                            else
                            {
                                temp.Append("\\" + ch);
                                i++;
                            }

                            state = FormattedFileStates.Start;

                            break;
                    }
                }

                result.Add(row);
            }

            return result;
        }
        public virtual string Serialize(string x)
        {
            if (!string.IsNullOrEmpty(x))
            {
                var buff = new CharBuffer(64);
                var i = 0;
                
                while (i < x.Length)
                {
                    var ch = x[i];

                    if (ch == '\\')
                    {
                        buff.Append("\\\\");
                        i++;
                    }
                    else if (ch == ColSeparator)
                    {
                        buff.Append("\\" + ColSeparator);
                        i++;
                    }
                    else if (i + RowSeparator.Length < x.Length && string.Equals(x.Substring(i, RowSeparator.Length), RowSeparator, System.StringComparison.Ordinal))
                    {
                        buff.Append("\\" + RowSeparator);
                        i += RowSeparator.Length;
                    }
                    else
                    {
                        buff.Append(ch);
                        i++;
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
