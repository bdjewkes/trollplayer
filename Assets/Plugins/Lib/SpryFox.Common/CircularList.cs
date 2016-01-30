using System.Collections.Generic;
using System.Collections;
using System;

namespace SpryFox.Common {
    
    // init with size limit, and new items will remove old items if over limit.
    // also has random access, unlike the Queue<T> version of this list.
    public class CircularList<T> : IEnumerable<T> {
        
        public CircularList(int capacity) {
            Assert.True(capacity > 0,
                        GetType(), " must be constructed with positive capacity, not ", 
                        capacity);
            m_storage = new T[capacity];
        }

        public int Count {
            get { return m_count; }
        }

        public void Add(T newItem) {
            if (m_count == m_storage.Length) {
                
                int newItemStorageIndex = m_storageOrigin;
                m_storageOrigin = StorageIndexFromCircular(1);
                m_storage[newItemStorageIndex] = newItem;

            } else {
                int newItemStorageIndex = StorageIndexFromCircular(m_count);
                m_storage[newItemStorageIndex] = newItem;
                ++m_count;
            }
        }

        public T Dequeue() {
            Assert.True(Count > 0,
                        "Cannot call Dequeue if no items in list");
            var dequeuedItem = m_storage[m_storageOrigin];
            m_storageOrigin = StorageIndexFromCircular(1);
            --m_count;
            return dequeuedItem;
        }

        public T this[int circularIndex] {
            get {
                // christ this assert can be expensive, even in debug buidls
                //Assert.ValueInRange(circularIndex, 0, Count, "circularIndex");

                int storageIndex = StorageIndexFromCircular(circularIndex);
                var fetchedItem = m_storage[storageIndex];
                return fetchedItem;
            }
            set {
                Assert.ValueInRange(circularIndex, 0, Count,
                                    "circularIndex");
                int storageIndex = StorageIndexFromCircular(circularIndex);
                m_storage[storageIndex] = value;
            }
        }

        public void Clear() {
            m_count = 0;
            m_storageOrigin = 0;
        }

#region IEnumerable<T>
        
        public IEnumerator<T> GetEnumerator() {
            for(int i = 0; i < m_count; ++i) {
                T item = this[i];
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
        
#endregion // IEnumerable<T>

        //////////////////////////////////////////////////

        int m_count = 0;
        int m_storageOrigin = 0;
        T[] m_storage;
        
        //////////////////////////////////////////////////

        int StorageIndexFromCircular(int circularIndex) {
            int storageIndex = (m_storageOrigin + circularIndex) % m_storage.Length;
            return storageIndex;
        }
    }
}