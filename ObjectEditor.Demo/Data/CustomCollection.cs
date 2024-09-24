using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Demo.Data
{
    public class CustomCollection : BaseClass, IDisposable, IEnumerable<object>, ICollection<object>, IEnumerable // should be considered as a collection of objects
    {
        private List<object> innerList = new List<object>();
        public CustomCollection() { }
        public CustomCollection(bool isReadonly) { IsReadOnly = isReadonly; }
        public void Dispose() { }
        public int Count => innerList.Count;
        public bool IsReadOnly { get; set; }
        public object this[int index] { get => innerList[index]; set => innerList[index] = value; }
        public void Add(object item) => innerList.Add(item);
        public void Clear() => innerList.Clear();
        public bool Contains(object item) => innerList.Contains(item);
        public void CopyTo(object[] array, int arrayIndex) => innerList.CopyTo(array, arrayIndex);
        public IEnumerator<object> GetEnumerator() => innerList.GetEnumerator();
        public int IndexOf(object item) => innerList.IndexOf(item);
        public void Insert(int index, object item) => innerList.Insert(index, item);
        public bool Remove(object item) => innerList.Remove(item);
        public void RemoveAt(int index) => innerList.RemoveAt(index);
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => innerList.GetEnumerator();
    }
}
