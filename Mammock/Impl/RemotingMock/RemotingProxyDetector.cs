namespace Mammock.Impl.RemotingMock
{
    /// <summary>
    /// The remoting proxy detector.
    /// </summary>
    internal class RemotingProxyDetector : IRemotingProxyOperation
    {
        /// <summary>
        /// The _detected.
        /// </summary>
        private bool _detected;

        /// <summary>
        /// Gets a value indicating whether Detected.
        /// </summary>
        public bool Detected
        {
            get { return _detected; }
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
            _detected = true;
        }

        #endregion
    }
}