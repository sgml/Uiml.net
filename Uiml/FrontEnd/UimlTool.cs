/*
    Uiml.Net: a .Net UIML renderer (http://research.edm.luc.ac.be/kris/research/uiml.net)

	 Copyright (C) 2003  Kris Luyten (kris.luyten@luc.ac.be)
	                     Expertise Centre for Digital Media (http://edm.luc.ac.be)
								Limburgs Universitair Centrum

	This program is free software; you can redistribute it and/or
	modify it under the terms of the GNU Lesser General Public License
	as published by the Free Software Foundation; either version 2.1
	of	the License, or (at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU Lesser General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

//[assembly: System.Reflection.AssemblyKeyFile ("uiml.net.snk")]


namespace Uiml.FrontEnd{

	using Uiml.Peers;

	using System;
	using System.Xml;
	using System.IO;

	using CommandLine;

	//using Uiml.Rendering.GTKsharp;
	using Uiml.Rendering;

	using Uiml.Executing;

	
	///<summary>
	/// Main application class; serves as a comand-line front-end for
	/// the uiml.net library
	///</summary>
	public class UimlTool{
		static public String[] options = {"voc","uiml","help","libs","version", "log"};
		static public String VERSION = "0.0.6-pre (09-07-2004)";
		static public char LIBSEP;

		private BackendFactory backendFactory;
		public static String FileName;
//		public static log4net.ILog logger;
		
		
		public static void Main(string[] args)
		{
			Options opt = new Options(args, options);
			if((opt.NrSwitches == 0)||(opt[options[2]].Equals("-"))) 
			{
				Help();
				return;
			}
			string document="", vocabulary="";

			if(opt.IsUsed(options[5])) //initialise logging facilities
			{
				if(!opt.HasArgument(options[5]))
				{
					Console.WriteLine("You have to specify an appender for logging");
					Console.WriteLine("Available appenders: log4net.Appender.ConsoleAppender log4net.Appender.CountingAppender");
				}
				else
				{
					//logger =  log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
					//SimpleLayout sl = new SimpleLayout();
					//IAppender appender =  
					//TODO
				}
				
			}
			else
			{
				//TODO
			}
		   
			if(opt.IsUsed(options[1])&&opt.HasArgument(options[1]))
			{
				if(opt.IsUsed(options[3]))
				{
					LoadLibraries(opt[options[3]]);
				}
				UimlTool ut;
				document = opt[options[1]];
				if(opt.IsUsed(options[0]))
				{
					vocabulary = opt[options[0]];
					ut = new UimlTool(document,vocabulary);
				}
				else
					ut = new UimlTool(document);					
			}
			else if(opt.IsUsed(options[4]))
			{
				Version();
			}
			else
			{
				Console.WriteLine("You have to specify a uiml document as input");
				Help();
			}
			
		}

		static public void Help()
		{
			Console.WriteLine("                                                                              	  ");
			Console.WriteLine("         Uiml.Net: a free Uiml 3.0 renderer for .Net                           	  ");
			Console.WriteLine("                                                                             	  ");
			Console.WriteLine("Copyright: Expertise Centre for Digital Media -- Limburgs Universitair Centrum	  ");
			Console.WriteLine("contact: kris.luyten@luc.ac.be                                                	  ");
			Console.WriteLine("web: http://lumumba.luc.ac.be/kris/projects/uiml.net                              ");
			Console.WriteLine("                                                                          	     ");
			Console.WriteLine("Please email the bugs you find using this tool to us                     	        ");
			Console.WriteLine("                                                                         	        ");
			Console.WriteLine("Options:                                                                     	  ");
			Console.WriteLine("    -uiml <file>                 Specify the input file (required)                ");
			Console.WriteLine("    -help                        Print this message                               ");
			Console.WriteLine("    -libs <file["+LIBSEP+"file"+LIBSEP+"...]>        The libraries containing te application logic        ");	
			Console.WriteLine("    -log <appender>                                                               ");
			Console.WriteLine("    -version                     Print version info                               ");	
		}

		static public void Version()
		{
			Console.WriteLine("uiml.net version {0}", VERSION);
		}

		static public void LoadLibraries(String libs)
		{
			ExternalLibraries eLib = ExternalLibraries.Instance;
			int j = libs.IndexOf(LIBSEP);
			while(j!=-1)
			{
				String nextLibrary = libs.Substring(0,j);
				eLib.Add(nextLibrary);
				libs = libs.Substring(j+1,libs.Length-j-1);
				j = libs.IndexOf(LIBSEP);
			}
			eLib.Add(libs);
		}


		private UimlDocument m_uimlDocument;
		private static IRenderer m_renderer;
		static public Uri InputFile = null;

		public UimlTool(string fName)
		{
			backendFactory = new BackendFactory();
			try{
				//validate(fName)
				Load(fName);
			}catch(XmlException e){
				Console.WriteLine(e.Message);
			}
		}

		public UimlTool(string fName, string voc)
		{
			try{
				Load(fName, voc);
			}catch(XmlException e){
				Console.WriteLine(e);
			}
		}

		public void Load(String fName)
		{
			Load(fName,null);
		}

		public void Load(String fName, String strVoc)
		{
			FileName = fName;
			//get the current directory:
			//InputFile = new Uri("file://" + Application.ExecutablePath() + "/" + FileName);

			if(strVoc!=null)
				new Vocabulary(strVoc);
			try
			{
				XmlDocument doc = new XmlDocument();
				Console.WriteLine("Loading UIML document...");
				doc.Load(fName);
				//doc.Load("copy.uiml");
				Console.WriteLine("Processing UIML document...");
				Process(doc);
				Console.WriteLine("Rendering UIML document...");
				Render();
			}
			catch(System.IO.FileNotFoundException sif)
			{
				Console.WriteLine("Could not read file {0}",FileName);
				#if !COMPACT
				Environment.Exit(-1);
				#else
				//TODO
				;
				#endif
			}
		}

		public UimlDocument Document
		{
			get
			{
				return m_uimlDocument;
			}
		}


		private void Process(XmlNode n)
		{
			m_uimlDocument = new UimlDocument(n);
		}

		private void Render()
		{
			Console.WriteLine(Document.UHead);
			//m_renderer = new GtkRenderer();
			//IRenderedInstance instance = m_renderer.Render(Document);
			//instance.ShowIt();

			//Check the vocabulary, and whether there is a registerd Renderer for this vocabulary			
			m_renderer =  backendFactory.CreateRenderer(Document.Vocabulary);
			Console.WriteLine("Loading renderer for {0}", Document.Vocabulary);
			if(m_renderer == null)
			{
				Console.WriteLine("No Vocabulary specified, please update the <peers> section of {0}", FileName);
				#if !COMPACT
				Environment.Exit(-1);
				#else
				//TODO
				;
				#endif
			}
			Console.WriteLine("Building ui instance");
			IRenderedInstance instance = m_renderer.Render(Document);
			Console.WriteLine("Showing the ui");
			instance.ShowIt();
		}

		public static IRenderer CurrentRenderer
		{
			get { return m_renderer; }
		}

		public const string UIML = "uiml";

	}

}