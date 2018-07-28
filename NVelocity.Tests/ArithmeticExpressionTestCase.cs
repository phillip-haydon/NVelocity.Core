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
namespace NVelocity
{
	using System.IO;
	using App;
	using Xunit;


    /// <summary>
	/// Tests to make sure that the parser executes arithmetic expressions correctly.
	/// </summary>
	
	public class ArithmeticExpressionTestCase
	{
		private VelocityEngine velocityEngine;
		private VelocityContext context;

		public ArithmeticExpressionTestCase()
		{
			velocityEngine = new VelocityEngine();
			velocityEngine.Init();

			// Reset the context
			context = new VelocityContext();
		}

		[Fact]
		public void DivisionInAssignmentExpressionAssignsValue()
		{
			context.Put("value_10", 10);
			context.Put("value_2", 2);

			string result = Eval("#set($result = $value_10 / $value_2)\r\n$result");

			Assert.Equal("5", result);
		}

		[Fact]
		public void DivisionByZeroInAssignmentExpressionAssignsNull()
		{
			context.Put("value_1", 1);
			context.Put("value_0", 0);

			string result = Eval("#set($result = $value_1 / $value_0)\r\n$result");

			// Check that $result is null
			Assert.Equal("$result", result);
		}

		private string Eval(string template)
		{
			using (StringWriter sw = new StringWriter())
			{
				bool ok = velocityEngine.Evaluate(context, sw, "", template);

				Assert.True(ok, "Evaluation returned failure");

				return sw.ToString();
			}
		}
	}
}