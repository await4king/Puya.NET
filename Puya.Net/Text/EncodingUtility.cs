namespace Puya.Text
{
    public class EncodingUtility : IEncodingUtility
    {
        public System.Text.Encoding GetEncoding(string encoding)
        {
            System.Text.Encoding result;

            encoding = encoding?.ToLower();

            if (string.IsNullOrEmpty(encoding))
            {
                result = System.Text.Encoding.Default;
            }
            else
            {
                switch (encoding)
                {
                    case "ascii":
                        result = System.Text.Encoding.ASCII;
                        break;
                    case "utf7":
                        result = System.Text.Encoding.UTF7;
                        break;
                    case "utf-8":
                    case "utf8":
                        result = System.Text.Encoding.UTF8;
                        break;
                    case "utf-32":
                    case "utf32":
                        result = System.Text.Encoding.UTF32;
                        break;
                    case "bigendianunicode":
                        result = System.Text.Encoding.BigEndianUnicode;
                        break;
                    case "unicode":
                        result = System.Text.Encoding.Unicode;
                        break;
                    default:
                        try
                        {
                            result = System.Text.Encoding.GetEncoding(encoding);
                        }
                        catch
                        {
                            result = System.Text.Encoding.Default;
                        }

                        break;
                }
            }

            return result;
        }
    }
}
