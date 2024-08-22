using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Common
{
    public static class ListExtension
    {
        public static void RemoveList<T>(this List<T> array, List<T> removeList)
        {
            for (int i = 0; i < removeList.Count; i++)
            {
                if (array.Contains(removeList[i]))

                {
                    array.RemoveAt(i);
                }
            }
        }

        public static bool CompareTo<T>(this List<T> list, List<T> compareTo)
        {
            if (list.Except(compareTo).Any())
                return false;
            if (compareTo.Except(list).Any())
                return false;
            return true;
        }

        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
        {
            List<T> tempList = new List<T>(list);
            T item = list[oldIndex];
            tempList.RemoveAt(oldIndex);
            list.Clear();
            int j = 0;
            for (int i = 0; i < tempList.Count + 1; i++)
            {
                list.Add(i == newIndex ? item : tempList[j]);
                j += i == newIndex ? 0 : 1;
            }
        }

        public static bool IsChildOfList<T>(this List<T> list, List<T> compareTo)
        {
            List<T> list1 = new List<T>(list);
            List<T> list2 = new List<T>(compareTo);
            if (list1.Except(list2).Any())
                return false;
            return true;
        }

        public static List<T> Intersect<T>(this List<T> list, List<T> intersectWith)
        {
            List<T> result = new List<T>();

            foreach (var element1 in list)
            {
                foreach (var element2 in intersectWith)
                {
                    if (element1.Equals(element2) && !result.Contains(element1))
                    {
                        result.Add((element1));
                    }
                }
            }


            return result;
        }

        public static bool IsSameList<T>(this List<T> list, List<T> compareTo)
        {
            List<T> list1 = new List<T>(list);
            List<T> list2 = new List<T>(compareTo);
            if (!list1.Except(list2).Any() && list1.Count == list2.Count)
            {
                return true;
            }

            return false;
        }

        public static void PrintList<T>(this List<T> list)
        {
            for (var index = 0; index < list.Count; index++)
            {
                var element = list[index];
                {
                    Debug.Log("Element "+index+ ": "+ element.ToString());
                }
            }
        }
    }
}