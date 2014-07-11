/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/17/2012
 * Time: 10:51
 * 
 */
using System;

namespace ASMEncoding.Helpers
{
	public class ASMDebugHelper
	{
		public static void assert(bool condition, string message)
		{
			if (!condition)
			{
				//_errorMessage = message;
				throw new Exception(message);
			}
		}
	}
}
