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
using System.Text;
using Castle.DynamicProxy;
using Mammock.Impl;
using Mammock.Interfaces;
using Mammock.Utilities;

namespace Mammock.Expectations
{
    /// <summary>
    /// Summary description for ArgsEqualExpectation.
    /// </summary>
    public class ArgsEqualExpectation : AbstractExpectation
    {
        /// <summary>
        /// The expected args.
        /// </summary>
        private readonly object[] expectedArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgsEqualExpectation"/> class. 
        /// Creates a new <see cref="ArgsEqualExpectation"/> instance.
        /// </summary>
        /// <param name="invocation">
        /// The invocation for this expectation
        /// </param>
        /// <param name="expectedArgs">
        /// Expected args.
        /// </param>
        /// <param name="expectedRange">
        /// Number of method calls for this expectations
        /// </param>
        public ArgsEqualExpectation(IInvocation invocation, object[] expectedArgs, Range expectedRange)
            : base(invocation, expectedRange)
        {
            this.expectedArgs = expectedArgs;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value></value>
        public override string ErrorMessage
        {
            get
            {
                MethodCallUtil.FormatArgumnet format = FormatArg;
                string methodCall = MethodCallUtil.StringPresentation(Originalinvocation, format, Method, ExpectedArgs);
                return this.CreateErrorMessage(methodCall);
            }
        }

        /// <summary>
        /// Get the expected args.
        /// </summary>
        public object[] ExpectedArgs
        {
            get { return expectedArgs; }
        }

        /// <summary>
        /// Validate the arguments for the method.
        /// </summary>
        /// <param name="args">
        /// The arguments with which the method was called
        /// </param>
        /// <returns>
        /// The do is expected.
        /// </returns>
        protected override bool DoIsExpected(object[] args)
        {
            return Validate.ArgsEqual(expectedArgs, args);
        }

        /// <summary>
        /// Determines if the object equal to expectation
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The equals.
        /// </returns>
        public override bool Equals(object obj)
        {
            ArgsEqualExpectation other = obj as ArgsEqualExpectation;
            if (other == null)
                return false;
            return Method.Equals(other.Method) && Validate.ArgsEqual(expectedArgs, other.expectedArgs);
        }

        /// <summary>
        /// Get the hash code
        /// </summary>
        /// <returns>
        /// The get hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return Method.GetHashCode();
        }


        /// <summary>
        /// The format arg.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <param name="i">
        /// The i.
        /// </param>
        /// <returns>
        /// The format arg.
        /// </returns>
        private static string FormatArg(Array args, int i)
        {
            if (args.Length <= i)
                return "missing parameter";
            object arg = args.GetValue(i);
            if (arg is Array)
            {
                Array arr = (Array) arg;
                StringBuilder sb = new StringBuilder();
                sb.Append('[');
                for (int j = 0; j < arr.Length; j++)
                {
                    sb.Append(FormatArg(arr, j));
                    if (j < arr.Length - 1)
                        sb.Append(", ");
                }

                sb.Append("]");
                return sb.ToString();
            }
            else if (arg is string)
            {
                return '"' + arg.ToString() + '"';
            }
            else if (arg == null)
            {
                return "null";
            }
            else if (arg is IMockedObject)
            {
                return arg.GetType().ToString();
            }
            else
            {
                return arg.ToString();
            }
        }
    }
}