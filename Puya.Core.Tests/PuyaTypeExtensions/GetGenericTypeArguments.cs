using Puya.Extensions;


namespace Puya.Core.Tests.PuyaTypeExtensions
{
    public class GetGenericTypeArguments
    {
        [Fact]
        public void Test_GetGenericTypeArguments_NonGenericType()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var result = type.GetGenericTypeArguments();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Test_GetGenericTypeArguments_GenericType()
        {
            // Arrange
            var type = typeof(Dictionary<int, string>);

            // Act
            var result = type.GetGenericTypeArguments();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(typeof(int), result[0]);
            Assert.Equal(typeof(string), result[1]);
        }

        [Fact]
        public void Test_GetGenericTypeArguments_GenericTypeWithParentOrder()
        {
            // Arrange
            var type = typeof(Dictionary<int, string>);

            // Act
            var result = type.GetGenericTypeArguments(0);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(typeof(int), result[0]);
            Assert.Equal(typeof(string), result[1]);
        }

        [Fact]
        public void Test_GetGenericTypeArguments_NestedGenericType()
        {
            // Arrange
            var type = typeof(Dictionary<int, List<string>>);

            // Act
            var result = type.GetGenericTypeArguments();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(typeof(int), result[0]);
            Assert.Equal(typeof(List<string>), result[1]);
        }

        [Fact]
        public void Test_GetGenericTypeArguments_GenericTypeWithMultipleParents()


        {
            // Arrange
            var type = typeof(DerivedGenericClass<int, string>);

            // Act
            var result = type.GetGenericTypeArguments(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(typeof(string), result[0]);
        }

        // Classes for testing purposes
        public class BaseGenericClass<T> { }

        public class DerivedGenericClass<T1, T2> : BaseGenericClass<T2> { }
    }
}