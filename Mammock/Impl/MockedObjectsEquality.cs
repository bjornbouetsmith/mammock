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

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Mammock.Interfaces;

namespace Mammock.Impl
{
    /// <summary>
    /// This class will provide hash code for hashtables without needing
    /// to call the GetHashCode() on the object, which may very well be mocked.
    /// This class has no state so it is a singelton to avoid creating a lot of objects 
    /// that does the exact same thing. See flyweight patterns.
    /// </summary>
    public class MockedObjectsEquality : IComparer, IEqualityComparer, IEqualityComparer<object>
    {
        /// <summary>
        /// The instance.
        /// </summary>
        private static readonly MockedObjectsEquality instance = new MockedObjectsEquality();

        /// <summary>
        /// The base hashcode.
        /// </summary>
        private static int baseHashcode;

        /// <summary>
        /// Prevents a default instance of the <see cref="MockedObjectsEquality"/> class from being created.
        /// </summary>
        private MockedObjectsEquality()
        {
        }

        /// <summary>
        /// The next hash code value for a mock object.
        /// This is safe for multi threading.
        /// </summary>
        public static int NextHashCode
        {
            get { return Interlocked.Increment(ref baseHashcode); }
        }

        /// <summary>
        /// The sole instance of <see cref="MockedObjectsEquality "/>
        /// </summary>
        public static MockedObjectsEquality Instance
        {
            get { return instance; }
        }

        #region IComparer Members

        /// <summary>
        /// Compares two instances of mocked objects
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The compare.
        /// </returns>
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return 1;
            if (y == null)
                return -1;

            IMockedObject one = MockRepository.GetMockedObjectOrNull(x);
            IMockedObject two = MockRepository.GetMockedObjectOrNull(y);
            if (one == null && two == null)
                return -2; // both of them are probably transperant proxies
            if (one == null)
                return 1;
            if (two == null)
                return -1;

            return one.ProxyHash - two.ProxyHash;
        }

        #endregion

        #region IEqualityComparer Members

        /// <summary>
        /// Get the hash code for a proxy object without calling GetHashCode()
        /// on the object.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The get hash code.
        /// </returns>
        public int GetHashCode(object obj)
        {
            IMockedObject mockedObject = MockRepository.GetMockedObjectOrNull(obj);
            if (mockedObject == null)
                return obj.GetHashCode();
            return mockedObject.ProxyHash;
        }

        /// <summary>
        /// Compare two mocked objects
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The equals.
        /// </returns>
        public new bool Equals(object x, object y)
        {
            return Compare(x, y) == 0;
        }

        #endregion

        #region IEqualityComparer<object> Members

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The equals.
        /// </returns>
        bool System.Collections.Generic.IEqualityComparer<object>.Equals(object x, object y)
        {
            return Compare(x, y) == 0;
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The get hash code.
        /// </returns>
        int System.Collections.Generic.IEqualityComparer<object>.GetHashCode(object obj)
        {
            return this.GetHashCode(obj);
        }

        #endregion
    }
}