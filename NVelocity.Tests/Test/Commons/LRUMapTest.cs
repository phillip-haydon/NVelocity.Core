// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
namespace NVelocity.Test.Commons
{
	using System;
	using System.Collections;
	using global::Commons.Collections;
	using Xunit;


    /// <summary>
	/// Tests for Commons.Collections.KeyedList
	/// </summary>
	
	public class LRUMapTest
	{
		[Fact]
		public void Test()
		{
			LRUMap map = new LRUMap(5);
			AssertAddedFirst(map, "One", 1);
			AssertAddedFirst(map, "Two", 2);
			AssertAddedFirst(map, "Three", 3);
			AssertAddedFirst(map, "Four", 4);
			AssertAddedFirst(map, "Five", 5);
			AssertAddedFirst(map, "Six", 6);
			Assert.True(!map.Contains("One"));
			AssertAddedFirst(map, "Seven", 7);
			Assert.True(!map.Contains("Two"));
			AssertAddedFirst(map, "Eight", 8);
			Assert.True(!map.Contains("Three"));
			AssertAddedFirst(map, "Nine", 9);
			Assert.True(!map.Contains("Four"));
			AssertAddedFirst(map, "Ten", 10);
			Assert.True(!map.Contains("Five"));

			map.Remove("Eight");
			Assert.Equal(4, map.Count);
			map.Add("One", 1);
			Assert.Equal(5, map.Count);
			Assert.True(map.Contains("One"));
			Assert.True(map.Contains("Six"));
			Assert.True(map.Contains("Seven"));
			Assert.True(map.Contains("Nine"));
			Assert.True(map.Contains("Ten"));
			Assert.Equal("Six", ((ArrayList) map.Keys)[map.Count - 1]);
			Assert.Equal("One", ((ArrayList) map.Keys)[0]);

			AssertGetIsMostRecent(map, "Six", 6);
			AssertGetIsMostRecent(map, "Nine", 9);
			AssertGetIsMostRecent(map, "Seven", 7);
			AssertGetIsMostRecent(map, "Ten", 10);
			AssertGetIsMostRecent(map, "One", 1);
			Assert.Equal("Six", ((ArrayList) map.Keys)[map.Count - 1]);

			AssertSetIsMostRecent(map, "One", "Uno");
			AssertSetIsMostRecent(map, "Two", "Dos");
			Assert.Equal(5, map.Count);
		}

		private void AssertAddedFirst(LRUMap map, Object key, Object value)
		{
			map.Add(key, value);
			Assert.Equal(key, ((ArrayList) map.Keys)[0]);
			Assert.Equal(value, ((ArrayList) map.Values)[0]);
			Assert.True(map.Count <= map.MaxSize);
		}

		private void AssertSetIsMostRecent(LRUMap map, Object key, Object value)
		{
			map[key] = value;
			Assert.Equal(key, ((ArrayList) map.Keys)[0]);
			Assert.Equal(value, ((ArrayList) map.Values)[0]);
			Assert.True(map.Count <= map.MaxSize);
		}

		private void AssertGetIsMostRecent(LRUMap map, Object key, Object value)
		{
			Object o = map[key];
			Assert.Equal(value, o);
			Assert.Equal(key, ((ArrayList) map.Keys)[0]);
			Assert.Equal(value, ((ArrayList) map.Values)[0]);
		}
	}
}