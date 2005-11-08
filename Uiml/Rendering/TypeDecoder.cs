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


namespace Uiml.Rendering
{
	using System;
	using System.Collections;
	using System.Reflection;

	public abstract class TypeDecoder : ITypeDecoder
	{
		abstract public Object[] GetArgs(Property p, Type[] types);
		abstract public Object   GetArg(System.Object o, Type t);
		abstract public Object[] GetMultipleArgs(Property[] p, Type[] types);

		abstract protected Object ConvertComplex(Type t, System.Object oValue);
		abstract protected Object ConvertComplex(Type t, Property p);
		

		protected Object ConvertPrimitive(Type t, Property p)
		{
			return ConvertPrimitive(t, (System.String)p.Value);
		}

		protected Object ConvertPrimitive(Type t, System.Object oValue)
		{
			string value = (string)oValue;
			if(oValue is string)
				value = (string)oValue;
			else if(t.FullName == "System.String")
				return oValue.ToString();
					

			try
			{
				MethodInfo method = t.GetMethod(PARSE, new Type [] { value.GetType() });
				return method.Invoke(null, new System.Object [] { value } );
			}
				catch(Exception e)
				{
					return value;
				}

		}
		
		public static string PARSE = "Parse";
	}

}

