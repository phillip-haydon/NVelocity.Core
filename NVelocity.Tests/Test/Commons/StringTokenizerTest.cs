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
	using global::Commons.Collections;
	
	using Xunit;

    
	public class StringTokenizerTest
	{
		[Fact]
		public void StringIsTokenizedWithDefaultDelimiters()
		{
			const string toTokenize = "First\tSecond\tThird";
			StringTokenizer tokenizer = new StringTokenizer(toTokenize);

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("First", tokenizer.NextToken());

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("Second", tokenizer.NextToken());

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("Third", tokenizer.NextToken());

			Assert.False(tokenizer.HasMoreTokens());
		}

		[Fact]
		public void StringIsTokenizedWithSpecifiedDelimiters()
		{
			const string toTokenize = "First,Second,Third";
			StringTokenizer tokenizer = new StringTokenizer(toTokenize, ",");

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("First", tokenizer.NextToken());

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("Second", tokenizer.NextToken());

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("Third", tokenizer.NextToken());

			Assert.False(tokenizer.HasMoreTokens());
		}

		[Fact]
		public void RepeatedStringIsTokenizedCorrectly()
		{
			const string toTokenize = "First\tFirstly\tThird";
			StringTokenizer tokenizer = new StringTokenizer(toTokenize);

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("First", tokenizer.NextToken());

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("Firstly", tokenizer.NextToken());

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("Third", tokenizer.NextToken());

			Assert.False(tokenizer.HasMoreTokens());
		}

		[Fact]
		public void ChangingDelimitersIsHandledCorrectly()
		{
			const string toTokenize = "First,more\tSecond,Third";
			StringTokenizer tokenizer = new StringTokenizer(toTokenize);

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("First,more", tokenizer.NextToken());

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("Second", tokenizer.NextToken(","));

			Assert.True(tokenizer.HasMoreTokens());
			Assert.Equal("Third", tokenizer.NextToken());

			Assert.False(tokenizer.HasMoreTokens());
		}

		[Fact]
		public void CountIsCorrect()
		{
			const string toTokenize = "First\tSecond\tThird";
			StringTokenizer tokenizer = new StringTokenizer(toTokenize);

			Assert.Equal(3, tokenizer.Count);

			tokenizer.NextToken();
			Assert.Equal(2, tokenizer.Count);

			tokenizer.NextToken();
			Assert.Equal(1, tokenizer.Count);

			string token = tokenizer.NextToken();
			// This assert assures that asking for the count does not
			// affect the tokens themselves.
			Assert.Equal("Third", token);
			Assert.Equal(0, tokenizer.Count);
		}
	}
}
