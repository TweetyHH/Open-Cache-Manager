// 
//  Copyright 2011  Kyle Campbell
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
using Mono.Unix;
using Gdk;

namespace ocmgtk
{


	[System.ComponentModel.ToolboxItem(true)]
	public partial class AttributeFilterWidget : Gtk.Bin
	{
		private enum AttrState { DIS, NO, YES}
		
		AttrState m_attrState = AttrState.DIS;
		string m_attribute;
		public String AttributeName
		{
			set { 
				m_attribute = value;
				m_disicon = IconManager.GetDisAttrIcon(value);
				m_noicon = IconManager.GetNAttrIcon(value);
				m_yesicon = IconManager.GetYAttrIcon(value);
				attrIcon.Pixbuf = m_disicon;
				this.TooltipText = value;
			}
			get { return m_attribute;}
		}
		
		public bool IsFiltered
		{
			get
			{
				if (m_attrState != AttributeFilterWidget.AttrState.DIS)
					return true;
				return false;
			}
		}
		
		public bool IsIncluded
		{
			get
			{
				if (m_attrState == AttributeFilterWidget.AttrState.YES)
					return true;
				return false;
			}
			set
			{
				System.Console.WriteLine("Calling set");
				if (value)
				{
					m_attrState = AttributeFilterWidget.AttrState.YES;
					attrIcon.Pixbuf = m_yesicon;
				}
				else
				{
					m_attrState = AttributeFilterWidget.AttrState.NO;
					attrIcon.Pixbuf = m_noicon;
				}
			}
		}
		
		protected virtual void OnClick (object sender, System.EventArgs e)
		{
			if (m_attrState == AttributeFilterWidget.AttrState.DIS)
			{
				m_attrState = AttributeFilterWidget.AttrState.YES;
				attrIcon.Pixbuf = m_yesicon;
			}
			else if (m_attrState == AttributeFilterWidget.AttrState.YES)
			{
				m_attrState = AttributeFilterWidget.AttrState.NO;
				attrIcon.Pixbuf = m_noicon;
			}
			else
			{
				m_attrState = AttributeFilterWidget.AttrState.DIS;
				attrIcon.Pixbuf = m_disicon;
			}
		}
		
		
		private Pixbuf m_disicon;
		private Pixbuf m_yesicon;
		private Pixbuf m_noicon;

		public AttributeFilterWidget ()
		{
			this.Build ();
		}
		
	}
}
