using System.Collections.Generic;
using Mammock.Impl.Invocation.Actions;
using Mammock.Impl.Invocation.Specifications;
using Mammock.Interfaces;

namespace Mammock.Impl.Invocation
{
    /// <summary>
    /// The invocation visitors factory.
    /// </summary>
    public class InvocationVisitorsFactory
    {
        /// <summary>
        /// The create standard invocation visitors.
        /// </summary>
        /// <param name="proxy_instance">
        /// The proxy_instance.
        /// </param>
        /// <param name="mockRepository">
        /// The mock Repository.
        /// </param>
        public IEnumerable<InvocationVisitor> CreateStandardInvocationVisitors(IMockedObject proxy_instance, 
                                                                               MockRepository mockRepository)
        {
            return new List<InvocationVisitor>
                       {
                           new InvocationVisitor(new IsAnInvocationOfAMethodBelongingToObject(), new Proceed()), 
                           new InvocationVisitor(
                               new IsAnInvocationOnAMockedObject(), 
                               new InvokeMethodAgainstMockedObject(proxy_instance)), 
                           new InvocationVisitor(new IsInvocationThatShouldTargetOriginal(proxy_instance), new Proceed()), 
                           new InvocationVisitor(
                               new IsAPropertyInvocation(proxy_instance), 
                               new InvokeProperty(proxy_instance, mockRepository))
                       };
        }
    }
}