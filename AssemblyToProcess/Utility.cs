using System;
using System.Linq.Expressions;

namespace AssemblyToProcess {
    public struct Abc { };
    public class Def { };
    public static class RuntimeName {
        /// <summary>
        /// Get the name of a variable
        /// </summary>
        /// <remarks>Use this method to get strongly-typed names of variables instead of "magic strings" that represent the name of some variable.</remarks>
        /// <param name="memberExpression">An expression with one member representing the variable to be getting the name of.</param>
        /// <example>String nameOfSomeVariable = Name.Of(() => someVariable);</example>
        /// <example>OnPropertyChanged(Name.Of(() => PropertyThatChanged)); // Instead of OnPropertyChanged("PropertyThatChanged");</example>
        public static String Of<T>(Expression<Func<T>> memberExpression) {
            MemberExpression body = null;
            if (memberExpression.Body is UnaryExpression)
                body = ((UnaryExpression)memberExpression.Body).Operand as MemberExpression;
            else if (memberExpression.Body is MemberExpression)
                body = memberExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException(String.Format("'{0}' should be a member expression", Name.Of(() => memberExpression)));
            return body.Member.Name;
        }
    }
}