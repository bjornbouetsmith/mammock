using System;
using System.Reflection;
using Castle.DynamicProxy;
using Mammock.Constraints;
using Mammock.Expectations;
using Mammock.Interfaces;

namespace Mammock.Impl
{
    /// <summary>
    /// Responsible for building expectations
    /// </summary>
    public class ExpectationBuilder
    {
        /// <summary>
        /// Builds the default expectation.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <param name="callCallRangeExpectation">
        /// The call call range expectation.
        /// </param>
        /// <returns>
        /// </returns>
        public IExpectation BuildDefaultExpectation(IInvocation invocation, MethodInfo method, object[] args, 
                                                    Func<Range> callCallRangeExpectation)
        {
            ParameterInfo[] parameters = method.GetParameters();
            if (!Array.Exists(parameters, p => p.IsOut))
            {
                return new ArgsEqualExpectation(invocation, args, callCallRangeExpectation());
            }

            // The value of an incoming out parameter variable is ignored
            AbstractConstraint[] constraints = new AbstractConstraint[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                constraints[i] = parameters[i].IsOut ? Is.Anything() : Is.Equal(args[i]);
            }

            return new ConstraintsExpectation(invocation, constraints, callCallRangeExpectation());
        }

        /// <summary>
        /// Builds the param expectation.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <returns>
        /// </returns>
        public IExpectation BuildParamExpectation(IInvocation invocation, MethodInfo method)
        {
            ArgManager.CheckMethodSignature(method);
            ConstraintsExpectation expectation = new ConstraintsExpectation(invocation, ArgManager.GetAllConstraints(), 
                                                                            new Range(1, null));
            expectation.OutRefParams = ArgManager.GetAllReturnValues();
            return expectation;
        }
    }
}