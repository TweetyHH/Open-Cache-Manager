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

namespace TestDriver
{
	class MainClass
	{
		
		/// <summary>
		/// MonoDevelops's NUnit integration in Ubuntu Jaunty won't attach to the debugger,
		/// so the point of this class is to provide an app that can run the tests
		/// through the debugger.
		/// </summary>
		/// <param name="args">
		/// <see cref="System.String"/>
		/// </param>
		public static void Main(string[] args)
		{
			ocmengine.test.CacheStoreTest test = new ocmengine.test.CacheStoreTest();
			ocmengine.test.UtilitiesTest test2 = new ocmengine.test.UtilitiesTest();
			test.TestParseGPX();
			test2.TestNegativeDecimalDegrees();
			test2.TestBearing();
			System.Console.WriteLine("Done");
		}
	}
}