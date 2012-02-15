using Castle.DynamicProxy;
using Mammock.Impl.InvocationSpecifications;

namespace Mammock.Impl.Invocation.Specifications
{
    /// <summary>
    /// Summary description for FollowsEventNamingStandard
    /// </summary>
    public class FollowsEventNamingStandard : ISpecification<IInvocation>
    {
        /// <summary>
        /// The add prefix.
        /// </summary>
        public const string AddPrefix = "add_";

        /// <summary>
        /// The remove prefix.
        /// </summary>
        public const string RemovePrefix = "remove_";

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
            return item.Method.Name.StartsWith(AddPrefix) ||
                   item.Method.Name.StartsWith(RemovePrefix);
        }

        #endregion
    }
}