using Castle.DynamicProxy;
using Mammock.Interfaces;

namespace Mammock.Impl.Invocation.Actions
{
    /// <summary>
    /// The handle event.
    /// </summary>
    public class HandleEvent : IInvocationActionn
    {
        /// <summary>
        /// The proxy instance.
        /// </summary>
        private readonly IMockedObject proxyInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleEvent"/> class. 
        /// The handle event.
        /// </summary>
        /// <param name="proxy_instance">
        /// The proxy_instance.
        /// </param>
        public HandleEvent(IMockedObject proxy_instance)
        {
            proxyInstance = proxy_instance;
        }

        #region IInvocationActionn Members

        /// <summary>
        /// The perform against.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        public void PerformAgainst(IInvocation invocation)
        {
            proxyInstance.HandleEvent(invocation.Method, invocation.Arguments);
        }

        #endregion
    }
}