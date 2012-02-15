using System.Diagnostics;
using System.IO;
using Castle.DynamicProxy;
using Mammock.Interfaces;
using Mammock.Utilities;

namespace Mammock.Impl
{
    /// <summary>
    /// Writes log information as stack traces about rhino mocks activity
    /// </summary>
    public class TraceWriterWithStackTraceExpectationWriter : IExpectationLogger
    {
        /// <summary>
        /// Allows to redirect output to a different location.
        /// </summary>
        public TextWriter AlternativeWriter;

        #region IExpectationLogger Members

        /// <summary>
        /// Logs the message
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        /// Logs the expectation as is was recorded
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        /// <param name="expectation">
        /// The expectation.
        /// </param>
        public void LogRecordedExpectation(IInvocation invocation, IExpectation expectation)
        {
            string methodCall = MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
            WriteLine("Recorded expectation: {0}", methodCall);
            WriteCurrentMethod();
        }

        /// <summary>
        /// Logs the expectation as it was recorded
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        /// <param name="expectation">
        /// The expectation.
        /// </param>
        public void LogReplayedExpectation(IInvocation invocation, IExpectation expectation)
        {
            string methodCall = MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
            WriteLine("Replayed expectation: {0}", methodCall);
            WriteCurrentMethod();
        }

        /// <summary>
        /// Logs the unexpected method call.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void LogUnexpectedMethodCall(IInvocation invocation, string message)
        {
            string methodCall = MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
            WriteLine("{1}: {0}", methodCall, message);
            WriteCurrentMethod();
        }

        #endregion

        /// <summary>
        /// The write line.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private void WriteLine(string msg, params object[] args)
        {
            string result = string.Format(msg, args);
            if (AlternativeWriter != null)
            {
                AlternativeWriter.WriteLine(result);
                return;
            }

            Debug.WriteLine(result);
        }

        /// <summary>
        /// The write current method.
        /// </summary>
        private void WriteCurrentMethod()
        {
            WriteLine(new StackTrace(true).ToString());
        }
    }
}