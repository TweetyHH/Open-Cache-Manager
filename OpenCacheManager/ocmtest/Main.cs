using System;

namespace ocmtest
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			ocmengine.nunit.TestParser ptest = new ocmengine.nunit.TestParser();
			ptest.TestSingleGPX();
			ptest.TestMultiGPX();
			System.Console.WriteLine("DONE!");
		}
	}
}