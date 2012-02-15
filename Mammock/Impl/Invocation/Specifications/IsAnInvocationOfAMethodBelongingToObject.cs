using System;
using System.Reflection;
using Castle.DynamicProxy;
using Mammock.Impl.InvocationSpecifications;

namespace Mammock.Impl.Invocation.Specifications
{
    /// <summary>
    /// The is an invocation of a method belonging to object.
    /// </summary>
    public class IsAnInvocationOfAMethodBelongingToObject : ISpecification<IInvocation>
    {
        /// <summary>
        /// The object methods.
        /// </summary>
        private static readonly MethodInfo[] objectMethods =
            new[]
                {
                    typeof (object).GetMethod("ToString"), typeof (object).GetMethod("Equals", new[] {typeof (object)}), 
                    typeof (object).GetMethod("GetHashCode"), typeof (object).GetMethod("GetType")
                };

        #region ISpecification<IInvocation> Members

        /// <summary>
        /// The is satisfied by.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The is satisfied by.
        /// </returns>
        public bool IsSatisfiedBy(IInvocation item)
        {
            return Array.IndexOf(objectMethods, item.Method) != -1;
        }

        #endregion
    }
}