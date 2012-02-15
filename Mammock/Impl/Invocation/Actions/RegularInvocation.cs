using System.Reflection;
using Castle.DynamicProxy;
using Mammock.Interfaces;

namespace Mammock.Impl.Invocation.Actions
{
    /// <summary>
    /// The regular invocation.
    /// </summary>
    public class RegularInvocation : IInvocationActionn
    {
        /// <summary>
        /// The mock repository.
        /// </summary>
        private readonly MockRepository mockRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegularInvocation"/> class. 
        /// The regular invocation.
        /// </summary>
        /// <param name="mock_repository">
        /// The mock_repository.
        /// </param>
        public RegularInvocation(MockRepository mock_repository)
        {
            mockRepository = mock_repository;
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
            object proxy = mockRepository.GetMockObjectFromInvocationProxy(invocation.Proxy);
            MethodInfo method = invocation.GetConcreteMethod();
            invocation.ReturnValue = mockRepository.MethodCall(invocation, proxy, method, invocation.Arguments);
        }

        #endregion
    }
}