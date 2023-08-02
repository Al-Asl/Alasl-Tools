using System.Collections.Generic;
using System;

namespace AlaslTools
{
    public static class CollectionExtensions
    {
        public static int BinarySearch<T>(this T[] array, T value) where T : IComparable<T>
        {
            if (array.Length == 0)
                return -1;
            return BinarySearch(array, value, 0, array.Length);
        }
        private static int BinarySearch<T>(T[] array, T value, int start, int end) where T : IComparable<T>
        {
            int size = end - start;

            int midIndex = start + size / 2;
            int res = value.CompareTo(array[midIndex]);

            if (res == 0) return midIndex;

            if (size == 1)
                return -1;

            if (res > 0)
                return BinarySearch(array, value, midIndex, end);
            else
                return BinarySearch(array, value, start, midIndex);
        }

        public static void Swap<T>(this T[] list, int a, int b)
        {
            var temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }
        public static void Fill<T>(this T[] array, int length, Func<T> constructor)
        {
            for (int i = 0; i < length; i++)
                array[i] = constructor();
        }
        public static void Fill<T>(this T[] array, Func<T> constructor)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = constructor();
        }
        public static void Swap<T>(this List<T> list, int a, int b)
        {
            var temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }
        public static void Fill<T>(this List<T> list, int count, Func<T> constructor)
        {
            list.Clear();
            for (int i = 0; i < count; i++)
                list.Add(constructor());
        }
        public static void Fill<T>(this List<T> list, int count) where T : new()
        {
            list.Clear();
            for (int i = 0; i < count; i++)
                list.Add(new T());
        }

        public static void SortedInsertion<T>(this List<T> list, T value) where T : IComparable<T>
        {
            if (list.Count == 0 || value.CompareTo(list[list.Count - 1]) >= 0)
            {
                list.Add(value);
                return;
            }
            if (value.CompareTo(list[0]) <= 0)
            {
                list.Insert(0, value);
                return;
            }

            SortedInsertion(list, value, 0, list.Count);
        }

        private static void SortedInsertion<T>(List<T> list, T value, int start, int end) where T : IComparable<T>
        {
            int size = end - start;

            int midIndex = start + size / 2;
            int res = value.CompareTo(list[midIndex]);

            if (res == 0)
                list.Insert(midIndex, value);
            else if (size == 1)
                list.Insert(end, value);
            else if (res > 0)
                SortedInsertion(list, value, midIndex, end);
            else
                SortedInsertion(list, value, start, midIndex);
        }
    }
}