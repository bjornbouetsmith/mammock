namespace Mammock.Constraints
{
    /// <summary>
    /// Provides a dummy field to pass as out or ref argument.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class OutRefArgDummy<T>
    {
        /// <summary>
        /// Dummy field to satisfy the compiler. Used for out and ref arguments.
        /// </summary>
        public T Dummy;
    }
}