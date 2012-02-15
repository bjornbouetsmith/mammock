#region license

// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Mammock.Impl.Invocation;
using Mammock.Impl.Invocation.Actions;
using Mammock.Impl.Invocation.Specifications;
using Mammock.Interfaces;

namespace Mammock.Impl.InvocationSpecifications
{
}

namespace Mammock.Impl
{
    /// <summary>
    /// Summary description for RhinoInterceptor.
    /// </summary>
    public class RhinoInterceptor : MarshalByRefObject, IInterceptor
    {
        /// <summary>
        /// The invocation_visitors.
        /// </summary>
        private readonly IEnumerable<InvocationVisitor> invocation_visitors;

        /// <summary>
        /// The proxy instance.
        /// </summary>
        private readonly IMockedObject proxyInstance;

        /// <summary>
        /// The repository.
        /// </summary>
        private readonly MockRepository repository;


        /// <summary>
        /// Initializes a new instance of the <see cref="RhinoInterceptor"/> class. 
        /// Creates a new <see cref="RhinoInterceptor"/> instance.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="proxyInstance">
        /// The proxy Instance.
        /// </param>
        /// <param name="invocation_visitors">
        /// The invocation_visitors.
        /// </param>
        public RhinoInterceptor(MockRepository repository, IMockedObject proxyInstance, 
                                IEnumerable<InvocationVisitor> invocation_visitors)
        {
            this.repository = repository;
            this.proxyInstance = proxyInstance;
            this.invocation_visitors = invocation_visitors;
        }

        #region IInterceptor Members

        /// <summary>
        /// Intercept a method call and direct it to the repository.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        public void Intercept(IInvocation invocation)
        {
            proxyInstance.MockedObjectInstance = invocation.Proxy;
            foreach (InvocationVisitor visitor in invocation_visitors)
            {
                if (visitor.CanWorkWith(invocation))
                {
                    visitor.RunAgainst(invocation);
                    return;
                }
            }

            if (new IsAnEventInvocation().IsSatisfiedBy(invocation))
                new HandleEvent(proxyInstance).PerformAgainst(invocation);
            new RegularInvocation(repository).PerformAgainst(invocation);
        }

        #endregion
    }
}