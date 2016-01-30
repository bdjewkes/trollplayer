using System.Collections.Generic;
using System;

public static class ListExtensions {
    // http://www.gamasutra.com/blogs/WendelinReich/20131109/203841/C_Memory_Management_for_Unity_Developers_part_1_of_3.php?print=1
    public static void ReverseNonAlloc<T>(this List<T> list) {
        int count = list.Count;

        for (int i = 0; i < count / 2; i++)
        {
            T tmp = list[i];
            list[i] = list[count - i - 1];
            list[count - i - 1] = tmp;
        }
    }

    // no-alloc version of .net's SequenceEqual(this IEnumerable<T>, IEnumerable<T>).
    // order and length should be same, and equality is tested for with Equals()
    public static bool IsSequenceEqual<T>(IList<T> lhs, IList<T> rhs) where T :IEquatable<T> {
        if (lhs.Count != rhs.Count) {
            return false;
        }

        for(int i = 0; i < lhs.Count; ++i) {
            bool isUnequalElement = lhs[i].Equals(rhs[i]) == false;
            if (isUnequalElement) {
                return false;
            }
        }
        
        return true;
    }

    // no-alloc 
    // tests to see objects in list are identical instances.
    public static bool IsSequenceSameContents<T>(IList<T> lhs, IList<T> rhs) {
        if (lhs.Count != rhs.Count) {
            return false;
        }

        for(int i = 0; i < lhs.Count; ++i) {
            bool isUnequalElement = Object.ReferenceEquals(lhs[i], rhs[i]) == false;
            if (isUnequalElement) {
                return false;
            }
        }
        
        return true;
    }


    // inserts at end, removes items from beginning if overfull
    public static void FixedLengthAppend<T>(this List<T> self, T item, int fixedLen) {
        if (self.Count >= fixedLen) {
            self.RemoveAt(0);
        }
        self.Add(item);
    }

    // inserts at beginning, removes items from end if overfull
    public static void FixedLengthInsert<T>(this List<T> self, T item, int fixedLen) {
        var count = self.Count;
        if (count >= fixedLen) {
            self.RemoveAt(count - 1);
        }
        self.Insert(0, item);
    }

    public static T Last<T>(this List<T> self) {
        var count = self.Count;
        if (count <= 0) throw new IndexOutOfRangeException("Can't call Last on empty list");
        return self[self.Count - 1];
    }

}