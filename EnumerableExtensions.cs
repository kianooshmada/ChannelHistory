namespace ExtensionMethods
{
    public static class EnumerableExtensions
    {
        public static ValuesTagFields MinByValue(this IEnumerable<ValuesTagFields> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Aggregate((a, b) => a.Value < b.Value ? a : b);
        }

        public static ValuesTagFields MaxByValue(this IEnumerable<ValuesTagFields> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Aggregate((a, b) => a.Value > b.Value ? a : b);
        }

        public static ValuesTagFields MinByTimeStamp(this IEnumerable<ValuesTagFields> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Aggregate((a, b) => a.TimeStamp < b.TimeStamp ? a : b);
        }

        public static ValuesTagFields MaxByTimeStamp(this IEnumerable<ValuesTagFields> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Aggregate((a, b) => a.TimeStamp > b.TimeStamp ? a : b);
        }
    }
}