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

using System.Collections;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;
using Mammock.Exceptions;
using Mammock.Generated;
using Mammock.Impl;
using Mammock.Interfaces;
using Mammock.Utilities;

namespace Mammock.MethodRecorders
{
    /// <summary>
    /// Unordered collection of method records, any expectation that exist
    /// will be matched.
    /// </summary>
    public class UnorderedMethodRecorder : MethodRecorderBase
    {
        /// <summary>
        /// The parent recorder we have redirected to.
        /// Useful for certain edge cases in orderring.
        /// See: FieldProblem_Entropy for the details.
        /// </summary>
        protected IMethodRecorder parentRecorderRedirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnorderedMethodRecorder"/> class. 
        /// Creates a new <see cref="UnorderedMethodRecorder"/> instance.
        /// </summary>
        /// <param name="parentRecorder">
        /// Parent recorder.
        /// </param>
        /// <param name="repeatableMethods">
        /// Repeatable methods
        /// </param>
        public UnorderedMethodRecorder(IMethodRecorder parentRecorder, 
                                       ProxyMethodExpectationsDictionary repeatableMethods)
            : base(parentRecorder, repeatableMethods)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnorderedMethodRecorder"/> class. 
        /// Creates a new <see cref="UnorderedMethodRecorder"/> instance.
        /// </summary>
        /// <param name="repeatableMethods">
        /// The repeatable Methods.
        /// </param>
        public UnorderedMethodRecorder(ProxyMethodExpectationsDictionary repeatableMethods)
            : base(repeatableMethods)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this instance has expectations that weren't satisfied yet.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has expectations; otherwise, <c>false</c>.
        /// </value>
        protected override bool DoHasExpectations
        {
            get
            {
                foreach (object action in recordedActions)
                {
                    ProxyMethodExpectationTriplet triplet = action as ProxyMethodExpectationTriplet;
                    if (triplet != null)
                    {
                        if (triplet.Expectation.CanAcceptCalls)
                            return true;
                    }
                    else
                    {
// Action is another recorder
                        IMethodRecorder innerRecorder = (IMethodRecorder) action;
                        if (innerRecorder.HasExpectations)
                            return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Records the specified call with the specified args on the mocked object.
        /// </summary>
        /// <param name="proxy">
        /// Mocked object.
        /// </param>
        /// <param name="method">
        /// Method.
        /// </param>
        /// <param name="expectation">
        /// Expectation.
        /// </param>
        protected override void DoRecord(object proxy, MethodInfo method, IExpectation expectation)
        {
            Validate.IsNotNull(proxy, "proxy");
            Validate.IsNotNull(method, "method");
            Validate.IsNotNull(expectation, "expectation");
            ProxyMethodExpectationTriplet entry = new ProxyMethodExpectationTriplet(proxy, method, expectation);
            recordedActions.Add(entry);
        }

        /// <summary>
        /// Get the expectation for this method on this object with this arguments 
        /// </summary>
        /// <param name="invocation">
        /// Invocation for this method
        /// </param>
        /// <param name="proxy">
        /// Mocked object.
        /// </param>
        /// <param name="method">
        /// Method.
        /// </param>
        /// <param name="args">
        /// Args.
        /// </param>
        /// <returns>
        /// True is the call was recorded, false otherwise
        /// </returns>
        protected override IExpectation DoGetRecordedExpectation(IInvocation invocation, object proxy, MethodInfo method, 
                                                                 object[] args)
        {
            IExpectation expectation = GetRecordedExpectationOrNull(proxy, method, args);
            if (expectation == null)
            {
                RhinoMocks.Logger.LogUnexpectedMethodCall(invocation, "Unexpected method call error");
                ExpectationViolationException expectationViolationException = UnexpectedMethodCall(invocation, proxy, 
                                                                                                   method, args);
                MockRepository.SetExceptionToBeThrownOnVerify(proxy, expectationViolationException);
                throw expectationViolationException;
            }

            return expectation;
        }

        /// <summary>
        /// Gets the all expectations for a mocked object and method combination,
        /// regardless of the expected arguments / callbacks / contraints.
        /// </summary>
        /// <param name="proxy">
        /// Mocked object.
        /// </param>
        /// <param name="method">
        /// Method.
        /// </param>
        /// <returns>
        /// List of all relevant expectation
        /// </returns>
        public override ExpectationsList GetAllExpectationsForProxyAndMethod(object proxy, MethodInfo method)
        {
            Validate.IsNotNull(proxy, "proxy");
            Validate.IsNotNull(method, "method");

            ExpectationsList expectations = new ExpectationsList();
            foreach (object action in recordedActions)
            {
                ProxyMethodExpectationTriplet triplet = action as ProxyMethodExpectationTriplet;
                if (triplet != null)
                {
                    if (MockedObjectsEquality.Instance.Equals(triplet.Proxy, proxy) &&
                        MethodsEquals(method, triplet))
                    {
                        expectations.Add(triplet.Expectation);
                    }
                }
                else
                {
// Action is another recorder
                    IMethodRecorder innerRecorder = (IMethodRecorder) action;
                    expectations.AddRange(innerRecorder.GetAllExpectationsForProxyAndMethod(proxy, method));
                }
            }

            return expectations;
        }

        /// <summary>
        /// The methods equals.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="triplet">
        /// The triplet.
        /// </param>
        /// <returns>
        /// The methods equals.
        /// </returns>
        private static bool MethodsEquals(MethodInfo method, ProxyMethodExpectationTriplet triplet)
        {
            if (method.IsGenericMethod == false)
                return triplet.Method == method;
            return triplet.Method.GetGenericMethodDefinition() == method.GetGenericMethodDefinition();
        }

        /// <summary>
        /// Gets the all expectations for proxy.
        /// </summary>
        /// <param name="proxy">
        /// Mocked object.
        /// </param>
        /// <returns>
        /// List of all relevant expectation
        /// </returns>
        protected override ExpectationsList DoGetAllExpectationsForProxy(object proxy)
        {
            Validate.IsNotNull(proxy, "proxy");

            ExpectationsList expectations = new ExpectationsList();
            foreach (object action in recordedActions)
            {
                ProxyMethodExpectationTriplet triplet = action as ProxyMethodExpectationTriplet;
                if (triplet != null)
                {
                    if (MockedObjectsEquality.Instance.Equals(triplet.Proxy, proxy))
                    {
                        expectations.Add(triplet.Expectation);
                    }
                }
                else
                {
// Action is another recorder
                    IMethodRecorder innerRecorder = (IMethodRecorder) action;
                    ExpectationsList expectationsForProxy = innerRecorder.GetAllExpectationsForProxy(proxy);
                    expectations.AddRange(expectationsForProxy);
                }
            }

            return expectations;
        }

        /// <summary>
        /// Replaces the old expectation with the new expectation for the specified proxy/method pair.
        /// This replace ALL expectations that equal to old expectations.
        /// </summary>
        /// <param name="proxy">
        /// Proxy.
        /// </param>
        /// <param name="method">
        /// Method.
        /// </param>
        /// <param name="oldExpectation">
        /// Old expectation.
        /// </param>
        /// <param name="newExpectation">
        /// New expectation.
        /// </param>
        protected override void DoReplaceExpectation(object proxy, MethodInfo method, IExpectation oldExpectation, 
                                                     IExpectation newExpectation)
        {
            Validate.IsNotNull(proxy, "proxy");
            Validate.IsNotNull(method, "method");
            Validate.IsNotNull(oldExpectation, "oldExpectation");
            Validate.IsNotNull(newExpectation, "newExpectation");
            foreach (object action in recordedActions)
            {
                ProxyMethodExpectationTriplet triplet = action as ProxyMethodExpectationTriplet;
                if (triplet != null)
                {
                    if (MockedObjectsEquality.Instance.Equals(triplet.Proxy, proxy) &&
                        triplet.Method == method &&
                        triplet.Expectation == oldExpectation)
                    {
                        triplet.Expectation = newExpectation;
                    }
                }

// Action cannot be another recorder, since then the RemoveExpectation would've
                // passed us to the top most recorder.
            }
        }

        /// <summary>
        /// Handle the real execution of this method for the derived class
        /// </summary>
        /// <param name="expectation">
        /// The expectation.
        /// </param>
        protected override void DoRemoveExpectation(IExpectation expectation)
        {
            for (int i = 0; i < recordedActions.Count; i++)
            {
                ProxyMethodExpectationTriplet triplet = recordedActions[i] as ProxyMethodExpectationTriplet;
                if (triplet != null)
                {
                    if (triplet.Expectation == expectation)
                        recordedActions.RemoveAt(i);
                }

// Action cannot be another recorder, since then the RemoveExpectation would've
                // passed us to the top most recorder.
            }
        }

        /// <summary>
        /// Handles the real getting of the recorded expectation or null.
        /// </summary>
        /// <param name="proxy">
        /// The proxy.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected override IExpectation DoGetRecordedExpectationOrNull(object proxy, MethodInfo method, object[] args)
        {
            Validate.IsNotNull(proxy, "proxy");
            Validate.IsNotNull(method, "method");
            Validate.IsNotNull(args, "args");

// Need this because we may want to modify the recordedAction list as we traverse it
            // See: ClearReplayerToCall();
            ArrayList traversalSafeCopy = new ArrayList(recordedActions);
            bool allSatisfied = true;
            foreach (object action in traversalSafeCopy)
            {
                ProxyMethodExpectationTriplet triplet = action as ProxyMethodExpectationTriplet;
                if (triplet != null)
                {
                    if (MockedObjectsEquality.Instance.Equals(triplet.Proxy, proxy) &&
                        triplet.Method == method &&
                        triplet.Expectation.CanAcceptCalls &&
                        triplet.Expectation.IsExpected(args))
                    {
                        triplet.Expectation.AddActualCall();
                        return triplet.Expectation;
                    }

                    if (!triplet.Expectation.ExpectationSatisfied)
                        allSatisfied = false;
                }
                else
                {
// Action is another recorder
                    IMethodRecorder innerRecorder = (IMethodRecorder) action;
                    if (ShouldConsiderThisReplayer(innerRecorder) == false)
                        continue;
                    IExpectation expectation = innerRecorder.GetRecordedExpectationOrNull(proxy, method, args);
                    if (expectation != null)
                    {
                        replayerToCall = innerRecorder;
                        return expectation;
                    }

                    if (innerRecorder.HasExpectations)
                        allSatisfied = false;
                }
            }

// We still have unsatisifed expectation or we don't have a parent recorder
            if (!allSatisfied || parentRecorder == null)
                return null;

// We only reach this place if we still has valid expectations, but they are not
            // mandatory, (AtLeastOnce(), etc). In this case, the recorder (and its children) cannot satisfy the 
            // expectation, so we move to the parent recorder and let it handle it.
            parentRecorder.ClearReplayerToCall(this);

// We need this to give the correct exception if the method is an unepxected one.
            // Check the redirection in UnexpectedMethodCall()
            parentRecorderRedirection = parentRecorder;
            return parentRecorder.GetRecordedExpectationOrNull(proxy, method, args);
        }

        /// <summary>
        /// Handle the real execution of this method for the derived class
        /// </summary>
        /// <param name="recorder">
        /// The recorder.
        /// </param>
        protected override void DoAddRecorder(IMethodRecorder recorder)
        {
            recordedActions.Add(recorder);
        }

        /// <summary>
        /// Gets the next expected calls string.
        /// </summary>
        /// <returns>
        /// The get expected calls message.
        /// </returns>
        public override string GetExpectedCallsMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Unordered: { ");
            foreach (object action in recordedActions)
            {
                ProxyMethodExpectationTriplet triplet = action as ProxyMethodExpectationTriplet;
                if (triplet != null)
                {
                    sb.Append(triplet.Expectation.ErrorMessage);
                }
                else
                {
                    string nested = ((IMethodRecorder) action).GetExpectedCallsMessage();
                    sb.Append(nested);
                }
            }

            sb.Append(" }");
            return sb.ToString();
        }

        /// <summary>
        /// Create an exception for an unexpected method call.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        /// <param name="proxy">
        /// The proxy.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public override ExpectationViolationException UnexpectedMethodCall(IInvocation invocation, object proxy, 
                                                                           MethodInfo method, object[] args)
        {
            // We have move to the parent recorder, we need to pass the call to it.
            if (parentRecorderRedirection != null)
                return parentRecorderRedirection.UnexpectedMethodCall(invocation, proxy, method, args);
            StringBuilder sb = new StringBuilder();
            CalcExpectedAndActual calc = new CalcExpectedAndActual(this, proxy, method, args);
            string methodAsString = MethodCallUtil.StringPresentation(invocation, method, args);
            sb.Append(methodAsString);
            sb.Append(" Expected #");
            if (calc.ExpectedMax == calc.ExpectedMin)
                sb.Append(calc.ExpectedMin);
            else
                sb.Append(calc.ExpectedMin).Append(" - ").Append(calc.ExpectedMax);
            sb.Append(", Actual #").Append(calc.Actual).Append('.');
            ExpectationsList list = GetAllExpectationsForProxyAndMethod(proxy, method);
            if (list.Count > 0)
            {
                string message = list[0].Message;
                if (message != null)
                {
                    sb.Append(System.Environment.NewLine)
                        .Append("Message: ")
                        .Append(message);
                }
            }

            AppendNextExpected(proxy, method, sb);
            return new ExpectationViolationException(sb.ToString());
        }

        /// <summary>
        /// The append next expected.
        /// </summary>
        /// <param name="proxy">
        /// The proxy.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="sb">
        /// The sb.
        /// </param>
        private void AppendNextExpected(object proxy, MethodInfo method, StringBuilder sb)
        {
            ExpectationsList list = GetAllExpectationsForProxyAndMethod(proxy, method);
            if (list.Count > 0)
            {
                IExpectation expectation = list[0];
                if (expectation.ExpectationSatisfied)
                    return; // avoid showing methods that were completed.
                sb.Append("\r\n");
                sb.Append(expectation.ErrorMessage).Append(" Expected #");
                sb.Append(expectation.Expected).Append(", Actual #");
                sb.Append(expectation.ActualCallsCount).Append(".");
            }
        }

        #region Nested type: CalcExpectedAndActual

        /// <summary>
        /// The calc expected and actual.
        /// </summary>
        private class CalcExpectedAndActual
        {
            /// <summary>
            /// The parent.
            /// </summary>
            private readonly UnorderedMethodRecorder parent;

            /// <summary>
            /// The actual.
            /// </summary>
            private int actual = 1;

            /// <summary>
            /// The expected max.
            /// </summary>
            private int expectedMax;

            /// <summary>
            /// The expected min.
            /// </summary>
            private int expectedMin;

            /// <summary>
            /// Initializes a new instance of the <see cref="CalcExpectedAndActual"/> class.
            /// </summary>
            /// <param name="parent">
            /// The parent.
            /// </param>
            /// <param name="proxy">
            /// The proxy.
            /// </param>
            /// <param name="method">
            /// The method.
            /// </param>
            /// <param name="args">
            /// The args.
            /// </param>
            public CalcExpectedAndActual(UnorderedMethodRecorder parent, object proxy, MethodInfo method, object[] args)
            {
                this.parent = parent;
                Calculate(proxy, method, args);
            }

            /// <summary>
            /// Gets Actual.
            /// </summary>
            public int Actual
            {
                get { return actual; }
            }


            /// <summary>
            /// Gets ExpectedMin.
            /// </summary>
            public int ExpectedMin
            {
                get { return expectedMin; }
            }

            /// <summary>
            /// Gets ExpectedMax.
            /// </summary>
            public int ExpectedMax
            {
                get { return expectedMax; }
            }

            /// <summary>
            /// The calculate.
            /// </summary>
            /// <param name="proxy">
            /// The proxy.
            /// </param>
            /// <param name="method">
            /// The method.
            /// </param>
            /// <param name="args">
            /// The args.
            /// </param>
            private void Calculate(object proxy, MethodInfo method, object[] args)
            {
                ExpectationsList list = parent.GetAllExpectationsForProxyAndMethod(proxy, method);
                foreach (IExpectation expectation in list)
                {
                    if (expectation.IsExpected(args))
                    {
                        expectedMin += expectation.Expected.Min;
                        expectedMax += expectation.Expected.Max ?? expectation.Expected.Min;
                        actual += expectation.ActualCallsCount;
                    }
                }
            }
        }

        #endregion
    }
}