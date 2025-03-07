﻿using MasterMemory.Internal;
using System;
using System.Collections.Generic;

namespace MasterMemory
{
    public abstract class TableBase<TElement>
    {
        protected readonly TElement[] data;

        // Common Properties
        public int Count => data.Length;
        public RangeView<TElement> All => new RangeView<TElement>(data, 0, data.Length, true);
        public RangeView<TElement> AllReverse => new RangeView<TElement>(data, 0, data.Length, false);
        public TElement[] GetRawDataUnsafe() => data;

        public TableBase(TElement[] sortedData)
        {
            this.data = sortedData;
        }

        // Util

        protected TElement[] CloneAndSortBy<TKey>(Func<TElement, TKey> indexSelector, IComparer<TKey> comparer)
        {
            var array = new TElement[data.Length];
            var sortSource = new TKey[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                array[i] = data[i];
                sortSource[i] = indexSelector(data[i]);
            }

            Array.Sort(sortSource, array, 0, array.Length, comparer);
            return array;
        }

        // Unique

        static protected TElement FindUniqueCore<TKey>(TElement[] indexArray, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, TKey key)
        {
            var index = BinarySearch.FindFirst(indexArray, key, keySelector, comparer);
            return (index != -1) ? indexArray[index] : default(TElement);
        }

        // Optimize for IntKey
        static protected TElement FindUniqueCoreInt(TElement[] indexArray, Func<TElement, int> keySelector, IComparer<int> _, int key)
        {
            var index = BinarySearch.FindFirstIntKey(indexArray, key, keySelector);
            return (index != -1) ? indexArray[index] : default(TElement);
        }

        static protected TElement FindUniqueClosestCore<TKey>(TElement[] indexArray, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, TKey key, bool selectLower)
        {
            var index = BinarySearch.FindClosest(indexArray, 0, indexArray.Length, key, keySelector, comparer, selectLower);
            return (index != -1) ? indexArray[index] : default(TElement);
        }

        static protected RangeView<TElement> FindUniqueRangeCore<TKey>(TElement[] indexArray, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, TKey min, TKey max, bool ascendant)
        {
            var lo = BinarySearch.FindClosest(indexArray, 0, indexArray.Length, min, keySelector, comparer, false);
            var hi = BinarySearch.FindClosest(indexArray, 0, indexArray.Length, max, keySelector, comparer, true);
            return new RangeView<TElement>(indexArray, lo, hi, ascendant);
        }

        // Many

        static protected RangeView<TElement> FindManyCore<TKey>(TElement[] indexKeys, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, TKey key)
        {
            var lo = BinarySearch.LowerBound(indexKeys, 0, indexKeys.Length, key, keySelector, comparer);
            if (lo == -1) return RangeView<TElement>.Empty;

            var hi = BinarySearch.UpperBound(indexKeys, 0, indexKeys.Length, key, keySelector, comparer);
            if (hi == -1) return RangeView<TElement>.Empty;

            return new RangeView<TElement>(indexKeys, lo, hi, true);
        }

        static protected RangeView<TElement> FindManyClosestCore<TKey>(TElement[] indexArray, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, TKey key, bool selectLower)
        {
            var closest = BinarySearch.FindClosest(indexArray, 0, indexArray.Length, key, keySelector, comparer, selectLower);
            if (closest == -1) return RangeView<TElement>.Empty;

            return FindManyCore(indexArray, keySelector, comparer, keySelector(indexArray[closest]));
        }

        static protected RangeView<TElement> FindManyRangeCore<TKey>(TElement[] indexArray, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, TKey min, TKey max, bool ascendant)
        {
            var lo = FindManyClosestCore(indexArray, keySelector, comparer, min, false).FirstIndex;
            var hi = FindManyClosestCore(indexArray, keySelector, comparer, min, true).LastIndex;
            return new RangeView<TElement>(indexArray, lo, hi, ascendant);
        }
    }
}
