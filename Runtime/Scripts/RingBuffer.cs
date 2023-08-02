using System.Collections.Generic;
using UnityEngine;

namespace AlaslTools
{
    public class RingBuffer<T>
    {
        private List<T> values;
        private int start, end;
        private int size;

        public RingBuffer(int size)
        {
            values = new List<T>(size);
            Clear();
        }

        public int Count => values.Count;
        public bool Full => size == values.Count;

        public void Clear()
        {
            values.Fill(values.Capacity, () => default(T));
            start = 0; end = 0; size = 0;
        }

        public void Push(T item)
        {
            values[end] = item;
            size = Mathf.Min(size + 1, values.Count);

            Incremnet(ref end);
            if (Full)
                Incremnet(ref start);
        }

        public bool TryPop(out T item)
        {
            if (size > 0)
            {
                size = Mathf.Max(size - 1, 0);
                Decrement(ref end);
                item = values[end];
                return true;
            }
            else
            {
                item = default;
                return false;
            }
        }

        private void Incremnet(ref int value)
        {
            value++;
            if (value == values.Count)
                value = 0;
        }

        private void Decrement(ref int value)
        {
            value--;
            if (value == -1)
                value = values.Count - 1;
        }
    }
}
