using Castle.DynamicProxy;

namespace Mammock.Interfaces
{
    /// <summary>
    /// The i invocation actionn.
    /// </summary>
    public interface IInvocationActionn
    {
        /// <summary>
        /// The perform against.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        void PerformAgainst(IInvocation invocation);
    }
}