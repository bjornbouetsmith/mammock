using Castle.DynamicProxy;
using Mammock.Impl.InvocationSpecifications;
using Mammock.Interfaces;

namespace Mammock.Impl.Invocation.Specifications
{
    /// <summary>
    /// The is an invocation on a mocked object.
    /// </summary>
    public class IsAnInvocationOnAMockedObject : ISpecification<IInvocation>
    {
        #region ISpecification<IInvocation> Members

        /// <summary>
        /// The is satisfied by.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        /// <returns>
        /// The is satisfied by.
        /// </returns>
        public bool IsSatisfiedBy(IInvocation invocation)
        {
            return invocation.Method.DeclaringType == typeof (IMockedObject);
        }

        #endregion
    }
}