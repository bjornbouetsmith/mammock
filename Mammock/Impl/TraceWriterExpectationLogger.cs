using System.Diagnostics;
using Castle.DynamicProxy;
using Mammock.Interfaces;
using Mammock.Utilities;

namespace Mammock.Impl
{
    /// <summary>
    /// Write rhino mocks log info to the trace
    /// </summary>
    public class TraceWriterExpectationLogger : IExpectationLogger
    {
        /// <summary>
        /// The _log recorded.
        /// </summary>
        private readonly bool _logRecorded = true;

        /// <summary>
        /// The _log replayed.
        /// </summary>
        private readonly bool _logReplayed = true;

        /// <summary>
        /// The _log unexpected.
        /// </summary>
        private readonly bool _logUnexpected = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceWriterExpectationLogger"/> class.
        /// </summary>
        public TraceWriterExpectationLogger()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceWriterExpectationLogger"/> class.
        /// </summary>
        /// <param name="logRecorded">
        /// if set to <c>true</c> [log recorded].
        /// </param>
        /// <param name="logReplayed">
        /// if set to <c>true</c> [log replayed].
        /// </param>
        /// <param name="logUnexpected">
        /// if set to <c>true</c> [log unexpected].
        /// </param>
        public TraceWriterExpectationLogger(bool logRecorded, bool logReplayed, bool logUnexpected)
        {
            _logRecorded = logRecorded;
            _logReplayed = logReplayed;
            _logUnexpected = logUnexpected;
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
            Trace.WriteLine(message);
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
            if (_logRecorded)
            {
                string methodCall =
                    MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
                Trace.WriteLine(string.Format("Recorded expectation: {0}", methodCall));
            }
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
            if (_logReplayed)
            {
                string methodCall =
                    MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
                Trace.WriteLine(string.Format("Replayed expectation: {0}", methodCall));
            }
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
            if (_logUnexpected)
            {
                string methodCall =
                    MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
                Trace.WriteLine(string.Format("{1}: {0}", methodCall, message));
            }
        }

        #endregion
    }
}