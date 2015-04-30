namespace ImageEditor.Utils
{
    using System;
    using System.Linq.Expressions;

    public static class ExpressionHelper
    {
        // <summary>
        // Get the name of a static or instance property from a property access lambda.
        // </summary>
        // <typeparam name="T">Type of the property.</typeparam>
        // <param name="propertyLambda">Lambda expression of the form: '() => Class.Property' or '() => object.Property'.</param>
        // <returns>The name of the property.</returns>

        #region Public Methods

        public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        {
            Guard.NotNull(propertyLambda, "propertyLambda");

            MemberExpression me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException(
                "You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name;
        }

        #endregion
    }
}