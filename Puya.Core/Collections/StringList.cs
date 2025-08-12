using System;
using System.Collections;
using System.Collections.Generic;

namespace Puya.Collections
{
    public class StringList : IList<string>
    {
        List<string> list;
        public StringList()
        {
            list = new List<string>();
        }
        public string this[int index]
        {
            get { return list[index]; }
            set
            {
                if (!Contains(value))
                {
                    list[index] = value;
                }
            }
        }
        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(string item)
        {
            if (!Contains(item))
            {
                list.Add(item);
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(string item)
        {
            var found = false;

            foreach (var value in list)
            {
                if (string.Equals(item, value, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(string item)
        {
            var result = -1;

            for (var i = 0; i < list.Count; i++)
            {
                if (string.Equals(item, list[i], StringComparison.OrdinalIgnoreCase))
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

        public void Insert(int index, string item)
        {
            if (!Contains(item))
            {
                list.Insert(index, item);
            }
        }

        public bool Remove(string item)
        {
            var index = IndexOf(item);

            if (index >= 0)
            {
                RemoveAt(index);

                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
