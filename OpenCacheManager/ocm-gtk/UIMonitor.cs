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
using Gdk;
using Mono.Unix;
using Gtk;
using ocmengine;

namespace ocmgtk
{
	
	
	public class UIMonitor
	{
		private static UIMonitor m_instance = null;
		private GeoCachePane m_pane = null;
		private Statusbar m_statusbar = null;
		private static Pixbuf TRADICON = new Pixbuf("../icons/scalable/hicolor/traditional.svg", 24, 24);
		private static Pixbuf LETTERICON = new Pixbuf("../icons/scalable/hicolor/letterbox.svg", 24, 24);
		private static Pixbuf MULTIICON = new Pixbuf("../icons/scalable/hicolor/multi.svg", 24, 24);
		private static Pixbuf MYSTERYICON = new Pixbuf("../icons/scalable/hicolor/unknown.svg", 24, 24);
		private static Pixbuf OTHERICON = new Pixbuf("../icons/scalable/hicolor/other.svg", 24, 24);
		private static Pixbuf FOUNDICON = new Pixbuf("../icons/scalable/hicolor/found.svg", 24, 24);
		private static Pixbuf EARTHICON = new Pixbuf("../icons/scalable/hicolor/earth.svg", 24, 24);
		private double m_home_lat = 45.37598;
		private double m_home_lon = -75.64923;
	
		public GeoCachePane GeoPane
		{
			get { return m_pane;}
			set { m_pane = value;}
		}
		
		public Statusbar StatusBar
		{
			get {return m_statusbar;}
			set {m_statusbar = value;}
		}
		
		public double HomeLat
		{
			get {return m_home_lat;}
		}
		
		public double HomeLon
		{
			get {return m_home_lon;}
		}
			
		
		private UIMonitor()
		{
		}
		
		public static UIMonitor getInstance()
		{
			lock (typeof(UIMonitor))
			{
				if (null == m_instance)
					m_instance = new UIMonitor();
				return m_instance;
			}
		}
		
		public void setSelectedCache(Geocache cache)
		{
			m_pane.setCache(cache);
		}
		
		public void setCacheCountStatus(int shown)
		{
			Engine eng = Engine.getInstance();
			m_statusbar.Push(m_statusbar.GetContextId("count"), 
			                                          String.Format(Catalog.GetString("Showing {0} of {1} caches"),
			                                          shown, eng.CacheCount));
		}
		
		public static Pixbuf getSmallCacheIcon(Geocache.CacheType type)
		{
			switch (type)
			{
				case Geocache.CacheType.FOUND:
					return UIMonitor.FOUNDICON;
				case Geocache.CacheType.TRADITIONAL:
					return UIMonitor.TRADICON;
				case Geocache.CacheType.MYSTERY:
					return UIMonitor.MYSTERYICON;
				case Geocache.CacheType.MULTI:
					return UIMonitor.MULTIICON;
				case Geocache.CacheType.LETTERBOX:
					return UIMonitor.LETTERICON;
				case Geocache.CacheType.EARTH:
					return UIMonitor.EARTHICON;
				default:
					return UIMonitor.OTHERICON;
			}
		}
		
	}
}
