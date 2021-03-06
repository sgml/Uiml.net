/*
  	 Uiml.Net: a Uiml.Net renderer (http://research.edm.uhasselt.be/kris/research/uiml.net/)
   
	 Copyright (C) 2003  Kris Luyten (kris.luyten@uhasselt.be)
	                     Expertise Centre for Digital Media (http://www.edm.uhasselt.be)
								Hasselt University

	This program is free software; you can redistribute it and/or
	modify it under the terms of the GNU Lesser General Public License
	as published by the Free Software Foundation; either version 2.1
	of	the License, or (at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Lesser General Public License for more details.

	You should have received a copy of the GNU Lesser General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

	Author: Jo Vermeulen <jo.vermeulen@uhasselt.be>
*/

using System;

namespace Uiml.Peers
{
	public class Location : ICloneable
	{
		private string m_value = "";
		private Protocol m_type = Protocol.Local;
		
		public Location(string s)
		{
			Process(s);
		}

        protected Location()
        {
        }

        public virtual object Clone()
        {
            Location location = new Location();
            location.m_value = m_value;
            location.m_type = m_type;

            return location;
        }

		public void Process(string s)
		{
			// split protocol, separator and actual value
			int separatorIndex = s.IndexOf(SEPARATOR);

			if (separatorIndex != -1)
			{
				switch(s.Substring(0, separatorIndex))
				{
					case XML_RPC:
						m_type = Protocol.XmlRpc;
						break;
					case SOAP:
						m_type = Protocol.Soap;
						break;
					case REST:
						m_type = Protocol.Rest;
						break;
                    case UIML:
                        // uiml:// style location
                        m_type = Protocol.Local;
                        m_value = Uiml.Utils.Location.Transform(s);
                        break;
				}

                if (m_value == string.Empty) // only if not already assigned
				    m_value = s.Substring(separatorIndex + SEPARATOR.Length);
			}
			else
			{
				// no separator, thus it's local
				m_type = Protocol.Local;
				m_value = s; // value is the complete string
			}
		}

		public string Value
		{
			get { return m_value; }
		}

		public Protocol Type
		{
			get { return m_type; }
		}
		
		public enum Protocol { Local, XmlRpc, Soap, Rest }
		public const string XML_RPC 	= "xmlrpc";
		public const string SOAP 	= "soap";
		public const string SEPARATOR 	= "://";
		public const string REST	= "rest";
        public const string UIML = "uiml";
	}
}
