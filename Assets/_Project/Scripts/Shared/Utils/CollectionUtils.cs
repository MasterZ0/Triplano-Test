using System.Collections.Generic;
using UnityEngine;

namespace TriplanoTest.Shared.Utils
{
    public static class CollectionUtils
    {
        public static Vector2Int[] IntSides = { Vector2Int.right, Vector2Int.down, Vector2Int.left, Vector2Int.up };
        public static Vector2Int[] IntCorners = { new Vector2Int(-1, 1), Vector2Int.one, new Vector2Int(1, -1), -Vector2Int.one };
        
        public static T[] CreateRepeatedArray<T>(int count, T element)
        {
            T[] repeated = new T[count];
        
            for(int i = 0; i < count; i++)
                repeated[i] = element;
            
            return repeated;
        }
        
        public static T GetElementAndRemove<T>(this List<T> list, int index)
        {
            T element = list[index];
            list.RemoveAt(index);
            return element;
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            bool isNull = list == null;
            if (isNull) return true;
            
            bool isEmpty = list.Count == 0;
            return isEmpty;
        }

        public static T[] MergeArrays<T>(T[] first, T[] second)
        {
            T[] mergedArray = new T[first.Length + second.Length];
            first.CopyTo(mergedArray, 0);
            second.CopyTo(mergedArray, first.Length);
            return mergedArray;
        }
    }
}