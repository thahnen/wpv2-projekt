using System;
using System.Windows;


namespace Graphsky {
    public static class Extensions {
        /**
         *  Extends the Tuple<T, T> to unpack it to two variables
         *  
         *  @param tup          the tuple to use the extended method on
         *  @param first        where to store the first item
         *  @param second       where to store the second item
         */
        public static void Unpack<T>(this Tuple<T, T> tup, out T first, out T second) {
            first = tup.Item1;
            second = tup.Item2;
        }


        /**
         *  Extends the Tuple<T, T> to return a Point from values
         *  
         *  @param tup          the tuple to use the extended method on
         *  @return             point if values are convertable to double, null otherwise
         */
        public static Point? ToPoint<T>(this Tuple<T, T> tup) {
            try {
                return new Point(
                    Convert.ToDouble(tup.Item1),
                    Convert.ToDouble(tup.Item2)
                );
            } catch (Exception) {
                return null;
            }
        }
    }
}
