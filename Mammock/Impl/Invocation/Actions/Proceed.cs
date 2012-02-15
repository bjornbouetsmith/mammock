using Castle.DynamicProxy;
using Mammock.Interfaces;

namespace Mammock.Impl.Invocation.Actions
{
    /// <summary>
    /// The proceed.
    /// </summary>
    public class Proceed : IInvocationActionn
    {
        #region IInvocationActionn Members

        /// <summary>
        /// The perform against.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        public void PerformAgainst(IInvocation invocation)
        {
            invocation.Proceed();
        }

        #endregion
    }
}