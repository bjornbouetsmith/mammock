using Castle.DynamicProxy;
using Mammock.Interfaces;

namespace Mammock.Impl.Invocation.Actions
{
    /// <summary>
    /// The invoke property.
    /// </summary>
    public class InvokeProperty : IInvocationActionn
    {
        /// <summary>
        /// The mock repository.
        /// </summary>
        private readonly MockRepository mockRepository;

        /// <summary>
        /// The proxy instance.
        /// </summary>
        private readonly IMockedObject proxyInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeProperty"/> class. 
        /// The invoke property.
        /// </summary>
        /// <param name="proxy_instance">
        /// The proxy_instance.
        /// </param>
        /// <param name="mockRepository">
        /// The mock Repository.
        /// </param>
        public InvokeProperty(IMockedObject proxy_instance, MockRepository mockRepository)
        {
            proxyInstance = proxy_instance;
            this.mockRepository = mockRepository;
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
            invocation.ReturnValue = proxyInstance.HandleProperty(invocation.GetConcreteMethod(), invocation.Arguments);
            mockRepository.RegisterPropertyBehaviorOn(proxyInstance);
            return;
        }

        #endregion
    }
}