/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

#include "stdafx.h"
#include "memmem.h"

#define MAX(x,y) ((x)>(y))?(x):(y)

unsigned char* memmem(
    unsigned char* haystack, size_t hlen,
    unsigned char* needle,   size_t nlen)
{
    return boyermoore_horspool_memmem(haystack, hlen, needle, nlen);
}

size_t bad_char_skip[UCHAR_MAX + 1]; 

// 16 seconds
unsigned char* boyermoore_horspool_memmem(
    unsigned char* haystack, size_t hlen,
    unsigned char* needle,   size_t nlen)
{
    size_t scan = 0;
 
    /* Sanity checks on the parameters */
    if (nlen <= 0 || hlen <= 0 || nlen > hlen || !haystack || !needle)
        return NULL;
 
    /* Initialize the table to default values */
    for (scan = 0; scan <= UCHAR_MAX; scan++) bad_char_skip[scan] = nlen;
 
    size_t last = nlen - 1;
 
    for (scan = 0; scan < last; scan++) bad_char_skip[needle[scan]] = last - scan;
 
    /* Search the haystack, while the needle can still be within it. */
    while (hlen >= nlen)
    {
        /* scan from the end of the needle */
        for (scan = last; haystack[scan] == needle[scan]; scan--)
        {
            if (scan == 0) return haystack;
        }
 
        hlen     -= bad_char_skip[haystack[last]];
        haystack += bad_char_skip[haystack[last]];
    }
 
    return NULL;
}

