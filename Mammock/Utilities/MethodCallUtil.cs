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
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;
using Mammock.Impl;
using Mammock.Interfaces;

namespace Mammock.Utilities
{
    /// <summary>
    /// Utility class for working with method calls.
    /// </summary>
    public static class MethodCallUtil
    {
        #region Delegates

        /// <summary>
        /// Delegate to format the argument for the string representation of
        /// the method call.
        /// </summary>
        public delegate string FormatArgumnet(Array args, int i);

        #endregion

        /// <summary>
        /// Return the string representation of a method call and its arguments.
        /// </summary>
        /// <param name="invocation">
        /// Invocation of the method, used to get the generics arguments
        /// </param>
        /// <param name="format">
        /// Delegate to format the parameter
        /// </param>
        /// <param name="method">
        /// The method
        /// </param>
        /// <param name="args">
        /// The method arguments
        /// </param>
        /// <returns>
        /// The string representation of this method call
        /// </returns>
        public static string StringPresentation(IInvocation invocation, FormatArgumnet format, MethodInfo method, 
                                                object[] args)
        {
            Validate.IsNotNull(format, "format");
            Validate.IsNotNull(method, "method");
            Validate.IsNotNull(args, "args");
            StringBuilder sb = new StringBuilder();
            sb.Append(method.DeclaringType.Name).Append(".").Append(method.Name);
            if (invocation != null)
            {
                if (method.IsGenericMethod)
                {
                    sb.Append("<");
                    foreach (Type genericArgument in invocation.GenericArguments)
                    {
                        sb.Append(genericArgument);
                        sb.Append(", ");
                    }

                    sb.Remove(sb.Length - 2, 2); // remove last ", " 
                    sb.Append(">");
                }
            }

            sb.Append("(");
            int numberOfParameters = method.GetParameters().Length;
            for (int i = 0; i < numberOfParameters; i++)
            {
                sb.Append(format(args, i));
                if (i < numberOfParameters - 1)
                    sb.Append(", ");
            }

            sb.Append(");");
            return sb.ToString();
        }

        /// <summary>
        /// Return the string representation of a method call and its arguments.
        /// </summary>
        /// <param name="invocation">
        /// The invocation of the method, used to get the generic parameters
        /// </param>
        /// <param name="method">
        /// The method
        /// </param>
        /// <param name="args">
        /// The method arguments
        /// </param>
        /// <returns>
        /// The string representation of this method call
        /// </returns>
        public static string StringPresentation(IInvocation invocation, MethodInfo method, object[] args)
        {
            return StringPresentation(invocation, DefaultFormatArgument, method, args);
        }

        #region Private Methods

        /// <summary>
        /// The default format argument.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <param name="i">
        /// The i.
        /// </param>
        /// <returns>
        /// The default format argument.
        /// </returns>
        private static string DefaultFormatArgument(Array args, int i)
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
                    sb.Append(DefaultFormatArgument(arr, j));
                    if (j < arr.Length - 1)
                        sb.Append(", ");
                }

                sb.Append("]");
                return sb.ToString();
            }

            if (arg is string)
                return "\"" + arg + "\"";
            else if (arg == null)
                return "null";
            else
                return MockingSafeToString(arg);
        }

        // we need to ensure that we won't re-eenterant into the repository
        // if the parameter is a mock object
        /// <summary>
        /// The mocking safe to string.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <returns>
        /// The mocking safe to string.
        /// </returns>
        private static string MockingSafeToString(object arg)
        {
            IMockedObject mock = arg as IMockedObject;
            if (mock == null)
                return arg.ToString();
            return mock.GetType().BaseType.FullName;
        }

        #endregion
    }
}