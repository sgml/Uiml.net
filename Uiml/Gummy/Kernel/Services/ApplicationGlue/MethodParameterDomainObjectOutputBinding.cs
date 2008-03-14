using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Uiml.Gummy.Domain;

namespace Uiml.Gummy.Kernel.Services.ApplicationGlue
{
    public class MethodParameterDomainObjectOutputBinding : MethodParameterDomainObjectBinding
    {
        public MethodParameterDomainObjectOutputBinding(MethodParameterModel param, DomainObject dom, Property prop)
            : base(param, dom, prop)
        {
        }

        public override XmlNode GetUiml(XmlDocument doc)
        {
            // <property>
            XmlElement prop = doc.CreateElement("property");
            XmlAttribute partName = doc.CreateAttribute("part-name");
            partName.Value = Parameter.Link.Part.Identifier;
            prop.Attributes.Append(partName);
            XmlAttribute name = doc.CreateAttribute("name");
            name.Value = Property.Name;
            prop.Attributes.Append(name);

            return prop;
        }
    }
}
