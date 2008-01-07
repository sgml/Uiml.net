using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Uiml.Gummy.Serialize;
using Uiml.Gummy.Kernel.Services;
using Uiml.Gummy.Serialize.SWF;

using System.Windows.Forms;

namespace Uiml.Gummy.Kernel
{
    public class DesignerKernel : IService
    {
        List<IService> m_services = new List<IService>();

        public DesignerKernel(string vocabulary): base()
        {
            //For this moment only SWF is accepted
            ActiveSerializer.Instance.Serializer = new SWFUimlSerializer();
        }

        public void Init()
        {
            LoadServices(null);
            for (int i = 0; i < m_services.Count; i++)
            {
                m_services[i].Init();
            }
        }

        public bool Open()
        {            
            for (int i = 0; i < m_services.Count; i++)
            {
                Console.WriteLine("Loading " + m_services[i].ServiceName);
                if (!m_services[i].Open())
                {                    
                    return false;
                }
            }
            try
            {
                Application.Run();
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                return false;
            }
        }

        public bool Close()
        {
            for (int i = 0; i < m_services.Count; i++)
            {
                Console.WriteLine("Closing " + m_services[i].ServiceName);
                if (!m_services[i].Close())
                {
                    return false;
                }
            }

            try
            {
                Application.Exit();
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                return false;
            }
        }

        public string ServiceName
        {
            get
            {
                return "gummy-kernel";
            }
        }

        //Load the services from an Xml Document
        public void LoadServices(XmlDocument doc)
        {
            m_services.Add(new ToolboxService());
            m_services.Add(new CanvasService());
            m_services.Add(new PropertiesService());
        }
    }
}
