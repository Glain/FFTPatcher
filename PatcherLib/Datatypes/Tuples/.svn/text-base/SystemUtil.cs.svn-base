#region (c)2009 Lokad - New BSD license

// Copyright (c) Lokad 2009 
// Company: http://www.lokad.com
// This code is released under the terms of the new BSD licence

#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Lokad
{
    class SystemUtil
    {
        internal static int GetHashCode( params object[] args )
        {
            unchecked
            {
                int result = 0;
                foreach (var o in args)
                {
                    result = (result * 397) ^ (o != null ? o.GetHashCode() : 0);
                }
                return result;
            }
        }

    }
}
