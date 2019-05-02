namespace DuckOfDoom.SightReading
{
    public static class Extensions
    {
        public static TResult Cast<T, TResult>(this T obj) where TResult : class, T
        {
            return obj as TResult;
        }
    }
}
