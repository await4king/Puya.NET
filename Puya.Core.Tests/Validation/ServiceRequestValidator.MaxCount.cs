using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_MaxCount_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntName : ServiceRequest
        {
            [MaxCount(3)]
            public int Name { get; set; }
        }
        public class ModelWithNullableIntName : ServiceRequest
        {
            [MaxCount(3)]
            public int? Name { get; set; }
        }
        public class ModelWithStringName : ServiceRequest
        {
            [MaxCount(3)]
            public string Name { get; set; }
        }
        public class ModelWithStringName1 : ServiceRequest
        {
            [MaxCount(3, "$")]
            public string Name { get; set; }
        }
        public class ModelWithObjectName : ServiceRequest
        {
            [MaxCount(3)]
            public object Name { get; set; }
        }
        [Fact]
        public void value_with_invalid_type1_should_fail()
        {
            ShouldFail(new ModelWithIntName(), "Name", "TypeMismatch", new { Expected = "String" });
        }
        [Fact]
        public void obj_null_or_empty_value_should_pass()
        {
            ShouldPass(new ModelWithObjectName());
            ShouldPass(new ModelWithObjectName { Name = "" });
        }
        [Fact]
        public void obj_value_with_invalid_value_type_should_fail()
        {
            ShouldFail(new ModelWithObjectName { Name = 10 }, "Name", "TypeMismatch", new { Expected = "String" });
        }
        [Fact]
        public void model_with_invalid_type_but_null_value_should_pass()
        {
            ShouldPass(new ModelWithNullableIntName());
        }
        [Fact]
        public void null_or_empty_value_should_pass()
        {
            ShouldPass(new ModelWithStringName());
            ShouldPass(new ModelWithStringName { Name = "" });
        }
        [Fact]
        public void null_or_empty_value_should_pass1()
        {
            ShouldPass(new ModelWithStringName1());
            ShouldPass(new ModelWithStringName1 { Name = "" });
        }
        [Fact]
        public void value_with_items_lower_than_max_should_pass()
        {
            ShouldPass(new ModelWithStringName { Name = "ali" });
            ShouldPass(new ModelWithStringName { Name = "ali,reza" });
            ShouldPass(new ModelWithStringName { Name = "ali,reza,saeed" });
        }
        [Fact]
        public void value_with_items_lower_than_max_should_pass1()
        {
            ShouldPass(new ModelWithStringName1 { Name = "ali" });
            ShouldPass(new ModelWithStringName1 { Name = "ali$reza" });
            ShouldPass(new ModelWithStringName1 { Name = "ali$reza$saeed" });
        }
        [Fact]
        public void obj_value_with_items_lower_than_max_should_pass()
        {
            ShouldPass(new ModelWithObjectName { Name = "ali" });
            ShouldPass(new ModelWithObjectName { Name = "ali,reza" });
            ShouldPass(new ModelWithObjectName { Name = "ali,reza,saeed" });
        }
        [Fact]
        public void value_with_items_greater_than_max_should_fail()
        {
            ShouldFail(new ModelWithStringName { Name = "ali,reza,saeed,hamid" }, "Name", "ItemCountMismatch", new { MinCount = 0, MaxCount = 3 });
        }
        [Fact]
        public void value_with_items_greater_than_max_should_fail1()
        {
            ShouldFail(new ModelWithStringName1 { Name = "ali$reza$saeed$hamid" }, "Name", "ItemCountMismatch", new { MinCount = 0, MaxCount = 3 });
        }
        [Fact]
        public void obj_value_with_items_greater_than_max_should_fail()
        {
            ShouldFail(new ModelWithObjectName { Name = "ali,reza,saeed,hamid" }, "Name", "ItemCountMismatch", new { MinCount = 0, MaxCount = 3 });
        }
    }
}
