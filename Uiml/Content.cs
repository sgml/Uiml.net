/*
    Uiml.Net: a .Net UIML renderer (http://lumumba.uhasselt.be/kris/research/uiml.net)

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
	
	using System;
	using System.Xml;
	using System.Collections;
    using System.Collections.Generic;

	///<summary>
	///Represents the content element:
	///  &lt;!ELEMENT content (constant*)&gt;
	///  &lt;!ATTLIST content id NMTOKEN #IMPLIED 
	///               source CDATA #IMPLIED 
	///               how (union|cascade|replace) "replace" 
	///               export (hidden|optional|required) "optional"&gt;
	///</summary>
	public class Content : UimlAttributes, IUimlElement {
		private String m_urlName;

		private ArrayList m_constantList;

		public Content()
		{
			m_constantList = new ArrayList();
		}

		public Content(XmlNode n) : this()
		{
			Process(n);        
        }

        public virtual object Clone()
        {
            Content clone = new Content();
            clone.CopyAttributesFrom(this);
            clone.m_urlName = m_urlName;
            if(m_constantList != null)
            {
                clone.m_constantList = new ArrayList();
                for(int i = 0; i<m_constantList.Count; i++)
                {
                    Constant c = (Constant)m_constantList[i];
                    clone.m_constantList.Add(c.Clone());
                }
            }
            return clone;
        }

		public void Process(XmlNode n)
		{
			if(n.Name != IAM)
				return;
			base.ReadAttributes(n);

			if(n.HasChildNodes)
			{
				XmlNodeList xnl = n.ChildNodes;
				for(int i=0; i<xnl.Count; i++)
					m_constantList.Add(new Constant(xnl[i]));
			}
		}

        public override XmlNode Serialize(XmlDocument doc)
        {
            XmlNode node = doc.CreateElement(IAM);
            List<XmlAttribute> attributes = CreateAttributes(doc);
            foreach (XmlAttribute attr in attributes)
            {
                node.Attributes.Append(attr);
            }

            for (int i = 0; i < Children.Count; i++)
            {
                IUimlElement child = (IUimlElement)Children[i];
                node.AppendChild(child.Serialize(doc));
            }

            return node;

        }

        public ArrayList Children
		{
			get { return m_constantList; }
		}

		public Constant Query(String constantName)
		{
			IEnumerator enumConsts = m_constantList.GetEnumerator();
			while(enumConsts.MoveNext())
				if(((Constant)enumConsts.Current).Identifier == constantName)
					return (Constant)enumConsts.Current;
			return null;
		}


		public const String IAM = "content";

	}
}
