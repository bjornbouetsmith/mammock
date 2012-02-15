using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Castle.DynamicProxy;

namespace Mammock.Impl.RemotingMock
{
    /// <summary>
    /// Implementation of IInvocation based on remoting proxy
    /// </summary>
    /// <remarks>
    /// Some methods are marked NotSupported since they either don't make sense
    /// for remoting proxies, or they are never called by Rhino Mocks
    /// </remarks>
    internal class RemotingInvocation : IInvocation
    {
        /// <summary>
        /// The _args.
        /// </summary>
        private readonly object[] _args;

        /// <summary>
        /// The _message.
        /// </summary>
        private readonly IMethodCallMessage _message;

        /// <summary>
        /// The _real proxy.
        /// </summary>
        private readonly RealProxy _realProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotingInvocation"/> class.
        /// </summary>
        /// <param name="realProxy">
        /// The real proxy.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public RemotingInvocation(RealProxy realProxy, IMethodCallMessage message)
        {
            _message = message;
            _realProxy = realProxy;
            this._args = (object[]) this._message.Properties["__Args"];
        }

        #region IInvocation Members

        /// <summary>
        /// Gets Arguments.
        /// </summary>
        public object[] Arguments
        {
            get { return _args; }
        }

        /// <summary>
        /// Gets GenericArguments.
        /// </summary>
        public Type[] GenericArguments
        {
            get
            {
                MethodBase method = _message.MethodBase;
                if (!method.IsGenericMethod)
                {
                    return new Type[0];
                }

                return method.GetGenericArguments();
            }
        }

        /// <summary>
        /// The get argument value.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The get argument value.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public object GetArgumentValue(int index)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// The get concrete method.
        /// </summary>
        /// <returns>
        /// </returns>
        public MethodInfo GetConcreteMethod()
        {
            return (MethodInfo) _message.MethodBase;
        }

        /// <summary>
        /// The get concrete method invocation target.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public MethodInfo GetConcreteMethodInvocationTarget()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets InvocationTarget.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public object InvocationTarget
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets Method.
        /// </summary>
        public MethodInfo Method
        {
            get { return GetConcreteMethod(); }
        }

        /// <summary>
        /// Gets MethodInvocationTarget.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public MethodInfo MethodInvocationTarget
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The proceed.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public void Proceed()
        {
            throw new InvalidOperationException("Proceed() is not applicable to remoting mocks.");
        }

        /// <summary>
        /// Gets Proxy.
        /// </summary>
        public object Proxy
        {
            get { return _realProxy.GetTransparentProxy(); }
        }

        /// <summary>
        /// Gets or sets ReturnValue.
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// The set argument value.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public void SetArgumentValue(int index, object value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets TargetType.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public Type TargetType
        {
            get { throw new NotSupportedException(); }
        }

        #endregion
    }
}