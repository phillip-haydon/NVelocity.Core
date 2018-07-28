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
namespace NVelocity.Test
{
	using System;
	using System.Collections;
	using System.Globalization;
	using System.IO;
	using App;
	using Xunit;

    /// <summary>
	/// Test Velocity processing
	/// </summary>
	public class VelocityTest
	{
		[Fact]
		public void MathOperations()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			VelocityContext context = new VelocityContext();

			context.Put("fval", 1.2f);
			context.Put("dval", 5.3);

			Velocity.Init();

			StringWriter sw = new StringWriter();

			Assert.True(Velocity.Evaluate(context, sw, string.Empty, "#set($total = 1 + 1)\r\n$total"));
			Assert.Equal("2", sw.GetStringBuilder().ToString());

			sw = new StringWriter();

			Assert.True(Velocity.Evaluate(context, sw, string.Empty, "#set($total = $fval + $fval)\r\n$total"));
			Assert.Equal("2.4", sw.GetStringBuilder().ToString());

			sw = new StringWriter();

			Assert.True(Velocity.Evaluate(context, sw, string.Empty, "#set($total = $dval + $dval)\r\n$total"));
			Assert.Equal("10.6", sw.GetStringBuilder().ToString());

			sw = new StringWriter();

			Assert.True(Velocity.Evaluate(context, sw, string.Empty, "#set($total = 1 + $dval)\r\n$total"));
			Assert.Equal("6.3", sw.GetStringBuilder().ToString());

			sw = new StringWriter();

			Assert.True(Velocity.Evaluate(context, sw, string.Empty, "#set($total = $fval * $dval)\r\n$total"));
			Assert.Equal("6.36000025272369", sw.GetStringBuilder().ToString());

			sw = new StringWriter();

			Assert.True(Velocity.Evaluate(context, sw, string.Empty, "#set($total = $fval - $dval)\r\n$total"));
			Assert.Equal("-4.09999995231628", sw.GetStringBuilder().ToString());

			sw = new StringWriter();

			Assert.True(Velocity.Evaluate(context, sw, string.Empty, "#set($total = $fval % $dval)\r\n$total"));
			Assert.Equal("1.20000004768372", sw.GetStringBuilder().ToString());

			sw = new StringWriter();

			Assert.True(Velocity.Evaluate(context, sw, string.Empty, "#set($total = $fval / $dval)\r\n$total"));
			Assert.Equal("0.22641510333655", sw.GetStringBuilder().ToString());
		}

		[Fact]
		public void Test_Evaluate()
		{
			VelocityContext c = new VelocityContext();
			c.Put("key", "value");
			c.Put("firstName", "Cort");
			c.Put("lastName", "Schaefer");
			Hashtable h = new Hashtable();
			h.Add("foo", "bar");
			c.Put("hashtable", h);
			c.Put("EnumData", typeof(EnumData));
			c.Put("enumValue", EnumData.Value2);

			AddressData address = new AddressData();
			address.Address1 = "9339 Grand Teton Drive";
			address.Address2 = "Office in the back";
			c.Put("address", address);

			ContactData contact = new ContactData();
			contact.Name = "Cort";
			contact.Address = address;
			c.Put("contact", contact);

			// test simple objects (no nesting)
			StringWriter sw = new StringWriter();
			bool ok = Velocity.Evaluate(c, sw, string.Empty, "$firstName is my first name, my last name is $lastName");
			Assert.True(ok, "Evaluation returned failure");
			String s = sw.ToString();
			Assert.Equal("Cort is my first name, my last name is Schaefer", s);

			// test nested object
			sw = new StringWriter();
			String template = "These are the individual properties:\naddr1=9339 Grand Teton Drive\naddr2=Office in the back";
			ok = Velocity.Evaluate(c, sw, string.Empty, template);
			Assert.True(ok, "Evaluation returned failure");
			s = sw.ToString();
			Assert.False(String.Empty.Equals(s), "test nested object");

			// test hashtable
			sw = new StringWriter();
			template = "Hashtable lookup: foo=$hashtable.foo";
			ok = Velocity.Evaluate(c, sw, string.Empty, template);
			Assert.True(ok, "Evaluation returned failure");
			s = sw.ToString();
			Assert.Equal("Hashtable lookup: foo=bar", s);

			// test nested properties
			//    	    sw = new StringWriter();
			//	    template = "These are the nested properties:\naddr1=$contact.Address.Address1\naddr2=$contact.Address.Address2";
			//	    ok = Velocity.Evaluate(c, sw, string.Empty, template);
			//	    Assert("Evaluation returned failure", ok);
			//	    s = sw.ToString();
			//	    Assert("test nested properties", s.Equals("These are the nested properties:\naddr1=9339 Grand Teton Drive\naddr2=Office in the back"));

			// test key not found in context
			sw = new StringWriter();
			template = "$!NOT_IN_CONTEXT";
			ok = Velocity.Evaluate(c, sw, string.Empty, template);
			Assert.True(ok, "Evaluation returned failure");
			s = sw.ToString();
			Assert.Equal(String.Empty, s);

			sw = new StringWriter();
			ok = Velocity.Evaluate(c, sw, string.Empty, "#if($enumValue == \"Value2\")equal#end");
			Assert.True(ok, "Evaluation returned failure");
			s = sw.ToString();
			Assert.Equal("equal", s);

			sw = new StringWriter();
			ok = Velocity.Evaluate(c, sw, string.Empty, "#if($enumValue == $EnumData.Value2)equal#end");
			Assert.True(ok, "Evaluation returned failure");
			s = sw.ToString();
			Assert.Equal("equal", s);

			// test nested properties where property not found
			//	    sw = new StringWriter();
			//	    template = "These are the non-existent nested properties:\naddr1=$contact.Address.Address1.Foo\naddr2=$contact.Bar.Address.Address2";
			//	    ok = Velocity.Evaluate(c, sw, string.Empty, template);
			//	    Assert("Evaluation returned failure", ok);
			//	    s = sw.ToString();
			//	    Assert("test nested properties where property not found", s.Equals("These are the non-existent nested properties:\naddr1=\naddr2="));
		}

		// inner classes to support tests --------------------------

		public class ContactData
		{
			private String name = String.Empty;
			private AddressData address = new AddressData();

			public String Name
			{
				get { return name; }
				set { name = value; }
			}

			public AddressData Address
			{
				get { return address; }
				set { address = value; }
			}
		}

		public class AddressData
		{
			private String address1 = String.Empty;
			private String address2 = String.Empty;

			public String Address1
			{
				get { return address1; }
				set { address1 = value; }
			}

			public String Address2
			{
				get { return address2; }
				set { address2 = value; }
			}
		}

		public enum EnumData
		{
			Value1,
			Value2,
			Value3
		}
	}
}