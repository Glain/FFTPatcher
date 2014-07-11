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

#include <stddef.h> 
#include <string.h> 
#include <limits.h>

#undef memmem 

/* Return the first occurrence of NEEDLE in HAYSTACK. */ 
unsigned char* memmem (unsigned char* haystack, 
              size_t haystack_len, 
              unsigned char* needle, 
              size_t needle_len);

unsigned char* boyermoore_horspool_memmem(
    unsigned char* haystack, size_t hlen,
    unsigned char* needle,   size_t nlen);

