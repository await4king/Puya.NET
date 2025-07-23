using System.Net;

namespace Puya.Serialization.UnitTests
{
    public enum Color { Red, Green, Blue }
    public class PuyaSerialization
    {
        [Fact]
        public void Test_ToQuerystring_object1()
        {
            var obj = new
            {
                name = "ali",
                age = 24
            };

            var qs = obj.ToQuerystring();

            Assert.Equal("?name=ali&age=24", qs);
        }
        [Fact]
        public void Test_ToQuerystring_object2()
        {
            var obj = new
            {
                name = "\"ali\"",
                family = "saeed pour&"
            };

            var qs1 = obj.ToQuerystring();
            var qs2 = $"?name={WebUtility.UrlEncode(obj.name)}&family={WebUtility.UrlEncode(obj.family)}";

            Assert.Equal(qs1, qs2);
        }
        [Fact]
        public void Test_ToQuerystring_object_with_enum()
        {
            var obj = new
            {
                color = Color.Red,
            };

            var qs1 = obj.ToQuerystring(new QuerySerializationOptions { EnumAsString = true });
            var qs2 = $"?color=Red";

            Assert.Equal(qs1, qs2);
        }
        [Fact]
        public void Test_ToQuerystring_empty_object()
        {
            var obj = new { };

            var qs = obj.ToQuerystring();

            Assert.Equal("", qs);
        }
        [Fact]
        public void Test_ToQuerystring_basictype1()
        {
            var obj = 23;

            var qs = obj.ToQuerystring();

            Assert.Equal("?23", qs);
        }
        [Fact]
        public void Test_ToQuerystring_basictype1_no_questionmark()
        {
            var obj = 23;

            var qs = obj.ToQuerystring(new QuerySerializationOptions { UseQuestionMark = false });

            Assert.Equal("23", qs);
        }
        [Fact]
        public void Test_ToQuerystring_list()
        {
            var obj = new List<string> { "red", "green", "blue" };

            var qs1 = obj.ToQuerystring();
            var qs2 = "";

            Assert.Equal(qs1, qs2);
        }
        [Fact]
        public void Test_ToQuerystring_object_with_list1()
        {
            var obj = new
            {
                colors = new List<string> { "red", "green", "blue" }
            };

            var qs1 = obj.ToQuerystring();
            var qs2 = "?colors=red,green,blue";

            Assert.Equal(qs1, qs2);
        }
        [Fact]
        public void Test_ToQuerystring_object_with_dictionary()
        {
            var obj = new
            {
                items = new Dictionary<string, object> { ["a"] = 10, ["b"] = true, ["c"] = "ali" }
            };

            var qs1 = obj.ToQuerystring();
            var qs2 = "?items.a=10&items.b=true&items.c=ali";

            Assert.Equal(qs1, qs2);
        }
        [Fact]
        public void Test_ToQuerystring_object_with_list2()
        {
            var obj = new
            {
                colors = new List<string> { "red", "green", "blue" }
            };

            var qs1 = obj.ToQuerystring(new QuerySerializationOptions { ExtendArrays = true });
            var qs2 = "?colors[0]=red&colors[1]=green&colors[2]=blue";

            Assert.Equal(qs1, qs2);
        }
        [Fact]
        public void Test_ToQuerystring_nested_object()
        {
            var obj = new
            {
                name = "ali",
                address = new
                {
                    city = new
                    {
                        code = "123",
                        name = "tehran"
                    }
                }
            };

            var qs1 = obj.ToQuerystring();
            var qs2 = "?name=ali&address.city.code=123&address.city.name=tehran";

            Assert.Equal(qs1, qs2);
        }
        [Fact]
        public void Test_ToQuerystring_nested_object_with_list1()
        {
            var obj = new
            {
                name = "ali",
                locations = new
                {
                    codes = new List<int> { 123, 456, 789 }
                }
            };

            var qs1 = obj.ToQuerystring();
            var qs2 = "?name=ali&locations.codes=123,456,789";

            Assert.Equal(qs1, qs2);
        }
        [Fact]
        public void Test_ToQuerystring_nested_object_with_list2()
        {
            var obj = new
            {
                name = "ali",
                locations = new
                {
                    codes = new List<int> { 123, 456, 789 }
                }
            };

            var qs1 = obj.ToQuerystring(new QuerySerializationOptions { ExtendArrays = true });
            var qs2 = "?name=ali&locations.codes[0]=123&locations.codes[1]=456&locations.codes[2]=789";

            Assert.Equal(qs1, qs2);
        }
        // ==================== Options ======================
        // ----------------- EnumAsString --------------------
        [Fact]
        public void Test_QuerystringOptions_EnumAsString1()
        {
            var options = Querystring.Options().EnumAsString(true);

            Assert.True(options.EnumAsString);
        }
        [Fact]
        public void Test_QuerystringOptions_EnumAsString2()
        {
            var options = Querystring.Options().EnumAsString();

            Assert.True(options.EnumAsString);
        }
        [Fact]
        public void Test_QuerystringOptions_EnumAsString3()
        {
            var options = Querystring.Options().EnumAsString(false);

            Assert.False(options.EnumAsString);
        }
        // ----------------- ExtendArrays --------------------
        [Fact]
        public void Test_QuerystringOptions_ExtendArrays1()
        {
            var options = Querystring.Options().ExtendArrays(true);

            Assert.True(options.ExtendArrays);
        }
        [Fact]
        public void Test_QuerystringOptions_ExtendArrays2()
        {
            var options = Querystring.Options().ExtendArrays();

            Assert.True(options.ExtendArrays);
        }
        [Fact]
        public void Test_QuerystringOptions_ExtendArrays3()
        {
            var options = Querystring.Options().ExtendArrays(false);

            Assert.False(options.ExtendArrays);
        }
        // ----------------- IgnoreNullOrEmpty --------------------
        [Fact]
        public void Test_QuerystringOptions_IgnoreNullOrEmpty1()
        {
            var options = Querystring.Options().IgnoreNullOrEmpty(true);

            Assert.True(options.IgnoreNullOrEmpty);
        }
        [Fact]
        public void Test_QuerystringOptions_IgnoreNullOrEmpty2()
        {
            var options = Querystring.Options().IgnoreNullOrEmpty();

            Assert.True(options.IgnoreNullOrEmpty);
        }
        [Fact]
        public void Test_QuerystringOptions_IgnoreNullOrEmpty3()
        {
            var options = Querystring.Options().IgnoreNullOrEmpty(false);

            Assert.False(options.IgnoreNullOrEmpty);
        }
        // ----------------- EncodePropNames --------------------
        [Fact]
        public void Test_QuerystringOptions_EncodePropNames1()
        {
            var options = Querystring.Options().EncodePropNames(true);

            Assert.True(options.EncodePropNames);
        }
        [Fact]
        public void Test_QuerystringOptions_EncodePropNames2()
        {
            var options = Querystring.Options().EncodePropNames();

            Assert.True(options.EncodePropNames);
        }
        [Fact]
        public void Test_QuerystringOptions_EncodePropNames3()
        {
            var options = Querystring.Options().EncodePropNames(false);

            Assert.False(options.EncodePropNames);
        }
        // ----------------- CaseSensitivePropNames --------------------
        [Fact]
        public void Test_QuerystringOptions_CaseSensitivePropNames1()
        {
            var options = Querystring.Options().CaseSensitivePropNames(true);

            Assert.True(options.CaseSensitivePropNames);
        }
        [Fact]
        public void Test_QuerystringOptions_CaseSensitivePropNames2()
        {
            var options = Querystring.Options().CaseSensitivePropNames();

            Assert.True(options.CaseSensitivePropNames);
        }
        [Fact]
        public void Test_QuerystringOptions_CaseSensitivePropNames3()
        {
            var options = Querystring.Options().CaseSensitivePropNames(false);

            Assert.False(options.CaseSensitivePropNames);
        }
        // ----------------- UseQuestionMark --------------------
        [Fact]
        public void Test_QuerystringOptions_UseQuestionMark1()
        {
            var options = Querystring.Options().UseQuestionMark(true);

            Assert.True(options.UseQuestionMark);
        }
        [Fact]
        public void Test_QuerystringOptions_UseQuestionMark2()
        {
            var options = Querystring.Options().UseQuestionMark();

            Assert.True(options.UseQuestionMark);
        }
        [Fact]
        public void Test_QuerystringOptions_UseQuestionMark3()
        {
            var options = Querystring.Options().UseQuestionMark(false);

            Assert.False(options.UseQuestionMark);
        }
        // ----------------- ArraySeparator --------------------
        [Fact]
        public void Test_QuerystringOptions_ArraySeparator()
        {
            var options = Querystring.Options().ArraySeparator(".");

            Assert.Equal(".", options.ArraySeparator);
        }
        // ----------------- DateTimeFormat --------------------
        [Fact]
        public void Test_QuerystringOptions_DateTimeFormat()
        {
            var options = Querystring.Options().DateTimeFormat("yy/MM/dd");

            Assert.Equal("yy/MM/dd", options.DateTimeFormat);
        }
    }
}