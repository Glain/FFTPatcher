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
#include <stdlib.h>
#include <assert.h>
#include <set>

#define MIN(x,y) (((x)<(y))?(x):(y))
#define MAX(x,y) (((x)>(y))?(x):(y))
#define MIN_NEEDLE_LENGTH 4
#define MAX_NEEDLE_LENGTH 35
#define MAX_HAYSTACK_LENGTH 3792

#include "CompressionJumps.h"


#ifdef _MANAGED
#pragma managed(push, off)
#endif

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    return TRUE;
}

int FindByte(unsigned char byteToFind, unsigned char* whereToLook, int whereToStart, int whereToEnd)
{
    for (int i = whereToStart; i <= whereToEnd; i++)
    {
        if (whereToLook[i] == byteToFind) return i;
    }

    return -1;
}

BOOL GetPositionOfMaxSubArray(unsigned char* haystack, int haystackLength, unsigned char* needle, int needleLength, 
                              int* location, int* size)
{
    if (needleLength >= MIN_NEEDLE_LENGTH)
    {
        for (int i = MIN(needleLength,haystackLength); i >= MIN_NEEDLE_LENGTH; i--)
        {
            unsigned char* loc = (unsigned char*)memmem(haystack, haystackLength, needle, i);
            if (loc != NULL)
            {
                *size = i;
                *location = haystackLength - (loc-haystack);
                return TRUE;
            }
        }
    }

    return FALSE;
}

void AddJump(unsigned char* destination, int jump, int length)
{
    int l = length - 4;
    int j = CompressionJumps[jump];

    destination[0] = 0xF0 | ((l&0x18) >> 3);
    destination[1] = (l&0x07) << 5;
    destination[1] |= (j&0x1F00) >> 8;
    destination[2] = j&0xFF;
}

typedef void (__stdcall *callback_t)(int);

int ShouldSkip(unsigned char input)
{
    if (input == 0xFE ||
        input == 0xE0 ||
        input == 0xFB ||
        input == 0xFC ||
        input == 0xFF )
        return 1;
    //else if ((input & 0xF0) == 0xD0 ||
    //         (input & 0xF0) == 0xE0)
    //    return 2;
    else
        return -1;
}

int GetNeedleLength(unsigned char* buffer, int whereToStart, int whereToEnd)
{
    int needleLength = 0;
    for(int i = whereToStart; i < whereToEnd; i++)
    {
        unsigned char b = buffer[i];
        if (b == 0xFE)
        {
             break;
        }
        needleLength++;
    }
    return needleLength;
}

__declspec(dllexport) void CompressSection(unsigned char* input, int inputLength, unsigned char* output, int* outputPosition)
{
    int loc = 0;
    int size = 0;

    for (int i = 0; i < inputLength; i++)
    {
        int skip = -1;
        if ((skip = ShouldSkip(input[i])) != -1)
        {
            for(int j = 0; j < skip; j++)
            {
                output[*outputPosition+j] = input[i+j];
            }

            (*outputPosition) += skip;
            i += skip - 1;
            continue;
        }

        int fe = i + GetNeedleLength(input, i, i+MIN(MAX_NEEDLE_LENGTH, inputLength-i)-1);
            
        while ( (input[fe-1] & 0xF0) == 0xE0 ||
                (input[fe-1] & 0xF0) == 0xD0 ||
                (input[fe-1] > 0xCF &&
                  input[fe-1] != 0xFA &&
                  input[fe-1] != 0xF8) )
        {
            fe -= 1;
        }

        if (GetPositionOfMaxSubArray(
               output+MAX(0,*outputPosition-MAX_HAYSTACK_LENGTH),
               MIN(*outputPosition, MAX_HAYSTACK_LENGTH),
               input+i,
               fe - i,
               &loc,
               &size))
        {
            AddJump(output+*outputPosition, loc, size);
            (*outputPosition) += 3;
            i += size - 1;
        }
        else
        {
            output[*outputPosition] = input[i];
            (*outputPosition) += 1;
            continue;
        }
    }
}

#ifdef _MANAGED
#pragma managed(pop)
#endif

