#region [ License information          ]
/* ************************************************************
 *
 * Copyright (c) Bjørn Bouet Smith, 2012
 *
 * This source code is subject to terms and conditions of 
 * Microsoft Public License (Ms-PL).
 * 
 * A copy of the license can be found in the license.txt
 * file at the root of this distribution. If you can not 
 * locate the License, please send an email to bjornsmith@gmail.com
 * 
 * By using this source code in any fashion, you are 
 * agreeing to be bound by the terms of the Microsoft 
 * Public License.
 *
 * You must not remove this notice, or any other, from this
 * software.
 *
 * ************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using Castle.DynamicProxy;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace Mammock.Tests.Delegates
{

    public class SimpleDelegateTest
    {
        public class DummyInterceptor : IInterceptor
        {
            public void Intercept(IInvocation invocation)
            {
                invocation.Proceed();
            }
        }

        delegate void OmgDelegate(string args);

        [Fact]
        public void SimpleShouldWork()
        {
            //ProxyGenerator pg = new ProxyGenerator();
            //ModuleScope moduleScope = new ModuleScope();

            //DummyInterceptor interceptor = new DummyInterceptor();

            //Type delegatetype = CreateCallableInterfaceFromDelegate(typeof(OmgDelegate), moduleScope);

            //object interfaceProxyWithoutTarget = pg.CreateInterfaceProxyWithoutTarget(delegatetype, new[] { typeof(IMockedObject) }, new ProxyGenerationOptions(), interceptor);

            //OmgDelegate dlg = (OmgDelegate)Delegate.CreateDelegate(typeof(OmgDelegate), interfaceProxyWithoutTarget, "Invoke");

            //dlg("test");
        }

        private Type CreateCallableInterfaceFromDelegate(Type delegateType, ModuleScope moduleScope)
        {
            Type type;

            long count = 1;

            AssemblyName aName = new AssemblyName("DynamicAssemblyExample");
            AssemblyBuilder ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.Run);

            // Create the module.
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);

            TypeBuilder typeBuilder = mb.DefineType(
                String.Format("ProxyDelegate_{0}_{1}", delegateType.Name, count),
                TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public);

            MethodInfo invoke = delegateType.GetMethod("Invoke");
            ParameterInfo[] parameters = invoke.GetParameters();

            Type returnType = invoke.ReturnType;
            Type[] parameterTypes = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i] = parameters[i].ParameterType;
            }

            typeBuilder.DefineMethod("Invoke", MethodAttributes.Abstract | MethodAttributes.Virtual | MethodAttributes.Public, CallingConventions.HasThis, returnType, parameterTypes);

            type = typeBuilder.CreateType();
            return type;
        }
    }
}
