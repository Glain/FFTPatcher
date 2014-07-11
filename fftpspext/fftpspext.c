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

#include <stdio.h>
#include <string.h>

#define MAXBUFFER 209715200 //200Mb

long int header22;
int filesize, i, start_pointer, end_pointer, size, dummy_count;
int buffer[MAXBUFFER];
char filename[80];

char path[3971][40];

void buildPaths(int);
	
FILE *input, *output;

int main(int argc, char *argv[])
{
    if (argc < 3)
    {
        printf("War of the Lions De/Archivier 1.0 by Joe Davidson, adapted from work by Brisma\n"
               "   -> fftpspext -d fftpack.bin //Extract\n"
               "   -> fftpspext -r newfile.bin //Rebuild\n");
    }
                         
    else if(strcmp(argv[1], "-d") == 0)
    {
    	
    	buildPaths(1);
        printf("Extracting file %s...", argv[2]);
        input = fopen(argv[2], "rb");
        
        // Get filesize
        fseek(input, 0, SEEK_END);
        filesize = ftell(input);
        printf("Original filesize: %d\n", filesize);
        
        fseek(input, 0x08, SEEK_SET);
        
        // Where does 3970 come from?
        // It is the number of entries in the file table
        for(i = 1; i <= 3970; i++)
        {
            // Read an int
            fread(&start_pointer, 4, 1, input);
            
            if (i == 3970) 
            {
                end_pointer = filesize;
            }
            else 
            {
                fread(&end_pointer, 4, 1, input);
            }
            
            size = end_pointer - start_pointer;
            
            if (size != 0) 
            {
            	if (path[i][0] == '\0')
            	{
            		sprintf(filename, "fftpack/unknown/fftpack.%d", i);
            	}
            	else
            	{
            		sprintf(filename, "fftpack/%s", path[i]);
            	}
                printf("\nExtracting file %s (%d bytes)", filename, size);
                output = fopen(filename, "wb");
                fseek(input, start_pointer, SEEK_SET);
                fread(&buffer, size, 1, input);
                fwrite(&buffer, size, 1, output);
                fclose(output);
                printf(" [OK]");
            }
            else 
            {
            	if (path[i][0] == '\0')
            	{
	            	sprintf(filename, "fftpack/unknown/fftpack.%d.dummy", i);
	                printf("\nIgnoring dummy file fftpack.%d [IGNORED]", i); //Dummy
            	}
            	else
            	{
            		sprintf(filename, "fftpack/%s", path[i]);
            	}
            	output = fopen(filename,"wb");
            	fclose(output);
                dummy_count++;
            }
            
            // Return to file table and get next file
            fseek(input, (4 * i) + 8, SEEK_SET);
        }
    
        fclose(input);
        
        printf("\n\nExtracted %d files, ignored %d dummy files.", (3970 - dummy_count), dummy_count);
    }
    
    else if(strcmp(argv[1], "-r") == 0)
    {
    	buildPaths(0);
        printf("Rebuilding file %s...", argv[2]);
        output = fopen(argv[2], "wb");
        
        //Rebuild header manually 
        fputc(0x80, output);
        fputc(0x0F, output);
        fputc(0x00, output);
        fputc(0x00, output);
        fputc(0x10, output);
        fputc(0x00, output);
        fputc(0x00, output);
        fputc(0x00, output);
        
        fseek(output, 15888, SEEK_SET);
        end_pointer = ftell(output);
        
        // end_pointer is where we'll start writing data
        
        start_pointer = 8;
        for(i = 1; i <= 3970; i++)
        {
        	if (path[i][0] == 0)
        	{
	            sprintf(filename, "fftpack/unknown/fftpack.%d", i);
        	}
        	else
        	{
        		sprintf(filename, "fftpack/%s", path[i]);
        	}
        	
            printf("\nReinserting file %s at offset 0x%X...", filename, end_pointer);

            // Write the address of the beginning of the file to the file table
            fseek(output, start_pointer, SEEK_SET);
            fwrite(&end_pointer, 4, 1, output);
            start_pointer += 4;
            
            fseek(output, end_pointer, SEEK_SET);
            if ((input = fopen(filename, "rb")) == NULL) 
            { 
                printf(" [IGNORED]"); 
                dummy_count++; 
            }
            else 
            {
                fseek(input, 0, SEEK_END);
                size = ftell(input);
                fseek(input, 0, SEEK_SET);
                fread(&buffer, size, 1, input);
                fclose(input);
                fwrite(&buffer, size, 1, output);
                printf(" [OK]");
            }
            end_pointer = ftell(output);
        }
        fclose(output);
        printf("\n\nRebuilt from %d files, ignoring %d dummy files", (3970 - dummy_count), dummy_count);
    }
    
    else 
    {
        printf("War of the Liosn De/Archivier 1.0 by Joe Davidson, adapted from work by Brisma\n"
               "   -> fftpspext -d fftpack.bin //Extract\n"
               "   -> fftpspext -r newfile.bin //Rebuild\n");
    }
    
    return 0;
}


void buildPaths(int shouldMake)
{
	if (shouldMake)
	{
		mkdir("fftpack", 0777);
		mkdir("fftpack/BATTLE", 0777);
		mkdir("fftpack/EFFECT", 0777);
		mkdir("fftpack/EVENT", 0777);
		mkdir("fftpack/MAP", 0777);
		mkdir("fftpack/MENU", 0777);
		mkdir("fftpack/SOUND", 0777);
		mkdir("fftpack/WORLD", 0777);
		mkdir("fftpack/SAVEIMAGE", 0777);
		mkdir("fftpack/unknown", 0777);
	}
	
	int i = 902;
	for (i = 902; i <= 935; i++)
	{
		sprintf(path[i], "SAVEIMAGE/%i.png", i);
	}
	strcpy(path[3], "SYSTEM.CNF");
	strcpy(path[4], "SLPS_007.70");
	strcpy(path[5], "BATTLE.BIN");
	strcpy(path[201], "BATTLE/10M.SPR");
	strcpy(path[202], "BATTLE/10W.SPR");
	strcpy(path[203], "BATTLE/20M.SPR");
	strcpy(path[204], "BATTLE/20W.SPR");
	strcpy(path[205], "BATTLE/40M.SPR");
	strcpy(path[206], "BATTLE/40W.SPR");
	strcpy(path[207], "BATTLE/60M.SPR");
	strcpy(path[208], "BATTLE/60W.SPR");
	strcpy(path[78], "BATTLE/ADORA.SPR");
	strcpy(path[79], "BATTLE/AGURI.SPR");
	strcpy(path[80], "BATTLE/AJORA.SPR");
	strcpy(path[81], "BATTLE/ARLI.SPR");
	strcpy(path[214], "BATTLE/ARLI2.SP2"); // Exact match
	strcpy(path[82], "BATTLE/ARU.SPR");
	strcpy(path[83], "BATTLE/ARUFU.SPR");
	strcpy(path[84], "BATTLE/ARUMA.SPR");
	strcpy(path[66], "BATTLE/ARUTE.SEQ");
	strcpy(path[56], "BATTLE/ARUTE.SHP");
	strcpy(path[85], "BATTLE/ARUTE.SPR");
	strcpy(path[86], "BATTLE/BARITEN.SPR");
	strcpy(path[87], "BATTLE/BARU.SPR");
	strcpy(path[88], "BATTLE/BARUNA.SPR");
	strcpy(path[89], "BATTLE/BEHI.SPR");
	strcpy(path[215], "BATTLE/BEHI2.SP2"); // Exact match
	strcpy(path[90], "BATTLE/BEIO.SPR");
	strcpy(path[216], "BATTLE/BIBU2.SP2"); // Exact match
	strcpy(path[91], "BATTLE/BIBUROS.SPR");
	strcpy(path[92], "BATTLE/BOM.SPR");
	strcpy(path[217], "BATTLE/BOM2.SP2"); // Exact match
	strcpy(path[93], "BATTLE/CLOUD.SPR");
	strcpy(path[62], "BATTLE/CYOKO.SEQ");
	strcpy(path[53], "BATTLE/CYOKO.SHP");
	strcpy(path[94], "BATTLE/CYOKO.SPR");
	strcpy(path[209], "BATTLE/CYOMON1.SPR");
	strcpy(path[210], "BATTLE/CYOMON2.SPR");
	strcpy(path[211], "BATTLE/CYOMON3.SPR");
	strcpy(path[212], "BATTLE/CYOMON4.SPR");
	strcpy(path[95], "BATTLE/DAISU.SPR");
	strcpy(path[96], "BATTLE/DAMI.SPR");
	strcpy(path[97], "BATTLE/DEMON.SPR");
	strcpy(path[218], "BATTLE/DEMON2.SP2"); // Exact match
	strcpy(path[98], "BATTLE/DILY.SPR");
	strcpy(path[99], "BATTLE/DILY2.SPR");
	strcpy(path[100], "BATTLE/DILY3.SPR");
	strcpy(path[101], "BATTLE/DORA.SPR");
	strcpy(path[102], "BATTLE/DORA1.SPR");
	strcpy(path[103], "BATTLE/DORA2.SPR");
	strcpy(path[219], "BATTLE/DORA22.SP2"); // Exact match
	strcpy(path[50], "BATTLE/EFC_FNT.TIM");
	strcpy(path[74], "BATTLE/EFF1.SEQ");
	strcpy(path[72], "BATTLE/EFF1.SHP");
	strcpy(path[75], "BATTLE/EFF2.SEQ");
	strcpy(path[73], "BATTLE/EFF2.SHP");
	strcpy(path[229], "BATTLE/ENTD1.ENT"); // Exact match
	strcpy(path[230], "BATTLE/ENTD2.ENT"); // Exact match
	strcpy(path[231], "BATTLE/ENTD3.ENT"); // Exact match
	strcpy(path[232], "BATTLE/ENTD4.ENT"); // Exact match
	strcpy(path[897], "BATTLE/ENTD5.ENT");
	strcpy(path[104], "BATTLE/ERU.SPR");
	strcpy(path[105], "BATTLE/FURAIA.SPR");
	strcpy(path[106], "BATTLE/FUSUI_M.SPR");
	strcpy(path[107], "BATTLE/FUSUI_W.SPR");
	strcpy(path[108], "BATTLE/FYUNE.SPR");
	strcpy(path[144], "BATTLE/GANDO.SPR");
	strcpy(path[153], "BATTLE/GARU.SPR");
	strcpy(path[111], "BATTLE/GIN_M.SPR");
	strcpy(path[112], "BATTLE/GOB.SPR");
	strcpy(path[113], "BATTLE/GORU.SPR");
	strcpy(path[114], "BATTLE/GYUMU.SPR");
	strcpy(path[115], "BATTLE/H61.SPR");
	strcpy(path[116], "BATTLE/H75.SPR");
	strcpy(path[117], "BATTLE/H76.SPR");
	strcpy(path[118], "BATTLE/H77.SPR");
	strcpy(path[119], "BATTLE/H78.SPR");
	strcpy(path[161], "BATTLE/H79.SPR");
	strcpy(path[121], "BATTLE/H80.SPR");
	strcpy(path[122], "BATTLE/H81.SPR");
	strcpy(path[123], "BATTLE/H82.SPR");
	strcpy(path[124], "BATTLE/H83.SPR");
	strcpy(path[125], "BATTLE/H85.SPR");
	strcpy(path[126], "BATTLE/HASYU.SPR");
	strcpy(path[127], "BATTLE/HEBI.SPR");
	strcpy(path[128], "BATTLE/HIME.SPR");
	strcpy(path[129], "BATTLE/HYOU.SPR");
	strcpy(path[220], "BATTLE/HYOU2.SP2"); // Exact match
	strcpy(path[130], "BATTLE/IKA.SPR");
	strcpy(path[221], "BATTLE/IRON2.SP2"); // Exact match
	strcpy(path[222], "BATTLE/IRON3.SP2"); // Exact match
	strcpy(path[223], "BATTLE/IRON4.SP2"); // Exact match
	strcpy(path[224], "BATTLE/IRON5.SP2"); // Exact match
	strcpy(path[131], "BATTLE/ITEM_M.SPR");
	strcpy(path[132], "BATTLE/ITEM_W.SPR");
	strcpy(path[133], "BATTLE/KANBA.SPR");
	strcpy(path[67], "BATTLE/KANZEN.SEQ");
	strcpy(path[57], "BATTLE/KANZEN.SHP");
	strcpy(path[134], "BATTLE/KANZEN.SPR");
	strcpy(path[135], "BATTLE/KASANEK.SPR");
	strcpy(path[136], "BATTLE/KASANEM.SPR");
	strcpy(path[137], "BATTLE/KI.SPR");
	strcpy(path[138], "BATTLE/KNIGHT_M.SPR");
	strcpy(path[139], "BATTLE/KNIGHT_W.SPR");
	strcpy(path[140], "BATTLE/KURO_M.SPR");
	strcpy(path[141], "BATTLE/KURO_W.SPR");
	strcpy(path[142], "BATTLE/KYUKU.SPR");
	strcpy(path[143], "BATTLE/LEDY.SPR");
	strcpy(path[109], "BATTLE/MARA.SPR");
	strcpy(path[145], "BATTLE/MINA_M.SPR");
	strcpy(path[146], "BATTLE/MINA_W.SPR");
	strcpy(path[147], "BATTLE/MINOTA.SPR");
	strcpy(path[225], "BATTLE/MINOTA2.SP2"); // Exact match
	strcpy(path[148], "BATTLE/MOL.SPR");
	strcpy(path[226], "BATTLE/MOL2.SP2"); // Exact match
	strcpy(path[63], "BATTLE/MON.SEQ");
	strcpy(path[54], "BATTLE/MON.SHP");
	strcpy(path[149], "BATTLE/MONK_M.SPR");
	strcpy(path[150], "BATTLE/MONK_W.SPR");
	strcpy(path[151], "BATTLE/MONO_M.SPR");
	strcpy(path[152], "BATTLE/MONO_W.SPR");
	strcpy(path[110], "BATTLE/MUSU.SPR");
	strcpy(path[154], "BATTLE/NINJA_M.SPR");
	strcpy(path[155], "BATTLE/NINJA_W.SPR");
	strcpy(path[156], "BATTLE/ODORI_W.SPR");
	strcpy(path[157], "BATTLE/ONMYO_M.SPR");
	strcpy(path[158], "BATTLE/ONMYO_W.SPR");
	strcpy(path[159], "BATTLE/ORAN.SPR");
	strcpy(path[160], "BATTLE/ORU.SPR");
	strcpy(path[64], "BATTLE/OTHER.SEQ");
	strcpy(path[55], "BATTLE/OTHER.SHP");
	strcpy(path[77], "BATTLE/OTHER.SPR");
	strcpy(path[120], "BATTLE/RAFA.SPR");
	strcpy(path[162], "BATTLE/RAGU.SPR");
	strcpy(path[163], "BATTLE/RAMUZA.SPR");
	strcpy(path[164], "BATTLE/RAMUZA2.SPR");
	strcpy(path[165], "BATTLE/RAMUZA3.SPR");
	strcpy(path[166], "BATTLE/REZE.SPR");
	strcpy(path[167], "BATTLE/REZE_D.SPR");
	strcpy(path[168], "BATTLE/RUDO.SPR");
	strcpy(path[65], "BATTLE/RUKA.SEQ");
	strcpy(path[169], "BATTLE/RYU_M.SPR");
	strcpy(path[170], "BATTLE/RYU_W.SPR");
	strcpy(path[171], "BATTLE/SAMU_M.SPR");
	strcpy(path[172], "BATTLE/SAMU_W.SPR");
	strcpy(path[173], "BATTLE/SAN_M.SPR");
	strcpy(path[174], "BATTLE/SAN_W.SPR");
	strcpy(path[175], "BATTLE/SERIA.SPR");
	strcpy(path[176], "BATTLE/SIMON.SPR");
	strcpy(path[177], "BATTLE/SIRO_M.SPR");
	strcpy(path[178], "BATTLE/SIRO_W.SPR");
	strcpy(path[213], "BATTLE/SOURYO.SPR");
	strcpy(path[179], "BATTLE/SUKERU.SPR");
	strcpy(path[180], "BATTLE/SYOU_M.SPR");
	strcpy(path[181], "BATTLE/SYOU_W.SPR");
	strcpy(path[182], "BATTLE/TETSU.SPR");
	strcpy(path[183], "BATTLE/THIEF_M.SPR");
	strcpy(path[184], "BATTLE/THIEF_W.SPR");
	strcpy(path[185], "BATTLE/TOKI_M.SPR");
	strcpy(path[186], "BATTLE/TOKI_W.SPR");
	strcpy(path[187], "BATTLE/TORI.SPR");
	strcpy(path[227], "BATTLE/TORI2.SP2"); // Exact match
	strcpy(path[58], "BATTLE/TYPE1.SEQ");
	strcpy(path[51], "BATTLE/TYPE1.SHP"); // Exact match
	strcpy(path[59], "BATTLE/TYPE2.SEQ");
	strcpy(path[52], "BATTLE/TYPE2.SHP");
	strcpy(path[60], "BATTLE/TYPE3.SEQ");
	strcpy(path[61], "BATTLE/TYPE4.SEQ");
	strcpy(path[188], "BATTLE/URI.SPR");
	strcpy(path[228], "BATTLE/URI2.SP2"); // Exact match
	strcpy(path[189], "BATTLE/VERI.SPR");
	strcpy(path[190], "BATTLE/VORU.SPR");
	strcpy(path[191], "BATTLE/WAJU_M.SPR");
	strcpy(path[192], "BATTLE/WAJU_W.SPR");
	strcpy(path[76], "BATTLE/WEP.SPR");
	strcpy(path[70], "BATTLE/WEP1.SEQ");
	strcpy(path[68], "BATTLE/WEP1.SHP");
	strcpy(path[71], "BATTLE/WEP2.SEQ");
	strcpy(path[69], "BATTLE/WEP2.SHP");
	strcpy(path[193], "BATTLE/WIGU.SPR");
	strcpy(path[194], "BATTLE/YUMI_M.SPR");
	strcpy(path[195], "BATTLE/YUMI_W.SPR");
	strcpy(path[196], "BATTLE/YUREI.SPR");
	strcpy(path[197], "BATTLE/ZARU.SPR");
	strcpy(path[198], "BATTLE/ZARU2.SPR");
	strcpy(path[199], "BATTLE/ZARUE.SPR");
	strcpy(path[200], "BATTLE/ZARUMOU.SPR");
	strcpy(path[233], "BATTLE/ZODIAC.BIN"); // Exact match

	strcpy(path[234], "EFFECT/E000.BIN");
	strcpy(path[235], "EFFECT/E001.BIN");
	strcpy(path[236], "EFFECT/E002.BIN");
	strcpy(path[237], "EFFECT/E003.BIN");
	strcpy(path[238], "EFFECT/E004.BIN");
	strcpy(path[239], "EFFECT/E005.BIN");
	strcpy(path[240], "EFFECT/E006.BIN");
	strcpy(path[241], "EFFECT/E007.BIN");
	strcpy(path[242], "EFFECT/E008.BIN");
	strcpy(path[243], "EFFECT/E009.BIN");
	strcpy(path[244], "EFFECT/E010.BIN");
	strcpy(path[245], "EFFECT/E011.BIN");
	strcpy(path[246], "EFFECT/E012.BIN");
	strcpy(path[247], "EFFECT/E013.BIN");
	strcpy(path[248], "EFFECT/E014.BIN");
	strcpy(path[249], "EFFECT/E015.BIN");
	strcpy(path[250], "EFFECT/E016.BIN");
	strcpy(path[251], "EFFECT/E017.BIN");
	strcpy(path[252], "EFFECT/E018.BIN");
	strcpy(path[253], "EFFECT/E019.BIN");
	strcpy(path[254], "EFFECT/E020.BIN");
	strcpy(path[255], "EFFECT/E021.BIN");
	strcpy(path[256], "EFFECT/E022.BIN");
	strcpy(path[257], "EFFECT/E023.BIN");
	strcpy(path[258], "EFFECT/E024.BIN");
	strcpy(path[259], "EFFECT/E025.BIN");
	strcpy(path[260], "EFFECT/E026.BIN");
	strcpy(path[261], "EFFECT/E027.BIN");
	strcpy(path[262], "EFFECT/E028.BIN");
	strcpy(path[263], "EFFECT/E029.BIN");
	strcpy(path[264], "EFFECT/E030.BIN");
	strcpy(path[265], "EFFECT/E031.BIN");
	strcpy(path[266], "EFFECT/E032.BIN");
	strcpy(path[267], "EFFECT/E033.BIN");
	strcpy(path[268], "EFFECT/E034.BIN");
	strcpy(path[269], "EFFECT/E035.BIN");
	strcpy(path[270], "EFFECT/E036.BIN");
	strcpy(path[271], "EFFECT/E037.BIN"); // dummy
	strcpy(path[272], "EFFECT/E038.BIN"); // dummy
	strcpy(path[273], "EFFECT/E039.BIN");
	strcpy(path[274], "EFFECT/E040.BIN");
	strcpy(path[275], "EFFECT/E041.BIN"); // Tentative
	strcpy(path[276], "EFFECT/E042.BIN"); // dummy
	strcpy(path[277], "EFFECT/E043.BIN");
	strcpy(path[278], "EFFECT/E044.BIN");
	strcpy(path[279], "EFFECT/E045.BIN");
	strcpy(path[280], "EFFECT/E046.BIN");
	strcpy(path[281], "EFFECT/E047.BIN");
	strcpy(path[282], "EFFECT/E048.BIN"); // dummy
	strcpy(path[283], "EFFECT/E049.BIN");
	strcpy(path[284], "EFFECT/E050.BIN");
	strcpy(path[285], "EFFECT/E051.BIN");
	strcpy(path[286], "EFFECT/E052.BIN");
	strcpy(path[287], "EFFECT/E053.BIN");
	strcpy(path[288], "EFFECT/E054.BIN");
	strcpy(path[289], "EFFECT/E055.BIN");
	strcpy(path[290], "EFFECT/E056.BIN");
	strcpy(path[291], "EFFECT/E057.BIN");
	strcpy(path[292], "EFFECT/E058.BIN");
	strcpy(path[293], "EFFECT/E059.BIN");
	strcpy(path[294], "EFFECT/E060.BIN");
	strcpy(path[295], "EFFECT/E061.BIN");
	strcpy(path[296], "EFFECT/E062.BIN");
	strcpy(path[297], "EFFECT/E063.BIN");
	strcpy(path[298], "EFFECT/E064.BIN"); // dummy
	strcpy(path[299], "EFFECT/E065.BIN");
	strcpy(path[300], "EFFECT/E066.BIN");
	strcpy(path[301], "EFFECT/E067.BIN");
	strcpy(path[302], "EFFECT/E068.BIN");
	strcpy(path[303], "EFFECT/E069.BIN");
	strcpy(path[304], "EFFECT/E070.BIN");
	strcpy(path[305], "EFFECT/E071.BIN");
	strcpy(path[306], "EFFECT/E072.BIN");
	strcpy(path[307], "EFFECT/E073.BIN");
	strcpy(path[308], "EFFECT/E074.BIN");
	strcpy(path[309], "EFFECT/E075.BIN"); // dummy
	strcpy(path[310], "EFFECT/E076.BIN");
	strcpy(path[311], "EFFECT/E077.BIN");
	strcpy(path[312], "EFFECT/E078.BIN");
	strcpy(path[313], "EFFECT/E079.BIN");
	strcpy(path[314], "EFFECT/E080.BIN");
	strcpy(path[315], "EFFECT/E081.BIN");
	strcpy(path[316], "EFFECT/E082.BIN");
	strcpy(path[317], "EFFECT/E083.BIN");
	strcpy(path[318], "EFFECT/E084.BIN");
	strcpy(path[319], "EFFECT/E085.BIN");
	strcpy(path[320], "EFFECT/E086.BIN");
	strcpy(path[321], "EFFECT/E087.BIN");
	strcpy(path[322], "EFFECT/E088.BIN");
	strcpy(path[323], "EFFECT/E089.BIN");
	strcpy(path[324], "EFFECT/E090.BIN");
	strcpy(path[325], "EFFECT/E091.BIN");
	strcpy(path[326], "EFFECT/E092.BIN");
	strcpy(path[327], "EFFECT/E093.BIN");
	strcpy(path[328], "EFFECT/E094.BIN");
	strcpy(path[329], "EFFECT/E095.BIN");
	strcpy(path[330], "EFFECT/E096.BIN");
	strcpy(path[331], "EFFECT/E097.BIN");
	strcpy(path[332], "EFFECT/E098.BIN");
	strcpy(path[333], "EFFECT/E099.BIN");
	strcpy(path[334], "EFFECT/E100.BIN");
	strcpy(path[335], "EFFECT/E101.BIN");
	strcpy(path[336], "EFFECT/E102.BIN");
	strcpy(path[337], "EFFECT/E103.BIN");
	strcpy(path[338], "EFFECT/E104.BIN");
	strcpy(path[339], "EFFECT/E105.BIN");
	strcpy(path[340], "EFFECT/E106.BIN");
	strcpy(path[341], "EFFECT/E107.BIN");
	strcpy(path[342], "EFFECT/E108.BIN");
	strcpy(path[343], "EFFECT/E109.BIN");
	strcpy(path[344], "EFFECT/E110.BIN");
	strcpy(path[345], "EFFECT/E111.BIN");
	strcpy(path[346], "EFFECT/E112.BIN");
	strcpy(path[347], "EFFECT/E113.BIN");
	strcpy(path[348], "EFFECT/E114.BIN");
	strcpy(path[349], "EFFECT/E115.BIN");
	strcpy(path[350], "EFFECT/E116.BIN");
	strcpy(path[351], "EFFECT/E117.BIN");
	strcpy(path[352], "EFFECT/E118.BIN");
	strcpy(path[353], "EFFECT/E119.BIN");
	strcpy(path[354], "EFFECT/E120.BIN");
	strcpy(path[355], "EFFECT/E121.BIN");
	strcpy(path[356], "EFFECT/E122.BIN");
	strcpy(path[357], "EFFECT/E123.BIN");
	strcpy(path[358], "EFFECT/E124.BIN");
	strcpy(path[359], "EFFECT/E125.BIN");
	strcpy(path[360], "EFFECT/E126.BIN");
	strcpy(path[361], "EFFECT/E127.BIN");
	strcpy(path[362], "EFFECT/E128.BIN");
	strcpy(path[363], "EFFECT/E129.BIN");
	strcpy(path[364], "EFFECT/E130.BIN");
	strcpy(path[365], "EFFECT/E131.BIN");
	strcpy(path[366], "EFFECT/E132.BIN");
	strcpy(path[367], "EFFECT/E133.BIN");
	strcpy(path[368], "EFFECT/E134.BIN");
	strcpy(path[369], "EFFECT/E135.BIN");
	strcpy(path[370], "EFFECT/E136.BIN");
	strcpy(path[371], "EFFECT/E137.BIN");
	strcpy(path[372], "EFFECT/E138.BIN");
	strcpy(path[373], "EFFECT/E139.BIN");
	strcpy(path[374], "EFFECT/E140.BIN");
	strcpy(path[375], "EFFECT/E141.BIN");
	strcpy(path[376], "EFFECT/E142.BIN");
	strcpy(path[377], "EFFECT/E143.BIN"); // dummy
	strcpy(path[378], "EFFECT/E144.BIN"); // dummy
	strcpy(path[379], "EFFECT/E145.BIN"); // dummy
	strcpy(path[380], "EFFECT/E146.BIN"); // dummy
	strcpy(path[381], "EFFECT/E147.BIN"); // dummy
	strcpy(path[382], "EFFECT/E148.BIN"); // dummy
	strcpy(path[383], "EFFECT/E149.BIN"); // dummy
	strcpy(path[384], "EFFECT/E150.BIN"); // dummy
	strcpy(path[385], "EFFECT/E151.BIN"); // dummy
	strcpy(path[386], "EFFECT/E152.BIN"); // dummy
	strcpy(path[387], "EFFECT/E153.BIN"); // dummy
	strcpy(path[388], "EFFECT/E154.BIN");
	strcpy(path[389], "EFFECT/E155.BIN"); // dummy
	strcpy(path[390], "EFFECT/E156.BIN");
	strcpy(path[391], "EFFECT/E157.BIN");
	strcpy(path[392], "EFFECT/E158.BIN");
	strcpy(path[393], "EFFECT/E159.BIN");
	strcpy(path[394], "EFFECT/E160.BIN");
	strcpy(path[395], "EFFECT/E161.BIN");
	strcpy(path[396], "EFFECT/E162.BIN"); // dummy
	strcpy(path[397], "EFFECT/E163.BIN");
	strcpy(path[398], "EFFECT/E164.BIN");
	strcpy(path[399], "EFFECT/E165.BIN");
	strcpy(path[400], "EFFECT/E166.BIN");
	strcpy(path[401], "EFFECT/E167.BIN"); // Tentative
	strcpy(path[402], "EFFECT/E168.BIN");
	strcpy(path[403], "EFFECT/E169.BIN");
	strcpy(path[404], "EFFECT/E170.BIN");
	strcpy(path[405], "EFFECT/E171.BIN");
	strcpy(path[406], "EFFECT/E172.BIN");
	strcpy(path[407], "EFFECT/E173.BIN");
	strcpy(path[408], "EFFECT/E174.BIN"); // Tentative
	strcpy(path[409], "EFFECT/E175.BIN");
	strcpy(path[410], "EFFECT/E176.BIN"); // Tentative
	strcpy(path[411], "EFFECT/E177.BIN");
	strcpy(path[412], "EFFECT/E178.BIN");
	strcpy(path[413], "EFFECT/E179.BIN");
	strcpy(path[414], "EFFECT/E180.BIN");
	strcpy(path[415], "EFFECT/E181.BIN");
	strcpy(path[416], "EFFECT/E182.BIN");
	strcpy(path[417], "EFFECT/E183.BIN");
	strcpy(path[418], "EFFECT/E184.BIN");
	strcpy(path[419], "EFFECT/E185.BIN");
	strcpy(path[420], "EFFECT/E186.BIN");
	strcpy(path[421], "EFFECT/E187.BIN");
	strcpy(path[422], "EFFECT/E188.BIN");
	strcpy(path[423], "EFFECT/E189.BIN");
	strcpy(path[424], "EFFECT/E190.BIN");
	strcpy(path[425], "EFFECT/E191.BIN");
	strcpy(path[426], "EFFECT/E192.BIN");
	strcpy(path[427], "EFFECT/E193.BIN");
	strcpy(path[428], "EFFECT/E194.BIN");
	strcpy(path[429], "EFFECT/E195.BIN");
	strcpy(path[430], "EFFECT/E196.BIN");
	strcpy(path[431], "EFFECT/E197.BIN");
	strcpy(path[432], "EFFECT/E198.BIN");
	strcpy(path[433], "EFFECT/E199.BIN");
	strcpy(path[434], "EFFECT/E200.BIN");
	strcpy(path[435], "EFFECT/E201.BIN");
	strcpy(path[436], "EFFECT/E202.BIN");
	strcpy(path[437], "EFFECT/E203.BIN");
	strcpy(path[438], "EFFECT/E204.BIN");
	strcpy(path[439], "EFFECT/E205.BIN");
	strcpy(path[440], "EFFECT/E206.BIN");
	strcpy(path[441], "EFFECT/E207.BIN"); // dummy
	strcpy(path[442], "EFFECT/E208.BIN");
	strcpy(path[443], "EFFECT/E209.BIN"); // dummy
	strcpy(path[444], "EFFECT/E210.BIN");
	strcpy(path[445], "EFFECT/E211.BIN");
	strcpy(path[446], "EFFECT/E212.BIN");
	strcpy(path[447], "EFFECT/E213.BIN");
	strcpy(path[448], "EFFECT/E214.BIN");
	strcpy(path[449], "EFFECT/E215.BIN");
	strcpy(path[450], "EFFECT/E216.BIN");
	strcpy(path[451], "EFFECT/E217.BIN");
	strcpy(path[452], "EFFECT/E218.BIN");
	strcpy(path[453], "EFFECT/E219.BIN");
	strcpy(path[454], "EFFECT/E220.BIN");
	strcpy(path[455], "EFFECT/E221.BIN");
	strcpy(path[456], "EFFECT/E222.BIN");
	strcpy(path[457], "EFFECT/E223.BIN");
	strcpy(path[458], "EFFECT/E224.BIN");
	strcpy(path[459], "EFFECT/E225.BIN");
	strcpy(path[460], "EFFECT/E226.BIN");
	strcpy(path[461], "EFFECT/E227.BIN"); // dummy
	strcpy(path[462], "EFFECT/E228.BIN"); // Tentative
	strcpy(path[463], "EFFECT/E229.BIN"); // Tentative
	strcpy(path[464], "EFFECT/E230.BIN"); // Tentative
	strcpy(path[465], "EFFECT/E231.BIN");
	strcpy(path[466], "EFFECT/E232.BIN");
	strcpy(path[467], "EFFECT/E233.BIN"); // Tentative
	strcpy(path[468], "EFFECT/E234.BIN");
	strcpy(path[469], "EFFECT/E235.BIN"); // Tentative
	strcpy(path[470], "EFFECT/E236.BIN"); // Tentative
	strcpy(path[471], "EFFECT/E237.BIN");
	strcpy(path[472], "EFFECT/E238.BIN");
	strcpy(path[473], "EFFECT/E239.BIN");
	strcpy(path[474], "EFFECT/E240.BIN"); // Tentative
	strcpy(path[475], "EFFECT/E241.BIN"); // Tentative
	strcpy(path[476], "EFFECT/E242.BIN"); // Tentative
	strcpy(path[477], "EFFECT/E243.BIN");
	strcpy(path[478], "EFFECT/E244.BIN"); // Tentative
	strcpy(path[479], "EFFECT/E245.BIN"); // Tentative
	strcpy(path[480], "EFFECT/E246.BIN"); // Tentative
	strcpy(path[481], "EFFECT/E247.BIN"); // Tentative
	strcpy(path[482], "EFFECT/E248.BIN"); // Tentative
	strcpy(path[483], "EFFECT/E249.BIN"); // Tentative
	strcpy(path[484], "EFFECT/E250.BIN"); // dummy
	strcpy(path[485], "EFFECT/E251.BIN"); // dummy
	strcpy(path[486], "EFFECT/E252.BIN"); // dummy
	strcpy(path[487], "EFFECT/E253.BIN"); // dummy
	strcpy(path[488], "EFFECT/E254.BIN"); // dummy
	strcpy(path[489], "EFFECT/E255.BIN"); // Tentative
	strcpy(path[490], "EFFECT/E256.BIN"); // Tentative
	strcpy(path[491], "EFFECT/E257.BIN"); // Tentative
	strcpy(path[492], "EFFECT/E258.BIN"); // Tentative
	strcpy(path[493], "EFFECT/E259.BIN"); // Tentative
	strcpy(path[494], "EFFECT/E260.BIN"); // Exact match
	strcpy(path[495], "EFFECT/E261.BIN");
	strcpy(path[496], "EFFECT/E262.BIN");
	strcpy(path[497], "EFFECT/E263.BIN");
	strcpy(path[498], "EFFECT/E264.BIN");
	strcpy(path[499], "EFFECT/E265.BIN");
	strcpy(path[500], "EFFECT/E266.BIN");
	strcpy(path[501], "EFFECT/E267.BIN");
	strcpy(path[502], "EFFECT/E268.BIN");
	strcpy(path[503], "EFFECT/E269.BIN");
	strcpy(path[504], "EFFECT/E270.BIN");
	strcpy(path[505], "EFFECT/E271.BIN");
	strcpy(path[506], "EFFECT/E272.BIN");
	strcpy(path[507], "EFFECT/E273.BIN");
	strcpy(path[508], "EFFECT/E274.BIN"); // dummy
	strcpy(path[509], "EFFECT/E275.BIN"); // dummy
	strcpy(path[510], "EFFECT/E276.BIN"); // dummy
	strcpy(path[511], "EFFECT/E277.BIN"); // dummy
	strcpy(path[512], "EFFECT/E278.BIN"); // dummy
	strcpy(path[513], "EFFECT/E279.BIN"); // dummy
	strcpy(path[514], "EFFECT/E280.BIN"); // dummy
	strcpy(path[515], "EFFECT/E281.BIN"); // dummy
	strcpy(path[516], "EFFECT/E282.BIN"); // dummy
	strcpy(path[517], "EFFECT/E283.BIN"); // dummy
	strcpy(path[518], "EFFECT/E284.BIN"); // dummy
	strcpy(path[519], "EFFECT/E285.BIN"); // dummy
	strcpy(path[520], "EFFECT/E286.BIN"); // dummy
	strcpy(path[521], "EFFECT/E287.BIN"); // dummy
	strcpy(path[522], "EFFECT/E288.BIN");
	strcpy(path[523], "EFFECT/E289.BIN");
	strcpy(path[524], "EFFECT/E290.BIN");
	strcpy(path[525], "EFFECT/E291.BIN");
	strcpy(path[526], "EFFECT/E292.BIN");
	strcpy(path[527], "EFFECT/E293.BIN");
	strcpy(path[528], "EFFECT/E294.BIN");
	strcpy(path[529], "EFFECT/E295.BIN");
	strcpy(path[530], "EFFECT/E296.BIN"); // dummy
	strcpy(path[531], "EFFECT/E297.BIN"); // dummy
	strcpy(path[532], "EFFECT/E298.BIN"); // dummy
	strcpy(path[533], "EFFECT/E299.BIN"); // dummy
	strcpy(path[534], "EFFECT/E300.BIN"); // dummy
	strcpy(path[535], "EFFECT/E301.BIN"); // dummy
	strcpy(path[536], "EFFECT/E302.BIN"); // dummy
	strcpy(path[537], "EFFECT/E303.BIN"); // dummy
	strcpy(path[538], "EFFECT/E304.BIN"); // dummy
	strcpy(path[539], "EFFECT/E305.BIN"); // dummy
	strcpy(path[540], "EFFECT/E306.BIN");
	strcpy(path[541], "EFFECT/E307.BIN");
	strcpy(path[542], "EFFECT/E308.BIN");
	strcpy(path[543], "EFFECT/E309.BIN");
	strcpy(path[544], "EFFECT/E310.BIN");
	strcpy(path[545], "EFFECT/E311.BIN");
	strcpy(path[546], "EFFECT/E312.BIN");
	strcpy(path[547], "EFFECT/E313.BIN");
	strcpy(path[548], "EFFECT/E314.BIN");
	strcpy(path[549], "EFFECT/E315.BIN"); // dummy
	strcpy(path[550], "EFFECT/E316.BIN");
	strcpy(path[551], "EFFECT/E317.BIN");
	strcpy(path[552], "EFFECT/E318.BIN");
	strcpy(path[553], "EFFECT/E319.BIN");
	strcpy(path[554], "EFFECT/E320.BIN");
	strcpy(path[555], "EFFECT/E321.BIN");
	strcpy(path[556], "EFFECT/E322.BIN");
	strcpy(path[557], "EFFECT/E323.BIN");
	strcpy(path[558], "EFFECT/E324.BIN");
	strcpy(path[559], "EFFECT/E325.BIN");
	strcpy(path[560], "EFFECT/E326.BIN");
	strcpy(path[561], "EFFECT/E327.BIN");
	strcpy(path[562], "EFFECT/E328.BIN");
	strcpy(path[563], "EFFECT/E329.BIN");
	strcpy(path[564], "EFFECT/E330.BIN");
	strcpy(path[565], "EFFECT/E331.BIN");
	strcpy(path[566], "EFFECT/E332.BIN");
	strcpy(path[567], "EFFECT/E333.BIN");
	strcpy(path[568], "EFFECT/E334.BIN");
	strcpy(path[569], "EFFECT/E335.BIN");
	strcpy(path[570], "EFFECT/E336.BIN");
	strcpy(path[571], "EFFECT/E337.BIN");
	strcpy(path[572], "EFFECT/E338.BIN");
	strcpy(path[573], "EFFECT/E339.BIN");
	strcpy(path[574], "EFFECT/E340.BIN");
	strcpy(path[575], "EFFECT/E341.BIN");
	strcpy(path[576], "EFFECT/E342.BIN");
	strcpy(path[577], "EFFECT/E343.BIN");
	strcpy(path[578], "EFFECT/E344.BIN");
	strcpy(path[579], "EFFECT/E345.BIN");
	strcpy(path[580], "EFFECT/E346.BIN");
	strcpy(path[581], "EFFECT/E347.BIN");
	strcpy(path[582], "EFFECT/E348.BIN");
	strcpy(path[583], "EFFECT/E349.BIN");
	strcpy(path[584], "EFFECT/E350.BIN");
	strcpy(path[585], "EFFECT/E351.BIN");
	strcpy(path[586], "EFFECT/E352.BIN");
	strcpy(path[587], "EFFECT/E353.BIN");
	strcpy(path[588], "EFFECT/E354.BIN");
	strcpy(path[589], "EFFECT/E355.BIN");
	strcpy(path[590], "EFFECT/E356.BIN");
	strcpy(path[591], "EFFECT/E357.BIN");
	strcpy(path[592], "EFFECT/E358.BIN");
	strcpy(path[593], "EFFECT/E359.BIN");
	strcpy(path[594], "EFFECT/E360.BIN");
	strcpy(path[595], "EFFECT/E361.BIN");
	strcpy(path[596], "EFFECT/E362.BIN");
	strcpy(path[597], "EFFECT/E363.BIN");
	strcpy(path[598], "EFFECT/E364.BIN");
	strcpy(path[599], "EFFECT/E365.BIN");
	strcpy(path[600], "EFFECT/E366.BIN");
	strcpy(path[601], "EFFECT/E367.BIN");
	strcpy(path[602], "EFFECT/E368.BIN");
	strcpy(path[603], "EFFECT/E369.BIN");
	strcpy(path[604], "EFFECT/E370.BIN");
	strcpy(path[605], "EFFECT/E371.BIN");
	strcpy(path[606], "EFFECT/E372.BIN");
	strcpy(path[607], "EFFECT/E373.BIN");
	strcpy(path[608], "EFFECT/E374.BIN");
	strcpy(path[609], "EFFECT/E375.BIN");
	strcpy(path[610], "EFFECT/E376.BIN");
	strcpy(path[611], "EFFECT/E377.BIN");
	strcpy(path[612], "EFFECT/E378.BIN");
	strcpy(path[613], "EFFECT/E379.BIN");
	strcpy(path[614], "EFFECT/E380.BIN");
	strcpy(path[615], "EFFECT/E381.BIN");
	strcpy(path[616], "EFFECT/E382.BIN");
	strcpy(path[617], "EFFECT/E383.BIN");
	strcpy(path[618], "EFFECT/E384.BIN");
	strcpy(path[619], "EFFECT/E385.BIN");
	strcpy(path[620], "EFFECT/E386.BIN");
	strcpy(path[621], "EFFECT/E387.BIN");
	strcpy(path[622], "EFFECT/E388.BIN");
	strcpy(path[623], "EFFECT/E389.BIN");
	strcpy(path[624], "EFFECT/E390.BIN");
	strcpy(path[625], "EFFECT/E391.BIN");
	strcpy(path[626], "EFFECT/E392.BIN");
	strcpy(path[627], "EFFECT/E393.BIN");
	strcpy(path[628], "EFFECT/E394.BIN");
	strcpy(path[629], "EFFECT/E395.BIN");
	strcpy(path[630], "EFFECT/E396.BIN");
	strcpy(path[631], "EFFECT/E397.BIN");
	strcpy(path[632], "EFFECT/E398.BIN");
	strcpy(path[633], "EFFECT/E399.BIN");
	strcpy(path[634], "EFFECT/E400.BIN");
	strcpy(path[635], "EFFECT/E401.BIN");
	strcpy(path[636], "EFFECT/E402.BIN");
	strcpy(path[637], "EFFECT/E403.BIN");
	strcpy(path[638], "EFFECT/E404.BIN");
	strcpy(path[639], "EFFECT/E405.BIN");
	strcpy(path[640], "EFFECT/E406.BIN");
	strcpy(path[641], "EFFECT/E407.BIN");
	strcpy(path[642], "EFFECT/E408.BIN");
	strcpy(path[643], "EFFECT/E409.BIN");
	strcpy(path[644], "EFFECT/E410.BIN");
	strcpy(path[645], "EFFECT/E411.BIN");
	strcpy(path[646], "EFFECT/E412.BIN");
	strcpy(path[647], "EFFECT/E413.BIN");
	strcpy(path[648], "EFFECT/E414.BIN"); // dummy
	strcpy(path[649], "EFFECT/E415.BIN"); // dummy
	strcpy(path[650], "EFFECT/E416.BIN"); // dummy
	strcpy(path[651], "EFFECT/E417.BIN"); // dummy
	strcpy(path[652], "EFFECT/E418.BIN"); // dummy
	strcpy(path[653], "EFFECT/E419.BIN"); // dummy
	strcpy(path[654], "EFFECT/E420.BIN"); // dummy
	strcpy(path[655], "EFFECT/E421.BIN"); // dummy
	strcpy(path[656], "EFFECT/E422.BIN"); // dummy
	strcpy(path[657], "EFFECT/E423.BIN"); // dummy
	strcpy(path[658], "EFFECT/E424.BIN"); // dummy
	strcpy(path[659], "EFFECT/E425.BIN"); // dummy
	strcpy(path[660], "EFFECT/E426.BIN"); // dummy
	strcpy(path[661], "EFFECT/E427.BIN"); // dummy
	strcpy(path[662], "EFFECT/E428.BIN"); // dummy
	strcpy(path[663], "EFFECT/E429.BIN"); // dummy
	strcpy(path[664], "EFFECT/E430.BIN"); // dummy
	strcpy(path[665], "EFFECT/E431.BIN"); // dummy
	strcpy(path[666], "EFFECT/E432.BIN"); // dummy
	strcpy(path[667], "EFFECT/E433.BIN"); // dummy
	strcpy(path[668], "EFFECT/E434.BIN"); // dummy
	strcpy(path[669], "EFFECT/E435.BIN"); // dummy
	strcpy(path[670], "EFFECT/E436.BIN"); // dummy
	strcpy(path[671], "EFFECT/E437.BIN"); // dummy
	strcpy(path[672], "EFFECT/E438.BIN"); // dummy
	strcpy(path[673], "EFFECT/E439.BIN"); // dummy
	strcpy(path[674], "EFFECT/E440.BIN"); // dummy
	strcpy(path[675], "EFFECT/E441.BIN"); // dummy
	strcpy(path[676], "EFFECT/E442.BIN"); // dummy
	strcpy(path[677], "EFFECT/E443.BIN"); // dummy
	strcpy(path[678], "EFFECT/E444.BIN"); // dummy
	strcpy(path[679], "EFFECT/E445.BIN"); // dummy
	strcpy(path[680], "EFFECT/E446.BIN"); // dummy
	strcpy(path[681], "EFFECT/E447.BIN"); // dummy
	strcpy(path[682], "EFFECT/E448.BIN"); // dummy
	strcpy(path[683], "EFFECT/E449.BIN"); // dummy
	strcpy(path[684], "EFFECT/E450.BIN"); // Tentative
	strcpy(path[685], "EFFECT/E451.BIN"); // Tentative
	strcpy(path[686], "EFFECT/E452.BIN"); // Tentative
	strcpy(path[687], "EFFECT/E453.BIN"); // Tentative
	strcpy(path[688], "EFFECT/E454.BIN"); // Tentative
	strcpy(path[689], "EFFECT/E455.BIN"); // Tentative
	strcpy(path[690], "EFFECT/E456.BIN"); // Tentative
	strcpy(path[691], "EFFECT/E457.BIN");
	strcpy(path[692], "EFFECT/E458.BIN"); // Tentative
	strcpy(path[693], "EFFECT/E459.BIN");
	strcpy(path[694], "EFFECT/E460.BIN"); // Tentative
	strcpy(path[695], "EFFECT/E461.BIN"); // Tentative
	strcpy(path[696], "EFFECT/E462.BIN"); // Tentative
	strcpy(path[697], "EFFECT/E463.BIN"); // Tentative
	strcpy(path[698], "EFFECT/E464.BIN"); // Tentative
	strcpy(path[699], "EFFECT/E465.BIN"); // Tentative
	strcpy(path[700], "EFFECT/E466.BIN"); // Tentative
	strcpy(path[701], "EFFECT/E467.BIN"); // Tentative
	strcpy(path[702], "EFFECT/E468.BIN"); // Tentative
	strcpy(path[703], "EFFECT/E469.BIN"); // Tentative
	strcpy(path[704], "EFFECT/E470.BIN");
	strcpy(path[705], "EFFECT/E471.BIN"); // Tentative
	strcpy(path[706], "EFFECT/E472.BIN"); // Tentative
	strcpy(path[707], "EFFECT/E473.BIN"); // Tentative
	strcpy(path[708], "EFFECT/E474.BIN"); // Tentative
	strcpy(path[709], "EFFECT/E475.BIN"); // Tentative
	strcpy(path[710], "EFFECT/E476.BIN"); // Tentative
	strcpy(path[711], "EFFECT/E477.BIN"); // Tentative
	strcpy(path[712], "EFFECT/E478.BIN"); // Tentative
	strcpy(path[713], "EFFECT/E479.BIN");
	strcpy(path[714], "EFFECT/E480.BIN"); // Tentative
	strcpy(path[715], "EFFECT/E481.BIN"); // Tentative
	strcpy(path[716], "EFFECT/E482.BIN"); // Tentative
	strcpy(path[717], "EFFECT/E483.BIN"); // Tentative
	strcpy(path[718], "EFFECT/E484.BIN");
	strcpy(path[719], "EFFECT/E485.BIN");
	strcpy(path[720], "EFFECT/E486.BIN");
	strcpy(path[721], "EFFECT/E487.BIN"); // dummy
	strcpy(path[722], "EFFECT/E488.BIN"); // dummy
	strcpy(path[723], "EFFECT/E489.BIN"); // dummy
	strcpy(path[724], "EFFECT/E490.BIN"); // dummy
	strcpy(path[725], "EFFECT/E491.BIN"); // dummy
	strcpy(path[726], "EFFECT/E492.BIN"); // dummy
	strcpy(path[727], "EFFECT/E493.BIN"); // dummy
	strcpy(path[728], "EFFECT/E494.BIN"); // dummy
	strcpy(path[729], "EFFECT/E495.BIN"); // dummy
	strcpy(path[730], "EFFECT/E496.BIN"); // dummy
	strcpy(path[731], "EFFECT/E497.BIN"); // dummy
	strcpy(path[732], "EFFECT/E498.BIN"); // dummy
	strcpy(path[733], "EFFECT/E499.BIN"); // dummy
	strcpy(path[734], "EFFECT/E500.BIN"); // dummy
	strcpy(path[735], "EFFECT/E501.BIN"); // dummy
	strcpy(path[736], "EFFECT/E502.BIN"); // dummy
	strcpy(path[737], "EFFECT/E503.BIN"); // dummy
	strcpy(path[738], "EFFECT/E504.BIN"); // dummy
	strcpy(path[739], "EFFECT/E505.BIN"); // dummy
	strcpy(path[740], "EFFECT/E506.BIN"); // dummy
	strcpy(path[741], "EFFECT/E507.BIN"); // dummy
	strcpy(path[742], "EFFECT/E508.BIN"); // dummy
	strcpy(path[743], "EFFECT/E509.BIN");
	strcpy(path[744], "EFFECT/E510.BIN");
	strcpy(path[745], "EFFECT/E511.BIN"); // dummy
	
	
	
	
	
	
	strcpy(path[12], "EVENT/ATTACK.OUT");
	strcpy(path[41], "EVENT/ATCHELP.LZW");
	// EVENT/BONUS.BIN not found (958464 bytes)
	strcpy(path[25], "EVENT/BTLEVT.BIN");
	strcpy(path[17], "EVENT/BUNIT.OUT");
	strcpy(path[16], "EVENT/CARD.OUT");
	strcpy(path[26], "EVENT/CHAPTER1.OUT");
	strcpy(path[27], "EVENT/CHAPTER2.OUT");
	strcpy(path[28], "EVENT/CHAPTER3.OUT");
	strcpy(path[29], "EVENT/CHAPTER4.OUT");
	strcpy(path[13], "EVENT/DEBUGCHR.OUT");
	strcpy(path[7], "EVENT/DEBUGMAP.OUT"); // dummy
	strcpy(path[36], "EVENT/END1.BIN");
	strcpy(path[37], "EVENT/END2.BIN");
	strcpy(path[38], "EVENT/END3.BIN");
	strcpy(path[39], "EVENT/END4.BIN");
	strcpy(path[40], "EVENT/END5.BIN");
	strcpy(path[15], "EVENT/EQUIP.OUT");
	strcpy(path[14], "EVENT/ETC.OUT");
	strcpy(path[49], "EVENT/EVTCHR.BIN"); // Tentatively
	strcpy(path[23], "EVENT/EVTFACE.BIN"); // Exact match
	strcpy(path[18], "EVENT/EVTOOL.OUT"); // dummy
	strcpy(path[20], "EVENT/FONT.BIN");
	strcpy(path[21], "EVENT/FRAME.BIN");
	strcpy(path[30], "EVENT/GAMEOVER.BIN");
	strcpy(path[47], "EVENT/HELP.LZW");
	strcpy(path[10], "EVENT/HELPMENU.OUT");
	// EVENT/ITEM.BIN not found (33280 bytes)
	strcpy(path[11], "EVENT/JOBSTTS.OUT");
	strcpy(path[46], "EVENT/JOIN.LZW");
	// EVENT/MAPTITLE.BIN not found (307200 bytes)
	strcpy(path[45], "EVENT/OPEN.LZW");
	strcpy(path[8], "EVENT/OPTION.OUT");
	strcpy(path[9], "EVENT/REQUIRE.OUT");
	strcpy(path[43], "EVENT/SAMPLE.LZW");
	strcpy(path[6], "EVENT/SMALL.OUT");
	// EVENT/SPELL.MES not found (16984 bytes)
	strcpy(path[22], "EVENT/TEST.EVT"); // ???
	strcpy(path[24], "EVENT/UNIT.BIN");
	strcpy(path[32], "EVENT/WIN001.BIN"); // Exact match
	strcpy(path[35], "EVENT/WLDFACE.BIN"); // Exact match
	strcpy(path[34], "EVENT/WLDFACE4.BIN"); // Exact match
	strcpy(path[48], "EVENT/WLDHELP.LZW");
	// EVENT/WORLD.LZW not found (56508 bytes)
	strcpy(path[2142], "MAP/MAP000.5");
	strcpy(path[2143], "MAP/MAP000.GNS");
	strcpy(path[2144], "MAP/MAP001.8"); // Exact match
	strcpy(path[2145], "MAP/MAP001.9");
	strcpy(path[2146], "MAP/MAP001.11");
	strcpy(path[2147], "MAP/MAP001.14"); // Exact match
	strcpy(path[2148], "MAP/MAP001.15");
	strcpy(path[2180], "MAP/MAP001.18"); // Exact match
	strcpy(path[2150], "MAP/MAP001.19");
	strcpy(path[2182], "MAP/MAP001.22"); // Exact match
	strcpy(path[2152], "MAP/MAP001.23");
	strcpy(path[2149], "MAP/MAP001.26"); // Exact match
	strcpy(path[2154], "MAP/MAP001.27");
	strcpy(path[2157], "MAP/MAP001.30"); // Exact match
	strcpy(path[2156], "MAP/MAP001.31");
	strcpy(path[2155], "MAP/MAP001.34"); // Exact match
	strcpy(path[2158], "MAP/MAP001.35");
	strcpy(path[2153], "MAP/MAP001.38"); // Exact match
	strcpy(path[2160], "MAP/MAP001.39");
	strcpy(path[2151], "MAP/MAP001.42"); // Exact match
	strcpy(path[2162], "MAP/MAP001.43");
	strcpy(path[2159], "MAP/MAP001.46"); // Exact match
	strcpy(path[2164], "MAP/MAP001.47");
	strcpy(path[2165], "MAP/MAP001.48");
	strcpy(path[2166], "MAP/MAP001.52"); // Exact match
	strcpy(path[2167], "MAP/MAP001.53");
	strcpy(path[2163], "MAP/MAP001.57"); // Exact match
	strcpy(path[2169], "MAP/MAP001.58");
	strcpy(path[2161], "MAP/MAP001.62"); // Exact match
	strcpy(path[2171], "MAP/MAP001.63");
	strcpy(path[2168], "MAP/MAP001.67"); // Exact match
	strcpy(path[2173], "MAP/MAP001.68");
	strcpy(path[2178], "MAP/MAP001.72"); // Exact match
	strcpy(path[2175], "MAP/MAP001.73");
	strcpy(path[2174], "MAP/MAP001.77"); // Exact match
	strcpy(path[2177], "MAP/MAP001.78");
	strcpy(path[2176], "MAP/MAP001.82"); // Exact match
	strcpy(path[2179], "MAP/MAP001.83");
	strcpy(path[2170], "MAP/MAP001.87"); // Exact match
	strcpy(path[2181], "MAP/MAP001.88");
	strcpy(path[2172], "MAP/MAP001.92"); // Exact match
	strcpy(path[2183], "MAP/MAP001.93");
	// MAP/MAP001.GNS not found (2388 bytes)
	strcpy(path[2207], "MAP/MAP002.12"); // Exact match
	strcpy(path[2188], "MAP/MAP002.13");
	strcpy(path[2189], "MAP/MAP002.16"); // Exact match
	strcpy(path[2210], "MAP/MAP002.17");
	strcpy(path[2211], "MAP/MAP002.20"); // Exact match
	strcpy(path[2212], "MAP/MAP002.21");
	strcpy(path[2213], "MAP/MAP002.24"); // Exact match
	strcpy(path[2214], "MAP/MAP002.25");
	strcpy(path[2215], "MAP/MAP002.28"); // Exact match
	strcpy(path[2216], "MAP/MAP002.29");
	strcpy(path[2217], "MAP/MAP002.32"); // Exact match
	strcpy(path[2218], "MAP/MAP002.33");
	strcpy(path[2219], "MAP/MAP002.36"); // Exact match
	strcpy(path[2200], "MAP/MAP002.37");
	strcpy(path[2193], "MAP/MAP002.40"); // Exact match
	strcpy(path[2202], "MAP/MAP002.41");
	strcpy(path[2191], "MAP/MAP002.44"); // Exact match
	strcpy(path[2204], "MAP/MAP002.45");
	strcpy(path[2205], "MAP/MAP002.54"); // Exact match
	strcpy(path[2206], "MAP/MAP002.55");
	strcpy(path[2187], "MAP/MAP002.58"); // Exact match
	strcpy(path[2208], "MAP/MAP002.59");
	strcpy(path[2195], "MAP/MAP002.62"); // Exact match
	strcpy(path[2190], "MAP/MAP002.63");
	strcpy(path[2197], "MAP/MAP002.66"); // Exact match
	strcpy(path[2192], "MAP/MAP002.67");
	strcpy(path[2199], "MAP/MAP002.70"); // Exact match
	strcpy(path[2194], "MAP/MAP002.71");
	strcpy(path[2209], "MAP/MAP002.74"); // Exact match
	strcpy(path[2196], "MAP/MAP002.75");
	strcpy(path[2203], "MAP/MAP002.78"); // Exact match
	strcpy(path[2198], "MAP/MAP002.79");
	strcpy(path[2201], "MAP/MAP002.82"); // Exact match
	strcpy(path[2220], "MAP/MAP002.83");
	strcpy(path[2221], "MAP/MAP002.86"); // Exact match
	strcpy(path[2222], "MAP/MAP002.87");
	strcpy(path[2185], "MAP/MAP002.8"); // Exact match
	strcpy(path[2186], "MAP/MAP002.9");
	strcpy(path[2223], "MAP/MAP002.90"); // Exact match
	strcpy(path[2224], "MAP/MAP002.91");
	// MAP/MAP002.GNS not found (2288 bytes)
	strcpy(path[2229], "MAP/MAP003.12"); // Exact match
	strcpy(path[2230], "MAP/MAP003.13");
	strcpy(path[2231], "MAP/MAP003.16"); // Exact match
	strcpy(path[2232], "MAP/MAP003.17");
	strcpy(path[2233], "MAP/MAP003.20"); // Exact match
	strcpy(path[2234], "MAP/MAP003.21");
	strcpy(path[2227], "MAP/MAP003.8"); // Exact match
	strcpy(path[2228], "MAP/MAP003.9");
	// MAP/MAP003.GNS not found (568 bytes)
	strcpy(path[2239], "MAP/MAP004.13");
	strcpy(path[2240], "MAP/MAP004.18");
	strcpy(path[2247], "MAP/MAP004.23"); // Exact match
	strcpy(path[2242], "MAP/MAP004.24");
	strcpy(path[2249], "MAP/MAP004.29"); // Exact match
	strcpy(path[2244], "MAP/MAP004.30");
	strcpy(path[2251], "MAP/MAP004.35"); // Exact match
	strcpy(path[2246], "MAP/MAP004.36");
	strcpy(path[2241], "MAP/MAP004.41"); // Exact match
	strcpy(path[2248], "MAP/MAP004.42");
	strcpy(path[2243], "MAP/MAP004.47"); // Exact match
	strcpy(path[2250], "MAP/MAP004.48");
	strcpy(path[2245], "MAP/MAP004.53"); // Exact match
	strcpy(path[2252], "MAP/MAP004.54");
	strcpy(path[2237], "MAP/MAP004.7"); // Exact match
	strcpy(path[2238], "MAP/MAP004.8");
	// MAP/MAP004.GNS not found (1368 bytes)
	strcpy(path[2257], "MAP/MAP005.10"); // Exact match
	strcpy(path[2258], "MAP/MAP005.11");
	strcpy(path[2259], "MAP/MAP005.13"); // Exact match
	strcpy(path[2260], "MAP/MAP005.14");
	strcpy(path[2261], "MAP/MAP005.16"); // Exact match
	strcpy(path[2262], "MAP/MAP005.17");
	strcpy(path[2263], "MAP/MAP005.19"); // Exact match
	strcpy(path[2264], "MAP/MAP005.20");
	strcpy(path[2265], "MAP/MAP005.23"); // Exact match
	strcpy(path[2266], "MAP/MAP005.24");
	strcpy(path[2267], "MAP/MAP005.27"); // Exact match
	strcpy(path[2268], "MAP/MAP005.28");
	strcpy(path[2269], "MAP/MAP005.31"); // Exact match
	strcpy(path[2270], "MAP/MAP005.32");
	strcpy(path[2273], "MAP/MAP005.35"); // Exact match
	strcpy(path[2272], "MAP/MAP005.36");
	strcpy(path[2271], "MAP/MAP005.39"); // Exact match
	strcpy(path[2274], "MAP/MAP005.40");
	strcpy(path[2255], "MAP/MAP005.7"); // Exact match
	strcpy(path[2256], "MAP/MAP005.8");
	// MAP/MAP005.GNS not found (1068 bytes)
	strcpy(path[2277], "MAP/MAP006.11"); // Exact match
	strcpy(path[2278], "MAP/MAP006.12");
	strcpy(path[2279], "MAP/MAP006.13");
	strcpy(path[2282], "MAP/MAP006.15"); // Exact match
	strcpy(path[2281], "MAP/MAP006.16");
	strcpy(path[2280], "MAP/MAP006.20"); // Exact match
	strcpy(path[2283], "MAP/MAP006.21");
	strcpy(path[2287], "MAP/MAP006.25"); // Exact match
	strcpy(path[2285], "MAP/MAP006.26");
	strcpy(path[2286], "MAP/MAP006.31");
	strcpy(path[2284], "MAP/MAP006.35"); // Exact match
	strcpy(path[2288], "MAP/MAP006.36");
	strcpy(path[2289], "MAP/MAP006.42"); // Exact match
	strcpy(path[2290], "MAP/MAP006.43");
	strcpy(path[2291], "MAP/MAP006.49"); // Exact match
	strcpy(path[2292], "MAP/MAP006.50");
	// MAP/MAP006.GNS not found (1468 bytes)
	strcpy(path[2297], "MAP/MAP007.11"); // Exact match
	strcpy(path[2298], "MAP/MAP007.12");
	strcpy(path[2299], "MAP/MAP007.15"); // Exact match
	strcpy(path[2300], "MAP/MAP007.16");
	strcpy(path[2301], "MAP/MAP007.19"); // Exact match
	strcpy(path[2302], "MAP/MAP007.20");
	strcpy(path[2295], "MAP/MAP007.8"); // Exact match
	strcpy(path[2296], "MAP/MAP007.9");
	// MAP/MAP007.GNS not found (628 bytes)
	strcpy(path[2307], "MAP/MAP008.11"); // Exact match
	strcpy(path[2308], "MAP/MAP008.12");
	strcpy(path[2309], "MAP/MAP008.15"); // Exact match
	strcpy(path[2310], "MAP/MAP008.16");
	strcpy(path[2311], "MAP/MAP008.19"); // Exact match
	strcpy(path[2320], "MAP/MAP008.20");
	strcpy(path[2305], "MAP/MAP008.27"); // Exact match
	strcpy(path[2314], "MAP/MAP008.28");
	strcpy(path[2315], "MAP/MAP008.31"); // Exact match
	strcpy(path[2316], "MAP/MAP008.32");
	strcpy(path[2317], "MAP/MAP008.35"); // Exact match
	strcpy(path[2318], "MAP/MAP008.36");
	strcpy(path[2319], "MAP/MAP008.39"); // Exact match
	strcpy(path[2312], "MAP/MAP008.40");
	strcpy(path[2313], "MAP/MAP008.7"); // Exact match
	strcpy(path[2306], "MAP/MAP008.8");
	// MAP/MAP008.GNS not found (1028 bytes)
	strcpy(path[2325], "MAP/MAP009.13"); // Exact match
	strcpy(path[2326], "MAP/MAP009.14");
	strcpy(path[2329], "MAP/MAP009.18"); // Exact match
	strcpy(path[2328], "MAP/MAP009.19");
	strcpy(path[2327], "MAP/MAP009.23"); // Exact match
	strcpy(path[2330], "MAP/MAP009.24");
	strcpy(path[2339], "MAP/MAP009.28"); // Exact match
	strcpy(path[2332], "MAP/MAP009.29");
	strcpy(path[2333], "MAP/MAP009.34"); // Exact match
	strcpy(path[2334], "MAP/MAP009.35");
	strcpy(path[2337], "MAP/MAP009.40"); // Exact match
	strcpy(path[2336], "MAP/MAP009.41");
	strcpy(path[2335], "MAP/MAP009.46"); // Exact match
	strcpy(path[2338], "MAP/MAP009.47");
	strcpy(path[2331], "MAP/MAP009.52"); // Exact match
	strcpy(path[2340], "MAP/MAP009.53");
	strcpy(path[2341], "MAP/MAP009.58"); // Exact match
	strcpy(path[2342], "MAP/MAP009.59");
	strcpy(path[2323], "MAP/MAP009.8"); // Exact match
	strcpy(path[2324], "MAP/MAP009.9");
	// MAP/MAP009.GNS not found (1468 bytes)
	strcpy(path[2344], "MAP/MAP010.8"); // Exact match
	strcpy(path[2345], "MAP/MAP010.9");
	// MAP/MAP010.GNS not found (248 bytes)
	strcpy(path[2359], "MAP/MAP011.11"); // Exact match
	strcpy(path[2351], "MAP/MAP011.12");
	strcpy(path[2352], "MAP/MAP011.14");
	strcpy(path[2350], "MAP/MAP011.17"); // Exact match
	strcpy(path[2354], "MAP/MAP011.18");
	strcpy(path[3821], "MAP/MAP011.22");
	strcpy(path[2353], "MAP/MAP011.27"); // Exact match
	strcpy(path[3822], "MAP/MAP011.28");
	strcpy(path[2358], "MAP/MAP011.33");
	strcpy(path[2356], "MAP/MAP011.39"); // Exact match
	strcpy(path[2360], "MAP/MAP011.40");
	strcpy(path[2361], "MAP/MAP011.43");
	strcpy(path[2362], "MAP/MAP011.47");
	strcpy(path[2363], "MAP/MAP011.49");
	strcpy(path[3827], "MAP/MAP011.53");
	strcpy(path[2347], "MAP/MAP011.7"); // Exact match
	strcpy(path[2348], "MAP/MAP011.8");
	strcpy(path[2349], "MAP/MAP011.9");
	// MAP/MAP011.GNS not found (1548 bytes)
	strcpy(path[2369], "MAP/MAP012.10");
	strcpy(path[2370], "MAP/MAP012.14"); // Exact match
	strcpy(path[2371], "MAP/MAP012.15");
	strcpy(path[2372], "MAP/MAP012.19"); // Exact match
	strcpy(path[2373], "MAP/MAP012.20");
	strcpy(path[2374], "MAP/MAP012.24"); // Exact match
	strcpy(path[2375], "MAP/MAP012.25");
	strcpy(path[2376], "MAP/MAP012.26");
	strcpy(path[2377], "MAP/MAP012.30"); // Exact match
	strcpy(path[2378], "MAP/MAP012.31");
	strcpy(path[2379], "MAP/MAP012.35"); // Exact match
	strcpy(path[2380], "MAP/MAP012.36");
	strcpy(path[2381], "MAP/MAP012.40"); // Exact match
	strcpy(path[2382], "MAP/MAP012.41");
	strcpy(path[2367], "MAP/MAP012.8"); // Exact match
	strcpy(path[2368], "MAP/MAP012.9");
	// MAP/MAP012.GNS not found (1288 bytes)
	strcpy(path[2387], "MAP/MAP013.11"); // Exact match
	strcpy(path[2388], "MAP/MAP013.12");
	strcpy(path[2389], "MAP/MAP013.15"); // Exact match
	strcpy(path[2390], "MAP/MAP013.16");
	strcpy(path[2391], "MAP/MAP013.19"); // Exact match
	strcpy(path[2392], "MAP/MAP013.20");
	strcpy(path[2385], "MAP/MAP013.8"); // Exact match
	strcpy(path[2386], "MAP/MAP013.9");
	// MAP/MAP013.GNS not found (568 bytes)
	strcpy(path[2406], "MAP/MAP014.12"); // Exact match
	strcpy(path[2399], "MAP/MAP014.13");
	strcpy(path[2400], "MAP/MAP014.16"); // Exact match
	strcpy(path[2401], "MAP/MAP014.17");
	strcpy(path[2410], "MAP/MAP014.20"); // Exact match
	strcpy(path[2403], "MAP/MAP014.21");
	strcpy(path[2404], "MAP/MAP014.25"); // Exact match
	strcpy(path[2405], "MAP/MAP014.26");
	strcpy(path[2398], "MAP/MAP014.30"); // Exact match
	strcpy(path[2407], "MAP/MAP014.31");
	strcpy(path[2408], "MAP/MAP014.35"); // Exact match
	strcpy(path[2409], "MAP/MAP014.36");
	strcpy(path[2402], "MAP/MAP014.40"); // Exact match
	strcpy(path[2411], "MAP/MAP014.41");
	strcpy(path[2395], "MAP/MAP014.7"); // Exact match
	strcpy(path[2396], "MAP/MAP014.8");
	strcpy(path[2397], "MAP/MAP014.9");
	// MAP/MAP014.GNS not found (1268 bytes)
	strcpy(path[2444], "MAP/MAP015.11"); // Exact match
	strcpy(path[2437], "MAP/MAP015.12");
	strcpy(path[2438], "MAP/MAP015.14"); // Exact match
	strcpy(path[2439], "MAP/MAP015.15");
	strcpy(path[2452], "MAP/MAP015.17"); // Exact match
	strcpy(path[2441], "MAP/MAP015.18");
	strcpy(path[2450], "MAP/MAP015.20"); // Exact match
	strcpy(path[2443], "MAP/MAP015.21");
	strcpy(path[2436], "MAP/MAP015.23"); // Exact match
	strcpy(path[2445], "MAP/MAP015.24");
	strcpy(path[2448], "MAP/MAP015.26"); // Exact match
	strcpy(path[2447], "MAP/MAP015.27");
	strcpy(path[2440], "MAP/MAP015.29"); // Exact match
	strcpy(path[2449], "MAP/MAP015.30");
	strcpy(path[2442], "MAP/MAP015.32"); // Exact match
	strcpy(path[2451], "MAP/MAP015.33");
	strcpy(path[2446], "MAP/MAP015.35"); // Exact match
	strcpy(path[2453], "MAP/MAP015.36");
	strcpy(path[2434], "MAP/MAP015.45"); // Exact match
	strcpy(path[2435], "MAP/MAP015.46");
	strcpy(path[2424], "MAP/MAP015.48"); // Exact match
	strcpy(path[2417], "MAP/MAP015.49");
	strcpy(path[2418], "MAP/MAP015.51"); // Exact match
	strcpy(path[2419], "MAP/MAP015.52");
	strcpy(path[2430], "MAP/MAP015.54"); // Exact match
	strcpy(path[2421], "MAP/MAP015.55");
	strcpy(path[2432], "MAP/MAP015.57"); // Exact match
	strcpy(path[2423], "MAP/MAP015.58");
	strcpy(path[2416], "MAP/MAP015.60"); // Exact match
	strcpy(path[2425], "MAP/MAP015.61");
	strcpy(path[2428], "MAP/MAP015.63"); // Exact match
	strcpy(path[2427], "MAP/MAP015.64");
	strcpy(path[2426], "MAP/MAP015.66"); // Exact match
	strcpy(path[2429], "MAP/MAP015.67");
	strcpy(path[2422], "MAP/MAP015.69"); // Exact match
	strcpy(path[2431], "MAP/MAP015.70");
	strcpy(path[2420], "MAP/MAP015.72"); // Exact match
	strcpy(path[2433], "MAP/MAP015.73");
	strcpy(path[2414], "MAP/MAP015.8"); // Exact match
	strcpy(path[2415], "MAP/MAP015.9");
	// MAP/MAP015.GNS not found (1928 bytes)
	strcpy(path[2457], "MAP/MAP016.12"); // Exact match
	strcpy(path[2458], "MAP/MAP016.13");
	strcpy(path[2459], "MAP/MAP016.16"); // Exact match
	strcpy(path[2460], "MAP/MAP016.17");
	strcpy(path[2461], "MAP/MAP016.20"); // Exact match
	strcpy(path[2470], "MAP/MAP016.21");
	strcpy(path[2455], "MAP/MAP016.30"); // Exact match
	strcpy(path[2464], "MAP/MAP016.31");
	strcpy(path[2465], "MAP/MAP016.34"); // Exact match
	strcpy(path[2466], "MAP/MAP016.35");
	strcpy(path[2467], "MAP/MAP016.38"); // Exact match
	strcpy(path[2468], "MAP/MAP016.39");
	strcpy(path[2469], "MAP/MAP016.42"); // Exact match
	strcpy(path[2462], "MAP/MAP016.43");
	strcpy(path[2463], "MAP/MAP016.8"); // Exact match
	strcpy(path[2456], "MAP/MAP016.9");
	// MAP/MAP016.GNS not found (1128 bytes)
	strcpy(path[2473], "MAP/MAP017.10");
	strcpy(path[2472], "MAP/MAP017.9"); // Exact match
	// MAP/MAP017.GNS not found (592 bytes)
	strcpy(path[2489], "MAP/MAP018.13"); // Exact match
	strcpy(path[2478], "MAP/MAP018.14");
	strcpy(path[2485], "MAP/MAP018.18"); // Exact match
	strcpy(path[2480], "MAP/MAP018.19");
	strcpy(path[2487], "MAP/MAP018.23"); // Exact match
	strcpy(path[2482], "MAP/MAP018.24");
	strcpy(path[2483], "MAP/MAP018.33"); // Exact match
	strcpy(path[2484], "MAP/MAP018.34");
	strcpy(path[2481], "MAP/MAP018.38"); // Exact match
	strcpy(path[2486], "MAP/MAP018.39");
	strcpy(path[2477], "MAP/MAP018.43"); // Exact match
	strcpy(path[2488], "MAP/MAP018.44");
	strcpy(path[2479], "MAP/MAP018.48"); // Exact match
	strcpy(path[2490], "MAP/MAP018.49");
	strcpy(path[2475], "MAP/MAP018.8"); // Exact match
	strcpy(path[2476], "MAP/MAP018.9");
	// MAP/MAP018.GNS not found (1248 bytes)
	strcpy(path[2494], "MAP/MAP019.11"); // Exact match
	strcpy(path[2495], "MAP/MAP019.12");
	strcpy(path[2496], "MAP/MAP019.15"); // Exact match
	strcpy(path[2497], "MAP/MAP019.16");
	strcpy(path[2498], "MAP/MAP019.19"); // Exact match
	strcpy(path[2499], "MAP/MAP019.20");
	strcpy(path[2508], "MAP/MAP019.23"); // Exact match
	strcpy(path[2501], "MAP/MAP019.24");
	strcpy(path[2502], "MAP/MAP019.27"); // Exact match
	strcpy(path[2503], "MAP/MAP019.28");
	strcpy(path[2504], "MAP/MAP019.31"); // Exact match
	strcpy(path[2505], "MAP/MAP019.32");
	strcpy(path[2506], "MAP/MAP019.35"); // Exact match
	strcpy(path[2507], "MAP/MAP019.36");
	strcpy(path[2500], "MAP/MAP019.39"); // Exact match
	strcpy(path[2509], "MAP/MAP019.40");
	strcpy(path[2510], "MAP/MAP019.43"); // Exact match
	strcpy(path[2511], "MAP/MAP019.44");
	strcpy(path[2492], "MAP/MAP019.7"); // Exact match
	strcpy(path[2493], "MAP/MAP019.8");
	// MAP/MAP019.GNS not found (1148 bytes)
	strcpy(path[2515], "MAP/MAP020.11"); // Exact match
	strcpy(path[2516], "MAP/MAP020.12");
	strcpy(path[2517], "MAP/MAP020.15"); // Exact match
	strcpy(path[2518], "MAP/MAP020.16");
	strcpy(path[2519], "MAP/MAP020.19"); // Exact match
	strcpy(path[2520], "MAP/MAP020.20");
	strcpy(path[2513], "MAP/MAP020.7"); // Exact match
	strcpy(path[2514], "MAP/MAP020.8");
	// MAP/MAP020.GNS not found (548 bytes)
	strcpy(path[2525], "MAP/MAP021.15");
	strcpy(path[2526], "MAP/MAP021.18"); // Exact match
	strcpy(path[2527], "MAP/MAP021.19");
	strcpy(path[2528], "MAP/MAP021.22"); // Exact match
	strcpy(path[2529], "MAP/MAP021.23");
	strcpy(path[2537], "MAP/MAP021.26"); // Exact match
	strcpy(path[2531], "MAP/MAP021.27");
	strcpy(path[2532], "MAP/MAP021.33");
	strcpy(path[2533], "MAP/MAP021.38"); // Exact match
	strcpy(path[2534], "MAP/MAP021.39");
	strcpy(path[2523], "MAP/MAP021.44"); // Exact match
	strcpy(path[2536], "MAP/MAP021.45");
	strcpy(path[2530], "MAP/MAP021.50"); // Exact match
	strcpy(path[2538], "MAP/MAP021.51");
	strcpy(path[2535], "MAP/MAP021.7"); // Exact match
	strcpy(path[2524], "MAP/MAP021.8");
	// MAP/MAP021.GNS not found (1368 bytes)
	strcpy(path[2543], "MAP/MAP022.12"); // Exact match
	strcpy(path[2544], "MAP/MAP022.13");
	strcpy(path[2545], "MAP/MAP022.16"); // Exact match
	strcpy(path[2546], "MAP/MAP022.17");
	strcpy(path[2547], "MAP/MAP022.20"); // Exact match
	strcpy(path[2548], "MAP/MAP022.21");
	strcpy(path[2549], "MAP/MAP022.24"); // Exact match
	strcpy(path[2550], "MAP/MAP022.25");
	strcpy(path[2551], "MAP/MAP022.30"); // Exact match
	strcpy(path[2552], "MAP/MAP022.31");
	strcpy(path[2553], "MAP/MAP022.36"); // Exact match
	strcpy(path[2554], "MAP/MAP022.37");
	strcpy(path[2557], "MAP/MAP022.42"); // Exact match
	strcpy(path[2556], "MAP/MAP022.43");
	strcpy(path[2555], "MAP/MAP022.48"); // Exact match
	strcpy(path[2558], "MAP/MAP022.49");
	strcpy(path[2559], "MAP/MAP022.54"); // Exact match
	strcpy(path[2560], "MAP/MAP022.55");
	strcpy(path[2541], "MAP/MAP022.8"); // Exact match
	strcpy(path[2542], "MAP/MAP022.9");
	// MAP/MAP022.GNS not found (1368 bytes)
	strcpy(path[2565], "MAP/MAP023.10");
	strcpy(path[2566], "MAP/MAP023.12"); // Exact match
	strcpy(path[2567], "MAP/MAP023.13");
	strcpy(path[2568], "MAP/MAP023.15"); // Exact match
	strcpy(path[2569], "MAP/MAP023.16");
	strcpy(path[2570], "MAP/MAP023.19"); // Exact match
	strcpy(path[2571], "MAP/MAP023.20");
	strcpy(path[2572], "MAP/MAP023.24"); // Exact match
	strcpy(path[2573], "MAP/MAP023.25");
	strcpy(path[2563], "MAP/MAP023.8"); // Exact match
	strcpy(path[2564], "MAP/MAP023.9");
	// MAP/MAP023.GNS not found (708 bytes)
	strcpy(path[2578], "MAP/MAP024.10"); // Exact match
	strcpy(path[2579], "MAP/MAP024.11");
	strcpy(path[2580], "MAP/MAP024.14"); // Exact match
	strcpy(path[2581], "MAP/MAP024.15");
	strcpy(path[2582], "MAP/MAP024.18"); // Exact match
	strcpy(path[2583], "MAP/MAP024.19");
	strcpy(path[2576], "MAP/MAP024.6"); // Exact match
	strcpy(path[2577], "MAP/MAP024.7");
	// MAP/MAP024.GNS not found (528 bytes)
	strcpy(path[2589], "MAP/MAP025.12"); // Exact match
	strcpy(path[2590], "MAP/MAP025.13");
	strcpy(path[2593], "MAP/MAP025.16"); // Exact match
	strcpy(path[2592], "MAP/MAP025.17");
	strcpy(path[2591], "MAP/MAP025.20"); // Exact match
	strcpy(path[2594], "MAP/MAP025.21");
	strcpy(path[2595], "MAP/MAP025.24"); // Exact match
	strcpy(path[2596], "MAP/MAP025.25");
	strcpy(path[2586], "MAP/MAP025.7"); // Exact match
	strcpy(path[2587], "MAP/MAP025.8");
	strcpy(path[2588], "MAP/MAP025.9");
	// MAP/MAP025.GNS not found (708 bytes)
	strcpy(path[2599], "MAP/MAP026.8"); // Exact match
	strcpy(path[2600], "MAP/MAP026.9");
	// MAP/MAP026.GNS not found (248 bytes)
	strcpy(path[2605], "MAP/MAP027.10");
	strcpy(path[2606], "MAP/MAP027.13"); // Exact match
	strcpy(path[2607], "MAP/MAP027.14");
	strcpy(path[2610], "MAP/MAP027.17"); // Exact match
	strcpy(path[2609], "MAP/MAP027.18");
	strcpy(path[2608], "MAP/MAP027.21"); // Exact match
	strcpy(path[2611], "MAP/MAP027.22");
	strcpy(path[2603], "MAP/MAP027.7"); // Exact match
	strcpy(path[2604], "MAP/MAP027.8");
	// MAP/MAP027.GNS not found (628 bytes)
	strcpy(path[2617], "MAP/MAP028.10"); // Exact match
	strcpy(path[2618], "MAP/MAP028.11");
	strcpy(path[2619], "MAP/MAP028.13"); // Exact match
	strcpy(path[2620], "MAP/MAP028.14");
	strcpy(path[2621], "MAP/MAP028.16"); // Exact match
	strcpy(path[2622], "MAP/MAP028.17");
	strcpy(path[2614], "MAP/MAP028.6"); // Exact match
	strcpy(path[2615], "MAP/MAP028.7");
	strcpy(path[2616], "MAP/MAP028.8");
	// MAP/MAP028.GNS not found (528 bytes)
	strcpy(path[2627], "MAP/MAP029.10");
	strcpy(path[2628], "MAP/MAP029.13"); // Exact match
	strcpy(path[2629], "MAP/MAP029.14");
	strcpy(path[2632], "MAP/MAP029.17"); // Exact match
	strcpy(path[2631], "MAP/MAP029.18");
	strcpy(path[2630], "MAP/MAP029.21"); // Exact match
	strcpy(path[2633], "MAP/MAP029.22");
	strcpy(path[2625], "MAP/MAP029.7"); // Exact match
	strcpy(path[2626], "MAP/MAP029.8");
	// MAP/MAP029.GNS not found (628 bytes)
	strcpy(path[2639], "MAP/MAP030.11"); // Exact match
	strcpy(path[2640], "MAP/MAP030.12");
	strcpy(path[2643], "MAP/MAP030.15"); // Exact match
	strcpy(path[2642], "MAP/MAP030.16");
	strcpy(path[2641], "MAP/MAP030.19"); // Exact match
	strcpy(path[2644], "MAP/MAP030.20");
	strcpy(path[2636], "MAP/MAP030.6"); // Exact match
	strcpy(path[2637], "MAP/MAP030.7");
	strcpy(path[2638], "MAP/MAP030.8");
	// MAP/MAP030.GNS not found (588 bytes)
	strcpy(path[2649], "MAP/MAP031.11"); // Exact match
	strcpy(path[2650], "MAP/MAP031.12");
	strcpy(path[2651], "MAP/MAP031.15"); // Exact match
	strcpy(path[2652], "MAP/MAP031.16");
	strcpy(path[2653], "MAP/MAP031.19"); // Exact match
	strcpy(path[2654], "MAP/MAP031.20");
	strcpy(path[2655], "MAP/MAP031.23"); // Exact match
	strcpy(path[2656], "MAP/MAP031.24");
	strcpy(path[2657], "MAP/MAP031.27"); // Exact match
	strcpy(path[2658], "MAP/MAP031.28");
	strcpy(path[2661], "MAP/MAP031.31"); // Exact match
	strcpy(path[2660], "MAP/MAP031.32");
	strcpy(path[2663], "MAP/MAP031.35"); // Exact match
	strcpy(path[2662], "MAP/MAP031.36");
	strcpy(path[2665], "MAP/MAP031.39"); // Exact match
	strcpy(path[2664], "MAP/MAP031.40");
	strcpy(path[2659], "MAP/MAP031.43"); // Exact match
	strcpy(path[2666], "MAP/MAP031.44");
	strcpy(path[2647], "MAP/MAP031.7"); // Exact match
	strcpy(path[2648], "MAP/MAP031.8");
	// MAP/MAP031.GNS not found (1148 bytes)
	strcpy(path[2671], "MAP/MAP032.11");
	strcpy(path[2672], "MAP/MAP032.14"); // Exact match
	strcpy(path[2673], "MAP/MAP032.15");
	strcpy(path[2676], "MAP/MAP032.18"); // Exact match
	strcpy(path[2675], "MAP/MAP032.19");
	strcpy(path[2674], "MAP/MAP032.22"); // Exact match
	strcpy(path[2677], "MAP/MAP032.23");
	strcpy(path[2669], "MAP/MAP032.8"); // Exact match
	strcpy(path[2670], "MAP/MAP032.9");
	// MAP/MAP032.GNS not found (648 bytes)
	strcpy(path[2682], "MAP/MAP033.18"); // Exact match
	strcpy(path[2683], "MAP/MAP033.19");
	strcpy(path[2680], "MAP/MAP033.8"); // Exact match
	strcpy(path[2681], "MAP/MAP033.9");
	// MAP/MAP033.GNS not found (528 bytes)
	strcpy(path[2687], "MAP/MAP034.11");
	strcpy(path[2688], "MAP/MAP034.15");
	strcpy(path[2689], "MAP/MAP034.19");
	strcpy(path[2685], "MAP/MAP034.6"); // Exact match
	strcpy(path[2686], "MAP/MAP034.7");
	// MAP/MAP034.GNS not found (588 bytes)
	strcpy(path[2694], "MAP/MAP035.11"); // Exact match
	strcpy(path[2695], "MAP/MAP035.12");
	strcpy(path[2696], "MAP/MAP035.15"); // Exact match
	strcpy(path[2697], "MAP/MAP035.16");
	strcpy(path[2698], "MAP/MAP035.19"); // Exact match
	strcpy(path[2699], "MAP/MAP035.20");
	strcpy(path[2700], "MAP/MAP035.23"); // Exact match
	strcpy(path[2701], "MAP/MAP035.24");
	strcpy(path[2702], "MAP/MAP035.27"); // Exact match
	strcpy(path[2703], "MAP/MAP035.28");
	strcpy(path[2708], "MAP/MAP035.31"); // Exact match
	strcpy(path[2705], "MAP/MAP035.32");
	strcpy(path[2706], "MAP/MAP035.35"); // Exact match
	strcpy(path[2707], "MAP/MAP035.36");
	strcpy(path[2704], "MAP/MAP035.39"); // Exact match
	strcpy(path[2709], "MAP/MAP035.40");
	strcpy(path[2710], "MAP/MAP035.43"); // Exact match
	strcpy(path[2711], "MAP/MAP035.44");
	strcpy(path[2692], "MAP/MAP035.7"); // Exact match
	strcpy(path[2693], "MAP/MAP035.8");
	// MAP/MAP035.GNS not found (1148 bytes)
	strcpy(path[2714], "MAP/MAP036.1");
	strcpy(path[2719], "MAP/MAP036.12"); // Exact match
	strcpy(path[2720], "MAP/MAP036.13");
	strcpy(path[2721], "MAP/MAP036.16"); // Exact match
	strcpy(path[2722], "MAP/MAP036.17");
	strcpy(path[2723], "MAP/MAP036.20"); // Exact match
	strcpy(path[2724], "MAP/MAP036.21");
	strcpy(path[2725], "MAP/MAP036.24"); // Exact match
	strcpy(path[2726], "MAP/MAP036.25");
	strcpy(path[2727], "MAP/MAP036.28"); // Exact match
	strcpy(path[2728], "MAP/MAP036.29");
	strcpy(path[2729], "MAP/MAP036.32"); // Exact match
	strcpy(path[2730], "MAP/MAP036.33");
	strcpy(path[2715], "MAP/MAP036.4"); // Exact match
	strcpy(path[2731], "MAP/MAP036.45"); // Exact match
	strcpy(path[2732], "MAP/MAP036.46");
	strcpy(path[2716], "MAP/MAP036.5");
	strcpy(path[2717], "MAP/MAP036.8"); // Exact match
	strcpy(path[2718], "MAP/MAP036.9");
	// MAP/MAP036.GNS not found (1188 bytes)
	strcpy(path[2738], "MAP/MAP037.10");
	strcpy(path[2735], "MAP/MAP037.6"); // Exact match
	strcpy(path[2736], "MAP/MAP037.7");
	strcpy(path[2737], "MAP/MAP037.9"); // Exact match
	// MAP/MAP037.GNS not found (308 bytes)
	strcpy(path[2741], "MAP/MAP038.11"); // Exact match
	strcpy(path[2742], "MAP/MAP038.12");
	strcpy(path[2743], "MAP/MAP038.15"); // Exact match
	strcpy(path[2744], "MAP/MAP038.16");
	strcpy(path[2745], "MAP/MAP038.19"); // Exact match
	strcpy(path[2746], "MAP/MAP038.20");
	strcpy(path[2749], "MAP/MAP038.23"); // Exact match
	strcpy(path[2748], "MAP/MAP038.24");
	strcpy(path[2747], "MAP/MAP038.27"); // Exact match
	strcpy(path[2750], "MAP/MAP038.28");
	strcpy(path[2751], "MAP/MAP038.31"); // Exact match
	strcpy(path[2752], "MAP/MAP038.32");
	strcpy(path[2755], "MAP/MAP038.35"); // Exact match
	strcpy(path[2754], "MAP/MAP038.36");
	strcpy(path[2757], "MAP/MAP038.39"); // Exact match
	strcpy(path[2756], "MAP/MAP038.40");
	strcpy(path[2753], "MAP/MAP038.43"); // Exact match
	strcpy(path[2758], "MAP/MAP038.44");
	strcpy(path[2759], "MAP/MAP038.47"); // Exact match
	strcpy(path[2760], "MAP/MAP038.48");
	// MAP/MAP038.GNS not found (1228 bytes)
	strcpy(path[2763], "MAP/MAP039.8"); // Exact match
	strcpy(path[2764], "MAP/MAP039.9");
	// MAP/MAP039.GNS not found (268 bytes)
	strcpy(path[2768], "MAP/MAP040.10");
	strcpy(path[2769], "MAP/MAP040.11");
	strcpy(path[2774], "MAP/MAP040.13"); // Exact match
	strcpy(path[2771], "MAP/MAP040.14");
	strcpy(path[2772], "MAP/MAP040.16"); // Exact match
	strcpy(path[2773], "MAP/MAP040.17");
	strcpy(path[2770], "MAP/MAP040.19"); // Exact match
	strcpy(path[2775], "MAP/MAP040.20");
	strcpy(path[2784], "MAP/MAP040.22"); // Exact match
	strcpy(path[2777], "MAP/MAP040.23");
	strcpy(path[2782], "MAP/MAP040.25"); // Exact match
	strcpy(path[2779], "MAP/MAP040.26");
	strcpy(path[2780], "MAP/MAP040.28"); // Exact match
	strcpy(path[2781], "MAP/MAP040.29");
	strcpy(path[2778], "MAP/MAP040.31"); // Exact match
	strcpy(path[2783], "MAP/MAP040.32");
	strcpy(path[2776], "MAP/MAP040.34"); // Exact match
	strcpy(path[2785], "MAP/MAP040.35");
	strcpy(path[2767], "MAP/MAP040.9"); // Exact match
	// MAP/MAP040.GNS not found (988 bytes)
	strcpy(path[2789], "MAP/MAP041.11");
	strcpy(path[2790], "MAP/MAP041.19"); // Exact match
	strcpy(path[2791], "MAP/MAP041.20");
	strcpy(path[2788], "MAP/MAP041.5");
	// MAP/MAP041.GNS not found (568 bytes)
	strcpy(path[2796], "MAP/MAP042.12"); // Exact match
	strcpy(path[2797], "MAP/MAP042.13");
	strcpy(path[2798], "MAP/MAP042.16"); // Exact match
	strcpy(path[2799], "MAP/MAP042.17");
	strcpy(path[2800], "MAP/MAP042.20"); // Exact match
	strcpy(path[2801], "MAP/MAP042.21");
	strcpy(path[2802], "MAP/MAP042.24"); // Exact match
	strcpy(path[2803], "MAP/MAP042.25");
	strcpy(path[2794], "MAP/MAP042.8"); // Exact match
	strcpy(path[2795], "MAP/MAP042.9");
	// MAP/MAP042.GNS not found (668 bytes)
	strcpy(path[2807], "MAP/MAP043.12"); // Exact match
	strcpy(path[2808], "MAP/MAP043.13");
	strcpy(path[2805], "MAP/MAP043.8"); // Exact match
	strcpy(path[2806], "MAP/MAP043.9");
	// MAP/MAP043.GNS not found (368 bytes)
	strcpy(path[2814], "MAP/MAP044.11"); // Exact match
	strcpy(path[2813], "MAP/MAP044.12");
	strcpy(path[2812], "MAP/MAP044.15"); // Exact match
	strcpy(path[2815], "MAP/MAP044.16");
	strcpy(path[2818], "MAP/MAP044.19"); // Exact match
	strcpy(path[2817], "MAP/MAP044.20");
	strcpy(path[2816], "MAP/MAP044.23"); // Exact match
	strcpy(path[2819], "MAP/MAP044.24");
	strcpy(path[2820], "MAP/MAP044.27"); // Exact match
	strcpy(path[2821], "MAP/MAP044.28");
	strcpy(path[2822], "MAP/MAP044.31"); // Exact match
	strcpy(path[2823], "MAP/MAP044.32");
	strcpy(path[2826], "MAP/MAP044.35"); // Exact match
	strcpy(path[2825], "MAP/MAP044.36");
	strcpy(path[2824], "MAP/MAP044.39"); // Exact match
	strcpy(path[2827], "MAP/MAP044.40");
	strcpy(path[2828], "MAP/MAP044.43"); // Exact match
	strcpy(path[2829], "MAP/MAP044.44");
	strcpy(path[2810], "MAP/MAP044.7"); // Exact match
	strcpy(path[2811], "MAP/MAP044.8");
	// MAP/MAP044.GNS not found (1148 bytes)
	strcpy(path[2834], "MAP/MAP045.10"); // Exact match
	strcpy(path[2835], "MAP/MAP045.11");
	strcpy(path[2832], "MAP/MAP045.6"); // Exact match
	strcpy(path[2833], "MAP/MAP045.7");
	// MAP/MAP045.GNS not found (328 bytes)
	strcpy(path[2840], "MAP/MAP046.12"); // Exact match
	strcpy(path[2841], "MAP/MAP046.13");
	strcpy(path[2842], "MAP/MAP046.16"); // Exact match
	strcpy(path[2843], "MAP/MAP046.17");
	strcpy(path[2844], "MAP/MAP046.20"); // Exact match
	strcpy(path[2845], "MAP/MAP046.21");
	strcpy(path[2846], "MAP/MAP046.24"); // Exact match
	strcpy(path[2847], "MAP/MAP046.25");
	strcpy(path[2838], "MAP/MAP046.8"); // Exact match
	strcpy(path[2839], "MAP/MAP046.9");
	// MAP/MAP046.GNS not found (668 bytes)
	strcpy(path[2849], "MAP/MAP047.16"); // Exact match
	strcpy(path[2850], "MAP/MAP047.17");
	strcpy(path[2851], "MAP/MAP047.20"); // Exact match
	strcpy(path[2852], "MAP/MAP047.21");
	strcpy(path[2853], "MAP/MAP047.25"); // Exact match
	strcpy(path[2854], "MAP/MAP047.26");
	strcpy(path[2857], "MAP/MAP047.30"); // Exact match
	strcpy(path[2856], "MAP/MAP047.31");
	strcpy(path[2855], "MAP/MAP047.35"); // Exact match
	strcpy(path[2858], "MAP/MAP047.36");
	strcpy(path[2859], "MAP/MAP047.40"); // Exact match
	strcpy(path[2860], "MAP/MAP047.41");
	strcpy(path[2867], "MAP/MAP047.45"); // Exact match
	strcpy(path[2862], "MAP/MAP047.46");
	strcpy(path[2865], "MAP/MAP047.50"); // Exact match
	strcpy(path[2864], "MAP/MAP047.51");
	strcpy(path[2863], "MAP/MAP047.55"); // Exact match
	strcpy(path[2866], "MAP/MAP047.56");
	strcpy(path[2861], "MAP/MAP047.60"); // Exact match
	strcpy(path[2868], "MAP/MAP047.61");
	// MAP/MAP047.GNS not found (1488 bytes)
	strcpy(path[2873], "MAP/MAP048.12"); // Exact match
	strcpy(path[2874], "MAP/MAP048.13");
	strcpy(path[2875], "MAP/MAP048.16"); // Exact match
	strcpy(path[2876], "MAP/MAP048.17");
	strcpy(path[2879], "MAP/MAP048.20"); // Exact match
	strcpy(path[2878], "MAP/MAP048.21");
	strcpy(path[2877], "MAP/MAP048.24"); // Exact match
	strcpy(path[2880], "MAP/MAP048.25");
	strcpy(path[2881], "MAP/MAP048.28"); // Exact match
	strcpy(path[2882], "MAP/MAP048.29");
	strcpy(path[2883], "MAP/MAP048.32"); // Exact match
	strcpy(path[2884], "MAP/MAP048.33");
	strcpy(path[2885], "MAP/MAP048.36"); // Exact match
	strcpy(path[2886], "MAP/MAP048.37");
	strcpy(path[2887], "MAP/MAP048.40"); // Exact match
	strcpy(path[2888], "MAP/MAP048.41");
	strcpy(path[2889], "MAP/MAP048.44"); // Exact match
	strcpy(path[2890], "MAP/MAP048.45");
	strcpy(path[2871], "MAP/MAP048.8"); // Exact match
	strcpy(path[2872], "MAP/MAP048.9");
	// MAP/MAP048.GNS not found (1168 bytes)
	strcpy(path[2894], "MAP/MAP049.10"); // Exact match
	strcpy(path[2895], "MAP/MAP049.11");
	strcpy(path[2896], "MAP/MAP049.14"); // Exact match
	strcpy(path[2897], "MAP/MAP049.15");
	strcpy(path[2900], "MAP/MAP049.18"); // Exact match
	strcpy(path[2899], "MAP/MAP049.19");
	strcpy(path[2898], "MAP/MAP049.22"); // Exact match
	strcpy(path[2901], "MAP/MAP049.23");
	strcpy(path[2902], "MAP/MAP049.26"); // Exact match
	strcpy(path[2903], "MAP/MAP049.27");
	strcpy(path[2904], "MAP/MAP049.30"); // Exact match
	strcpy(path[2905], "MAP/MAP049.31");
	strcpy(path[2906], "MAP/MAP049.34"); // Exact match
	strcpy(path[2907], "MAP/MAP049.35");
	strcpy(path[2908], "MAP/MAP049.38"); // Exact match
	strcpy(path[2909], "MAP/MAP049.39");
	strcpy(path[2910], "MAP/MAP049.42"); // Exact match
	strcpy(path[2911], "MAP/MAP049.43");
	strcpy(path[2892], "MAP/MAP049.7"); // Exact match
	strcpy(path[2893], "MAP/MAP049.8");
	// MAP/MAP049.GNS not found (1128 bytes)
	strcpy(path[2916], "MAP/MAP050.11"); // Exact match
	strcpy(path[2917], "MAP/MAP050.12");
	strcpy(path[2918], "MAP/MAP050.15"); // Exact match
	strcpy(path[2919], "MAP/MAP050.16");
	strcpy(path[2920], "MAP/MAP050.19"); // Exact match
	strcpy(path[2921], "MAP/MAP050.20");
	strcpy(path[2922], "MAP/MAP050.23"); // Exact match
	strcpy(path[2923], "MAP/MAP050.24");
	strcpy(path[2924], "MAP/MAP050.27"); // Exact match
	strcpy(path[2925], "MAP/MAP050.28");
	strcpy(path[2926], "MAP/MAP050.31"); // Exact match
	strcpy(path[2927], "MAP/MAP050.32");
	strcpy(path[2928], "MAP/MAP050.35"); // Exact match
	strcpy(path[2929], "MAP/MAP050.36");
	strcpy(path[2930], "MAP/MAP050.39"); // Exact match
	strcpy(path[2931], "MAP/MAP050.40");
	strcpy(path[2932], "MAP/MAP050.43"); // Exact match
	strcpy(path[2933], "MAP/MAP050.44");
	strcpy(path[2914], "MAP/MAP050.7"); // Exact match
	strcpy(path[2915], "MAP/MAP050.8");
	// MAP/MAP050.GNS not found (1148 bytes)
	strcpy(path[2938], "MAP/MAP051.16"); // Exact match
	strcpy(path[2939], "MAP/MAP051.17");
	strcpy(path[2947], "MAP/MAP051.20"); // Exact match
	strcpy(path[2941], "MAP/MAP051.21");
	strcpy(path[2949], "MAP/MAP051.24"); // Exact match
	strcpy(path[2943], "MAP/MAP051.25");
	strcpy(path[2944], "MAP/MAP051.28"); // Exact match
	strcpy(path[2945], "MAP/MAP051.29");
	strcpy(path[2946], "MAP/MAP051.32");
	strcpy(path[2940], "MAP/MAP051.37"); // Exact match
	strcpy(path[2948], "MAP/MAP051.38");
	strcpy(path[2942], "MAP/MAP051.43"); // Exact match
	strcpy(path[2950], "MAP/MAP051.44");
	strcpy(path[2951], "MAP/MAP051.49"); // Exact match
	strcpy(path[2952], "MAP/MAP051.50");
	strcpy(path[2936], "MAP/MAP051.7"); // Exact match
	strcpy(path[2937], "MAP/MAP051.8");
	// MAP/MAP051.GNS not found (1328 bytes)
	strcpy(path[2956], "MAP/MAP052.10");
	strcpy(path[2955], "MAP/MAP052.9"); // Exact match
	// MAP/MAP052.GNS not found (288 bytes)
	strcpy(path[2961], "MAP/MAP053.10");
	strcpy(path[2962], "MAP/MAP053.19");
	strcpy(path[2963], "MAP/MAP053.22");
	strcpy(path[2959], "MAP/MAP053.7"); // Exact match
	strcpy(path[2960], "MAP/MAP053.8");
	// MAP/MAP053.GNS not found (648 bytes)
	strcpy(path[2966], "MAP/MAP054.7"); // Exact match
	strcpy(path[2967], "MAP/MAP054.8");
	// MAP/MAP054.GNS not found (228 bytes)
	strcpy(path[2970], "MAP/MAP055.12"); // Exact match
	strcpy(path[2971], "MAP/MAP055.13");
	strcpy(path[2973], "MAP/MAP055.14");
	strcpy(path[2972], "MAP/MAP055.15");
	// MAP/MAP055.GNS not found (468 bytes)
	strcpy(path[2976], "MAP/MAP056.0");
	strcpy(path[2981], "MAP/MAP056.11"); // Exact match
	strcpy(path[2982], "MAP/MAP056.12");
	strcpy(path[2983], "MAP/MAP056.16"); // Exact match
	strcpy(path[2984], "MAP/MAP056.17");
	strcpy(path[2989], "MAP/MAP056.21"); // Exact match
	strcpy(path[2986], "MAP/MAP056.22");
	strcpy(path[2985], "MAP/MAP056.26"); // Exact match
	strcpy(path[2988], "MAP/MAP056.27");
	strcpy(path[2977], "MAP/MAP056.3"); // Exact match
	strcpy(path[2987], "MAP/MAP056.31"); // Exact match
	strcpy(path[2990], "MAP/MAP056.32");
	strcpy(path[2991], "MAP/MAP056.36"); // Exact match
	strcpy(path[2992], "MAP/MAP056.37");
	strcpy(path[2978], "MAP/MAP056.4");
	strcpy(path[2993], "MAP/MAP056.47"); // Exact match
	strcpy(path[2994], "MAP/MAP056.48");
	strcpy(path[2979], "MAP/MAP056.7"); // Exact match
	strcpy(path[2980], "MAP/MAP056.8");
	// MAP/MAP056.GNS not found (1228 bytes)
	strcpy(path[2997], "MAP/MAP057.8"); // Exact match
	strcpy(path[2998], "MAP/MAP057.9");
	// MAP/MAP057.GNS not found (248 bytes)
	strcpy(path[3000], "MAP/MAP058.8"); // Exact match
	strcpy(path[3001], "MAP/MAP058.9");
	// MAP/MAP058.GNS not found (248 bytes)
	strcpy(path[3003], "MAP/MAP059.8"); // Exact match
	strcpy(path[3004], "MAP/MAP059.9");
	// MAP/MAP059.GNS not found (248 bytes)
	strcpy(path[3006], "MAP/MAP060.8"); // Exact match
	strcpy(path[3007], "MAP/MAP060.9");
	// MAP/MAP060.GNS not found (248 bytes)
	strcpy(path[3013], "MAP/MAP061.10");
	strcpy(path[3010], "MAP/MAP061.7"); // Exact match
	strcpy(path[3011], "MAP/MAP061.8");
	strcpy(path[3012], "MAP/MAP061.9");
	// MAP/MAP061.GNS not found (368 bytes)
	strcpy(path[3017], "MAP/MAP062.11"); // Exact match
	strcpy(path[3018], "MAP/MAP062.12");
	strcpy(path[3019], "MAP/MAP062.15"); // Exact match
	strcpy(path[3020], "MAP/MAP062.16");
	strcpy(path[3021], "MAP/MAP062.19"); // Exact match
	strcpy(path[3022], "MAP/MAP062.20");
	strcpy(path[3015], "MAP/MAP062.7"); // Exact match
	// MAP/MAP062.8 not found (55008 bytes)
	// MAP/MAP062.GNS not found (548 bytes)
	strcpy(path[3027], "MAP/MAP063.10"); // Exact match
	strcpy(path[3028], "MAP/MAP063.11");
	strcpy(path[3029], "MAP/MAP063.14"); // Exact match
	strcpy(path[3030], "MAP/MAP063.15");
	strcpy(path[3033], "MAP/MAP063.18"); // Exact match
	strcpy(path[3032], "MAP/MAP063.19");
	strcpy(path[3031], "MAP/MAP063.22"); // Exact match
	strcpy(path[3034], "MAP/MAP063.23");
	strcpy(path[3025], "MAP/MAP063.6"); // Exact match
	strcpy(path[3026], "MAP/MAP063.7");
	// MAP/MAP063.GNS not found (628 bytes)
	strcpy(path[3037], "MAP/MAP064.10");
	strcpy(path[3038], "MAP/MAP064.14");
	strcpy(path[3039], "MAP/MAP064.19");
	strcpy(path[3036], "MAP/MAP064.9"); // Exact match
	// MAP/MAP064.GNS not found (548 bytes)
	strcpy(path[3042], "MAP/MAP065.8"); // Exact match
	strcpy(path[3043], "MAP/MAP065.9");
	// MAP/MAP065.GNS not found (248 bytes)
	strcpy(path[3048], "MAP/MAP066.12"); // Exact match
	strcpy(path[3049], "MAP/MAP066.13");
	strcpy(path[3058], "MAP/MAP066.16"); // Exact match
	strcpy(path[3051], "MAP/MAP066.17");
	strcpy(path[3060], "MAP/MAP066.20"); // Exact match
	strcpy(path[3053], "MAP/MAP066.21");
	strcpy(path[3054], "MAP/MAP066.24"); // Exact match
	strcpy(path[3055], "MAP/MAP066.25");
	strcpy(path[3062], "MAP/MAP066.28"); // Exact match
	strcpy(path[3057], "MAP/MAP066.29");
	strcpy(path[3050], "MAP/MAP066.32"); // Exact match
	strcpy(path[3059], "MAP/MAP066.33");
	strcpy(path[3056], "MAP/MAP066.36"); // Exact match
	strcpy(path[3061], "MAP/MAP066.37");
	strcpy(path[3052], "MAP/MAP066.40"); // Exact match
	strcpy(path[3063], "MAP/MAP066.41");
	strcpy(path[3045], "MAP/MAP066.7"); // Exact match
	strcpy(path[3046], "MAP/MAP066.8");
	strcpy(path[3047], "MAP/MAP066.9");
	// MAP/MAP066.GNS not found (1108 bytes)
	strcpy(path[3069], "MAP/MAP067.12"); // Exact match
	strcpy(path[3070], "MAP/MAP067.13");
	strcpy(path[3083], "MAP/MAP067.16"); // Exact match
	strcpy(path[3072], "MAP/MAP067.17");
	strcpy(path[3081], "MAP/MAP067.20"); // Exact match
	strcpy(path[3074], "MAP/MAP067.21");
	strcpy(path[3075], "MAP/MAP067.24"); // Exact match
	strcpy(path[3076], "MAP/MAP067.25");
	strcpy(path[3079], "MAP/MAP067.28"); // Exact match
	strcpy(path[3078], "MAP/MAP067.29");
	strcpy(path[3077], "MAP/MAP067.32"); // Exact match
	strcpy(path[3080], "MAP/MAP067.33");
	strcpy(path[3073], "MAP/MAP067.36"); // Exact match
	strcpy(path[3082], "MAP/MAP067.37");
	strcpy(path[3071], "MAP/MAP067.40"); // Exact match
	strcpy(path[3084], "MAP/MAP067.41");
	strcpy(path[3066], "MAP/MAP067.7"); // Exact match
	strcpy(path[3067], "MAP/MAP067.8");
	strcpy(path[3068], "MAP/MAP067.9");
	// MAP/MAP067.GNS not found (1108 bytes)
	strcpy(path[3096], "MAP/MAP068.11"); // Exact match
	strcpy(path[3089], "MAP/MAP068.12");
	strcpy(path[3090], "MAP/MAP068.15"); // Exact match
	strcpy(path[3091], "MAP/MAP068.16");
	strcpy(path[3100], "MAP/MAP068.19"); // Exact match
	strcpy(path[3101], "MAP/MAP068.20");
	strcpy(path[3086], "MAP/MAP068.28"); // Exact match
	strcpy(path[3095], "MAP/MAP068.29");
	strcpy(path[3088], "MAP/MAP068.32"); // Exact match
	strcpy(path[3097], "MAP/MAP068.33");
	strcpy(path[3098], "MAP/MAP068.36"); // Exact match
	strcpy(path[3099], "MAP/MAP068.37");
	strcpy(path[3092], "MAP/MAP068.40"); // Exact match
	strcpy(path[3093], "MAP/MAP068.41");
	strcpy(path[3094], "MAP/MAP068.7"); // Exact match
	strcpy(path[3087], "MAP/MAP068.8");
	// MAP/MAP068.GNS not found (1088 bytes)
	strcpy(path[3104], "MAP/MAP069.7"); // Exact match
	strcpy(path[3105], "MAP/MAP069.8");
	// MAP/MAP069.GNS not found (228 bytes)
	strcpy(path[3110], "MAP/MAP070.10"); // Exact match
	strcpy(path[3111], "MAP/MAP070.11");
	strcpy(path[3108], "MAP/MAP070.6"); // Exact match
	strcpy(path[3109], "MAP/MAP070.7");
	// MAP/MAP070.GNS not found (328 bytes)
	strcpy(path[3124], "MAP/MAP071.12"); // Exact match
	strcpy(path[3117], "MAP/MAP071.13");
	strcpy(path[3126], "MAP/MAP071.16"); // Exact match
	strcpy(path[3119], "MAP/MAP071.17");
	strcpy(path[3130], "MAP/MAP071.20"); // Exact match
	strcpy(path[3121], "MAP/MAP071.21");
	strcpy(path[3128], "MAP/MAP071.24"); // Exact match
	strcpy(path[3123], "MAP/MAP071.25");
	strcpy(path[3116], "MAP/MAP071.28"); // Exact match
	strcpy(path[3125], "MAP/MAP071.29");
	strcpy(path[3118], "MAP/MAP071.32"); // Exact match
	strcpy(path[3127], "MAP/MAP071.33");
	strcpy(path[3120], "MAP/MAP071.36"); // Exact match
	strcpy(path[3129], "MAP/MAP071.37");
	strcpy(path[3122], "MAP/MAP071.40"); // Exact match
	strcpy(path[3131], "MAP/MAP071.41");
	strcpy(path[3132], "MAP/MAP071.44"); // Exact match
	strcpy(path[3133], "MAP/MAP071.45");
	strcpy(path[3114], "MAP/MAP071.8"); // Exact match
	strcpy(path[3115], "MAP/MAP071.9");
	// MAP/MAP071.GNS not found (1168 bytes)
	strcpy(path[3138], "MAP/MAP072.10");
	strcpy(path[3136], "MAP/MAP072.13"); // Exact match
	strcpy(path[3140], "MAP/MAP072.14");
	strcpy(path[3141], "MAP/MAP072.17"); // Exact match
	strcpy(path[3142], "MAP/MAP072.18");
	strcpy(path[3146], "MAP/MAP072.20"); // Exact match
	strcpy(path[3144], "MAP/MAP072.21");
	strcpy(path[3145], "MAP/MAP072.23");
	strcpy(path[3150], "MAP/MAP072.26"); // Exact match
	strcpy(path[3147], "MAP/MAP072.27");
	strcpy(path[3152], "MAP/MAP072.30"); // Exact match
	strcpy(path[3149], "MAP/MAP072.31");
	strcpy(path[3143], "MAP/MAP072.34"); // Exact match
	strcpy(path[3151], "MAP/MAP072.35");
	strcpy(path[3148], "MAP/MAP072.38"); // Exact match
	strcpy(path[3153], "MAP/MAP072.39");
	strcpy(path[3139], "MAP/MAP072.8"); // Exact match
	strcpy(path[3137], "MAP/MAP072.9");
	// MAP/MAP072.GNS not found (1088 bytes)
	strcpy(path[3160], "MAP/MAP073.10"); // Exact match
	strcpy(path[3161], "MAP/MAP073.11");
	strcpy(path[3156], "MAP/MAP073.2"); // Exact match
	strcpy(path[3162], "MAP/MAP073.23"); // Exact match
	strcpy(path[3163], "MAP/MAP073.24");
	strcpy(path[3157], "MAP/MAP073.3");
	strcpy(path[3158], "MAP/MAP073.6"); // Exact match
	strcpy(path[3159], "MAP/MAP073.7");
	// MAP/MAP073.GNS not found (608 bytes)
	strcpy(path[3168], "MAP/MAP074.10");
	strcpy(path[3175], "MAP/MAP074.12"); // Exact match
	strcpy(path[3170], "MAP/MAP074.13");
	strcpy(path[3173], "MAP/MAP074.15"); // Exact match
	strcpy(path[3172], "MAP/MAP074.16");
	strcpy(path[3179], "MAP/MAP074.18"); // Exact match
	strcpy(path[3174], "MAP/MAP074.19");
	strcpy(path[3169], "MAP/MAP074.21"); // Exact match
	strcpy(path[3176], "MAP/MAP074.22");
	strcpy(path[3177], "MAP/MAP074.24"); // Exact match
	strcpy(path[3178], "MAP/MAP074.25");
	strcpy(path[3171], "MAP/MAP074.27"); // Exact match
	strcpy(path[3180], "MAP/MAP074.28");
	strcpy(path[3183], "MAP/MAP074.30"); // Exact match
	strcpy(path[3182], "MAP/MAP074.31");
	strcpy(path[3181], "MAP/MAP074.33"); // Exact match
	strcpy(path[3184], "MAP/MAP074.34");
	strcpy(path[3166], "MAP/MAP074.8"); // Exact match
	strcpy(path[3167], "MAP/MAP074.9");
	// MAP/MAP074.GNS not found (968 bytes)
	strcpy(path[3189], "MAP/MAP075.11"); // Exact match
	strcpy(path[3190], "MAP/MAP075.12");
	strcpy(path[3197], "MAP/MAP075.14"); // Exact match
	strcpy(path[3192], "MAP/MAP075.15");
	strcpy(path[3205], "MAP/MAP075.17"); // Exact match
	strcpy(path[3194], "MAP/MAP075.18");
	strcpy(path[3201], "MAP/MAP075.20"); // Exact match
	strcpy(path[3196], "MAP/MAP075.21");
	strcpy(path[3191], "MAP/MAP075.25"); // Exact match
	strcpy(path[3198], "MAP/MAP075.26");
	strcpy(path[3203], "MAP/MAP075.30"); // Exact match
	strcpy(path[3200], "MAP/MAP075.31");
	strcpy(path[3199], "MAP/MAP075.35"); // Exact match
	strcpy(path[3202], "MAP/MAP075.36");
	strcpy(path[3195], "MAP/MAP075.40"); // Exact match
	strcpy(path[3204], "MAP/MAP075.41");
	strcpy(path[3193], "MAP/MAP075.45"); // Exact match
	strcpy(path[3206], "MAP/MAP075.46");
	strcpy(path[3187], "MAP/MAP075.8"); // Exact match
	strcpy(path[3188], "MAP/MAP075.9");
	// MAP/MAP075.GNS not found (1188 bytes)
	strcpy(path[3212], "MAP/MAP076.11");
	strcpy(path[3218], "MAP/MAP076.14"); // Exact match
	strcpy(path[3214], "MAP/MAP076.15");
	strcpy(path[3215], "MAP/MAP076.18"); // Exact match
	strcpy(path[3216], "MAP/MAP076.19");
	strcpy(path[3217], "MAP/MAP076.21");
	strcpy(path[3213], "MAP/MAP076.24"); // Exact match
	strcpy(path[3219], "MAP/MAP076.25");
	strcpy(path[3222], "MAP/MAP076.28"); // Exact match
	strcpy(path[3221], "MAP/MAP076.29");
	strcpy(path[3220], "MAP/MAP076.32"); // Exact match
	strcpy(path[3223], "MAP/MAP076.33");
	strcpy(path[3224], "MAP/MAP076.36"); // Exact match
	strcpy(path[3225], "MAP/MAP076.37");
	strcpy(path[3209], "MAP/MAP076.6"); // Exact match
	strcpy(path[3210], "MAP/MAP076.7");
	strcpy(path[3211], "MAP/MAP076.9");
	// MAP/MAP076.GNS not found (1068 bytes)
	strcpy(path[3230], "MAP/MAP077.11"); // Exact match
	strcpy(path[3231], "MAP/MAP077.12");
	strcpy(path[3232], "MAP/MAP077.14"); // Exact match
	strcpy(path[3233], "MAP/MAP077.15");
	strcpy(path[3234], "MAP/MAP077.17"); // Exact match
	strcpy(path[3235], "MAP/MAP077.18");
	strcpy(path[3236], "MAP/MAP077.20"); // Exact match
	strcpy(path[3237], "MAP/MAP077.21");
	strcpy(path[3238], "MAP/MAP077.25"); // Exact match
	strcpy(path[3239], "MAP/MAP077.26");
	strcpy(path[3240], "MAP/MAP077.30"); // Exact match
	strcpy(path[3241], "MAP/MAP077.31");
	strcpy(path[3242], "MAP/MAP077.35"); // Exact match
	strcpy(path[3243], "MAP/MAP077.36");
	strcpy(path[3244], "MAP/MAP077.40"); // Exact match
	strcpy(path[3245], "MAP/MAP077.41");
	strcpy(path[3246], "MAP/MAP077.45"); // Exact match
	strcpy(path[3247], "MAP/MAP077.46");
	strcpy(path[3228], "MAP/MAP077.8"); // Exact match
	strcpy(path[3229], "MAP/MAP077.9");
	// MAP/MAP077.GNS not found (1188 bytes)
	strcpy(path[3253], "MAP/MAP078.13"); // Exact match
	strcpy(path[3252], "MAP/MAP078.14");
	strcpy(path[3251], "MAP/MAP078.18"); // Exact match
	strcpy(path[3254], "MAP/MAP078.19");
	strcpy(path[3257], "MAP/MAP078.22"); // Exact match
	strcpy(path[3256], "MAP/MAP078.23");
	strcpy(path[3255], "MAP/MAP078.26"); // Exact match
	strcpy(path[3258], "MAP/MAP078.27");
	strcpy(path[3259], "MAP/MAP078.31"); // Exact match
	strcpy(path[3260], "MAP/MAP078.32");
	strcpy(path[3267], "MAP/MAP078.35"); // Exact match
	strcpy(path[3262], "MAP/MAP078.36");
	strcpy(path[3265], "MAP/MAP078.39"); // Exact match
	strcpy(path[3264], "MAP/MAP078.40");
	strcpy(path[3263], "MAP/MAP078.43"); // Exact match
	strcpy(path[3266], "MAP/MAP078.44");
	strcpy(path[3261], "MAP/MAP078.47"); // Exact match
	strcpy(path[3268], "MAP/MAP078.48");
	strcpy(path[3249], "MAP/MAP078.8"); // Exact match
	strcpy(path[3250], "MAP/MAP078.9");
	// MAP/MAP078.GNS not found (1228 bytes)
	strcpy(path[3275], "MAP/MAP079.10");
	strcpy(path[3276], "MAP/MAP079.11");
	strcpy(path[3277], "MAP/MAP079.12");
	strcpy(path[3278], "MAP/MAP079.13");
	strcpy(path[3279], "MAP/MAP079.14");
	strcpy(path[3280], "MAP/MAP079.15");
	strcpy(path[3281], "MAP/MAP079.16");
	strcpy(path[3271], "MAP/MAP079.6"); // Exact match
	strcpy(path[3272], "MAP/MAP079.7");
	strcpy(path[3273], "MAP/MAP079.8");
	strcpy(path[3274], "MAP/MAP079.9");
	// MAP/MAP079.GNS not found (768 bytes)
	strcpy(path[3300], "MAP/MAP080.11"); // Exact match
	strcpy(path[3287], "MAP/MAP080.12");
	strcpy(path[3286], "MAP/MAP080.15"); // Exact match
	strcpy(path[3289], "MAP/MAP080.16");
	strcpy(path[3288], "MAP/MAP080.19"); // Exact match
	strcpy(path[3291], "MAP/MAP080.20");
	strcpy(path[3296], "MAP/MAP080.23"); // Exact match
	strcpy(path[3293], "MAP/MAP080.24");
	strcpy(path[3294], "MAP/MAP080.27"); // Exact match
	strcpy(path[3295], "MAP/MAP080.28");
	strcpy(path[3292], "MAP/MAP080.31"); // Exact match
	strcpy(path[3297], "MAP/MAP080.32");
	strcpy(path[3290], "MAP/MAP080.35"); // Exact match
	strcpy(path[3299], "MAP/MAP080.36");
	strcpy(path[3298], "MAP/MAP080.39"); // Exact match
	strcpy(path[3301], "MAP/MAP080.40");
	strcpy(path[3283], "MAP/MAP080.6"); // Exact match
	strcpy(path[3284], "MAP/MAP080.7");
	strcpy(path[3285], "MAP/MAP080.8");
	// MAP/MAP080.GNS not found (1088 bytes)
	strcpy(path[3306], "MAP/MAP081.10"); // Exact match
	strcpy(path[3307], "MAP/MAP081.11");
	strcpy(path[3314], "MAP/MAP081.14"); // Exact match
	strcpy(path[3309], "MAP/MAP081.15");
	strcpy(path[3322], "MAP/MAP081.18"); // Exact match
	strcpy(path[3311], "MAP/MAP081.19");
	strcpy(path[3320], "MAP/MAP081.22"); // Exact match
	strcpy(path[3313], "MAP/MAP081.23");
	strcpy(path[3308], "MAP/MAP081.26"); // Exact match
	strcpy(path[3315], "MAP/MAP081.27");
	strcpy(path[3318], "MAP/MAP081.30"); // Exact match
	strcpy(path[3317], "MAP/MAP081.31");
	strcpy(path[3312], "MAP/MAP081.34"); // Exact match
	strcpy(path[3319], "MAP/MAP081.35");
	strcpy(path[3310], "MAP/MAP081.38"); // Exact match
	strcpy(path[3321], "MAP/MAP081.39");
	strcpy(path[3316], "MAP/MAP081.42"); // Exact match
	strcpy(path[3323], "MAP/MAP081.43");
	strcpy(path[3304], "MAP/MAP081.6"); // Exact match
	strcpy(path[3305], "MAP/MAP081.7");
	// MAP/MAP081.GNS not found (1128 bytes)
	strcpy(path[3329], "MAP/MAP082.11");
	strcpy(path[3341], "MAP/MAP082.14"); // Exact match
	strcpy(path[3331], "MAP/MAP082.15");
	strcpy(path[3339], "MAP/MAP082.18"); // Exact match
	strcpy(path[3333], "MAP/MAP082.19");
	strcpy(path[3334], "MAP/MAP082.21");
	strcpy(path[3335], "MAP/MAP082.24"); // Exact match
	strcpy(path[3336], "MAP/MAP082.25");
	strcpy(path[3337], "MAP/MAP082.28"); // Exact match
	strcpy(path[3338], "MAP/MAP082.29");
	strcpy(path[3330], "MAP/MAP082.32"); // Exact match
	strcpy(path[3340], "MAP/MAP082.33");
	strcpy(path[3332], "MAP/MAP082.36"); // Exact match
	strcpy(path[3342], "MAP/MAP082.37");
	strcpy(path[3326], "MAP/MAP082.6"); // Exact match
	strcpy(path[3327], "MAP/MAP082.7");
	strcpy(path[3328], "MAP/MAP082.9");
	// MAP/MAP082.GNS not found (1068 bytes)
	strcpy(path[3347], "MAP/MAP083.10");
	strcpy(path[3348], "MAP/MAP083.12");
	strcpy(path[3349], "MAP/MAP083.14");
	strcpy(path[3350], "MAP/MAP083.16");
	strcpy(path[3351], "MAP/MAP083.18");
	strcpy(path[3352], "MAP/MAP083.20");
	strcpy(path[3353], "MAP/MAP083.22");
	strcpy(path[3354], "MAP/MAP083.24");
	strcpy(path[3355], "MAP/MAP083.26");
	strcpy(path[3356], "MAP/MAP083.28");
	strcpy(path[3357], "MAP/MAP083.38");
	strcpy(path[3345], "MAP/MAP083.8"); // Exact match
	strcpy(path[3346], "MAP/MAP083.9");
	// MAP/MAP083.GNS not found (1316 bytes)
	strcpy(path[3370], "MAP/MAP084.10"); // Exact match
	strcpy(path[3363], "MAP/MAP084.11");
	strcpy(path[3362], "MAP/MAP084.14"); // Exact match
	strcpy(path[3365], "MAP/MAP084.15");
	strcpy(path[3372], "MAP/MAP084.18"); // Exact match
	strcpy(path[3367], "MAP/MAP084.19");
	strcpy(path[3374], "MAP/MAP084.22"); // Exact match
	strcpy(path[3369], "MAP/MAP084.23");
	strcpy(path[3364], "MAP/MAP084.26"); // Exact match
	strcpy(path[3371], "MAP/MAP084.27");
	strcpy(path[3376], "MAP/MAP084.30"); // Exact match
	strcpy(path[3373], "MAP/MAP084.31");
	strcpy(path[3378], "MAP/MAP084.34"); // Exact match
	strcpy(path[3375], "MAP/MAP084.35");
	strcpy(path[3366], "MAP/MAP084.38"); // Exact match
	strcpy(path[3377], "MAP/MAP084.39");
	strcpy(path[3368], "MAP/MAP084.42"); // Exact match
	strcpy(path[3379], "MAP/MAP084.43");
	strcpy(path[3360], "MAP/MAP084.6"); // Exact match
	strcpy(path[3361], "MAP/MAP084.7");
	// MAP/MAP084.GNS not found (1128 bytes)
	strcpy(path[3385], "MAP/MAP085.10");
	strcpy(path[3392], "MAP/MAP085.12"); // Exact match
	strcpy(path[3387], "MAP/MAP085.13");
	strcpy(path[3400], "MAP/MAP085.15"); // Exact match
	strcpy(path[3389], "MAP/MAP085.16");
	strcpy(path[3390], "MAP/MAP085.18"); // Exact match
	strcpy(path[3391], "MAP/MAP085.19");
	strcpy(path[3386], "MAP/MAP085.21"); // Exact match
	strcpy(path[3393], "MAP/MAP085.22");
	strcpy(path[3396], "MAP/MAP085.24"); // Exact match
	strcpy(path[3395], "MAP/MAP085.25");
	strcpy(path[3394], "MAP/MAP085.27"); // Exact match
	strcpy(path[3397], "MAP/MAP085.28");
	strcpy(path[3398], "MAP/MAP085.30"); // Exact match
	strcpy(path[3399], "MAP/MAP085.31");
	strcpy(path[3388], "MAP/MAP085.33"); // Exact match
	strcpy(path[3401], "MAP/MAP085.34");
	strcpy(path[3382], "MAP/MAP085.6"); // Exact match
	strcpy(path[3383], "MAP/MAP085.7");
	strcpy(path[3384], "MAP/MAP085.9"); // Exact match
	// MAP/MAP085.GNS not found (948 bytes)
	strcpy(path[3407], "MAP/MAP086.10");
	strcpy(path[3408], "MAP/MAP086.12"); // Exact match
	strcpy(path[3409], "MAP/MAP086.13");
	strcpy(path[3420], "MAP/MAP086.15"); // Exact match
	strcpy(path[3411], "MAP/MAP086.16");
	strcpy(path[3422], "MAP/MAP086.18"); // Exact match
	strcpy(path[3413], "MAP/MAP086.19");
	strcpy(path[3414], "MAP/MAP086.21"); // Exact match
	strcpy(path[3415], "MAP/MAP086.22");
	strcpy(path[3416], "MAP/MAP086.24"); // Exact match
	strcpy(path[3417], "MAP/MAP086.25");
	strcpy(path[3410], "MAP/MAP086.27"); // Exact match
	strcpy(path[3419], "MAP/MAP086.28");
	strcpy(path[3412], "MAP/MAP086.30"); // Exact match
	strcpy(path[3421], "MAP/MAP086.31");
	strcpy(path[3418], "MAP/MAP086.33"); // Exact match
	strcpy(path[3423], "MAP/MAP086.34");
	strcpy(path[3404], "MAP/MAP086.6"); // Exact match
	strcpy(path[3405], "MAP/MAP086.7");
	strcpy(path[3406], "MAP/MAP086.9"); // Exact match
	// MAP/MAP086.GNS not found (948 bytes)
	strcpy(path[3428], "MAP/MAP087.10");
	strcpy(path[3429], "MAP/MAP087.11");
	strcpy(path[3430], "MAP/MAP087.12");
	strcpy(path[3431], "MAP/MAP087.13");
	strcpy(path[3432], "MAP/MAP087.14");
	strcpy(path[3433], "MAP/MAP087.15");
	strcpy(path[3434], "MAP/MAP087.16");
	strcpy(path[3435], "MAP/MAP087.17");
	strcpy(path[3436], "MAP/MAP087.18");
	strcpy(path[3426], "MAP/MAP087.8"); // Exact match
	strcpy(path[3427], "MAP/MAP087.9");
	// MAP/MAP087.GNS not found (808 bytes)
	strcpy(path[3449], "MAP/MAP088.11"); // Exact match
	strcpy(path[3442], "MAP/MAP088.12");
	strcpy(path[3443], "MAP/MAP088.14"); // Exact match
	strcpy(path[3444], "MAP/MAP088.15");
	strcpy(path[3447], "MAP/MAP088.17"); // Exact match
	strcpy(path[3446], "MAP/MAP088.18");
	strcpy(path[3445], "MAP/MAP088.20"); // Exact match
	strcpy(path[3448], "MAP/MAP088.21");
	strcpy(path[3441], "MAP/MAP088.23"); // Exact match
	strcpy(path[3450], "MAP/MAP088.24");
	strcpy(path[3451], "MAP/MAP088.26"); // Exact match
	strcpy(path[3452], "MAP/MAP088.27");
	strcpy(path[3453], "MAP/MAP088.29"); // Exact match
	strcpy(path[3454], "MAP/MAP088.30");
	strcpy(path[3455], "MAP/MAP088.32"); // Exact match
	strcpy(path[3456], "MAP/MAP088.33");
	strcpy(path[3457], "MAP/MAP088.35"); // Exact match
	strcpy(path[3458], "MAP/MAP088.36");
	strcpy(path[3439], "MAP/MAP088.8"); // Exact match
	strcpy(path[3440], "MAP/MAP088.9");
	// MAP/MAP088.GNS not found (988 bytes)
	strcpy(path[3463], "MAP/MAP089.10"); // Exact match
	strcpy(path[3464], "MAP/MAP089.11");
	strcpy(path[3465], "MAP/MAP089.14"); // Exact match
	strcpy(path[3466], "MAP/MAP089.15");
	strcpy(path[3467], "MAP/MAP089.18"); // Exact match
	strcpy(path[3468], "MAP/MAP089.19");
	strcpy(path[3469], "MAP/MAP089.22"); // Exact match
	strcpy(path[3470], "MAP/MAP089.23");
	strcpy(path[3471], "MAP/MAP089.26"); // Exact match
	strcpy(path[3472], "MAP/MAP089.27");
	strcpy(path[3473], "MAP/MAP089.30"); // Exact match
	strcpy(path[3474], "MAP/MAP089.31");
	strcpy(path[3477], "MAP/MAP089.34"); // Exact match
	strcpy(path[3476], "MAP/MAP089.35");
	strcpy(path[3475], "MAP/MAP089.38"); // Exact match
	strcpy(path[3478], "MAP/MAP089.39");
	strcpy(path[3479], "MAP/MAP089.42"); // Exact match
	strcpy(path[3480], "MAP/MAP089.43");
	strcpy(path[3461], "MAP/MAP089.6"); // Exact match
	strcpy(path[3462], "MAP/MAP089.7");
	// MAP/MAP089.GNS not found (1128 bytes)
	strcpy(path[3485], "MAP/MAP090.10"); // Exact match
	strcpy(path[3486], "MAP/MAP090.11");
	strcpy(path[3487], "MAP/MAP090.14"); // Exact match
	strcpy(path[3488], "MAP/MAP090.15");
	strcpy(path[3489], "MAP/MAP090.18"); // Exact match
	strcpy(path[3490], "MAP/MAP090.19");
	strcpy(path[3499], "MAP/MAP090.22"); // Exact match
	strcpy(path[3492], "MAP/MAP090.23");
	strcpy(path[3493], "MAP/MAP090.26"); // Exact match
	strcpy(path[3494], "MAP/MAP090.27");
	strcpy(path[3495], "MAP/MAP090.30"); // Exact match
	strcpy(path[3496], "MAP/MAP090.31");
	strcpy(path[3497], "MAP/MAP090.34"); // Exact match
	strcpy(path[3498], "MAP/MAP090.35");
	strcpy(path[3491], "MAP/MAP090.38"); // Exact match
	strcpy(path[3500], "MAP/MAP090.39");
	strcpy(path[3501], "MAP/MAP090.42"); // Exact match
	strcpy(path[3502], "MAP/MAP090.43");
	strcpy(path[3483], "MAP/MAP090.6"); // Exact match
	strcpy(path[3484], "MAP/MAP090.7");
	// MAP/MAP090.GNS not found (1128 bytes)
	strcpy(path[3507], "MAP/MAP091.10");
	strcpy(path[3508], "MAP/MAP091.13"); // Exact match
	strcpy(path[3509], "MAP/MAP091.14");
	strcpy(path[3510], "MAP/MAP091.17"); // Exact match
	strcpy(path[3511], "MAP/MAP091.18");
	strcpy(path[3512], "MAP/MAP091.21"); // Exact match
	strcpy(path[3513], "MAP/MAP091.22");
	strcpy(path[3505], "MAP/MAP091.8"); // Exact match
	strcpy(path[3506], "MAP/MAP091.9");
	// MAP/MAP091.GNS not found (628 bytes)
	strcpy(path[3518], "MAP/MAP092.12"); // Exact match
	strcpy(path[3519], "MAP/MAP092.13");
	strcpy(path[3528], "MAP/MAP092.16"); // Exact match
	strcpy(path[3529], "MAP/MAP092.17");
	strcpy(path[3530], "MAP/MAP092.20"); // Exact match
	strcpy(path[3531], "MAP/MAP092.21");
	strcpy(path[3516], "MAP/MAP092.30"); // Exact match
	strcpy(path[3525], "MAP/MAP092.31");
	strcpy(path[3526], "MAP/MAP092.34"); // Exact match
	strcpy(path[3527], "MAP/MAP092.35");
	strcpy(path[3520], "MAP/MAP092.38"); // Exact match
	strcpy(path[3521], "MAP/MAP092.39");
	strcpy(path[3522], "MAP/MAP092.42"); // Exact match
	strcpy(path[3523], "MAP/MAP092.43");
	strcpy(path[3524], "MAP/MAP092.8"); // Exact match
	strcpy(path[3517], "MAP/MAP092.9");
	// MAP/MAP092.GNS not found (1088 bytes)
	strcpy(path[3536], "MAP/MAP093.10"); // Exact match
	strcpy(path[3537], "MAP/MAP093.11");
	strcpy(path[3538], "MAP/MAP093.14"); // Exact match
	strcpy(path[3539], "MAP/MAP093.15");
	strcpy(path[3540], "MAP/MAP093.18"); // Exact match
	strcpy(path[3541], "MAP/MAP093.19");
	strcpy(path[3534], "MAP/MAP093.6"); // Exact match
	strcpy(path[3535], "MAP/MAP093.7");
	// MAP/MAP093.GNS not found (528 bytes)
	strcpy(path[3546], "MAP/MAP094.16"); // Exact match
	strcpy(path[3547], "MAP/MAP094.17");
	strcpy(path[3544], "MAP/MAP094.7"); // Exact match
	strcpy(path[3545], "MAP/MAP094.8");
	// MAP/MAP094.GNS not found (448 bytes)
	strcpy(path[3555], "MAP/MAP095.11"); // Exact match
	strcpy(path[3552], "MAP/MAP095.12");
	strcpy(path[3553], "MAP/MAP095.15"); // Exact match
	strcpy(path[3554], "MAP/MAP095.16");
	strcpy(path[3559], "MAP/MAP095.19"); // Exact match
	strcpy(path[3556], "MAP/MAP095.20");
	strcpy(path[3557], "MAP/MAP095.28"); // Exact match
	strcpy(path[3558], "MAP/MAP095.29");
	strcpy(path[3551], "MAP/MAP095.32"); // Exact match
	strcpy(path[3560], "MAP/MAP095.33");
	strcpy(path[3549], "MAP/MAP095.36"); // Exact match
	strcpy(path[3564], "MAP/MAP095.37");
	strcpy(path[3563], "MAP/MAP095.40"); // Exact match
	strcpy(path[3562], "MAP/MAP095.41");
	strcpy(path[3561], "MAP/MAP095.7"); // Exact match
	strcpy(path[3550], "MAP/MAP095.8");
	// MAP/MAP095.GNS not found (1048 bytes)
	strcpy(path[3566], "MAP/MAP096.12"); // Exact match
	strcpy(path[3569], "MAP/MAP096.13");
	strcpy(path[3572], "MAP/MAP096.16"); // Exact match
	strcpy(path[3571], "MAP/MAP096.17");
	strcpy(path[3570], "MAP/MAP096.20"); // Exact match
	strcpy(path[3573], "MAP/MAP096.21");
	strcpy(path[3568], "MAP/MAP096.7"); // Exact match
	strcpy(path[3567], "MAP/MAP096.8");
	// MAP/MAP096.GNS not found (568 bytes)
	strcpy(path[3585], "MAP/MAP097.11"); // Exact match
	strcpy(path[3578], "MAP/MAP097.12");
	strcpy(path[3587], "MAP/MAP097.15"); // Exact match
	strcpy(path[3580], "MAP/MAP097.16");
	strcpy(path[3591], "MAP/MAP097.18"); // Exact match
	strcpy(path[3582], "MAP/MAP097.19");
	strcpy(path[3593], "MAP/MAP097.21"); // Exact match
	strcpy(path[3584], "MAP/MAP097.22");
	strcpy(path[3577], "MAP/MAP097.25"); // Exact match
	strcpy(path[3586], "MAP/MAP097.26");
	strcpy(path[3579], "MAP/MAP097.29"); // Exact match
	strcpy(path[3588], "MAP/MAP097.30");
	strcpy(path[3589], "MAP/MAP097.33"); // Exact match
	strcpy(path[3590], "MAP/MAP097.34");
	strcpy(path[3583], "MAP/MAP097.37"); // Exact match
	strcpy(path[3592], "MAP/MAP097.38");
	strcpy(path[3581], "MAP/MAP097.41"); // Exact match
	strcpy(path[3594], "MAP/MAP097.42");
	strcpy(path[3575], "MAP/MAP097.7"); // Exact match
	strcpy(path[3576], "MAP/MAP097.8");
	// MAP/MAP097.GNS not found (1108 bytes)
	strcpy(path[3598], "MAP/MAP098.10"); // Exact match
	strcpy(path[3599], "MAP/MAP098.11");
	strcpy(path[3600], "MAP/MAP098.14"); // Exact match
	strcpy(path[3601], "MAP/MAP098.15");
	strcpy(path[3612], "MAP/MAP098.18"); // Exact match
	strcpy(path[3603], "MAP/MAP098.19");
	strcpy(path[3610], "MAP/MAP098.22"); // Exact match
	strcpy(path[3605], "MAP/MAP098.23");
	strcpy(path[3606], "MAP/MAP098.26"); // Exact match
	strcpy(path[3607], "MAP/MAP098.27");
	strcpy(path[3614], "MAP/MAP098.30"); // Exact match
	strcpy(path[3609], "MAP/MAP098.31");
	strcpy(path[3608], "MAP/MAP098.34"); // Exact match
	strcpy(path[3611], "MAP/MAP098.35");
	strcpy(path[3602], "MAP/MAP098.38"); // Exact match
	strcpy(path[3613], "MAP/MAP098.39");
	strcpy(path[3604], "MAP/MAP098.42"); // Exact match
	strcpy(path[3615], "MAP/MAP098.43");
	strcpy(path[3596], "MAP/MAP098.6"); // Exact match
	strcpy(path[3597], "MAP/MAP098.7");
	// MAP/MAP098.GNS not found (1128 bytes)
	strcpy(path[3619], "MAP/MAP099.10"); // Exact match
	strcpy(path[3620], "MAP/MAP099.11");
	strcpy(path[3621], "MAP/MAP099.14"); // Exact match
	strcpy(path[3622], "MAP/MAP099.15");
	strcpy(path[3623], "MAP/MAP099.18"); // Exact match
	strcpy(path[3624], "MAP/MAP099.19");
	strcpy(path[3625], "MAP/MAP099.22"); // Exact match
	strcpy(path[3626], "MAP/MAP099.23");
	strcpy(path[3627], "MAP/MAP099.26"); // Exact match
	strcpy(path[3628], "MAP/MAP099.27");
	strcpy(path[3629], "MAP/MAP099.30"); // Exact match
	strcpy(path[3630], "MAP/MAP099.31");
	strcpy(path[3631], "MAP/MAP099.34"); // Exact match
	strcpy(path[3632], "MAP/MAP099.35");
	strcpy(path[3633], "MAP/MAP099.38"); // Exact match
	strcpy(path[3634], "MAP/MAP099.39");
	strcpy(path[3635], "MAP/MAP099.42"); // Exact match
	strcpy(path[3636], "MAP/MAP099.43");
	strcpy(path[3617], "MAP/MAP099.6"); // Exact match
	strcpy(path[3618], "MAP/MAP099.7");
	// MAP/MAP099.GNS not found (1128 bytes)
	strcpy(path[3638], "MAP/MAP100.7"); // Exact match
	strcpy(path[3639], "MAP/MAP100.8");
	// MAP/MAP100.GNS not found (228 bytes)
	strcpy(path[3643], "MAP/MAP101.10");
	strcpy(path[3642], "MAP/MAP101.9"); // Exact match
	// MAP/MAP101.GNS not found (268 bytes)
	strcpy(path[3645], "MAP/MAP102.7"); // Exact match
	strcpy(path[3646], "MAP/MAP102.8");
	// MAP/MAP102.GNS not found (228 bytes)
	strcpy(path[3649], "MAP/MAP103.10");
	strcpy(path[3650], "MAP/MAP103.11");
	strcpy(path[3651], "MAP/MAP103.13"); // Exact match
	strcpy(path[3652], "MAP/MAP103.14");
	strcpy(path[3655], "MAP/MAP103.16"); // Exact match
	strcpy(path[3654], "MAP/MAP103.17");
	strcpy(path[3653], "MAP/MAP103.19"); // Exact match
	strcpy(path[3656], "MAP/MAP103.20");
	strcpy(path[3657], "MAP/MAP103.23"); // Exact match
	strcpy(path[3658], "MAP/MAP103.24");
	strcpy(path[3659], "MAP/MAP103.27"); // Exact match
	strcpy(path[3660], "MAP/MAP103.28");
	strcpy(path[3661], "MAP/MAP103.31"); // Exact match
	strcpy(path[3662], "MAP/MAP103.32");
	strcpy(path[3665], "MAP/MAP103.35"); // Exact match
	strcpy(path[3664], "MAP/MAP103.36");
	strcpy(path[3663], "MAP/MAP103.39"); // Exact match
	strcpy(path[3666], "MAP/MAP103.40");
	strcpy(path[3648], "MAP/MAP103.9"); // Exact match
	// MAP/MAP103.GNS not found (1088 bytes)
	strcpy(path[3671], "MAP/MAP104.10");
	strcpy(path[3669], "MAP/MAP104.8"); // Exact match
	strcpy(path[3670], "MAP/MAP104.9");
	// MAP/MAP104.GNS not found (328 bytes)
	strcpy(path[3676], "MAP/MAP105.14"); // Exact match
	strcpy(path[3675], "MAP/MAP105.15");
	strcpy(path[3678], "MAP/MAP105.17");
	strcpy(path[3679], "MAP/MAP105.19");
	strcpy(path[3680], "MAP/MAP105.21");
	strcpy(path[3681], "MAP/MAP105.23");
	strcpy(path[3674], "MAP/MAP105.6"); // Exact match
	strcpy(path[3677], "MAP/MAP105.7");
	// MAP/MAP105.GNS not found (728 bytes)
	strcpy(path[3686], "MAP/MAP106.11");
	strcpy(path[3687], "MAP/MAP106.13");
	strcpy(path[3688], "MAP/MAP106.15");
	strcpy(path[3689], "MAP/MAP106.17");
	strcpy(path[3683], "MAP/MAP106.6"); // Exact match
	strcpy(path[3684], "MAP/MAP106.7");
	strcpy(path[3685], "MAP/MAP106.9");
	// MAP/MAP106.GNS not found (628 bytes)
	strcpy(path[3694], "MAP/MAP107.11");
	strcpy(path[3695], "MAP/MAP107.13");
	strcpy(path[3696], "MAP/MAP107.15");
	strcpy(path[3697], "MAP/MAP107.17");
	strcpy(path[3691], "MAP/MAP107.6"); // Exact match
	strcpy(path[3692], "MAP/MAP107.7");
	strcpy(path[3693], "MAP/MAP107.9");
	// MAP/MAP107.GNS not found (628 bytes)
	strcpy(path[3702], "MAP/MAP108.11");
	strcpy(path[3703], "MAP/MAP108.13");
	strcpy(path[3704], "MAP/MAP108.15");
	strcpy(path[3705], "MAP/MAP108.17");
	strcpy(path[3699], "MAP/MAP108.6"); // Exact match
	strcpy(path[3700], "MAP/MAP108.7");
	strcpy(path[3701], "MAP/MAP108.9");
	// MAP/MAP108.GNS not found (628 bytes)
	strcpy(path[3710], "MAP/MAP109.11");
	strcpy(path[3711], "MAP/MAP109.13");
	strcpy(path[3712], "MAP/MAP109.15");
	strcpy(path[3713], "MAP/MAP109.17");
	strcpy(path[3707], "MAP/MAP109.6"); // Exact match
	strcpy(path[3708], "MAP/MAP109.7");
	strcpy(path[3709], "MAP/MAP109.9");
	// MAP/MAP109.GNS not found (628 bytes)
	strcpy(path[3718], "MAP/MAP110.11");
	strcpy(path[3719], "MAP/MAP110.13");
	strcpy(path[3720], "MAP/MAP110.15");
	strcpy(path[3721], "MAP/MAP110.17");
	strcpy(path[3715], "MAP/MAP110.6"); // Exact match
	strcpy(path[3716], "MAP/MAP110.7");
	strcpy(path[3717], "MAP/MAP110.9");
	// MAP/MAP110.GNS not found (628 bytes)
	strcpy(path[3725], "MAP/MAP111.11");
	strcpy(path[3726], "MAP/MAP111.13");
	strcpy(path[3727], "MAP/MAP111.15");
	strcpy(path[3728], "MAP/MAP111.17");
	strcpy(path[3729], "MAP/MAP111.19");
	strcpy(path[3723], "MAP/MAP111.8"); // Exact match
	strcpy(path[3724], "MAP/MAP111.9");
	// MAP/MAP111.GNS not found (668 bytes)
	strcpy(path[3734], "MAP/MAP112.10");
	strcpy(path[3735], "MAP/MAP112.12");
	strcpy(path[3736], "MAP/MAP112.14");
	strcpy(path[3737], "MAP/MAP112.16");
	strcpy(path[3731], "MAP/MAP112.6"); // Exact match
	strcpy(path[3732], "MAP/MAP112.7");
	strcpy(path[3733], "MAP/MAP112.8");
	// MAP/MAP112.GNS not found (608 bytes)
	strcpy(path[3742], "MAP/MAP113.11");
	strcpy(path[3743], "MAP/MAP113.13");
	strcpy(path[3744], "MAP/MAP113.15");
	strcpy(path[3745], "MAP/MAP113.17");
	strcpy(path[3739], "MAP/MAP113.6"); // Exact match
	strcpy(path[3740], "MAP/MAP113.7");
	strcpy(path[3741], "MAP/MAP113.9");
	// MAP/MAP113.GNS not found (628 bytes)
	strcpy(path[3750], "MAP/MAP114.11");
	strcpy(path[3751], "MAP/MAP114.13");
	strcpy(path[3752], "MAP/MAP114.15");
	strcpy(path[3753], "MAP/MAP114.17");
	strcpy(path[3747], "MAP/MAP114.6"); // Exact match
	strcpy(path[3748], "MAP/MAP114.7");
	strcpy(path[3749], "MAP/MAP114.9");
	// MAP/MAP114.GNS not found (628 bytes)
	strcpy(path[3757], "MAP/MAP115.10"); // Exact match
	strcpy(path[3758], "MAP/MAP115.11");
	strcpy(path[3765], "MAP/MAP115.14"); // Exact match
	strcpy(path[3760], "MAP/MAP115.15");
	strcpy(path[3771], "MAP/MAP115.18"); // Exact match
	strcpy(path[3762], "MAP/MAP115.19");
	strcpy(path[3773], "MAP/MAP115.22"); // Exact match
	strcpy(path[3764], "MAP/MAP115.23");
	strcpy(path[3759], "MAP/MAP115.26"); // Exact match
	strcpy(path[3766], "MAP/MAP115.27");
	strcpy(path[3767], "MAP/MAP115.30"); // Exact match
	strcpy(path[3768], "MAP/MAP115.31");
	strcpy(path[3763], "MAP/MAP115.34"); // Exact match
	strcpy(path[3770], "MAP/MAP115.35");
	strcpy(path[3761], "MAP/MAP115.38"); // Exact match
	strcpy(path[3772], "MAP/MAP115.39");
	strcpy(path[3769], "MAP/MAP115.42"); // Exact match
	strcpy(path[3774], "MAP/MAP115.43");
	strcpy(path[3755], "MAP/MAP115.6"); // Exact match
	strcpy(path[3756], "MAP/MAP115.7");
	// MAP/MAP115.GNS not found (1128 bytes)
	strcpy(path[3777], "MAP/MAP116.6"); // Exact match
	strcpy(path[3778], "MAP/MAP116.7");
	// MAP/MAP116.GNS not found (208 bytes)
	strcpy(path[3780], "MAP/MAP117.6"); // Exact match
	strcpy(path[3781], "MAP/MAP117.7");
	// MAP/MAP117.GNS not found (208 bytes)
	strcpy(path[3786], "MAP/MAP118.6"); // Exact match
	strcpy(path[3784], "MAP/MAP118.7");
	// MAP/MAP118.GNS not found (208 bytes)
	strcpy(path[3783], "MAP/MAP119.6"); // Exact match
	strcpy(path[3787], "MAP/MAP119.7");
	// MAP/MAP119.GNS not found (208 bytes)
	strcpy(path[3789], "MAP/MAP125.6"); // Exact match
	strcpy(path[3790], "MAP/MAP125.7");
	// MAP/MAP125.GNS not found (208 bytes)

	strcpy(path[746], "MENU/BK_SHOP.TIM");
	strcpy(path[747], "MENU/BK_SHOP2.TIM");
	strcpy(path[748], "MENU/BK_SHOP3.TIM");
	strcpy(path[749], "MENU/BK_HONE.TIM");
	strcpy(path[750], "MENU/BK_HONE2.TIM");
	strcpy(path[751], "MENU/BK_HONE3.TIM");
	strcpy(path[752], "MENU/BK_FITR.TIM");
	strcpy(path[753], "MENU/BK_FITR2.TIM");
	strcpy(path[754], "MENU/BK_FITR3.TIM");


	strcpy(path[769], "MENU/FFTSAVE.DAT");
	strcpy(path[755], "MENU/TUTO1.SCR");
	strcpy(path[756], "MENU/TUTO1.MES");
	strcpy(path[757], "MENU/TUTO2.SCR");
	strcpy(path[758], "MENU/TUTO2.MES");
	strcpy(path[759], "MENU/TUTO3.SCR");
	strcpy(path[760], "MENU/TUTO3.MES");
	strcpy(path[761], "MENU/TUTO4.SCR");
	strcpy(path[762], "MENU/TUTO4.MES");
	strcpy(path[763], "MENU/TUTO5.SCR");
	strcpy(path[764], "MENU/TUTO5.MES");
	strcpy(path[765], "MENU/TUTO6.SCR");
	strcpy(path[766], "MENU/TUTO6.MES");
	strcpy(path[767], "MENU/TUTO7.SCR");
	strcpy(path[768], "MENU/TUTO7.MES");
	strcpy(path[771], "WORLD/WLDPIC.BIN"); // Exact match
	
	strcpy(path[773], "WORLD/SNPLBIN.BIN"); // Exact match
	// WORLD/SNPLMES.BIN not found (245760 bytes)
	strcpy(path[775], "WORLD/WLDBK.BIN");
	strcpy(path[776], "WORLD/WLDCORE.BIN");
	strcpy(path[772], "WORLD/WLDMES.BIN");
	// WORLD/WLDTEX.TM2 not found (274432 bytes)
	strcpy(path[777], "WORLD/WORLD.BIN");
	strcpy(path[778], "SOUND/ENV.SED");
	strcpy(path[779], "SOUND/SYSTEM.SED");
	strcpy(path[781], "SOUND/MUSIC_00.SMD");
	strcpy(path[782], "SOUND/MUSIC_01.SMD");
	strcpy(path[783], "SOUND/MUSIC_02.SMD");
	strcpy(path[784], "SOUND/MUSIC_03.SMD");
	strcpy(path[785], "SOUND/MUSIC_04.SMD");
	strcpy(path[786], "SOUND/MUSIC_05.SMD");
	strcpy(path[787], "SOUND/MUSIC_06.SMD");
	strcpy(path[788], "SOUND/MUSIC_07.SMD");
	strcpy(path[789], "SOUND/MUSIC_08.SMD");
	strcpy(path[790], "SOUND/MUSIC_09.SMD");
	strcpy(path[791], "SOUND/MUSIC_10.SMD");
	strcpy(path[792], "SOUND/MUSIC_11.SMD");
	strcpy(path[793], "SOUND/MUSIC_12.SMD");
	strcpy(path[794], "SOUND/MUSIC_13.SMD");
	strcpy(path[795], "SOUND/MUSIC_14.SMD");
	strcpy(path[796], "SOUND/MUSIC_15.SMD");
	strcpy(path[797], "SOUND/MUSIC_16.SMD");
	strcpy(path[798], "SOUND/MUSIC_17.SMD");
	strcpy(path[799], "SOUND/MUSIC_18.SMD");
	strcpy(path[800], "SOUND/MUSIC_19.SMD");
	strcpy(path[801], "SOUND/MUSIC_20.SMD");
	strcpy(path[802], "SOUND/MUSIC_21.SMD");
	strcpy(path[803], "SOUND/MUSIC_22.SMD");
	strcpy(path[804], "SOUND/MUSIC_23.SMD");
	strcpy(path[805], "SOUND/MUSIC_24.SMD");
	strcpy(path[806], "SOUND/MUSIC_25.SMD");
	strcpy(path[807], "SOUND/MUSIC_26.SMD");
	strcpy(path[808], "SOUND/MUSIC_27.SMD");
	strcpy(path[809], "SOUND/MUSIC_28.SMD");
	strcpy(path[810], "SOUND/MUSIC_29.SMD");
	strcpy(path[811], "SOUND/MUSIC_30.SMD");
	strcpy(path[812], "SOUND/MUSIC_31.SMD");
	strcpy(path[813], "SOUND/MUSIC_32.SMD");
	strcpy(path[814], "SOUND/MUSIC_33.SMD");
	strcpy(path[815], "SOUND/MUSIC_34.SMD");
	strcpy(path[816], "SOUND/MUSIC_35.SMD");
	strcpy(path[817], "SOUND/MUSIC_36.SMD");
	strcpy(path[818], "SOUND/MUSIC_37.SMD");
	strcpy(path[819], "SOUND/MUSIC_38.SMD");
	strcpy(path[820], "SOUND/MUSIC_39.SMD");
	strcpy(path[821], "SOUND/MUSIC_40.SMD");
	strcpy(path[822], "SOUND/MUSIC_41.SMD");
	strcpy(path[823], "SOUND/MUSIC_42.SMD");
	strcpy(path[824], "SOUND/MUSIC_43.SMD");
	strcpy(path[825], "SOUND/MUSIC_44.SMD");
	strcpy(path[826], "SOUND/MUSIC_45.SMD");
	strcpy(path[827], "SOUND/MUSIC_46.SMD");
	strcpy(path[828], "SOUND/MUSIC_47.SMD");
	strcpy(path[829], "SOUND/MUSIC_48.SMD");
	strcpy(path[830], "SOUND/MUSIC_49.SMD");
	strcpy(path[831], "SOUND/MUSIC_50.SMD");
	strcpy(path[832], "SOUND/MUSIC_51.SMD");
	strcpy(path[833], "SOUND/MUSIC_52.SMD");
	strcpy(path[834], "SOUND/MUSIC_53.SMD");
	strcpy(path[835], "SOUND/MUSIC_54.SMD");
	strcpy(path[836], "SOUND/MUSIC_55.SMD");
	strcpy(path[837], "SOUND/MUSIC_56.SMD");
	strcpy(path[838], "SOUND/MUSIC_57.SMD");
	strcpy(path[839], "SOUND/MUSIC_58.SMD");
	strcpy(path[840], "SOUND/MUSIC_59.SMD");
	strcpy(path[841], "SOUND/MUSIC_60.SMD");
	strcpy(path[842], "SOUND/MUSIC_61.SMD");
	strcpy(path[843], "SOUND/MUSIC_62.SMD");
	strcpy(path[844], "SOUND/MUSIC_63.SMD");
	strcpy(path[845], "SOUND/MUSIC_64.SMD");
	strcpy(path[846], "SOUND/MUSIC_65.SMD");
	strcpy(path[847], "SOUND/MUSIC_66.SMD");
	strcpy(path[848], "SOUND/MUSIC_67.SMD");
	strcpy(path[849], "SOUND/MUSIC_68.SMD");
	strcpy(path[850], "SOUND/MUSIC_69.SMD");
	strcpy(path[851], "SOUND/MUSIC_70.SMD");
	strcpy(path[852], "SOUND/MUSIC_71.SMD");
	strcpy(path[853], "SOUND/MUSIC_72.SMD");
	strcpy(path[854], "SOUND/MUSIC_73.SMD");
	strcpy(path[855], "SOUND/MUSIC_74.SMD");
	strcpy(path[856], "SOUND/MUSIC_75.SMD");
	strcpy(path[857], "SOUND/MUSIC_76.SMD");
	strcpy(path[858], "SOUND/MUSIC_77.SMD");
	strcpy(path[859], "SOUND/MUSIC_78.SMD");
	strcpy(path[860], "SOUND/MUSIC_79.SMD");
	strcpy(path[861], "SOUND/MUSIC_80.SMD");
	strcpy(path[862], "SOUND/MUSIC_81.SMD");
	strcpy(path[863], "SOUND/MUSIC_82.SMD");
	strcpy(path[864], "SOUND/MUSIC_83.SMD");
	strcpy(path[865], "SOUND/MUSIC_84.SMD");
	strcpy(path[866], "SOUND/MUSIC_85.SMD");
	strcpy(path[867], "SOUND/MUSIC_86.SMD");
	strcpy(path[868], "SOUND/MUSIC_87.SMD");
	strcpy(path[869], "SOUND/MUSIC_88.SMD");
	strcpy(path[870], "SOUND/MUSIC_89.SMD");
	strcpy(path[871], "SOUND/MUSIC_90.SMD");
	strcpy(path[872], "SOUND/MUSIC_91.SMD");
	strcpy(path[873], "SOUND/MUSIC_92.SMD");
	strcpy(path[874], "SOUND/MUSIC_93.SMD");
	strcpy(path[875], "SOUND/MUSIC_94.SMD");
	strcpy(path[876], "SOUND/MUSIC_95.SMD");
	strcpy(path[877], "SOUND/MUSIC_96.SMD");
	strcpy(path[878], "SOUND/MUSIC_97.SMD");
	strcpy(path[879], "SOUND/MUSIC_98.SMD");
	strcpy(path[880], "SOUND/MUSIC_99.SMD");
	strcpy(path[780], "SOUND/WAVESET.WD");
	strcpy(path[1000], "EFFECT/E066.BIN.duplicate");
	strcpy(path[1011], "EFFECT/E078.BIN.duplicate");
	strcpy(path[1014], "EFFECT/E081.BIN.duplicate");
	strcpy(path[1015], "EFFECT/E082.BIN.duplicate");
	strcpy(path[1016], "EFFECT/E083.BIN.duplicate");
	strcpy(path[1017], "EFFECT/E084.BIN.duplicate");
	strcpy(path[1018], "EFFECT/E085.BIN.duplicate");
	strcpy(path[1019], "EFFECT/E086.BIN.duplicate");
	strcpy(path[1020], "EFFECT/E087.BIN.duplicate");
	strcpy(path[1028], "EFFECT/E095.BIN.duplicate");
	strcpy(path[1029], "EFFECT/E096.BIN.duplicate");
	strcpy(path[1030], "EFFECT/E097.BIN.duplicate");
	strcpy(path[1031], "EFFECT/E098.BIN.duplicate");
	strcpy(path[1032], "EFFECT/E099.BIN.duplicate");
	strcpy(path[1033], "EFFECT/E100.BIN.duplicate");
	strcpy(path[1034], "EFFECT/E101.BIN.duplicate");
	strcpy(path[1035], "EFFECT/E102.BIN.duplicate");
	strcpy(path[1036], "EFFECT/E103.BIN.duplicate");
	strcpy(path[1037], "EFFECT/E104.BIN.duplicate");
	strcpy(path[1038], "EFFECT/E105.BIN.duplicate");
	strcpy(path[1039], "EFFECT/E106.BIN.duplicate");
	strcpy(path[1040], "EFFECT/E107.BIN.duplicate");
	strcpy(path[1041], "EFFECT/E108.BIN.duplicate");
	strcpy(path[1042], "EFFECT/E109.BIN.duplicate");
	strcpy(path[1043], "EFFECT/E110.BIN.duplicate");
	strcpy(path[1044], "EFFECT/E111.BIN.duplicate");
	strcpy(path[1045], "EFFECT/E112.BIN.duplicate");
	strcpy(path[1046], "EFFECT/E113.BIN.duplicate");
	strcpy(path[1047], "EFFECT/E114.BIN.duplicate");
	strcpy(path[1048], "EFFECT/E115.BIN.duplicate");
	strcpy(path[1049], "EFFECT/E116.BIN.duplicate");
	strcpy(path[1050], "EFFECT/E117.BIN.duplicate");
	strcpy(path[1051], "EFFECT/E118.BIN.duplicate");
	strcpy(path[1052], "EFFECT/E119.BIN.duplicate");
	strcpy(path[1053], "EFFECT/E120.BIN.duplicate");
	strcpy(path[1054], "EFFECT/E121.BIN.duplicate");
	strcpy(path[1055], "EFFECT/E122.BIN.duplicate");
	strcpy(path[1056], "EFFECT/E123.BIN.duplicate");
	strcpy(path[1057], "EFFECT/E124.BIN.duplicate");
	strcpy(path[1058], "EFFECT/E125.BIN.duplicate");
	strcpy(path[1059], "EFFECT/E126.BIN.duplicate");
	strcpy(path[1060], "EFFECT/E127.BIN.duplicate");
	strcpy(path[1061], "EFFECT/E128.BIN.duplicate");
	strcpy(path[1062], "EFFECT/E129.BIN.duplicate");
	strcpy(path[1063], "EFFECT/E130.BIN.duplicate");
	strcpy(path[1064], "EFFECT/E131.BIN.duplicate");
	strcpy(path[1065], "EFFECT/E132.BIN.duplicate");
	strcpy(path[1066], "EFFECT/E133.BIN.duplicate");
	strcpy(path[1068], "EFFECT/E135.BIN.duplicate");
	strcpy(path[1069], "EFFECT/E136.BIN.duplicate");
	strcpy(path[1070], "EFFECT/E137.BIN.duplicate");
	strcpy(path[1072], "EFFECT/E139.BIN.duplicate");
	strcpy(path[1073], "EFFECT/E140.BIN.duplicate");
	strcpy(path[1074], "EFFECT/E141.BIN.duplicate");
	strcpy(path[1075], "EFFECT/E142.BIN.duplicate");
	strcpy(path[1076], "EFFECT/E154.BIN.duplicate");
	strcpy(path[1077], "EFFECT/E156.BIN.duplicate");
	strcpy(path[1078], "EFFECT/E157.BIN.duplicate");
	strcpy(path[1079], "EFFECT/E158.BIN.duplicate");
	strcpy(path[1080], "EFFECT/E159.BIN.duplicate");
	strcpy(path[1081], "EFFECT/E160.BIN.duplicate");
	strcpy(path[1083], "EFFECT/E163.BIN.duplicate");
	strcpy(path[1084], "EFFECT/E164.BIN.duplicate");
	strcpy(path[1085], "EFFECT/E165.BIN.duplicate");
	strcpy(path[1086], "EFFECT/E166.BIN.duplicate");
	strcpy(path[1088], "EFFECT/E168.BIN.duplicate");
	strcpy(path[1089], "EFFECT/E169.BIN.duplicate");
	strcpy(path[1090], "EFFECT/E170.BIN.duplicate");
	strcpy(path[1091], "EFFECT/E171.BIN.duplicate");
	strcpy(path[1092], "EFFECT/E172.BIN.duplicate");
	strcpy(path[1093], "EFFECT/E173.BIN.duplicate");
	strcpy(path[1095], "EFFECT/E175.BIN.duplicate");
	strcpy(path[1097], "EFFECT/E177.BIN.duplicate");
	strcpy(path[1098], "EFFECT/E178.BIN.duplicate");
	strcpy(path[1099], "EFFECT/E179.BIN.duplicate");
	strcpy(path[1100], "EFFECT/E180.BIN.duplicate");
	strcpy(path[1101], "EFFECT/E181.BIN.duplicate");
	strcpy(path[1102], "EFFECT/E182.BIN.duplicate");
	strcpy(path[1103], "EFFECT/E183.BIN.duplicate");
	strcpy(path[1104], "EFFECT/E184.BIN.duplicate");
	strcpy(path[1105], "EFFECT/E185.BIN.duplicate");
	strcpy(path[1106], "EFFECT/E186.BIN.duplicate");
	strcpy(path[1107], "EFFECT/E187.BIN.duplicate");
	strcpy(path[1108], "EFFECT/E188.BIN.duplicate");
	strcpy(path[1109], "EFFECT/E189.BIN.duplicate");
	strcpy(path[1110], "EFFECT/E190.BIN.duplicate");
	strcpy(path[1111], "EFFECT/E191.BIN.duplicate");
	strcpy(path[1112], "EFFECT/E192.BIN.duplicate");
	strcpy(path[1114], "EFFECT/E194.BIN.duplicate");
	strcpy(path[1115], "EFFECT/E195.BIN.duplicate");
	strcpy(path[1116], "EFFECT/E196.BIN.duplicate");
	strcpy(path[1117], "EFFECT/E197.BIN.duplicate");
	strcpy(path[1118], "EFFECT/E198.BIN.duplicate");
	strcpy(path[1119], "EFFECT/E199.BIN.duplicate");
	strcpy(path[1120], "EFFECT/E200.BIN.duplicate");
	strcpy(path[1121], "EFFECT/E201.BIN.duplicate");
	strcpy(path[1122], "EFFECT/E202.BIN.duplicate");
	strcpy(path[1123], "EFFECT/E203.BIN.duplicate");
	strcpy(path[1124], "EFFECT/E204.BIN.duplicate");
	strcpy(path[1125], "EFFECT/E205.BIN.duplicate");
	strcpy(path[1126], "EFFECT/E206.BIN.duplicate");
	strcpy(path[1127], "EFFECT/E208.BIN.duplicate");
	strcpy(path[1128], "EFFECT/E210.BIN.duplicate");
	strcpy(path[1129], "EFFECT/E211.BIN.duplicate");
	strcpy(path[1130], "EFFECT/E212.BIN.duplicate");
	strcpy(path[1131], "EFFECT/E213.BIN.duplicate");
	strcpy(path[1132], "EFFECT/E214.BIN.duplicate");
	strcpy(path[1133], "EFFECT/E215.BIN.duplicate");
	strcpy(path[1134], "EFFECT/E216.BIN.duplicate");
	strcpy(path[1135], "EFFECT/E217.BIN.duplicate");
	strcpy(path[1136], "EFFECT/E218.BIN.duplicate");
	strcpy(path[1137], "EFFECT/E219.BIN.duplicate");
	strcpy(path[1138], "EFFECT/E220.BIN.duplicate");
	strcpy(path[1139], "EFFECT/E221.BIN.duplicate");
	strcpy(path[1141], "EFFECT/E223.BIN.duplicate");
	strcpy(path[1142], "EFFECT/E224.BIN.duplicate");
	strcpy(path[1143], "EFFECT/E225.BIN.duplicate");
	strcpy(path[1144], "EFFECT/E226.BIN.duplicate");
	strcpy(path[1148], "EFFECT/E231.BIN.duplicate");
	strcpy(path[1149], "EFFECT/E232.BIN.duplicate");
	strcpy(path[1151], "EFFECT/E234.BIN.duplicate");
	strcpy(path[1154], "EFFECT/E237.BIN.duplicate");
	strcpy(path[1155], "EFFECT/E238.BIN.duplicate");
	strcpy(path[1156], "EFFECT/E239.BIN.duplicate");
	strcpy(path[1160], "EFFECT/E243.BIN.duplicate");
	strcpy(path[1172], "EFFECT/E260.BIN.duplicate");
	strcpy(path[1173], "EFFECT/E261.BIN.duplicate");
	strcpy(path[1174], "EFFECT/E262.BIN.duplicate");
	strcpy(path[1175], "EFFECT/E263.BIN.duplicate");
	strcpy(path[1176], "EFFECT/E264.BIN.duplicate");
	strcpy(path[1177], "EFFECT/E265.BIN.duplicate");
	strcpy(path[1178], "EFFECT/E266.BIN.duplicate");
	strcpy(path[1179], "EFFECT/E267.BIN.duplicate");
	strcpy(path[1180], "EFFECT/E268.BIN.duplicate");
	strcpy(path[1181], "EFFECT/E269.BIN.duplicate");
	strcpy(path[1182], "EFFECT/E270.BIN.duplicate");
	strcpy(path[1183], "EFFECT/E271.BIN.duplicate");
	strcpy(path[1184], "EFFECT/E272.BIN.duplicate");
	strcpy(path[1185], "EFFECT/E273.BIN.duplicate");
	strcpy(path[1186], "EFFECT/E288.BIN.duplicate");
	strcpy(path[1187], "EFFECT/E289.BIN.duplicate");
	strcpy(path[1188], "EFFECT/E290.BIN.duplicate");
	strcpy(path[1189], "EFFECT/E291.BIN.duplicate");
	strcpy(path[1190], "EFFECT/E292.BIN.duplicate");
	strcpy(path[1191], "EFFECT/E293.BIN.duplicate");
	strcpy(path[1192], "EFFECT/E294.BIN.duplicate");
	strcpy(path[1193], "EFFECT/E295.BIN.duplicate");
	strcpy(path[1194], "EFFECT/E306.BIN.duplicate");
	strcpy(path[1195], "EFFECT/E307.BIN.duplicate");
	strcpy(path[1196], "EFFECT/E308.BIN.duplicate");
	strcpy(path[1197], "EFFECT/E309.BIN.duplicate");
	strcpy(path[1198], "EFFECT/E310.BIN.duplicate");
	strcpy(path[1199], "EFFECT/E311.BIN.duplicate");
	strcpy(path[1200], "EFFECT/E312.BIN.duplicate");
	strcpy(path[1201], "EFFECT/E313.BIN.duplicate");
	strcpy(path[1203], "EFFECT/E316.BIN.duplicate");
	strcpy(path[1205], "EFFECT/E318.BIN.duplicate");
	strcpy(path[1206], "EFFECT/E319.BIN.duplicate");
	strcpy(path[1207], "EFFECT/E320.BIN.duplicate");
	strcpy(path[1208], "EFFECT/E321.BIN.duplicate");
	strcpy(path[1209], "EFFECT/E322.BIN.duplicate");
	strcpy(path[1210], "EFFECT/E323.BIN.duplicate");
	strcpy(path[1211], "EFFECT/E324.BIN.duplicate");
	strcpy(path[1212], "EFFECT/E325.BIN.duplicate");
	strcpy(path[1213], "EFFECT/E326.BIN.duplicate");
	strcpy(path[1214], "EFFECT/E327.BIN.duplicate");
	strcpy(path[1215], "EFFECT/E328.BIN.duplicate");
	strcpy(path[1216], "EFFECT/E329.BIN.duplicate");
	strcpy(path[1217], "EFFECT/E330.BIN.duplicate");
	strcpy(path[1218], "EFFECT/E331.BIN.duplicate");
	strcpy(path[1219], "EFFECT/E332.BIN.duplicate");
	strcpy(path[1220], "EFFECT/E333.BIN.duplicate");
	strcpy(path[1221], "EFFECT/E334.BIN.duplicate");
	strcpy(path[1222], "EFFECT/E335.BIN.duplicate");
	strcpy(path[1224], "EFFECT/E337.BIN.duplicate");
	strcpy(path[1226], "EFFECT/E339.BIN.duplicate");
	strcpy(path[1227], "EFFECT/E340.BIN.duplicate");
	strcpy(path[1229], "EFFECT/E342.BIN.duplicate");
	strcpy(path[1230], "EFFECT/E343.BIN.duplicate");
	strcpy(path[1231], "EFFECT/E344.BIN.duplicate");
	strcpy(path[1232], "EFFECT/E345.BIN.duplicate");
	strcpy(path[1233], "EFFECT/E346.BIN.duplicate");
	strcpy(path[1234], "EFFECT/E347.BIN.duplicate");
	strcpy(path[1235], "EFFECT/E348.BIN.duplicate");
	strcpy(path[1236], "EFFECT/E349.BIN.duplicate");
	strcpy(path[1237], "EFFECT/E350.BIN.duplicate");
	strcpy(path[1238], "EFFECT/E351.BIN.duplicate");
	strcpy(path[1239], "EFFECT/E352.BIN.duplicate");
	strcpy(path[1240], "EFFECT/E353.BIN.duplicate");
	strcpy(path[1241], "EFFECT/E354.BIN.duplicate");
	strcpy(path[1243], "EFFECT/E356.BIN.duplicate");
	strcpy(path[1244], "EFFECT/E357.BIN.duplicate");
	strcpy(path[1245], "EFFECT/E358.BIN.duplicate");
	strcpy(path[1246], "EFFECT/E359.BIN.duplicate");
	strcpy(path[1247], "EFFECT/E360.BIN.duplicate");
	strcpy(path[1248], "EFFECT/E361.BIN.duplicate");
	strcpy(path[1249], "EFFECT/E362.BIN.duplicate");
	strcpy(path[1250], "EFFECT/E363.BIN.duplicate");
	strcpy(path[1251], "EFFECT/E364.BIN.duplicate");
	strcpy(path[1252], "EFFECT/E365.BIN.duplicate");
	strcpy(path[1253], "EFFECT/E366.BIN.duplicate");
	strcpy(path[1254], "EFFECT/E367.BIN.duplicate");
	strcpy(path[1255], "EFFECT/E368.BIN.duplicate");
	strcpy(path[1256], "EFFECT/E369.BIN.duplicate");
	strcpy(path[1257], "EFFECT/E370.BIN.duplicate");
	strcpy(path[1259], "EFFECT/E372.BIN.duplicate");
	strcpy(path[1261], "EFFECT/E374.BIN.duplicate");
	strcpy(path[1262], "EFFECT/E375.BIN.duplicate");
	strcpy(path[1264], "EFFECT/E377.BIN.duplicate");
	strcpy(path[1265], "EFFECT/E378.BIN.duplicate");
	strcpy(path[1266], "EFFECT/E379.BIN.duplicate");
	strcpy(path[1267], "EFFECT/E380.BIN.duplicate");
	strcpy(path[1273], "EFFECT/E386.BIN.duplicate");
	strcpy(path[1274], "EFFECT/E387.BIN.duplicate");
	strcpy(path[1275], "EFFECT/E388.BIN.duplicate");
	strcpy(path[1276], "EFFECT/E389.BIN.duplicate");
	strcpy(path[1277], "EFFECT/E390.BIN.duplicate");
	strcpy(path[1278], "EFFECT/E391.BIN.duplicate");
	strcpy(path[1279], "EFFECT/E392.BIN.duplicate");
	strcpy(path[1280], "EFFECT/E393.BIN.duplicate");
	strcpy(path[1281], "EFFECT/E394.BIN.duplicate");
	strcpy(path[1283], "EFFECT/E396.BIN.duplicate");
	strcpy(path[1284], "EFFECT/E397.BIN.duplicate");
	strcpy(path[1285], "EFFECT/E398.BIN.duplicate");
	strcpy(path[1286], "EFFECT/E399.BIN.duplicate");
	strcpy(path[1287], "EFFECT/E400.BIN.duplicate");
	strcpy(path[1291], "EFFECT/E404.BIN.duplicate");
	strcpy(path[1292], "EFFECT/E405.BIN.duplicate");
	strcpy(path[1293], "EFFECT/E406.BIN.duplicate");
	strcpy(path[1294], "EFFECT/E407.BIN.duplicate");
	strcpy(path[1297], "EFFECT/E410.BIN.duplicate");
	strcpy(path[1300], "EFFECT/E413.BIN.duplicate");
	strcpy(path[1308], "EFFECT/E457.BIN.duplicate");
	strcpy(path[1310], "EFFECT/E459.BIN.duplicate");
	strcpy(path[1321], "EFFECT/E470.BIN.duplicate");
	strcpy(path[1330], "EFFECT/E479.BIN.duplicate");
	strcpy(path[1335], "EFFECT/E484.BIN.duplicate");
	strcpy(path[1337], "EFFECT/E486.BIN.duplicate");
	strcpy(path[1338], "EFFECT/E509.BIN.duplicate");
	strcpy(path[1339], "EFFECT/E510.BIN.duplicate");
	strcpy(path[2355], "MAP/MAP011.22.duplicate");
	strcpy(path[2357], "MAP/MAP011.28.duplicate");
	strcpy(path[2364], "MAP/MAP011.53.duplicate");
	strcpy(path[940], "EFFECT/E001.BIN.duplicate");
	strcpy(path[941], "EFFECT/E002.BIN.duplicate");
	strcpy(path[942], "EFFECT/E003.BIN.duplicate");
	strcpy(path[947], "EFFECT/E008.BIN.duplicate");
	strcpy(path[948], "EFFECT/E009.BIN.duplicate");
	strcpy(path[949], "EFFECT/E010.BIN.duplicate");
	strcpy(path[950], "EFFECT/E011.BIN.duplicate");
	strcpy(path[951], "EFFECT/E012.BIN.duplicate");
	strcpy(path[952], "EFFECT/E013.BIN.duplicate");
	strcpy(path[953], "EFFECT/E014.BIN.duplicate");
	strcpy(path[955], "EFFECT/E016.BIN.duplicate");
	strcpy(path[956], "EFFECT/E017.BIN.duplicate");
	strcpy(path[957], "EFFECT/E018.BIN.duplicate");
	strcpy(path[958], "EFFECT/E019.BIN.duplicate");
	strcpy(path[959], "EFFECT/E020.BIN.duplicate");
	strcpy(path[960], "EFFECT/E021.BIN.duplicate");
	strcpy(path[961], "EFFECT/E022.BIN.duplicate");
	strcpy(path[962], "EFFECT/E023.BIN.duplicate");
	strcpy(path[963], "EFFECT/E024.BIN.duplicate");
	strcpy(path[964], "EFFECT/E025.BIN.duplicate");
	strcpy(path[965], "EFFECT/E026.BIN.duplicate");
	strcpy(path[966], "EFFECT/E027.BIN.duplicate");
	strcpy(path[967], "EFFECT/E028.BIN.duplicate");
	strcpy(path[968], "EFFECT/E029.BIN.duplicate");
	strcpy(path[969], "EFFECT/E030.BIN.duplicate");
	strcpy(path[970], "EFFECT/E031.BIN.duplicate");
	strcpy(path[971], "EFFECT/E032.BIN.duplicate");
	strcpy(path[973], "EFFECT/E034.BIN.duplicate");
	strcpy(path[975], "EFFECT/E036.BIN.duplicate");
	strcpy(path[976], "EFFECT/E039.BIN.duplicate");
	strcpy(path[977], "EFFECT/E040.BIN.duplicate");
	strcpy(path[979], "EFFECT/E043.BIN.duplicate");
	strcpy(path[980], "EFFECT/E044.BIN.duplicate");
	strcpy(path[981], "EFFECT/E045.BIN.duplicate");
	strcpy(path[982], "EFFECT/E046.BIN.duplicate");
	strcpy(path[984], "EFFECT/E049.BIN.duplicate");
	strcpy(path[985], "EFFECT/E050.BIN.duplicate");
	strcpy(path[986], "EFFECT/E051.BIN.duplicate");
	strcpy(path[987], "EFFECT/E052.BIN.duplicate");
	strcpy(path[988], "EFFECT/E053.BIN.duplicate");
	strcpy(path[989], "EFFECT/E054.BIN.duplicate");
	strcpy(path[990], "EFFECT/E055.BIN.duplicate");
	strcpy(path[991], "EFFECT/E056.BIN.duplicate");
	strcpy(path[992], "EFFECT/E057.BIN.duplicate");
	strcpy(path[993], "EFFECT/E058.BIN.duplicate");
	strcpy(path[994], "EFFECT/E059.BIN.duplicate");
	strcpy(path[995], "EFFECT/E060.BIN.duplicate");
	strcpy(path[996], "EFFECT/E061.BIN.duplicate");
	strcpy(path[997], "EFFECT/E062.BIN.duplicate");
	strcpy(path[998], "EFFECT/E063.BIN.duplicate");
}