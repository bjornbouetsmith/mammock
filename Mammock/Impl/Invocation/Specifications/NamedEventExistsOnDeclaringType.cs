using Castle.DynamicProxy;
using Mammock.Impl.InvocationSpecifications;

namespace Mammock.Impl.Invocation.Specifications
{
    /// <summary>
    /// Summary descritpion for NamedEventExistsOnDeclaringType
    /// </summary>
    public class NamedEventExistsOnDeclaringType : ISpecification<IInvocation>
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
            return
                item.Method.DeclaringType.GetEvent(
                    item.Method.Name.Substring(FollowsEventNamingStandard.AddPrefix.Length)) != null ||
                item.Method.DeclaringType.GetEvent(
                    item.Method.Name.Substring(FollowsEventNamingStandard.RemovePrefix.Length)) != null;
        }

        #endregion
    }
}