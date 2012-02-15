using Mammock.Impl.InvocationSpecifications;

namespace Mammock.Impl.Invocation.Specifications
{
    /// <summary>
    /// Summary for AndSpecification
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class AndSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// The left_side.
        /// </summary>
        private readonly ISpecification<T> left_side;

        /// <summary>
        /// The right_side.
        /// </summary>
        private readonly ISpecification<T> right_side;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndSpecification{T}"/> class. 
        /// The and specification.
        /// </summary>
        /// <param name="left_side">
        /// The left_side.
        /// </param>
        /// <param name="right_side">
        /// The right_side.
        /// </param>
        public AndSpecification(ISpecification<T> left_side, ISpecification<T> right_side)
        {
            this.left_side = left_side;
            this.right_side = right_side;
        }

        #region ISpecification<T> Members

        /// <summary>
        /// The is satisfied by.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The is satisfied by.
        /// </returns>
        public bool IsSatisfiedBy(T item)
        {
            return left_side.IsSatisfiedBy(item) && right_side.IsSatisfiedBy(item);
        }

        #endregion
    }
}