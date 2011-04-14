// Type: System.Collections.Generic.List`1
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime;
using System.Security;

namespace System.Collections.Generic
{
    [DebuggerTypeProxy(typeof (Mscorlib_CollectionDebugView<T>))]
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public class List<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
    {
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public List();

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public List(int capacity);

        public List(IEnumerable<T> collection);

        public int Capacity { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; set; }

        #region IList Members

        int IList.Add(object item);

        [SecuritySafeCritical]
        bool IList.Contains(object item);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        void ICollection.CopyTo(Array array, int arrayIndex);

        [SecuritySafeCritical]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        int IList.IndexOf(object item);

        void IList.Insert(int index, object item);

        [SecuritySafeCritical]
        void IList.Remove(object item);

        bool IList.IsFixedSize { get; }

        bool IList.IsReadOnly { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        bool ICollection.IsSynchronized { get; }
        object ICollection.SyncRoot { get; }
        object IList.this[int index] { get; set; }

        #endregion

        #region IList<T> Members

        public void Add(T item);
        public void Clear();
        public bool Contains(T item);
        public void CopyTo(T[] array, int arrayIndex);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        IEnumerator<T> IEnumerable<T>.GetEnumerator();

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        IEnumerator IEnumerable.GetEnumerator();

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int IndexOf(T item);

        public void Insert(int index, T item);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Remove(T item);

        public void RemoveAt(int index);

        public int Count { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        get; }

        bool ICollection<T>.IsReadOnly { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        public T this[int index] { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        set; }

        #endregion

        public void AddRange(IEnumerable<T> collection);
        public ReadOnlyCollection<T> AsReadOnly();
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer);
        public int BinarySearch(T item);
        public int BinarySearch(T item, IComparer<T> comparer);

        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public void CopyTo(T[] array);

        public void CopyTo(int index, T[] array, int arrayIndex, int count);
        public bool Exists(Predicate<T> match);
        public T Find(Predicate<T> match);
        public List<T> FindAll(Predicate<T> match);
        public int FindIndex(Predicate<T> match);
        public int FindIndex(int startIndex, Predicate<T> match);
        public int FindIndex(int startIndex, int count, Predicate<T> match);
        public T FindLast(Predicate<T> match);
        public int FindLastIndex(Predicate<T> match);
        public int FindLastIndex(int startIndex, Predicate<T> match);
        public int FindLastIndex(int startIndex, int count, Predicate<T> match);
        public void ForEach(Action<T> action);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public List<T>.Enumerator GetEnumerator();

        public List<T> GetRange(int index, int count);

        public int IndexOf(T item, int index);
        public int IndexOf(T item, int index, int count);
        public void InsertRange(int index, IEnumerable<T> collection);
        public int LastIndexOf(T item);
        public int LastIndexOf(T item, int index);
        public int LastIndexOf(T item, int index, int count);

        public int RemoveAll(Predicate<T> match);
        public void RemoveRange(int index, int count);
        public void Reverse();
        public void Reverse(int index, int count);
        public void Sort();
        public void Sort(IComparer<T> comparer);
        public void Sort(int index, int count, IComparer<T> comparer);
        public void Sort(Comparison<T> comparison);
        public T[] ToArray();
        public void TrimExcess();
        public bool TrueForAll(Predicate<T> match);

        #region Nested type: Enumerator

        [Serializable]
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            #region IEnumerator<T> Members

            [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
            public void Dispose();

            public bool MoveNext();
            void IEnumerator.Reset();

            public T Current { [TargetedPatchingOptOut(
                "Performance critical to inline this type of method across NGen image boundaries")]
            get; }

            object IEnumerator.Current { get; }

            #endregion
        }

        #endregion
    }
}
