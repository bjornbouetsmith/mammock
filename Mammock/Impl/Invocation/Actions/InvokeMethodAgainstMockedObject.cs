using Castle.DynamicProxy;
using Mammock.Interfaces;

namespace Mammock.Impl.Invocation.Actions
{
    /// <summary>
    /// The invoke method against mocked object.
    /// </summary>
    public class InvokeMethodAgainstMockedObject : IInvocationActionn
    {
        /// <summary>
        /// The proxy instance.
        /// </summary>
        private readonly IMockedObject proxyInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeMethodAgainstMockedObject"/> class. 
        /// The invoke method against mocked object.
        /// </summary>
        /// <param name="proxy_instance">
        /// The proxy_instance.
        /// </param>
        public InvokeMethodAgainstMockedObject(IMockedObject proxy_instance)
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
            invocation.ReturnValue = invocation.Method.Invoke(proxyInstance, invocation.Arguments);
        }

        #endregion
    }
}