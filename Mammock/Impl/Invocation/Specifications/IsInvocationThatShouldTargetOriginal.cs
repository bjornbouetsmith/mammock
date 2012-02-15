using Castle.DynamicProxy;
using Mammock.Impl.InvocationSpecifications;
using Mammock.Interfaces;

namespace Mammock.Impl.Invocation.Specifications
{
    /// <summary>
    /// The is invocation that should target original.
    /// </summary>
    public class IsInvocationThatShouldTargetOriginal : ISpecification<IInvocation>
    {
        /// <summary>
        /// The proxy instance.
        /// </summary>
        private readonly IMockedObject proxyInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsInvocationThatShouldTargetOriginal"/> class. 
        /// The is invocation that should target original.
        /// </summary>
        /// <param name="proxyInstance">
        /// The proxy Instance.
        /// </param>
        public IsInvocationThatShouldTargetOriginal(IMockedObject proxyInstance)
        {
            this.proxyInstance = proxyInstance;
        }

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
            return proxyInstance.ShouldCallOriginal(item.GetConcreteMethod());
        }

        #endregion
    }
}