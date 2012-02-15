using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Castle.DynamicProxy;
using Mammock.Interfaces;

namespace Mammock.Impl.RemotingMock
{
    /// <summary>
    /// The remoting proxy.
    /// </summary>
    internal class RemotingProxy : RealProxy
    {
        /// <summary>
        /// The _interceptor.
        /// </summary>
        private readonly IInterceptor _interceptor;

        /// <summary>
        /// The _mocked object.
        /// </summary>
        private readonly IMockedObject _mockedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotingProxy"/> class.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="interceptor">
        /// The interceptor.
        /// </param>
        /// <param name="mockedObject">
        /// The mocked object.
        /// </param>
        public RemotingProxy(Type type, IInterceptor interceptor, IMockedObject mockedObject)
            :
                base(type)
        {
            _interceptor = interceptor;
            _mockedObject = mockedObject;
        }

        /// <summary>
        /// Gets MockedObject.
        /// </summary>
        public IMockedObject MockedObject
        {
            get { return _mockedObject; }
        }

        /// <summary>
        /// The return value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="outParams">
        /// The out params.
        /// </param>
        /// <param name="mcm">
        /// The mcm.
        /// </param>
        /// <returns>
        /// </returns>
        private static IMessage ReturnValue(object value, object[] outParams, IMethodCallMessage mcm)
        {
            return new ReturnMessage(value, outParams, outParams == null ? 0 : outParams.Length, mcm.LogicalCallContext, 
                                     mcm);
        }

        /// <summary>
        /// The invoke.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <returns>
        /// </returns>
        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage mcm = msg as IMethodCallMessage;
            if (mcm == null) return null;

            if (IsEqualsMethod(mcm))
            {
                return ReturnValue(HandleEquals(mcm), mcm);
            }

            if (IsGetHashCodeMethod(mcm))
            {
                return ReturnValue(GetHashCode(), mcm);
            }

            if (IsGetTypeMethod(mcm))
            {
                return ReturnValue(GetProxiedType(), mcm);
            }

            if (IsToStringMethod(mcm))
            {
                string retVal = string.Format("RemotingMock_{1}<{0}>", this.GetProxiedType().Name, this.GetHashCode());
                return ReturnValue(retVal, mcm);
            }

            RemotingInvocation invocation = new RemotingInvocation(this, mcm);
            _interceptor.Intercept(invocation);

            return ReturnValue(invocation.ReturnValue, invocation.Arguments, mcm);
        }

        /// <summary>
        /// The is get type method.
        /// </summary>
        /// <param name="mcm">
        /// The mcm.
        /// </param>
        /// <returns>
        /// The is get type method.
        /// </returns>
        private bool IsGetTypeMethod(IMethodCallMessage mcm)
        {
            if (mcm.MethodName != "GetType") return false;
            if (mcm.MethodBase.DeclaringType != typeof (object)) return false;
            Type[] args = mcm.MethodSignature as Type[];
            if (args == null) return false;
            return args.Length == 0;
        }

        /// <summary>
        /// The is equals method.
        /// </summary>
        /// <param name="mcm">
        /// The mcm.
        /// </param>
        /// <returns>
        /// The is equals method.
        /// </returns>
        private static bool IsEqualsMethod(IMethodMessage mcm)
        {
            if (mcm.MethodName != "Equals") return false;
            Type[] argTypes = mcm.MethodSignature as Type[];
            if (argTypes == null) return false;
            if (argTypes.Length == 1 && argTypes[0] == typeof (object)) return true;
            return false;
        }

        /// <summary>
        /// The is get hash code method.
        /// </summary>
        /// <param name="mcm">
        /// The mcm.
        /// </param>
        /// <returns>
        /// The is get hash code method.
        /// </returns>
        private static bool IsGetHashCodeMethod(IMethodMessage mcm)
        {
            if (mcm.MethodName != "GetHashCode") return false;
            Type[] argTypes = mcm.MethodSignature as Type[];
            if (argTypes == null) return false;
            return argTypes.Length == 0;
        }

        /// <summary>
        /// The is to string method.
        /// </summary>
        /// <param name="mcm">
        /// The mcm.
        /// </param>
        /// <returns>
        /// The is to string method.
        /// </returns>
        private static bool IsToStringMethod(IMethodCallMessage mcm)
        {
            if (mcm.MethodName != "ToString") return false;
            Type[] args = mcm.MethodSignature as Type[];
            if (args == null) return false;
            return args.Length == 0;
        }


        /// <summary>
        /// The handle equals.
        /// </summary>
        /// <param name="mcm">
        /// The mcm.
        /// </param>
        /// <returns>
        /// The handle equals.
        /// </returns>
        private bool HandleEquals(IMethodMessage mcm)
        {
            object another = mcm.Args[0];
            if (another == null) return false;

            if (another is IRemotingProxyOperation)
            {
                ((IRemotingProxyOperation) another).Process(this);
                return false;
            }

            return ReferenceEquals(GetTransparentProxy(), another);
        }

        /// <summary>
        /// The return value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="mcm">
        /// The mcm.
        /// </param>
        /// <returns>
        /// </returns>
        private static IMessage ReturnValue(object value, IMethodCallMessage mcm)
        {
            return new ReturnMessage(value, null, 0, mcm.LogicalCallContext, mcm);
        }
    }
}