using System.Collections.Generic;
using System.Linq;

namespace VerticalMade.NumbersToWords
{
    internal static class CoreExtensions
    {
        /// <summary>
        /// Projects the given enumerable and appends an index to each
        /// value. Useful for inline ordering for joined datasets.
        /// </summary>
        public static IEnumerable<WithIndexResult<T>> WithIndex<T>(this IEnumerable<T> source)
            => source
                .Select((a, i) => new WithIndexResult<T>(a, i));
        
        public class WithIndexResult<T>
        {
            public readonly T Value;
            public readonly int Index;

            public WithIndexResult(T value, int index)
            {
                Value = value;
                Index = index;
            }

            /// <summary>
            /// Allows the following syntax:
            ///     foreach (var (item, index) in enumerable.WithIndex())
            /// </summary>
            public void Deconstruct(out T value, out int index)
            {
                value = Value;
                index = Index;
            }
        }
    }
}