/*
    Uiml.Net: a .Net UIML renderer (http://research.edm.uhasselt.be/kris/research/uiml.net)

	 Copyright (C) 2003  Kris Luyten (kris.luyten@uhasselt.be)
	                     Expertise Centre for Digital Media (http://edm.uhasselt.be)
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
*/

namespace Uiml{


	using System.Xml;
    using System.Xml.Serialization;
    using System;
    using System.Collections.Generic;


	///<summary>
	/// Holds 4 common attributes of UIML tags: id, source, how and export
	///</summary>
	public abstract class UimlAttributes {

		protected string m_identifier = null;
		protected string m_source = null;
		protected string m_how = null;
		protected string m_export = null;

		public enum HOW_VALS { Union,  Cascade,  Replace };
		public const string REPLACE = "replace";
		public const string CASCADE = "cascade";
		public const string UNION   = "union";
		
		public enum EXPORT_VALS { Hidden, Optional, Required };
		public const string HIDDEN = "hidden";
		public const string OPTIONAL = "optional";
		public const string REQUIRED = "required";


        protected void CopyAttributesFrom(UimlAttributes attr)
        {
            m_identifier = attr.m_identifier;
            m_source = attr.m_source;
            m_how = attr.m_how;
            m_export = attr.m_export;
        }

		protected void ReadAttributes(XmlNode n)
		{
			XmlAttributeCollection attr = n.Attributes;
			if(attr.GetNamedItem(HOW) != null)
				 How = attr.GetNamedItem(HOW).Value;
			if(attr.GetNamedItem(EXPORT) != null)
				 Export = attr.GetNamedItem(EXPORT).Value;
			if(attr.GetNamedItem(SOURCE) != null)
				 Source = attr.GetNamedItem(SOURCE).Value;
			if(attr.GetNamedItem(ID) != null)
				 Identifier = attr.GetNamedItem(ID).Value;
		}

		public string Identifier
		{
			get { return m_identifier;}
			set { m_identifier = value; }
		}

		public string Source
		{
			get { return m_source;}
			set { m_source = value; }
		}

		public bool SourceAvailable
		{
			get { return (m_source != null); }
		}

		public string How
		{
			get { return m_how;}
			set { m_how = value; }
		}

		public string Export
		{
			get { return m_export;}
			set { m_export = value; }
		}

        public abstract XmlNode Serialize(XmlDocument doc);

        public List<XmlAttribute> CreateAttributes(XmlDocument doc)
        {
            List<XmlAttribute> attributesList = new List<XmlAttribute>();

            if (m_identifier != null)
            {
                XmlAttribute id = doc.CreateAttribute(ID);
                id.Value = m_identifier;
                attributesList.Add(id);
            }
            if (m_source != null)
            {
                XmlAttribute source = doc.CreateAttribute(SOURCE);
                source.Value = m_source;
                attributesList.Add(source);
            }
            if (m_how != null)
            {
                XmlAttribute how = doc.CreateAttribute(HOW);
                how.Value = m_how;
                attributesList.Add(how);
            }
            if (m_export != null)
            {
                XmlAttribute export = doc.CreateAttribute(EXPORT);
                export.Value = m_export;
                attributesList.Add(export);
            }
            return attributesList;
        }

		public const string ID           = "id";
		public const string SOURCE			= "source";
		public const string HOW				= "how";
		public const string EXPORT			= "export";

	}
}
