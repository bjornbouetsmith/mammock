using Castle.DynamicProxy;
using Mammock.Impl.InvocationSpecifications;
using Mammock.Interfaces;

namespace Mammock.Impl.Invocation
{
    /// <summary>
    /// The invocation visitor.
    /// </summary>
    public class InvocationVisitor
    {
        /// <summary>
        /// The criteria.
        /// </summary>
        private readonly ISpecification<IInvocation> criteria;

        /// <summary>
        /// The invocation action.
        /// </summary>
        private readonly IInvocationActionn invocationAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationVisitor"/> class. 
        /// The invocation visitor.
        /// </summary>
        /// <param name="criteria">
        /// The criteria.
        /// </param>
        /// <param name="invocationAction">
        /// The invocation Action.
        /// </param>
        public InvocationVisitor(ISpecification<IInvocation> criteria, IInvocationActionn invocationAction)
        {
            this.criteria = criteria;
            this.invocationAction = invocationAction;
        }

        /// <summary>
        /// The can work with.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The can work with.
        /// </returns>
        public bool CanWorkWith(IInvocation item)
        {
            return criteria.IsSatisfiedBy(item);
        }

        /// <summary>
        /// The run against.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        public void RunAgainst(IInvocation invocation)
        {
            invocationAction.PerformAgainst(invocation);
        }
    }
}