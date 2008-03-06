using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Uiml.Peers;
using Uiml.Rendering;
using Uiml.Gummy.Domain;
using Uiml.Gummy.Visual;
using Uiml.Gummy.Serialize;

using System.Windows.Forms;
using System.Drawing;

namespace Uiml.Gummy.Kernel.Services
{
    public class ToolboxService : Form, IService
    {
        private List<DomainObject> m_domainObjects = new List<DomainObject>();
        private FlowLayoutPanel layout;
        private ToolboxServiceConfiguration m_config;

        public ToolboxService() : base()
        {
            InitializeComponent();
            m_config = new ToolboxServiceConfiguration(this);           
        }

        public void Init()
        {
            Text = "Toolbox";
            List<VisualDomainObject> visualDomainObjects = new List<VisualDomainObject>();
            Hashtable dclasses = ActiveSerializer.Instance.Serializer.Voc.DClasses;
            IDictionaryEnumerator enumerator = dclasses.GetEnumerator();

            Size size = new Size(100, 40);
            int x = 0;
            int y = 0;
            int counter = 1;
            int height = 40;
            
            while (enumerator.MoveNext())
            {
                DClass dclass = (DClass)enumerator.Entry.Value;
                if (ActiveSerializer.Instance.Serializer.Accept(dclass) && dclass.UsedInTag == "part")
                {
                    DomainObject domObject = DomainObjectFactory.Instance.Create(dclass);                    
                    VisualDomainObject visDomObject = new VisualDomainObject(domObject);

                    // create container for image and label
                    TableLayoutPanel table = new TableLayoutPanel();                    
                    table.RowCount = 1;
                    table.ColumnCount = 2;
                    table.AutoSize = true;
                    table.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    table.Controls.Add(visDomObject, 0, 0);
                    Label l = new Label();
                    l.Text = domObject.Part.Class;
                    l.Dock = DockStyle.Fill;
                    l.TextAlign = ContentAlignment.MiddleLeft;
                    table.Controls.Add(l, 1, 0);
                    layout.Controls.Add(table);

                    visualDomainObjects.Add(visDomObject);

                    if (counter % 2 == 0)
                    {
                        y += size.Height + 5;
                        height += size.Height + 5;
                        x = 0;
                    }
                    else
                    {
                        x += size.Width + 5;
                    }
                    counter++;
                }
            }
            int k = visualDomainObjects.Count;
        }

        public bool Open()
        {
            this.Visible = true;
            return true;
        }

        public bool Close()
        {
            this.Visible = false;
            return true;
        }

        public string ServiceName
        {
            get
            {
                return "gummy-toolbox";
            }
        }

        public System.Windows.Forms.Control ServiceControl
        {
            get
            {
                return this;
            }
        }

        public IServiceConfiguration ServiceConfiguration
        {
            get 
            {
                return m_config;
            }
        }

        public void NotifyConfigurationChanged()
        {
            switch (m_config.Widgetset)
            {
                case "Gtk#":
                    DesignerKernel.Instance.Platform = "swf-1.1";
                    break;
                case "Compact Windows Forms":
                    DesignerKernel.Instance.Platform = "cswf-1.0";
                    break;
                case "iDTV Swing":
                    DesignerKernel.Instance.Platform = "idtv-1.0";
                    break;
                case "Windows Forms":
                default:
                    DesignerKernel.Instance.Platform = "swf-1.1";
                    break;
            }
        }

        private void InitializeComponent()
        {
            this.layout = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // layout
            // 
            this.layout.AutoScroll = true;
            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout.Location = new System.Drawing.Point(0, 0);
            this.layout.Name = "layout";
            this.layout.Size = new System.Drawing.Size(182, 522);
            this.layout.TabIndex = 0;
            // 
            // ToolboxService
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(182, 522);
            this.Controls.Add(this.layout);
            this.Name = "ToolboxService";
            this.ResumeLayout(false);

        }
    }
}
