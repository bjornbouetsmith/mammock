using Castle.DynamicProxy;
using Mammock.Impl.InvocationSpecifications;

namespace Mammock.Impl.Invocation.Specifications
{
    /// <summary>
    /// The is an event invocation.
    /// </summary>
    public class IsAnEventInvocation : ISpecification<IInvocation>
    {
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
            return new AndSpecification<IInvocation>(new FollowsEventNamingStandard(), 
                                                     new NamedEventExistsOnDeclaringType()).IsSatisfiedBy(item);
        }

        #endregion
    }

    /// <summary>
    /// The any invocation.
    /// </summary>
    public class AnyInvocation : ISpecification<IInvocation>
    {
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
            return true;
        }

        #endregion
    }
}