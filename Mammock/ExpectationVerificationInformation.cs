using System.Collections.Generic;
using Mammock.Interfaces;

namespace Mammock
{
    /// <summary>
    /// The expectation verification information.
    /// </summary>
    internal class ExpectationVerificationInformation
    {
        /// <summary>
        /// Gets or sets Expected.
        /// </summary>
        public IExpectation Expected { get; set; }

        /// <summary>
        /// Gets or sets ArgumentsForAllCalls.
        /// </summary>
        public IList<object[]> ArgumentsForAllCalls { get; set; }
    }
}