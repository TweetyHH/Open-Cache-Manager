
using System;
using System.IO;
using NUnit.Framework;

namespace ocmengine.nunit
{
	
	[TestFixture]
	public class TestParser
	{
		
		[Test]
		public void TestSingleGPX()
		{
			FileStream fs = File.Open("../src/OpenCacheManager/test/simple.gpx", FileMode.Open);
			GPXParser parser = new GPXParser();
			parser.parseGPXFile(fs);
			Assert.AreEqual(true,true);
		}
		
		[Test]
		public void TestMultiGPX()
		{
			FileStream fs = File.Open("../src/OpenCacheManager/test/multi.gpx", FileMode.Open);
			GPXParser parser = new GPXParser();
			parser.parseGPXFile(fs);
			Assert.AreEqual(true,true);
		}
	}
}
