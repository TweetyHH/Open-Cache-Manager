// 
//  Copyright 2010  Kyle Campbell
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using NUnit.Framework;
using ocmengine;

namespace ocmengine.test
{
	[TestFixture]
	public class UtilitiesTest
	{
		[Test]
		public void TestPositiveDecimalDegrees()
		{
			Assert.AreEqual(new DegreeMinutes(45, 25.44702) , Utilities.convertDDtoDM(45.424117));
		}
		
		[Test]
		public void TestNegativeDecimalDegrees()
		{
			Assert.AreEqual(new DegreeMinutes(-45, 25.44702) , Utilities.convertDDtoDM(-45.424117));
		}
		
		[Test]
		public void TestBearing()
		{
			Assert.AreEqual(127, Utilities.calculateBearing(53.14722, 52.204444, -1.84944, 0.140556));
		}
	}
}
