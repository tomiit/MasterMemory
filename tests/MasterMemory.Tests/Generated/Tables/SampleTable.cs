﻿using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;

namespace MasterMemory.Tests.Tables
{
   public sealed partial class SampleTable : TableBase<Sample>
   {
        readonly Func<Sample, int> primaryIndexSelector;

        readonly Sample[] secondaryIndex1;
        readonly Func<Sample, (int Id, int Age, string FirstName, string LastName)> secondaryIndex1Selector;
        readonly Sample[] secondaryIndex2;
        readonly Func<Sample, (int Id, int Age)> secondaryIndex2Selector;
        readonly Sample[] secondaryIndex3;
        readonly Func<Sample, (int Id, int Age, string FirstName)> secondaryIndex3Selector;
        readonly Sample[] secondaryIndex5;
        readonly Func<Sample, int> secondaryIndex5Selector;
        readonly Sample[] secondaryIndex6;
        readonly Func<Sample, (string FirstName, int Age)> secondaryIndex6Selector;
        readonly Sample[] secondaryIndex0;
        readonly Func<Sample, (string FirstName, string LastName)> secondaryIndex0Selector;
        readonly Sample[] secondaryIndex4;
        readonly Func<Sample, string> secondaryIndex4Selector;

        public SampleTable(Sample[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Id;
            this.secondaryIndex1Selector = x => (x.Id, x.Age, x.FirstName, x.LastName);
            this.secondaryIndex1 = CloneAndSortBy(this.secondaryIndex1Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName, string LastName)>.Default);
            this.secondaryIndex2Selector = x => (x.Id, x.Age);
            this.secondaryIndex2 = CloneAndSortBy(this.secondaryIndex2Selector, System.Collections.Generic.Comparer<(int Id, int Age)>.Default);
            this.secondaryIndex3Selector = x => (x.Id, x.Age, x.FirstName);
            this.secondaryIndex3 = CloneAndSortBy(this.secondaryIndex3Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName)>.Default);
            this.secondaryIndex5Selector = x => x.Age;
            this.secondaryIndex5 = CloneAndSortBy(this.secondaryIndex5Selector, System.Collections.Generic.Comparer<int>.Default);
            this.secondaryIndex6Selector = x => (x.FirstName, x.Age);
            this.secondaryIndex6 = CloneAndSortBy(this.secondaryIndex6Selector, System.Collections.Generic.Comparer<(string FirstName, int Age)>.Default);
            this.secondaryIndex0Selector = x => (x.FirstName, x.LastName);
            this.secondaryIndex0 = CloneAndSortBy(this.secondaryIndex0Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default);
            this.secondaryIndex4Selector = x => x.FirstName;
            this.secondaryIndex4 = CloneAndSortBy(this.secondaryIndex4Selector, System.Collections.Generic.Comparer<string>.Default);
        }

        public RangeView<Sample> SortByIdAndAgeAndFirstNameAndLastName => new RangeView<Sample>(secondaryIndex1, 0, secondaryIndex1.Length, true);
        public RangeView<Sample> SortByIdAndAge => new RangeView<Sample>(secondaryIndex2, 0, secondaryIndex2.Length, true);
        public RangeView<Sample> SortByIdAndAgeAndFirstName => new RangeView<Sample>(secondaryIndex3, 0, secondaryIndex3.Length, true);
        public RangeView<Sample> SortByAge => new RangeView<Sample>(secondaryIndex5, 0, secondaryIndex5.Length, true);
        public RangeView<Sample> SortByFirstNameAndAge => new RangeView<Sample>(secondaryIndex6, 0, secondaryIndex6.Length, true);
        public RangeView<Sample> SortByFirstNameAndLastName => new RangeView<Sample>(secondaryIndex0, 0, secondaryIndex0.Length, true);
        public RangeView<Sample> SortByFirstName => new RangeView<Sample>(secondaryIndex4, 0, secondaryIndex4.Length, true);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public Sample FindById(int key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
				var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return default;
        }

        public Sample FindClosestById(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeById(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }

        public Sample FindByIdAndAgeAndFirstNameAndLastName((int Id, int Age, string FirstName, string LastName) key)
        {
            return FindUniqueCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName, string LastName)>.Default, key);
        }

        public Sample FindClosestByIdAndAgeAndFirstNameAndLastName((int Id, int Age, string FirstName, string LastName) key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName, string LastName)>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByIdAndAgeAndFirstNameAndLastName((int Id, int Age, string FirstName, string LastName) min, (int Id, int Age, string FirstName, string LastName) max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName, string LastName)>.Default, min, max, ascendant);
        }

        public Sample FindByIdAndAge((int Id, int Age) key)
        {
            return FindUniqueCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<(int Id, int Age)>.Default, key);
        }

        public Sample FindClosestByIdAndAge((int Id, int Age) key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<(int Id, int Age)>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByIdAndAge((int Id, int Age) min, (int Id, int Age) max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<(int Id, int Age)>.Default, min, max, ascendant);
        }

        public Sample FindByIdAndAgeAndFirstName((int Id, int Age, string FirstName) key)
        {
            return FindUniqueCore(secondaryIndex3, secondaryIndex3Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName)>.Default, key);
        }

        public Sample FindClosestByIdAndAgeAndFirstName((int Id, int Age, string FirstName) key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex3, secondaryIndex3Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName)>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByIdAndAgeAndFirstName((int Id, int Age, string FirstName) min, (int Id, int Age, string FirstName) max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex3, secondaryIndex3Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName)>.Default, min, max, ascendant);
        }

        public RangeView<Sample> FindByAge(int key)
        {
            return FindManyCore(secondaryIndex5, secondaryIndex5Selector, System.Collections.Generic.Comparer<int>.Default, key);
        }

        public RangeView<Sample> FindClosestByAge(int key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex5, secondaryIndex5Selector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByAge(int min, int max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex5, secondaryIndex5Selector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }

        public RangeView<Sample> FindByFirstNameAndAge((string FirstName, int Age) key)
        {
            return FindManyCore(secondaryIndex6, secondaryIndex6Selector, System.Collections.Generic.Comparer<(string FirstName, int Age)>.Default, key);
        }

        public RangeView<Sample> FindClosestByFirstNameAndAge((string FirstName, int Age) key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex6, secondaryIndex6Selector, System.Collections.Generic.Comparer<(string FirstName, int Age)>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByFirstNameAndAge((string FirstName, int Age) min, (string FirstName, int Age) max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex6, secondaryIndex6Selector, System.Collections.Generic.Comparer<(string FirstName, int Age)>.Default, min, max, ascendant);
        }

        public Sample FindByFirstNameAndLastName((string FirstName, string LastName) key)
        {
            return FindUniqueCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default, key);
        }

        public Sample FindClosestByFirstNameAndLastName((string FirstName, string LastName) key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByFirstNameAndLastName((string FirstName, string LastName) min, (string FirstName, string LastName) max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default, min, max, ascendant);
        }

        public RangeView<Sample> FindByFirstName(string key)
        {
            return FindManyCore(secondaryIndex4, secondaryIndex4Selector, System.Collections.Generic.Comparer<string>.Default, key);
        }

        public RangeView<Sample> FindClosestByFirstName(string key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex4, secondaryIndex4Selector, System.Collections.Generic.Comparer<string>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByFirstName(string min, string max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex4, secondaryIndex4Selector, System.Collections.Generic.Comparer<string>.Default, min, max, ascendant);
        }

    }
}