namespace Mammock.Impl.InvocationSpecifications
{
    /// <summary>
    /// The i specification.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// The is satisfied by.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The is satisfied by.
        /// </returns>
        bool IsSatisfiedBy(T item);
    }
}