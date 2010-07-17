using System.Timers;
using org.freedesktop.DBus;
using NDesk.DBus;

namespace ocmgtk
{
	
	public class GPS
	{
		private Connection DbusConnection;
		public Gpsd gps;
		
		public double lastLat = 0;
		public double lastLon = 0;
		
		public GPS ()
		{
			DbusConnection = Bus.System;
			gps = DbusConnection.GetObject<Gpsd> ("org.gpsd", new ObjectPath ("/org/gpsd"));
			gps.fix += HandleGpsfix;
			/*Timer pollInterval = new Timer(60000);
			pollInterval.AutoReset = true;
			pollInterval.Elapsed += HandlePollIntervalElapsed;*/
		}
		
		private void GetCoord ()
		{
		
		}

		void HandleGpsfix (GPSFix fix)
		{
			System.Console.WriteLine("GPS FIX:" + fix.latitude + " " + fix.longitude);
		}

		void HandlePollIntervalElapsed (object sender, ElapsedEventArgs e)
		{
			
		}		
	}

	public struct GPSFix
	{
		public double time;
		public int mode;
		public double ept;
		public double latitude;
		public double longitude;
		public double eph;
		public double altitude;
		public double epv;
		public double track;
		public double epd;
		public double speed;
		public double eps;
		public double climb;
		public double epc;
	}

	public delegate void GPSPositionChangedHandler (GPSFix fix);

	[Interface("org.gpsd")]
	public interface Gpsd
	{
		event GPSPositionChangedHandler fix;
	}
}
