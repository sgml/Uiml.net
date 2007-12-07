using System;
using System.Collections.Generic;
using System.Text;
using Uiml.Gummy.Serialize;
using Uiml.Gummy.Domain;
using Uiml;
using System.Drawing;
using Uiml.Rendering;

namespace Uiml.Gummy.Serialize.SWF
{
    public class SWFSizeManipulator : SizeManipulator
    {
        private Property m_sizewProperty = null;
        private Property m_sizehProperty = null;

        public SWFSizeManipulator(DomainObject dom, IRenderer renderer) : base(dom, renderer)
        {
            checkProperties();
        }

        public override object Clone()
        {
            SWFSizeManipulator clone = new SWFSizeManipulator(null,m_renderer);
            return clone;
        }

        private void checkProperties()
        {
            if (DomainObject == null)
                return;
            if (m_sizewProperty == null)
                m_sizewProperty = DomainObject.FindProperty("width");
            if (m_sizehProperty == null)
                m_sizehProperty = DomainObject.FindProperty("height");
        }

        public override System.Drawing.Size Size
        {
            get
            {
                checkProperties();
                string width = (string)m_sizewProperty.Value;
                string height = (string)m_sizehProperty.Value;                
                Size size = new Size(Convert.ToInt32(width), Convert.ToInt32(height));
                return size;
            }
            set
            {
                checkProperties();
                m_sizewProperty.Value = value.Width + "";
                m_sizehProperty.Value = value.Height + "";
            }
        }

        public static string IAM = "size";
    }
}
