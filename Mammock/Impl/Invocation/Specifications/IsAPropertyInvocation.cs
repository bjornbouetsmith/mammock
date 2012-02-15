using Castle.DynamicProxy;
using Mammock.Impl.InvocationSpecifications;
using Mammock.Interfaces;

namespace Mammock.Impl.Invocation.Specifications
{
    /// <summary>
    /// The is a property invocation.
    /// </summary>
    public class IsAPropertyInvocation : ISpecification<IInvocation>
    {
        /// <summary>
        /// The proxy instance.
        /// </summary>
        private readonly IMockedObject proxyInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsAPropertyInvocation"/> class. 
        /// The is a property invocation.
        /// </summary>
        /// <param name="proxy_instance">
        /// The proxy_instance.
        /// </param>
        public IsAPropertyInvocation(IMockedObject proxy_instance)
        {
            proxyInstance = proxy_instance;
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
            return proxyInstance.IsPropertyMethod(item.GetConcreteMethod());
        }

        #endregion
    }
}