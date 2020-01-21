using System;


namespace Graphsky {
    public static class Extensions {
        /**
         *  Extends the Tuple<int, int> to unpack it to two variables
         *  
         *  @param tup          the tuple to use the extended method on
         *  @param first        where to store the first item
         *  @param second       where to store the second item
         */
        public static void Unpack<T>(this Tuple<T, T> tup, out T first, out T second) {
            first = tup.Item1;
            second = tup.Item2;
        }
    }
}
