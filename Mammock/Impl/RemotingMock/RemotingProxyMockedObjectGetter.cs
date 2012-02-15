using Mammock.Interfaces;

namespace Mammock.Impl.RemotingMock
{
    /// <summary>
    /// The remoting proxy mocked object getter.
    /// </summary>
    internal class RemotingProxyMockedObjectGetter : IRemotingProxyOperation
    {
        /// <summary>
        /// The _mocked object.
        /// </summary>
        private IMockedObject _mockedObject;

        /// <summary>
        /// Gets MockedObject.
        /// </summary>
        public IMockedObject MockedObject
        {
            get { return _mockedObject; }
        }

        #region IRemotingProxyOperation Members

        /// <summary>
        /// The process.
        /// </summary>
        /// <param name="proxy">
        /// The proxy.
        /// </param>
        public void Process(RemotingProxy proxy)
        {
            _mockedObject = proxy.MockedObject;
        }

        #endregion
    }
}