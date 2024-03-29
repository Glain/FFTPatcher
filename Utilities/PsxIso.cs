﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using PatcherLib.Utilities;

namespace FFTPatcher
{
    internal static class PsxIso
    {
        public static void PatchPsxIso( FFTPatch FFTPatch, BackgroundWorker backgroundWorker, DoWorkEventArgs e, IGeneratePatchList patchList )
        {
            string filename = patchList.FileName;
            int numberOfTasks = patchList.PatchCount * 2;
            int tasksComplete = 0;
            List<PatchedByteArray> patches = new List<PatchedByteArray>();

            Action<string> sendProgress =
                delegate( string message )
                {
                    backgroundWorker.ReportProgress( tasksComplete++ * 100 / numberOfTasks, message );
                };

            const Context context = Context.US_PSX;

            if ( patchList.Abilities )
            {
                patches.AddRange( FFTPatch.Abilities.GetPatches( context ) );
                sendProgress( "Getting Abilities patches" );
            }

            if (patchList.AbilityAnimations)
            {
                patches.AddRange(FFTPatch.AbilityAnimations.GetPatches(context));
                sendProgress("Getting Ability Animations patches");
            }
            if ( patchList.AbilityEffects )
            {
                patches.AddRange( FFTPatch.Abilities.AllEffects.GetPatches( context ) );
                sendProgress( "Getting Ability Effects patches" );
            }

            if ( patchList.ActionMenus )
            {
                patches.AddRange( FFTPatch.ActionMenus.GetPatches( context ) );
                sendProgress( "Getting Action Menus patches" );
            }

            if ( patchList.ENTD.Exists( b => b == true ) )
            {
                IList<PatchedByteArray> entdPatches = FFTPatch.ENTDs.GetPatches( context );
                for ( int i = 0; i < 4; i++ )
                {
                    if ( patchList.ENTD[i] )
                    {
                        patches.Add( entdPatches[i] );
                        sendProgress( string.Format( "Getting ENTD {0} patches", i ) );
                    }
                }
            }

            if ( patchList.InflictStatus )
            {
                patches.AddRange( FFTPatch.InflictStatuses.GetPatches( context ) );
                sendProgress( "Getting Inflict Status patches" );
            }
            if ( patchList.ItemAttributes )
            {
                patches.AddRange( FFTPatch.ItemAttributes.GetPatches( context ) );
                sendProgress( "Getting Item Attributes patches" );
            }
            if ( patchList.Items )
            {
                patches.AddRange( FFTPatch.Items.GetPatches( context ) );
                sendProgress( "Getting Item patches" );
            }
            if ( patchList.JobLevels )
            {
                patches.AddRange( FFTPatch.JobLevels.GetPatches( context ) );
                sendProgress( "Getting Job Levels patches" );
            }
            if ( patchList.Jobs )
            {
                patches.AddRange( FFTPatch.Jobs.GetPatches( context ) );
                sendProgress( "Getting Jobs patches" );
            }
            if ( patchList.MonsterSkills )
            {
                patches.AddRange( FFTPatch.MonsterSkills.GetPatches( context ) );
                sendProgress( "Getting Monster Skills patches" );
            }
            if ( patchList.MoveFindItems )
            {
                patches.AddRange( FFTPatch.MoveFind.GetPatches( context ) );
                sendProgress( "Getting Move/Find Items patches" );
            }
            foreach ( PatchedByteArray patch in patchList.OtherPatches )
            {
                patches.Add( patch );
                sendProgress( "Getting other patches" );
            }
            if ( patchList.Poach )
            {
                patches.AddRange( FFTPatch.PoachProbabilities.GetPatches( context ) );
                sendProgress( "Getting Poach Probabilities patches" );
            }
            if ( patchList.Skillsets )
            {
                patches.AddRange( FFTPatch.SkillSets.GetPatches( context ) );
                sendProgress( "Getting Skillsets patches" );
            }
            if ( patchList.StatusAttributes )
            {
                patches.AddRange( FFTPatch.StatusAttributes.GetPatches( context ) );
                sendProgress( "Getting Status attributes patches" );
            }

            if (patchList.StoreInventory)
            {
                patches.AddRange( FFTPatch.StoreInventories.GetPatches( context ) );
                sendProgress( "Getting store inventories patches" );
            }
            if (patchList.Propositions)
            {
                patches.AddRange( FFTPatch.Propositions.GetPatches( context ) );
                sendProgress( "Getting errands patches" );
            }

            using ( FileStream stream = new FileStream( filename, FileMode.Open, FileAccess.ReadWrite ) )
            {
                foreach ( PatchedByteArray patch in patches )
                {
                    IsoPatch.PatchFileAtSector( IsoPatch.IsoType.Mode2Form1, stream, true, patch.Sector,
                        patch.Offset, patch.GetBytes(), true, patch.MaskWrite);
                    sendProgress( "Patching ISO" );
                }

                //if ( patchList.RegenECC )
                //{
                //    IsoPatch.FixupECC( IsoPatch.IsoType.Mode2Form1, stream );
                //    sendProgress( "Fixing ECC/EDC" );
                //}
            }

        }

        public static void PatchPsxIsoSimple(FFTPatch fftPatch, string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                List<PatchedByteArray> patches = GetPatches(fftPatch);
                foreach (PatchedByteArray patch in patches)
                {
                    IsoPatch.PatchFileAtSector(IsoPatch.IsoType.Mode2Form1, stream, true, 
                        patch.Sector, patch.Offset, patch.GetBytes(), true, patch.MaskWrite);
                }
            }
        }

        public static List<PatchedByteArray> GetPatches(FFTPatch fftPatch)
        {
            List<PatchedByteArray> patches = new List<PatchedByteArray>();
            const Context context = Context.US_PSX;

            patches.AddRange(fftPatch.Abilities.GetPatches(context));
            patches.AddRange(fftPatch.AbilityAnimations.GetPatches(context));
            patches.AddRange(fftPatch.Abilities.AllEffects.GetPatches(context));
            patches.AddRange(fftPatch.ActionMenus.GetPatches(context));

            IList<PatchedByteArray> entdPatches = fftPatch.ENTDs.GetPatches(context);
            for (int i = 0; i < 4; i++)
            {
                patches.Add(entdPatches[i]);
            }

            patches.AddRange(fftPatch.InflictStatuses.GetPatches(context));
            patches.AddRange(fftPatch.ItemAttributes.GetPatches(context));
            patches.AddRange(fftPatch.Items.GetPatches(context));
            patches.AddRange(fftPatch.JobLevels.GetPatches(context));
            patches.AddRange(fftPatch.Jobs.GetPatches(context));
            patches.AddRange(fftPatch.MonsterSkills.GetPatches(context));
            patches.AddRange(fftPatch.MoveFind.GetPatches(context));
            patches.AddRange(fftPatch.PoachProbabilities.GetPatches(context));
            patches.AddRange(fftPatch.SkillSets.GetPatches(context));
            patches.AddRange(fftPatch.StatusAttributes.GetPatches(context));
            patches.AddRange(fftPatch.StoreInventories.GetPatches(context));
            patches.AddRange(fftPatch.Propositions.GetPatches(context));

            return patches;
        }

        public static void PatchPsxSavestate(FFTPatch FFTPatch, BackgroundWorker backgroundWorker, DoWorkEventArgs e, IGeneratePatchList patchList)
        {
            
            string filename = patchList.FileName;
            int numberOfTasks = patchList.PatchCount * 2;
            int tasksComplete = 0;
            List<PatchedByteArray> patches = new List<PatchedByteArray>();

            Action<string> sendProgress =
                delegate(string message)
                {
                    backgroundWorker.ReportProgress(tasksComplete++ * 100 / numberOfTasks, message);
                };

            const Context context = Context.US_PSX;

            if (patchList.Abilities)
            {
                patches.AddRange(FFTPatch.Abilities.GetPatches(context));
                sendProgress("Getting Abilities patches");
            }

            if (patchList.AbilityAnimations)
            {
                patches.AddRange(FFTPatch.AbilityAnimations.GetPatches(context));
                sendProgress("Getting Ability Animations patches");
            }
            if (patchList.AbilityEffects)
            {
                patches.AddRange(FFTPatch.Abilities.AllEffects.GetPatches(context));
                sendProgress("Getting Ability Effects patches");
            }

            if (patchList.ActionMenus)
            {
                patches.AddRange(FFTPatch.ActionMenus.GetPatches(context));
                sendProgress("Getting Action Menus patches");
            }

            if (patchList.ENTD.Exists(b => b == true))
            {
                IList<PatchedByteArray> entdPatches = FFTPatch.ENTDs.GetPatches(context);
                for (int i = 0; i < 4; i++)
                {
                    if (patchList.ENTD[i])
                    {
                        patches.Add(entdPatches[i]);
                        sendProgress(string.Format("Getting ENTD {0} patches", i));
                    }
                }
            }

            if (patchList.InflictStatus)
            {
                patches.AddRange(FFTPatch.InflictStatuses.GetPatches(context));
                sendProgress("Getting Inflict Status patches");
            }
            if (patchList.ItemAttributes)
            {
                patches.AddRange(FFTPatch.ItemAttributes.GetPatches(context));
                sendProgress("Getting Item Attributes patches");
            }
            if (patchList.Items)
            {
                patches.AddRange(FFTPatch.Items.GetPatches(context));
                sendProgress("Getting Item patches");
            }
            if (patchList.JobLevels)
            {
                patches.AddRange(FFTPatch.JobLevels.GetPatches(context));
                sendProgress("Getting Job Levels patches");
            }
            if (patchList.Jobs)
            {
                patches.AddRange(FFTPatch.Jobs.GetPatches(context));
                sendProgress("Getting Jobs patches");
            }
            if (patchList.MonsterSkills)
            {
                patches.AddRange(FFTPatch.MonsterSkills.GetPatches(context));
                sendProgress("Getting Monster Skills patches");
            }
            if (patchList.MoveFindItems)
            {
                patches.AddRange(FFTPatch.MoveFind.GetPatches(context));
                sendProgress("Getting Move/Find Items patches");
            }
            foreach (PatchedByteArray patch in patchList.OtherPatches)
            {
                patches.Add(patch);
                sendProgress("Getting other patches");
            }
            if (patchList.Poach)
            {
                patches.AddRange(FFTPatch.PoachProbabilities.GetPatches(context));
                sendProgress("Getting Poach Probabilities patches");
            }
            if (patchList.Skillsets)
            {
                patches.AddRange(FFTPatch.SkillSets.GetPatches(context));
                sendProgress("Getting Skillsets patches");
            }
            if (patchList.StatusAttributes)
            {
                patches.AddRange(FFTPatch.StatusAttributes.GetPatches(context));
                sendProgress("Getting Status attributes patches");
            }

            if (patchList.StoreInventory)
            {
                patches.AddRange(FFTPatch.StoreInventories.GetPatches(context));
                sendProgress("Getting store inventories patches");
            }
            if (patchList.Propositions)
            {
                patches.AddRange(FFTPatch.Propositions.GetPatches(context));
                sendProgress("Getting errands patches");
            }

            //byte[] filecopy = File.ReadAllBytes(patchList.FileName);

            using (BinaryReader reader = new BinaryReader(File.Open(patchList.FileName, FileMode.Open)))
            {
                PatcherLib.Iso.PsxIso.PatchPsxSaveState(reader, patches);     
            }
            
        }

        public static void PatchPsxSavestateSimple(FFTPatch fftPatch, string filename)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                PatcherLib.Iso.PsxIso.PatchPsxSaveState(reader, GetPatches(fftPatch));
            }
        }

        public static string CreatePatchXML(FFTPatch fftPatch)
        {
            return PatcherLib.Utilities.Utilities.CreatePatchXML(GetPatches(fftPatch), Context.US_PSX, true, true, "FFTPatcher Edits", null);
        }
    }
}
