using System.IO;
using Castle.DynamicProxy;
using Mammock.Interfaces;
using Mammock.Utilities;

namespace Mammock.Impl
{
    /// <summary>
    /// Rudimetry implementation that simply logs methods calls as text.
    /// </summary>
    public class TextWriterExpectationLogger : IExpectationLogger
    {
        /// <summary>
        /// The writer.
        /// </summary>
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriterExpectationLogger"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public TextWriterExpectationLogger(TextWriter writer)
        {
            this.writer = writer;
        }

        #region IExpectationLogger Members

        /// <summary>
        /// Logs the message
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Log(string message)
        {
            writer.WriteLine(message);
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
        public void LogRecordedExpectation(IInvocation invocation, IExpectation expectation)
        {
            string methodCall = MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
            writer.WriteLine("Recorded expectation: {0}", methodCall);
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
            writer.WriteLine("Replayed expectation: {0}", methodCall);
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
            writer.WriteLine("{1}: {0}", methodCall, message);
        }

        #endregion
    }
}