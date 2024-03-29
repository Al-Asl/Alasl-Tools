﻿using System.Collections;
using System.Collections.Generic;

namespace AlaslTools
{

    public class BiDirectionalList<T> : IEnumerable<T>
    {
        public int Count => forward.Count;

        private List<T> forward;
        private Dictionary<T, int> reverse;

        public T this[int i]
        {
            get => forward[i];
            set
            {
                reverse.Remove(forward[i]);
                forward[i] = value;
                reverse[value] = i;
            }
        }

        public int GetIndex(T value) => reverse[value];

        public bool Contains(T value) => reverse.ContainsKey(value);

        public BiDirectionalList()
        {
            forward = new List<T>();
            reverse = new Dictionary<T, int>();
        }

        public BiDirectionalList(IEnumerable<T> list) : this(new List<T>(list)) { }

        public BiDirectionalList(List<T> list)
        {
            forward = list;
            reverse = new Dictionary<T, int>(list.Count);
            for (int i = 0; i < list.Count; i++)
                reverse[list[i]] = i;
        }

        public List<T> GetList() => forward;

        public void Add(T element)
        {
            forward.Add(element);
            reverse[element] = forward.Count - 1;
        }

        public void Clear()
        {
            forward.Clear();
            reverse.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return forward.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)forward).GetEnumerator();
        }
    }
}