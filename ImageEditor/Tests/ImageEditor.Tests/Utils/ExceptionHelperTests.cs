namespace ImageEditor.Tests.Utils
{
    using System;

    using ImageEditor.Utils;

    using NUnit.Framework;

    [TestFixture]
    public class ExceptionHelperTests
    {
        #region Tests

        [Test]
        public void GetFullExceptionMessage_ExceptionIsNull_ReturnsEmptyString()
        {
            // Act & Assert
            Assert.IsEmpty(ExceptionHelper.GetFullExceptionMessage(null));
        }

        [Test]
        public void GetFullExceptionMessage_ReturnsFullExceptionMessage()
        {
            // Arrange
            Exception exceptionOne = new Exception("exception_one");
            Exception exceptionTwo = new Exception("exception_two", exceptionOne);
            Exception exceptionThree = new Exception("exception_three", exceptionTwo);

            // Act
            string message = string.Format("{1}{0}{2}{0}{3}", Environment.NewLine, exceptionThree.Message,
            exceptionTwo.Message, exceptionOne.Message);

            // Assert
            Assert.AreEqual(message, ExceptionHelper.GetFullExceptionMessage(exceptionThree));
        }

        #endregion
    }
}