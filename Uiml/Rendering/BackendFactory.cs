/*
 	 Uiml.Net: a Uiml.Net renderer (http://lumumba.luc.ac.be/kris/research/uiml.net/)

	 Copyright (C) 2003  Kris Luyten (kris.luyten@luc.ac.be)
	                     Expertise Centre for Digital Media (http://edm.luc.ac.be)
								Limburgs Universitair Centrum

	This program is free software; you can redistribute it and/or
	modify it under the terms of the GNU General Public License
	as published by the Free Software Foundation; either version 2
	of	the License, or (at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

namespace Uiml.Rendering
{
	using System;
	using System.Collections;

	public class BackendFactory
	{
		public BackendFactory()
		{
		}


		///<summary>
		/// Creates a renderer for a given vocabulary name
		///</summary>
		///TODO: replace by more generic code!
		public IRenderer CreateRenderer(String name)
		{
			switch(name)
			{
				case Uiml.Rendering.GTKsharp.GtkRenderer.NAME:
					return new Uiml.Rendering.GTKsharp.GtkRenderer();
					
				//Uncomment this if you want to use wx.NET as a backend renderer
				//case Uiml.Rendering.WXnet.WxRenderer.NAME:
				//		return new Uiml.Rendering.WXnet.WxRenderer();
				
				//Uncomment this if you want to use System.Windows.Forms as a backend renderer
				//case Uiml.Rendering.SWF.SWFRenderer.NAME:
				//		return new Uiml.Rendering.SWF.SWFRenderer();

				default:
					return null;
			}
		}
	}
}

