namespace GameboyColourDecolouriser
{
    public static class ImageProcessingExtensions
    {
        //https://stackoverflow.com/a/13091986
        /// <summary>
        /// Checks a <see cref="IEnumerable{T}"/> contains all elements of another <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="IEnumerable{T}"/> type.</typeparam>
        /// <param name="containingList">The containing list we will look for the <paramref name="lookupList"/>.</param>
        /// <param name="lookupList">The list to look for in the <paramref name="containingList"/>.</param>
        /// <returns>True if <paramref name="containingList"/> contains all <paramref name="lookupList"/>, otherwise false.</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> containingList, IEnumerable<T> lookupList)
        {
            return !lookupList.Except(containingList).Any();
        }

        // https://docs.microsoft.com/en-us/archive/msdn-magazine/2017/june/essential-net-custom-iterators-with-yield
        /// <summary>
        /// An iterator to iterate through a <see cref="T:[,]"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="T:[,]"/>.</typeparam>
        /// <param name="twoDimensionalArray">The <see cref="T:[,]"/> to iterate through.</param>
        /// <returns>Each element of the <see cref="T:[,]"/> in turn when being iterated.</returns>
        public static IEnumerable<T> ToIEnumerable<T>(this T[,] twoDimensionalArray)
        {
            for (int i = 0; i < twoDimensionalArray.GetLength(0); i++)
            {
                for (int j = 0; j < twoDimensionalArray.GetLength(1); j++)
                {
                    yield return twoDimensionalArray[i, j];
                }
            }
        }

        /// <summary>
        /// An iterator to iterate through a <see cref="T:[,]"/> while also returning coordinates.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="T:[,]"/>.</typeparam>
        /// <param name="twoDimensionalArray">The <see cref="T:[,]"/> to iterate through.</param>
        /// <returns>Coordinated of each returned item in the <see cref="T:[,]"/> and each element of the <see cref="T:[,]"/> in turn when being iterated.</returns>
        public static IEnumerable<((int x, int y) coordinates, T item)> ToIEnumerableWithCoords<T>(this T[,] twoDimensionalArray)
        {
            for (int i = 0; i < twoDimensionalArray.GetLength(0); i++)
            {
                for (int j = 0; j < twoDimensionalArray.GetLength(1); j++)
                {
                    yield return ((i, j), twoDimensionalArray[i, j]);
                }
            }
        }
    }
}
