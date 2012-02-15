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
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Mammock.Impl
{
    /// <summary>
    /// This class is reponsible for taking a delegate and creating a wrapper
    /// interface around it, so it can be mocked.
    /// </summary>
    internal class DelegateTargetInterfaceCreator
    {
        /// <summary>
        /// The assembly builder.
        /// </summary>
        private readonly AssemblyBuilder assemblyBuilder;

        /// <summary>
        /// The assembly name.
        /// </summary>
        private readonly AssemblyName assemblyName = new AssemblyName("MammockDelegateDynamicAssembly");

        /// <summary>
        /// The delegate target interfaces.
        /// </summary>
        private readonly IDictionary delegateTargetInterfaces = new Hashtable();

        /// <summary>
        /// The module builder.
        /// </summary>
        private readonly ModuleBuilder moduleBuilder;

        /// <summary>
        /// The counter.
        /// </summary>
        private long counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateTargetInterfaceCreator"/> class.
        /// </summary>
        public DelegateTargetInterfaceCreator()
        {
            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
        }

        /// <summary>
        /// Gets a type with an "Invoke" method suitable for use as a target of the
        /// specified delegate type.
        /// </summary>
        /// <param name="delegateType">
        /// </param>
        /// <returns>
        /// </returns>
        public Type GetDelegateTargetInterface(Type delegateType)
        {
            Type type;
            lock (delegateTargetInterfaces)
            {
                type = (Type) delegateTargetInterfaces[delegateType];

                if (type == null)
                {
                    type = CreateCallableInterfaceFromDelegate(delegateType);
                    delegateTargetInterfaces[delegateType] = type;
                }
            }

            return type;
        }

        /// <summary>
        /// The create callable interface from delegate.
        /// </summary>
        /// <param name="delegateType">
        /// The delegate type.
        /// </param>
        /// <returns>
        /// </returns>
        private Type CreateCallableInterfaceFromDelegate(Type delegateType)
        {
            Type type;
            long count = Interlocked.Increment(ref counter);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                string.Format("ProxyDelegate_{0}_{1}", delegateType.Name, count), 
                TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public);

            MethodInfo invoke = delegateType.GetMethod("Invoke");
            ParameterInfo[] parameters = invoke.GetParameters();

            Type returnType = invoke.ReturnType;
            Type[] parameterTypes = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i] = parameters[i].ParameterType;
            }

            typeBuilder.DefineMethod("Invoke", 
                                     MethodAttributes.Abstract | MethodAttributes.Virtual | MethodAttributes.Public, 
                                     CallingConventions.HasThis, returnType, parameterTypes);

            // typeBuilder.AddInterfaceImplementation(typeof(IMockedObject));
            type = typeBuilder.CreateType();
            return type;
        }
    }
}