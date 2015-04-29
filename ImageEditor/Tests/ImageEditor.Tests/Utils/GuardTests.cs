namespace ImageEditor.Tests.Utils
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class GuardTests
    {
        [Test]
        public void GreaterThanZero_ArgumentValueIsEqualToZero_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            String argumentName = "someArgumentName";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => ImageEditor.Utils.Guard.GreaterThanZero(0.0, argumentName));
        }

        [Test]
        public void GreaterThanZero_ArgumentValueIsGreaterThanZero_DoesNotThrowAnyException()
        {
            // Arrange
            String argumentName = "someArgumentName";

            // Act & Assert
            ImageEditor.Utils.Guard.GreaterThanZero(1.0, argumentName);
        }

        [Test]
        public void GreaterThanZero_ArgumentValueIsLessThanZero_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            String argumentName = "someArgumentName";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => ImageEditor.Utils.Guard.GreaterThanZero(-1.0, argumentName));
        }

        [Test]
        public void IntNonNegative_ArgumentValueIsGreaterOrEqualToZero_DoesNotThrowAnyException()
        {
            // Arrange
            String argumentName = "someArgumentName";

            // Act & Assert
            ImageEditor.Utils.Guard.IntNonNegative(0, argumentName);
            ImageEditor.Utils.Guard.IntNonNegative(1, argumentName);
        }

        [Test]
        public void IntNonNegative_ArgumentValueIsNegative_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => ImageEditor.Utils.Guard.IntNonNegative(-1, "someArgumentName"));
        }

        [Test]
        public void NotNull_ArgumentValueIsNotNull_DoesNotThrowAnyException()
        {
            // Act & Assert
            ImageEditor.Utils.Guard.NotNull(new Object(), "someObject");
        }

        [Test]
        public void NotNull_ArgumentValueIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ImageEditor.Utils.Guard.NotNull(null, "someArgumentName"));
        }
    }
}