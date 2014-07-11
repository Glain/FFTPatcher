using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FFTPatcher.TextEditor
{
    class ATCHELP : SectionedFile
    {
        public ATCHELP( 
            GenericCharMap map, 
            FFTPatcher.TextEditor.FFTTextFactory.FileInfo layout, 
            IList<byte> bytes, 
            string fileComments, 
            IList<string> sectionComments ) :
            base( map, layout, bytes, fileComments, sectionComments )
        {
        }

        public override PatcherLib.Datatypes.Set<KeyValuePair<string, byte>> GetPreferredDTEPairs( PatcherLib.Datatypes.Set<string> replacements, PatcherLib.Datatypes.Set<KeyValuePair<string, byte>> currentPairs, Stack<byte> dteBytes, System.ComponentModel.BackgroundWorker worker )
        {
            return base.GetPreferredDTEPairs( replacements, currentPairs, dteBytes, worker );
        }

        const int itemsIndex = 13;
        //public string GetLongestWeapon()
        //{
        //}

        //public string GetLongestShield()
        //{
        //}

        //public string GetLongestArmor()
        //{
        //}

        //public string GetLongestHelmet()
        //{
        //}

        //public string GetLongestAccessory()
        //{
        //}

    }
}
