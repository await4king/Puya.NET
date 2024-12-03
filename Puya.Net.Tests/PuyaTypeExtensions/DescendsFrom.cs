using Puya.Extensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Puya.Net.Tests.PuyaTypeExtensions
{
    public class DescendsFrom
    {
        [Fact]
        public void DescendsFrom_NonGenericSubclass_ReturnsTrue()
        {
            // Arrange
            var type = typeof(DerivedClass);
            var targetType = typeof(BaseClass);

            // Act
            var result = type.DescendsFrom(targetType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DescendsFrom_GenericSubclass_ReturnsTrue()
        {
            // Arrange
            var type = typeof(GenericDerivedClass<int>);
            var targetType = typeof(GenericBaseClass<>);

            // Act
            var result = type.DescendsFrom(targetType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DescendsFrom_NonGenericClass_ReturnsFalse()
        {
            // Arrange
            var type = typeof(BaseClass);
            var targetType = typeof(DerivedClass);

            // Act
            var result = type.DescendsFrom(targetType);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DescendsFrom_GenericClass_ReturnsFalse()
        {
            // Arrange
            var type = typeof(GenericBaseClass<int>);
            var targetType = typeof(GenericDerivedClass<>);

            // Act
            var result = type.DescendsFrom(targetType);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DescendsFrom_NullTargetType_ThrowsArgumentNullException()
        {
            // Arrange
            var type = typeof(BaseClass);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => type.DescendsFrom(null));
        }

        // Classes for testing purposes
        public class BaseClass { }

        public class DerivedClass : BaseClass { }

        public class GenericBaseClass<T> { }

        public class GenericDerivedClass<T> : GenericBaseClass<T> { }
    }
}