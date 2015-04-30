namespace ImageEditor.Tests.Utils
{
    using System;

    using ImageEditor.Utils;

    using NUnit.Framework;

    [TestFixture]
    public class ExpressionHelperTests
    {
        [Test]
        public void GetPropertyName_PropertyLambdaIsInvalid_Throws()
        {
            // Arrange
            SomeClass someClass = new SomeClass();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ExpressionHelper.GetPropertyName(() => someClass.SomeMethod()));
        }

        [Test]
        public void GetPropertyName_PropertyLambdaIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ExpressionHelper.GetPropertyName<object>(null));
        }

        [Test]
        public void GetPropertyName_PropertyLambdaIsValid_ReturnsValidPropertyName()
        {
            // Arrange
            SomeClass someClass = new SomeClass();

            // Act & Assert
            Assert.AreEqual("SomeProperty", ExpressionHelper.GetPropertyName(() => someClass.SomeProperty));
        }

        #region Nested type: SomeClass

        private class SomeClass
        {
            public object SomeProperty
            {
                get;
                set;
            }

            public object SomeMethod()
            {
                return null;
            }
        }

        #endregion
    }
}