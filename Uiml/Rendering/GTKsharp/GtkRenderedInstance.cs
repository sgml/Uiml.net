/*
  	 Uiml.Net: a Uiml.Net renderer (http://lumumba.uhasselt.be/kris/research/uiml.net/)
   
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


namespace Uiml.Rendering.GTKsharp
{
    using System;
	using Gtk;
	using GLib;
	using GtkSharp;

	///<summary>
	/// This class serves as a container for rendering a GTK# User Interface.
	/// The GtkRenderer will return an instance of this class. Use
	/// ShowIt to show the interface on screen.
	///</summary>
	public class GtkRenderedInstance : Window, IRenderedInstance{
		private static GLib.GType gtype = GLib.GType.Invalid;
		
        private static int numMainloops = 0;

		public GtkRenderedInstance() : base("UIML container")
		{
            // events
            Destroyed +=new System.EventHandler(OnCloseWindow);
            Realized += new EventHandler(OnInit);
            Mapped += new EventHandler(OnActivateWindow);
        }

		public static new GLib.GType GType {
			get {
				if (gtype == GLib.GType.Invalid)
					gtype = RegisterGType (typeof (GtkRenderedInstance));
				return gtype;
			}
		}


		public GtkRenderedInstance(string title) : base(title)
		{}

		///<summary>
		/// this should be overridable by the UIML document specification
		///</summary>
		static void Window_Delete(Widget obj, DeleteEventArgs args)
		{
			SignalArgs sa = (SignalArgs) args;
			Application.Quit ();
            
			sa.RetVal = true;
		}

		public void ShowIt()
        {
            //DeleteEvent  += new DeleteEventHandler(Window_Delete);

            ShowAll();
            this.Present(); // raise window if it already exists
            if (GtkRenderedInstance.numMainloops == 0)
            {
                GtkRenderedInstance.numMainloops++;
                Application.Run();
            }
		}

        public void CloseIt()
        {
            this.Destroy();
        }

        #region CloseWindow event
        public event EventHandler CloseWindow;
        public void OnCloseWindow(object sender, EventArgs e)
        {
            if (CloseWindow != null)
                CloseWindow(this, e);
        }
        #endregion

        #region Init event
        public event EventHandler Init;
        public void OnInit(object sender, EventArgs e)
        {
            if (Init != null)
                Init(this, e);
        }
        #endregion

        #region ActivateWindow event
        public event EventHandler ActivateWindow;
        public void OnActivateWindow(object sender, EventArgs e)
        {
            if (ActivateWindow != null)
                ActivateWindow(this, e);
        }
        #endregion
	}
}
