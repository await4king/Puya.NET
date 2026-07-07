using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_MinCount_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntName : ServiceRequest
        {
            [MinCount(1)]
            public int Name { get; set; }
        }
        public class ModelWithNullableIntName : ServiceRequest
        {
            [MinCount(1)]
            public int? Name { get; set; }
        }
        public class ModelWithStringName : ServiceRequest
        {
            [MinCount(1)]
            public string Name { get; set; }
        }
        public class ModelWithStringName1 : ServiceRequest
        {
            [MinCount(1, "$")]
            public string Name { get; set; }
        }
        public class ModelWithStringName2 : ServiceRequest
        {
            [MinCount(2)]
            public string Name { get; set; }
        }
        public class ModelWithObjectName : ServiceRequest
        {
            [MinCount(1)]
            public object Name { get; set; }
        }
        public class ModelWithObjectName2 : ServiceRequest
        {
            [MinCount(2)]
            public object Name { get; set; }
        }
        [Fact]
        public void value_with_invalid_type1_should_fail()
        {
            ShouldFail(new ModelWithIntName(), "Name", "TypeMismatch", new { Expected = "String" });
        }
        [Fact]
        public void obj_null_or_empty_value_should_fail()
        {
            ShouldFail(new ModelWithObjectName(), "Name", "NoItems");
            ShouldFail(new ModelWithObjectName { Name = "" }, "Name", "NoItems");
        }
        [Fact]
        public void obj_value_with_invalid_value_type_should_fail()
        {
            ShouldFail(new ModelWithObjectName { Name = 10 }, "Name", "TypeMismatch", new { Expected = "String" });
        }
        [Fact]
        public void model_with_invalid_type_but_null_value_should_fail()
        {
            ShouldFail(new ModelWithNullableIntName(), "Name", "NoItems");
        }
        [Fact]
        public void null_or_empty_value_should_fail()
        {
            ShouldFail(new ModelWithStringName(), "Name", "NoItems");
            ShouldFail(new ModelWithStringName { Name = "" }, "Name", "NoItems");
        }
        [Fact]
        public void null_or_empty_value_should_fail1()
        {
            ShouldFail(new ModelWithStringName1(), "Name", "NoItems");
            ShouldFail(new ModelWithStringName1 { Name = "" }, "Name", "NoItems");
        }
        [Fact]
        public void value_with_items_lt_min_should_fail()
        {
            ShouldFail(new ModelWithStringName2 { Name = "ali" }, "Name", "ItemCountMismatch");
        }
        [Fact]
        public void obj_value_with_items_lt_min_should_fail()
        {
            ShouldFail(new ModelWithObjectName2 { Name = "ali" }, "Name", "ItemCountMismatch");
        }
        [Fact]
        public void value_with_items_gte_min_should_pass()
        {
            ShouldPass(new ModelWithStringName { Name = "ali" });
            ShouldPass(new ModelWithStringName { Name = "ali,reza" });
            ShouldPass(new ModelWithStringName { Name = "ali,reza,saeed" });
        }
        [Fact]
        public void value_with_items_gte_min_should_pass1()
        {
            ShouldPass(new ModelWithStringName1 { Name = "ali" });
            ShouldPass(new ModelWithStringName1 { Name = "ali$reza" });
            ShouldPass(new ModelWithStringName1 { Name = "ali$reza$saeed" });
        }
        [Fact]
        public void obj_value_with_items_gte_min_should_pass()
        {
            ShouldPass(new ModelWithObjectName { Name = "ali" });
            ShouldPass(new ModelWithObjectName { Name = "ali,reza" });
            ShouldPass(new ModelWithObjectName { Name = "ali,reza,saeed" });
        }
    }
}
