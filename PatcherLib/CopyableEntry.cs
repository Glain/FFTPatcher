using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PatcherLib
{
    public interface ICopyableEntry<T>
    {
        T Copy();
    }

    public static class CopyableEntry
    {
        public static List<T> CopyList<T>(List<T> list) where T : class, ICopyableEntry<T>
        {
            if (list == null)
                return null;

            List<T> listCopy = new List<T>(list.Capacity);
            foreach (T entry in list)
            {
                T entryCopy = (entry == null) ? null : entry.Copy();
                listCopy.Add(entryCopy);
            }

            return listCopy;
        }

        public static T[] CopyArray<T>(IList<T> list) where T : class, ICopyableEntry<T>
        {
            if (list == null)
                return null;

            int count = list.Count;
            T[] arrayCopy = new T[count];
            for (int index = 0; index < count; index++)
            {
                T entryCopy = (list[index] == null) ? null : list[index].Copy();
                arrayCopy[index] = entryCopy;
            }

            return arrayCopy;
        }
    }
}
