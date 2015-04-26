namespace ImageEditor.Utils
{
    using System;
    using System.Text;

    public static class ExceptionHelper
    {
        #region Public Methods

        /// <summary>
        ///     Gets the full message from exception and all it's inner exceptions.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static string GetFullExceptionMessage(Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);

                exception = exception.InnerException;
            }

            string message = stringBuilder.ToString()
            .Trim();

            return message;
        }

        #endregion
    }
}