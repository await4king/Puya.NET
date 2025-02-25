using Newtonsoft.Json.Linq;

namespace Puya.Extensions
{
    public static class NewtonsoftExtensions
    {
        public static object Query(this JObject obj, string path)
        {
            if (obj == null || string.IsNullOrEmpty(path))
            {
                return null;
            }

            JToken cur = obj;

            var arr = path.Split(new char[] { '.' });

            foreach (var part in arr)
            {
                if (cur == null)
                {
                    break;
                }

                try
                {
                    cur = cur[part];
                }
                catch
                {
                    cur = null;
                    break;
                }
            }

            return cur;
        }
    }
}
