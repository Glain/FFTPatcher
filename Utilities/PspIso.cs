using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher
{
    internal static class PspIso
    {
        public static void PatchISO( FFTPatch FFTPatch, BackgroundWorker worker, DoWorkEventArgs args, IGeneratePatchList patches )
        {
            PatchISO(FFTPatch, patches.FileName, worker, patches);
        }

        /// <summary>
        /// Patches the ISO.
        /// </summary>
        /// <param name="filename">The filename of the ISO to patch.</param>
        public static void PatchISO( FFTPatch FFTPatch, string filename, BackgroundWorker worker, IGeneratePatchList patches )
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream( filename, FileMode.Open );
                PatchISO(FFTPatch, stream, worker, patches);
            }
            catch ( NotSupportedException )
            {
                throw;
            }
            finally
            {
                if ( stream != null )
                {
                    stream.Flush();
                    stream.Close();
                    stream = null;
                }
            }
        }
        /// <summary>
        /// Patches the ISO.
        /// </summary>
        /// <param name="stream">The stream of the ISO to patch.</param>
        public static void PatchISO( FFTPatch FFTPatch, Stream stream, BackgroundWorker worker, IGeneratePatchList patchList )
        {
            PatcherLib.Iso.PspIso.PspIsoInfo info = PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo( stream );

            if ( PatcherLib.Iso.PspIso.IsJP( stream, info ) )
            {
                throw new NotSupportedException( "Unrecognized image." );
            }

            int numberOfTasks = patchList.PatchCount * 2;
            int tasksComplete = 0;

            List<PatchedByteArray> patches = new List<PatchedByteArray>();

            Action<string> sendProgress =
                delegate( string message )
                {
                    worker.ReportProgress( ++tasksComplete * 100 / numberOfTasks, message );
                };

            if ( patchList.RegenECC )
            {
                PatcherLib.Iso.PspIso.DecryptISO( stream, info );
                sendProgress( "Decrypting EBOOT.BIN" );
            }

            const Context context = Context.US_PSP;
            if ( patchList.Abilities )
            {
                patches.AddRange( FFTPatch.Abilities.GetPatches( context ) );
                sendProgress( "Getting Abilities patches" );
                sendProgress( "Getting Abilities patches" );
            }

            if (patchList.AbilityAnimations)
            {
                patches.AddRange(FFTPatch.AbilityAnimations.GetPatches(context));
                sendProgress("Getting Ability Animations patches");
                sendProgress("Getting Ability Animations patches");
            }

            if ( patchList.AbilityEffects )
            {
                patches.AddRange( FFTPatch.Abilities.AllEffects.GetPatches( context ) );
                sendProgress( "Getting Ability Effects patches" );
                sendProgress( "Getting Ability Effects patches" );
            }

            if ( patchList.ActionMenus )
            {
                patches.AddRange( FFTPatch.ActionMenus.GetPatches( context ) );
                sendProgress( "Getting Action Menus patches" );
                sendProgress( "Getting Action Menus patches" );
            }

            if ( patchList.ENTD.Exists( b => b == true ) )
            {
                IList<PatchedByteArray> entdPatches = FFTPatch.ENTDs.GetPatches( context );
                for ( int i = 0; i < 5; i++ )
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
                sendProgress( "Getting Inflict Status patches" );
            }
            if ( patchList.ItemAttributes )
            {
                patches.AddRange( FFTPatch.ItemAttributes.GetPatches( context ) );
                sendProgress( "Getting Item Attributes patches" );
                sendProgress( "Getting Item Attributes patches" );
                sendProgress( "Getting Item Attributes patches" );
                sendProgress( "Getting Item Attributes patches" );
            }
            if ( patchList.Items )
            {
                patches.AddRange( FFTPatch.Items.GetPatches( context ) );
                sendProgress( "Getting Item patches" );
                sendProgress( "Getting Item patches" );
                sendProgress( "Getting Item patches" );
                sendProgress( "Getting Item patches" );
            }
            if ( patchList.JobLevels )
            {
                patches.AddRange( FFTPatch.JobLevels.GetPatches( context ) );
                sendProgress( "Getting Job Levels patches" );
                sendProgress( "Getting Job Levels patches" );
            }
            if ( patchList.Jobs )
            {
                patches.AddRange( FFTPatch.Jobs.GetPatches( context ) );
                sendProgress( "Getting Jobs patches" );
                sendProgress( "Getting Jobs patches" );
            }
            if ( patchList.MonsterSkills )
            {
                patches.AddRange( FFTPatch.MonsterSkills.GetPatches( context ) );
                sendProgress( "Getting Monster Skills patches" );
                sendProgress( "Getting Monster Skills patches" );
            }
            if ( patchList.MoveFindItems )
            {
                patches.AddRange( FFTPatch.MoveFind.GetPatches( context ) );
                sendProgress( "Getting Move/Find Items patches" );
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
                sendProgress( "Getting Poach Probabilities patches" );
            }
            if ( patchList.Skillsets )
            {
                patches.AddRange( FFTPatch.SkillSets.GetPatches( context ) );
                sendProgress( "Getting Skillsets patches" );
                sendProgress( "Getting Skillsets patches" );
            }
            if ( patchList.StatusAttributes )
            {
                patches.AddRange( FFTPatch.StatusAttributes.GetPatches( context ) );
                sendProgress( "Getting Status attributes patches" );
                sendProgress( "Getting Status attributes patches" );
            }

            if (patchList.StoreInventory)
            {
                patches.AddRange( FFTPatch.StoreInventories.GetPatches( context ) );
                sendProgress( "Getting store inventories patches" );
                sendProgress( "Getting store inventories patches" );
            }
            if (patchList.Propositions)
            {
                patches.AddRange( FFTPatch.Propositions.GetPatches( context ) );
                sendProgress( "Getting errands patches" );
                sendProgress( "Getting errands patches" );
            }

            foreach ( PatchedByteArray patch in patches )
            {
                PatcherLib.Iso.PspIso.ApplyPatch( stream, info, patch );
                sendProgress( "Patching ISO" );
            }
        }

        public static void PatchISOSimple(FFTPatch FFTPatch, string filename)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(filename, FileMode.Open);
                PatchISOSimple(FFTPatch, stream);
            }
            catch (NotSupportedException)
            {
                throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Flush();
                    stream.Close();
                    stream = null;
                }
            }
        }

        public static void PatchISOSimple(FFTPatch FFTPatch, Stream stream)
        {
            PatcherLib.Iso.PspIso.PspIsoInfo info = PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo(stream);

            if (PatcherLib.Iso.PspIso.IsJP(stream, info))
            {
                throw new NotSupportedException("Unrecognized image.");
            }
            else
            {
                PatcherLib.Iso.PspIso.DecryptISO(stream, info);
            }

            List<PatchedByteArray> patches = GetPatches(FFTPatch);
            
            foreach (PatchedByteArray patch in patches)
            {
                PatcherLib.Iso.PspIso.ApplyPatch(stream, info, patch);
            }
        }

        public static string CreatePatchXML(FFTPatch FFTPatch)
        {
            return PatcherLib.Utilities.Utilities.CreatePatchXML(GetPatches(FFTPatch), Context.US_PSP, true, true, "FFTPatcher Edits", null);
        }

        public static List<PatchedByteArray> GetPatches(FFTPatch FFTPatch)
        {
            List<PatchedByteArray> patches = new List<PatchedByteArray>();
            const Context context = Context.US_PSP;

            patches.AddRange(FFTPatch.Abilities.GetPatches(context));
            patches.AddRange(FFTPatch.AbilityAnimations.GetPatches(context));
            patches.AddRange(FFTPatch.Abilities.AllEffects.GetPatches(context));
            patches.AddRange(FFTPatch.ActionMenus.GetPatches(context));

            IList<PatchedByteArray> entdPatches = FFTPatch.ENTDs.GetPatches(context);
            for (int i = 0; i < 5; i++)
            {
                patches.Add(entdPatches[i]);
            }

            patches.AddRange(FFTPatch.InflictStatuses.GetPatches(context));
            patches.AddRange(FFTPatch.ItemAttributes.GetPatches(context));
            patches.AddRange(FFTPatch.Items.GetPatches(context));
            patches.AddRange(FFTPatch.JobLevels.GetPatches(context));
            patches.AddRange(FFTPatch.Jobs.GetPatches(context));
            patches.AddRange(FFTPatch.MonsterSkills.GetPatches(context));
            patches.AddRange(FFTPatch.MoveFind.GetPatches(context));
            patches.AddRange(FFTPatch.PoachProbabilities.GetPatches(context));
            patches.AddRange(FFTPatch.SkillSets.GetPatches(context));
            patches.AddRange(FFTPatch.StatusAttributes.GetPatches(context));
            patches.AddRange(FFTPatch.StoreInventories.GetPatches(context));
            patches.AddRange(FFTPatch.Propositions.GetPatches(context));

            return patches;
        }
    }
}
