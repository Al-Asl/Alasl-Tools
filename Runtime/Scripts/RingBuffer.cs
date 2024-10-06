using System.Collections.Generic;

namespace AlaslTools
{
    public class RingBuffer<T> : IEnumerable<T> , IReadOnlyCollection<T>
    {
        private T[] values;
        private int start, end;
        private int size;

        public RingBuffer(int size)
        {
            values = new T[size];
            Reset();
        }

        public int Count => size;
        public int Capacity => values.Length;

        public void PushStart(T item)
        {
            values[start] = item;

            if (size != values.Length) size++;

            if (size == values.Length)
                end = start;

            Decrement(ref start);
        }

        public T PopStart()
        {
            Increment(ref start);

            if (size != 0) size--;

            return values[start];
        }

        public void PushEnd(T item)
        {
            values[end] = item;

            if (size != values.Length) size++;

            if (size == values.Length)
                start = end;

            Increment(ref end);
        }

        public T PopEnd()
        {
            Decrement(ref end);

            if (size != 0) size--;

            return values[end];
        }

        public void Reset()
        {
            start = 0;
            end = 1;
            size = 0;
        }

        private void Increment(ref int index)
        {
            index = index == values.Length - 1 ? 0 : index + 1;
        }

        private void Decrement(ref int index)
        {
            index = index == 0 ? values.Length - 1 : index - 1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < size; i++)
                yield return values[((start + 1) + i) % values.Length];
        }

        public IEnumerable<T> Reverse()
        {
            for (int i = 0; i < size; i++)
                yield return values[((end - 1) - i + values.Length) % values.Length];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}