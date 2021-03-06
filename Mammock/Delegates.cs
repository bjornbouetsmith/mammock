namespace Mammock
{
    /// <summary>
    /// This class defines a lot of method signatures, which we will use
    /// to allow compatability on net-2.0
    /// </summary>
    public class Delegates
    {
        #region Delegates

        /// <summary>
        /// dummy
        /// </summary>
        public delegate void Action();

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        public delegate void Action<TArg0, TArg1>(TArg0 args0, TArg1 args1);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        public delegate void Action<TArg0, TArg1, TArg2>(TArg0 args0, TArg1 args1, TArg2 args2);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        public delegate void Action<TArg0, TArg1, TArg2, TArg3>(TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        public delegate void Action<TArg0, TArg1, TArg2, TArg3, TArg4>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        /// <typeparam name="TArg5">
        /// </typeparam>
        /// <summary>
        /// dummy
        /// </summary>
        public delegate void Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4, TArg5 args5);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        /// <typeparam name="TArg5">
        /// </typeparam>
        /// <typeparam name="TArg6">
        /// </typeparam>
        public delegate void Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4, TArg5 args5, TArg6 args6);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        /// <typeparam name="TArg5">
        /// </typeparam>
        /// <typeparam name="TArg6">
        /// </typeparam>
        /// <typeparam name="TArg7">
        /// </typeparam>
        public delegate void Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4, TArg5 args5, TArg6 args6, TArg7 args7);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        /// <typeparam name="TArg5">
        /// </typeparam>
        /// <typeparam name="TArg6">
        /// </typeparam>
        /// <typeparam name="TArg7">
        /// </typeparam>
        /// <typeparam name="TArg8">
        /// </typeparam>
        public delegate void Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4, TArg5 args5, TArg6 args6, TArg7 args7, 
            TArg8 args8);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        /// <typeparam name="TArg5">
        /// </typeparam>
        /// <typeparam name="TArg6">
        /// </typeparam>
        /// <typeparam name="TArg7">
        /// </typeparam>
        /// <typeparam name="TArg8">
        /// </typeparam>
        /// <typeparam name="TArg9">
        /// </typeparam>
        public delegate void Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4, TArg5 args5, TArg6 args6, TArg7 args7, 
            TArg8 args8, TArg9 args9);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        public delegate TReturnValue Function<TReturnValue>();

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        /// <typeparam name="TArg0">
        /// </typeparam>
        public delegate TReturnValue Function<TReturnValue, TArg0>(TArg0 args0);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        public delegate TReturnValue Function<TReturnValue, TArg0, TArg1>(TArg0 args0, TArg1 args1);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        public delegate TReturnValue Function<TReturnValue, TArg0, TArg1, TArg2>(TArg0 args0, TArg1 args1, TArg2 args2);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        public delegate TReturnValue Function<TReturnValue, TArg0, TArg1, TArg2, TArg3>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        public delegate TReturnValue Function<TReturnValue, TArg0, TArg1, TArg2, TArg3, TArg4>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        /// <typeparam name="TArg5">
        /// </typeparam>
        public delegate TReturnValue Function<TReturnValue, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4, TArg5 args5);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        /// <typeparam name="TArg5">
        /// </typeparam>
        /// <typeparam name="TArg6">
        /// </typeparam>
        public delegate TReturnValue Function<TReturnValue, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4, TArg5 args5, TArg6 args6);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        /// <typeparam name="TArg5">
        /// </typeparam>
        /// <typeparam name="TArg6">
        /// </typeparam>
        /// <typeparam name="TArg7">
        /// </typeparam>
        public delegate TReturnValue Function<TReturnValue, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4, TArg5 args5, TArg6 args6, TArg7 args7);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        /// <typeparam name="TArg5">
        /// </typeparam>
        /// <typeparam name="TArg6">
        /// </typeparam>
        /// <typeparam name="TArg7">
        /// </typeparam>
        /// <typeparam name="TArg8">
        /// </typeparam>
        public delegate TReturnValue Function
            <TReturnValue, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4, TArg5 args5, TArg6 args6, TArg7 args7, 
            TArg8 args8);

        /// <summary>
        /// dummy
        /// </summary>
        /// <typeparam name="TReturnValue">
        /// </typeparam>
        /// <typeparam name="TArg0">
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// </typeparam>
        /// <typeparam name="TArg5">
        /// </typeparam>
        /// <typeparam name="TArg6">
        /// </typeparam>
        /// <typeparam name="TArg7">
        /// </typeparam>
        /// <typeparam name="TArg8">
        /// </typeparam>
        /// <typeparam name="TArg9">
        /// </typeparam>
        public delegate TReturnValue Function
            <TReturnValue, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
            TArg0 args0, TArg1 args1, TArg2 args2, TArg3 args3, TArg4 args4, TArg5 args5, TArg6 args6, TArg7 args7, 
            TArg8 args8, TArg9 args9);

        #endregion
    }
}