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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using PatcherLib;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    public static class FFTPatch
    {
		#region Instance Variables (2) 

        private static IDictionary<ElementName, string> elementNames = PatcherLib.Utilities.Utilities.BuildDictionary<ElementName, string>( new object[] {
            ElementName.Propositions, "propositions",
            ElementName.Abilities, "abilities",
            ElementName.AbilityAnimations, "abilityAnimations",
            ElementName.AbilityEffects, "abilityEffects", 
            ElementName.ItemAbilityEffects, "itemAbilityEffects",
            ElementName.ReactionAbilityEffects, "reactionAbilityEffects",
            ElementName.Items, "items", 
            ElementName.ItemAttributes, "itemAttributes", 
            ElementName.PSPItems, "pspItems", 
            ElementName.PSPItemAttributes, "pspItemAttributes", 
            ElementName.Jobs, "jobs", 
            ElementName.JobLevels, "jobLevels",
            ElementName.SkillSets, "skillSets", 
            ElementName.MonsterSkills, "monsterSkills", 
            ElementName.ActionMenus, "actionMenus", 
            ElementName.InflictStatuses, "inflictStatuses", 
            ElementName.StatusAttributes, "statusAttributes", 
            ElementName.Poaching, "poaching",
            ElementName.ENTD1, "entd1", 
            ElementName.ENTD2, "entd2", 
            ElementName.ENTD3, "entd3", 
            ElementName.ENTD4, "entd4", 
            ElementName.ENTD5, "entd5", 
            ElementName.MoveFindItems, "moveFindItems",
            ElementName.StoreInventories, "storeInventories" } );

		#endregion Instance Variables 

		#region Public Properties (15) 

        public static AllAbilities Abilities { get; private set; }

        public static AllAnimations AbilityAnimations { get; private set; }

        public static AllActionMenus ActionMenus { get; private set; }

        public static Context Context { get; private set; }

        public static AllENTDs ENTDs { get; private set; }

        public static AllInflictStatuses InflictStatuses { get; private set; }

        public static AllItemAttributes ItemAttributes { get; private set; }

        public static AllItems Items { get; private set; }

        public static JobLevels JobLevels { get; private set; }

        public static AllJobs Jobs { get; private set; }

        public static AllMonsterSkills MonsterSkills { get; private set; }

        public static AllMoveFindItems MoveFind { get; private set; }

        public static AllPoachProbabilities PoachProbabilities { get; private set; }

        public static AllSkillSets SkillSets { get; private set; }

        public static AllStatusAttributes StatusAttributes { get; private set; }

        public static AllStoreInventories StoreInventories { get; private set; }

        public static AllPropositions Propositions { get; private set; }

		#endregion Public Properties 

		#region Public Methods (7) 

        public static void ConvertPsxPatchToPsp( string filename )
        {
            Dictionary<string, byte[]> fileList = new Dictionary<string, byte[]>();
            using( ZipFile zipFile = new ZipFile( filename ) )
            {
                foreach( ZipEntry entry in zipFile )
                {
                    byte[] bytes = new byte[entry.Size];
                    StreamUtils.ReadFully( zipFile.GetInputStream( entry ), bytes );
                    fileList[entry.Name] = bytes;
                }
            }

            File.Delete( filename );

            if( fileList["type"].ToUTF8String() == Context.US_PSX.ToString() )
            {
                List<byte> amBytes = new List<byte>( fileList["actionMenus"] );
                amBytes.AddRange( PSPResources.Binaries.ActionEvents.Sub( 0xE0, 0xE2 ) );
                fileList["actionMenus"] = amBytes.ToArray();

                AllJobs aj = new AllJobs( Context.US_PSX, fileList["jobs"] );
                List<Job> jobs = new List<Job>( aj.Jobs );
                AllJobs defaultPspJobs = new AllJobs( Context.US_PSP, PSPResources.Binaries.Jobs );
                for( int i = 0; i < jobs.Count; i++ )
                {
                    jobs[i].Equipment.Unknown1 = defaultPspJobs.Jobs[i].Equipment.Unknown1;
                    jobs[i].Equipment.Unknown2 = defaultPspJobs.Jobs[i].Equipment.Unknown2;
                    jobs[i].Equipment.Unknown3 = defaultPspJobs.Jobs[i].Equipment.Unknown3;
                    jobs[i].Equipment.FellSword = defaultPspJobs.Jobs[i].Equipment.FellSword;
                    jobs[i].Equipment.LipRouge = defaultPspJobs.Jobs[i].Equipment.LipRouge;
                    jobs[i].Equipment.Unknown6 = defaultPspJobs.Jobs[i].Equipment.Unknown6;
                    jobs[i].Equipment.Unknown7 = defaultPspJobs.Jobs[i].Equipment.Unknown7;
                    jobs[i].Equipment.Unknown8 = defaultPspJobs.Jobs[i].Equipment.Unknown8;
                }
                for( int i = 160; i < 169; i++ )
                {
                    jobs.Add( defaultPspJobs.Jobs[i] );
                }
                ReflectionHelpers.SetFieldOrProperty( aj, "Jobs", jobs.ToArray() );
                fileList["jobs"] = aj.ToByteArray( Context.US_PSP );

                JobLevels jl = new JobLevels( Context.US_PSX, fileList["jobLevels"] );
                JobLevels pspJobLevels = new JobLevels( Context.US_PSP, PSPResources.Binaries.JobLevels );
                foreach( string jobName in new string[19] { "Archer", "Arithmetician", "Bard", "BlackMage", "Chemist", "Dancer", "Dragoon", "Geomancer",
                            "Knight", "Mime", "Monk", "Mystic", "Ninja", "Orator", "Samurai", "Summoner", "Thief", "TimeMage", "WhiteMage" } )
                {
                    Requirements psxR = ReflectionHelpers.GetFieldOrProperty<Requirements>( jl, jobName );
                    Requirements pspR = ReflectionHelpers.GetFieldOrProperty<Requirements>( pspJobLevels, jobName );
                    psxR.Unknown1 = pspR.Unknown1;
                    psxR.Unknown2 = pspR.Unknown2;
                    psxR.DarkKnight = pspR.DarkKnight;
                    psxR.OnionKnight = pspR.OnionKnight;
                }
                ReflectionHelpers.SetFieldOrProperty( jl, "OnionKnight", pspJobLevels.OnionKnight );
                ReflectionHelpers.SetFieldOrProperty( jl, "DarkKnight", pspJobLevels.DarkKnight );
                ReflectionHelpers.SetFieldOrProperty( jl, "Unknown", pspJobLevels.Unknown );
                fileList["jobLevels"] = jl.ToByteArray( Context.US_PSP );

                List<byte> ssBytes = new List<byte>( fileList["skillSets"] );
                ssBytes.AddRange( PSPResources.Binaries.SkillSets.Sub( ssBytes.Count ) );
                fileList["skillSets"] = ssBytes.ToArray();

                fileList["entd5"] = PSPResources.Binaries.ENTD5.ToArray();



                fileList["type"] = Encoding.UTF8.GetBytes( Context.US_PSP.ToString() );

                fileList["pspItemAttributes"] = PSPResources.Binaries.NewItemAttributes.ToArray();
                fileList["pspItems"] = PSPResources.Binaries.NewItems.ToArray();

                if (!AllPropositions.CanFixBuggyLevelBonuses( Context.US_PSP ))
                {
                    fileList["BuggyPropositions"] = new byte[0];
                }
                else if (fileList.ContainsKey( "BuggyPropositions" ))
                {
                    fileList.Remove( "BuggyPropositions" );
                }
            }

            using( FileStream outFile = new FileStream( filename, FileMode.Create, FileAccess.ReadWrite ) )
            using( ZipOutputStream output = new ZipOutputStream( outFile ) )
            {
                foreach( KeyValuePair<string, byte[]> entry in fileList )
                {
                    WriteFileToZip( output, entry.Key, entry.Value );
                }
            }
        }

        public static void GenerateDigest( string filename )
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            StringBuilder sb = new StringBuilder();

            using( XmlWriter writer = XmlWriter.Create( sb, settings ) )
            {
                writer.WriteStartElement( "digest" );
                IXmlDigest[] digestable = new IXmlDigest[] {
                    Abilities, Items, ItemAttributes, Jobs, JobLevels, SkillSets, MonsterSkills, ActionMenus, StatusAttributes,
                    InflictStatuses, PoachProbabilities, ENTDs, MoveFind };
                foreach( IXmlDigest digest in digestable )
                {
                    digest.WriteXmlDigest( writer );
                }
                writer.WriteEndElement();
            }


#if DEBUG
            using( FileStream stream = new FileStream( filename + ".xml", FileMode.Create ) )
            {
                byte[] bytes = sb.ToString().ToByteArray();
                stream.Write( bytes, 0, bytes.Length );
            }
#endif

            settings.ConformanceLevel = ConformanceLevel.Fragment;
            using( MemoryStream memoryStream = new MemoryStream( ResourcesClass.ZipFileContents[ResourcesClass.Paths.DigestTransform].ToArray() ) )
            using( XmlReader transformXmlReader = XmlReader.Create( memoryStream ) )
            using( StringReader inputReader = new StringReader( sb.ToString() ) )
            using( XmlReader inputXmlReader = XmlReader.Create( inputReader ) )
            using( XmlWriter outputWriter = XmlWriter.Create( filename, settings ) )
            {
                System.Xml.Xsl.XslCompiledTransform t = new System.Xml.Xsl.XslCompiledTransform();
                t.Load( transformXmlReader );
                t.Transform( inputXmlReader, outputWriter );
            }
        }

        /// <summary>
        /// Reads an XML fftpatch file.
        /// </summary>
        public static void LoadPatch( string filename )
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load( filename );
                LoadOldStylePatch( doc );
            }
            catch ( XmlException )
            {
                // Is new format file
                LoadNewStylePatch( filename );
            }
            FireDataChangedEvent();
        }

        /// <summary>
        /// Builds a new (unmodified) patch from a context.
        /// </summary>
        public static void New( Context context )
        {
            Context = context;
            BuildFromContext();
            FireDataChangedEvent();
        }

        public static void OpenPatchedPsxIso(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                Context = Context.US_PSX;
                LoadDataFromBytes(
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.Abilities),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.AbilityEffects),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.ItemAbilityEffects),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.ReactionAbilityEffects),
                    PatcherLib.Iso.PsxIso.GetBlock( stream, PatcherLib.Iso.PsxIso.AbilityAnimations ),
                    PatcherLib.Iso.PsxIso.GetBlock( stream, PatcherLib.Iso.PsxIso.OldItems ),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.OldItemAttributes),
                    null,
                    null,
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.Jobs),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.JobLevels),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.SkillSets),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.MonsterSkills),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.ActionEvents),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.StatusAttributes),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.InflictStatuses),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.PoachProbabilities),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.ENTD1),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.ENTD2),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.ENTD3),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.ENTD4),
                    null,
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.MoveFindItems),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.StoreInventories),
                    PatcherLib.Iso.PsxIso.GetBlock(stream, PatcherLib.Iso.PsxIso.Propositions),
                    AllPropositions.IsoHasBuggyLevelBonuses(stream, Context.US_PSX)
                    );
                FireDataChangedEvent();
            }
        }

        public static void OpenPatchedPspIso(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                Context = Context.US_PSP;
                PatcherLib.Iso.PspIso.PspIsoInfo info = PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo(stream);
                LoadDataFromBytes(
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.Abilities[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.AbilityEffects[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.ItemAbilityEffects[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.ReactionAbilityEffects[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.AbilityAnimations[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.OldItems[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.OldItemAttributes[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.NewItems[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.NewItemAttributes[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.Jobs[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.JobLevels[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.SkillSets[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.MonsterSkills[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.ActionEvents[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.StatusAttributes[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.InflictStatuses[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.PoachProbabilities[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.ENTD1 ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.ENTD2 ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.ENTD3 ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.ENTD4 ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.ENTD5 ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.MoveFindItems[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.StoreInventories[0] ),
                    PatcherLib.Iso.PspIso.GetBlock( stream, info, PatcherLib.Iso.PspIso.Propositions[0] ),
                    AllPropositions.IsoHasBuggyLevelBonuses(stream, Context.US_PSP)) ;
                FireDataChangedEvent();
            }
        }

        /// <summary>
        /// Saves this patch to an XML document.
        /// </summary>
        public static void SavePatchToFile( string path )
        {
            SavePatchToFile( path, FFTPatch.Context, true );
        }

        /// <summary>
        /// Saves this patch to an XML document.
        /// </summary>
        public static void SavePatchToFile( string path, Context destinationContext, bool saveDigest )
        {
            SaveZippedPatch( path, destinationContext );
            if ( saveDigest )
            {
                GenerateDigest( Path.Combine( Path.GetDirectoryName( path ), Path.GetFileNameWithoutExtension( path ) + ".digest.html" ) );
            }
        }

		#endregion Public Methods 

		#region Private Methods (11) 

        private static void BuildFromContext()
        {
            switch( Context )
            {
                case Context.US_PSP:
                    Abilities = new AllAbilities( PSPResources.Binaries.Abilities, PSPResources.Binaries.AbilityEffects, PSPResources.Binaries.ItemAbilityEffects, PSPResources.Binaries.ReactionAbilityEffects );
                    AbilityAnimations = new AllAnimations( Context, PSPResources.Binaries.AbilityAnimations, PSPResources.Binaries.AbilityAnimations );
                    Items = new AllItems(
                        PSPResources.Binaries.OldItems,
                        PSPResources.Binaries.NewItems );
                    ItemAttributes = new AllItemAttributes(
                        PSPResources.Binaries.OldItemAttributes,
                        PSPResources.Binaries.NewItemAttributes );
                    Jobs = new AllJobs( Context, PSPResources.Binaries.Jobs );
                    JobLevels = new JobLevels( Context, PSPResources.Binaries.JobLevels,
                        new JobLevels( Context, PSPResources.Binaries.JobLevels ) );
                    SkillSets = new AllSkillSets( Context, PSPResources.Binaries.SkillSets,
                        PSPResources.Binaries.SkillSets );
                    MonsterSkills = new AllMonsterSkills( PSPResources.Binaries.MonsterSkills );
                    ActionMenus = new AllActionMenus( PSPResources.Binaries.ActionEvents, Context );
                    StatusAttributes = new AllStatusAttributes( PSPResources.Binaries.StatusAttributes );
                    InflictStatuses = new AllInflictStatuses( PSPResources.Binaries.InflictStatuses );
                    PoachProbabilities = new AllPoachProbabilities( PSPResources.Binaries.PoachProbabilities );
                    ENTDs = new AllENTDs( PSPResources.Binaries.ENTD1, PSPResources.Binaries.ENTD2, PSPResources.Binaries.ENTD3, PSPResources.Binaries.ENTD4, PSPResources.Binaries.ENTD5 );
                    MoveFind = new AllMoveFindItems( Context, PSPResources.Binaries.MoveFind, new AllMoveFindItems( Context, PSPResources.Binaries.MoveFind ) );
                    StoreInventories = new AllStoreInventories( Context, PSPResources.Binaries.StoreInventories, PSPResources.Binaries.StoreInventories );
                    Propositions = new AllPropositions( PSPResources.Binaries.Propositions, PSPResources.Binaries.Propositions, false );
                    break;
                case Context.US_PSX:
                    Abilities = new AllAbilities( PSXResources.Binaries.Abilities, PSXResources.Binaries.AbilityEffects, PSXResources.Binaries.ItemAbilityEffects, PSXResources.Binaries.ReactionAbilityEffects );
                    AbilityAnimations = new AllAnimations( Context, PSXResources.Binaries.AbilityAnimations, PSXResources.Binaries.AbilityAnimations );
                    Items = new AllItems( PSXResources.Binaries.OldItems, null );
                    ItemAttributes = new AllItemAttributes( PSXResources.Binaries.OldItemAttributes, null );
                    Jobs = new AllJobs( Context, PSXResources.Binaries.Jobs );
                    JobLevels = new JobLevels( Context, PSXResources.Binaries.JobLevels,
                        new JobLevels( Context, PSXResources.Binaries.JobLevels ) );
                    SkillSets = new AllSkillSets( Context, PSXResources.Binaries.SkillSets,
                        PSXResources.Binaries.SkillSets );
                    MonsterSkills = new AllMonsterSkills( PSXResources.Binaries.MonsterSkills );
                    ActionMenus = new AllActionMenus( PSXResources.Binaries.ActionEvents, Context );
                    StatusAttributes = new AllStatusAttributes( PSXResources.Binaries.StatusAttributes );
                    InflictStatuses = new AllInflictStatuses( PSXResources.Binaries.InflictStatuses );
                    PoachProbabilities = new AllPoachProbabilities( PSXResources.Binaries.PoachProbabilities );
                    ENTDs = new AllENTDs( PSXResources.Binaries.ENTD1, PSXResources.Binaries.ENTD2, PSXResources.Binaries.ENTD3, PSXResources.Binaries.ENTD4 );
                    MoveFind = new AllMoveFindItems( Context, PSXResources.Binaries.MoveFind, new AllMoveFindItems( Context, PSXResources.Binaries.MoveFind ) );
                    StoreInventories = new AllStoreInventories( Context, PSXResources.Binaries.StoreInventories, PSXResources.Binaries.StoreInventories );
                    Propositions = new AllPropositions( PSXResources.Binaries.Propositions, PSXResources.Binaries.Propositions, false );
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private static void FireDataChangedEvent()
        {
            if( DataChanged != null )
            {
                DataChanged( null, EventArgs.Empty );
            }
        }

        private static StringBuilder GetBase64StringIfNonDefault( byte[] bytes, byte[] def )
        {
            if ( !PatcherLib.Utilities.Utilities.CompareArrays( bytes, def ) )
            {
                return new StringBuilder( Convert.ToBase64String( bytes, Base64FormattingOptions.InsertLineBreaks ) );
            }
            return null;
        }

        private static IList<byte> GetFromNodeOrReturnDefault( XmlNode node, string name, IList<byte> def )
        {
            XmlNode n = node.SelectSingleNode( name );
            if( n != null )
            {
                try
                {
                    byte[] result = Convert.FromBase64String( n.InnerText );
                    return result;
                }
                catch( Exception )
                {
                }
            }

            return def;
        }

        private static byte[] GetZipEntry( ZipFile file, string entry, bool throwOnError )
        {
            if ( file.FindEntry( entry, false ) == -1 )
            {
                if ( throwOnError )
                {
                    throw new FormatException( "entry not found" );
                }
                else
                {
                    return null;
                }
            }
            else
            {
                ZipEntry zEntry = file.GetEntry( entry );
                Stream s = file.GetInputStream( zEntry );
                byte[] result = new byte[zEntry.Size];
                StreamUtils.ReadFully( s, result );
                return result;
            }
        }

        private static void LoadDataFromBytes(
            IList<byte> abilities, IList<byte> abilityEffects, IList<byte> itemAbilityEffects, IList<byte> reactionEffects, IList<byte> abilityAnimations,
            IList<byte> oldItems, IList<byte> oldItemAttributes,
            IList<byte> newItems, IList<byte> newItemAttributes,
            IList<byte> jobs, IList<byte> jobLevels,
            IList<byte> skillSets, IList<byte> monsterSkills,
            IList<byte> actionMenus,
            IList<byte> statusAttributes, IList<byte> inflictStatuses,
            IList<byte> poach,
            IList<byte> entd1, IList<byte> entd2, IList<byte> entd3, IList<byte> entd4, IList<byte> entd5,
            IList<byte> moveFind,
            IList<byte> inventories, 
            IList<byte> propositions, bool brokenLevelBonuses )
        {
            try
            {
                bool psp = Context == Context.US_PSP;
                var Abilities = new AllAbilities( abilities, abilityEffects, itemAbilityEffects, reactionEffects );
                var AbilityAnimations = new AllAnimations( Context, abilityAnimations, psp ? PSPResources.Binaries.AbilityAnimations : PSXResources.Binaries.AbilityAnimations );
                var Items = new AllItems( oldItems, newItems != null ? newItems : null );
                var ItemAttributes = new AllItemAttributes( oldItemAttributes, newItemAttributes != null ? newItemAttributes : null );
                var Jobs = new AllJobs( Context, jobs );
                var JobLevels = new JobLevels( Context, jobLevels,
                    new JobLevels( Context, Context == Context.US_PSP ? PSPResources.Binaries.JobLevels : PSXResources.Binaries.JobLevels ) );
                var SkillSets = new AllSkillSets( Context, skillSets,
                    Context == Context.US_PSP ? PSPResources.Binaries.SkillSets : PSXResources.Binaries.SkillSets );
                var MonsterSkills = new AllMonsterSkills( monsterSkills );
                var ActionMenus = new AllActionMenus( actionMenus, Context );
                var StatusAttributes = new AllStatusAttributes( statusAttributes );
                var InflictStatuses = new AllInflictStatuses( inflictStatuses );
                var PoachProbabilities = new AllPoachProbabilities( poach );
                var ENTDs = psp ? new AllENTDs( entd1, entd2, entd3, entd4, entd5 ) : new AllENTDs( entd1, entd2, entd3, entd4 );
                var MoveFind = new AllMoveFindItems( Context, moveFind, new AllMoveFindItems( Context, psp ? PSPResources.Binaries.MoveFind : PSXResources.Binaries.MoveFind ) );
                var StoreInventories = new AllStoreInventories( Context, inventories, psp ? PSPResources.Binaries.StoreInventories : PSXResources.Binaries.StoreInventories );
                var Propositions = new AllPropositions( propositions, psp ? PSPResources.Binaries.Propositions : PSXResources.Binaries.Propositions, brokenLevelBonuses );
                FFTPatch.Propositions = Propositions;

                FFTPatch.Abilities = Abilities;
                FFTPatch.AbilityAnimations = AbilityAnimations;
                FFTPatch.Items = Items;
                FFTPatch.ItemAttributes = ItemAttributes;
                FFTPatch.Jobs = Jobs;
                FFTPatch.JobLevels = JobLevels;
                FFTPatch.SkillSets = SkillSets;
                FFTPatch.MonsterSkills = MonsterSkills;
                FFTPatch.ActionMenus = ActionMenus;
                FFTPatch.StatusAttributes = StatusAttributes;
                FFTPatch.InflictStatuses = InflictStatuses;
                FFTPatch.PoachProbabilities = PoachProbabilities;
                FFTPatch.ENTDs = ENTDs;
                FFTPatch.MoveFind = MoveFind;
                FFTPatch.StoreInventories = StoreInventories;
            }
            catch( Exception )
            {
                throw new LoadPatchException();
            }
        }

        private static void LoadNewStylePatch( string filename )
        {
            using ( ZipFile file = new ZipFile( filename ) )
            {
                string fileVersion = Encoding.UTF8.GetString( GetZipEntry( file, "version", true ) );
                Context = (Context)Enum.Parse( typeof( Context ), Encoding.UTF8.GetString( GetZipEntry( file, "type", true ) ) );
                bool psp = Context == Context.US_PSP;

                IDictionary<ElementName, IList<byte>> defaults = psp ? DefaultPspElements : DefaultPsxElements;

                var buggyzipEntry = GetZipEntry( file, "BuggyPropositions", false );
                var propsZipEntry = GetZipEntry(file, elementNames[ElementName.Propositions], false);
                bool buggy = (buggyzipEntry != null && propsZipEntry != null) || propsZipEntry == null;
                
                LoadDataFromBytes(
                    GetZipEntry( file, elementNames[ElementName.Abilities], false ) ?? defaults[ElementName.Abilities],
                    GetZipEntry( file, elementNames[ElementName.AbilityEffects], false ) ?? defaults[ElementName.AbilityEffects],
                    GetZipEntry( file, elementNames[ElementName.ItemAbilityEffects], false ) ?? defaults[ElementName.ItemAbilityEffects],
                    GetZipEntry( file, elementNames[ElementName.ReactionAbilityEffects], false) ?? defaults[ElementName.ReactionAbilityEffects],
                    GetZipEntry( file, elementNames[ElementName.AbilityAnimations], false)?? defaults[ElementName.AbilityAnimations],
                    GetZipEntry( file, elementNames[ElementName.Items], false ) ?? defaults[ElementName.Items],
                    GetZipEntry( file, elementNames[ElementName.ItemAttributes], false ) ?? defaults[ElementName.ItemAttributes],
                    psp ? ( GetZipEntry( file, elementNames[ElementName.PSPItems], false ) ?? defaults[ElementName.PSPItems] ) : null,
                    psp ? ( GetZipEntry( file, elementNames[ElementName.PSPItemAttributes], false ) ?? defaults[ElementName.PSPItemAttributes] ) : null,
                    GetZipEntry( file, elementNames[ElementName.Jobs], false ) ?? defaults[ElementName.Jobs],
                    GetZipEntry( file, elementNames[ElementName.JobLevels], false ) ?? defaults[ElementName.JobLevels],
                    GetZipEntry( file, elementNames[ElementName.SkillSets], false ) ?? defaults[ElementName.SkillSets],
                    GetZipEntry( file, elementNames[ElementName.MonsterSkills], false ) ?? defaults[ElementName.MonsterSkills],
                    GetZipEntry( file, elementNames[ElementName.ActionMenus], false ) ?? defaults[ElementName.ActionMenus],
                    GetZipEntry( file, elementNames[ElementName.StatusAttributes], false ) ?? defaults[ElementName.StatusAttributes],
                    GetZipEntry( file, elementNames[ElementName.InflictStatuses], false ) ?? defaults[ElementName.InflictStatuses],
                    GetZipEntry( file, elementNames[ElementName.Poaching], false ) ?? defaults[ElementName.Poaching],
                    GetZipEntry( file, elementNames[ElementName.ENTD1], false ) ?? defaults[ElementName.ENTD1],
                    GetZipEntry( file, elementNames[ElementName.ENTD2], false ) ?? defaults[ElementName.ENTD2],
                    GetZipEntry( file, elementNames[ElementName.ENTD3], false ) ?? defaults[ElementName.ENTD3],
                    GetZipEntry( file, elementNames[ElementName.ENTD4], false ) ?? defaults[ElementName.ENTD4],
                    psp ? ( GetZipEntry( file, elementNames[ElementName.ENTD5], false ) ?? defaults[ElementName.ENTD5] ) : null,
                    GetZipEntry( file, elementNames[ElementName.MoveFindItems], false ) ?? defaults[ElementName.MoveFindItems],
                    GetZipEntry( file, elementNames[ElementName.StoreInventories], false ) ?? defaults[ElementName.StoreInventories],
                    propsZipEntry ?? defaults[ElementName.Propositions], buggy );
            }
        }

        private static void LoadOldStylePatch( XmlDocument doc )
        {
            XmlNode rootNode = doc.SelectSingleNode( "/patch" );
            string type = rootNode.Attributes["type"].InnerText;
            Context = (Context)Enum.Parse( typeof( Context ), type );
            bool psp = Context == Context.US_PSP;

            IList<byte> abilities = GetFromNodeOrReturnDefault( rootNode, "abilities", psp ? PSPResources.Binaries.Abilities : PSXResources.Binaries.Abilities );
            IList<byte> abilityEffects = GetFromNodeOrReturnDefault( rootNode, "abilityEffects", psp ? PSPResources.Binaries.AbilityEffects : PSXResources.Binaries.AbilityEffects );
            IList<byte> itemAbilityEffects = GetFromNodeOrReturnDefault( rootNode, "itemAbilityEffects", psp ? PSPResources.Binaries.ItemAbilityEffects : PSXResources.Binaries.ItemAbilityEffects );
            IList<byte> reactionAbilityEffects = GetFromNodeOrReturnDefault( rootNode, "reactionAbilityEffects", psp ? PSPResources.Binaries.ReactionAbilityEffects : PSXResources.Binaries.ReactionAbilityEffects );
            IList<byte> abilityAnimations = GetFromNodeOrReturnDefault( rootNode, "abilityAnimations", psp ? PSPResources.Binaries.AbilityAnimations : PSXResources.Binaries.AbilityAnimations );
            IList<byte> oldItems = GetFromNodeOrReturnDefault( rootNode, "items", psp ? PSPResources.Binaries.OldItems : PSXResources.Binaries.OldItems );
            IList<byte> oldItemAttributes = GetFromNodeOrReturnDefault( rootNode, "itemAttributes", psp ? PSPResources.Binaries.OldItemAttributes : PSXResources.Binaries.OldItemAttributes );
            IList<byte> newItems = psp ? GetFromNodeOrReturnDefault( rootNode, "pspItems", PSPResources.Binaries.NewItems ) : null;
            IList<byte> newItemAttributes = psp ? GetFromNodeOrReturnDefault( rootNode, "pspItemAttributes", PSPResources.Binaries.NewItemAttributes ) : null;
            IList<byte> jobs = GetFromNodeOrReturnDefault( rootNode, "jobs", psp ? PSPResources.Binaries.Jobs : PSXResources.Binaries.Jobs );
            IList<byte> jobLevels = GetFromNodeOrReturnDefault( rootNode, "jobLevels", psp ? PSPResources.Binaries.JobLevels : PSXResources.Binaries.JobLevels );
            IList<byte> skillSets = GetFromNodeOrReturnDefault( rootNode, "skillSets", psp ? PSPResources.Binaries.SkillSets : PSXResources.Binaries.SkillSets );
            IList<byte> monsterSkills = GetFromNodeOrReturnDefault( rootNode, "monsterSkills", psp ? PSPResources.Binaries.MonsterSkills : PSXResources.Binaries.MonsterSkills );
            IList<byte> actionMenus = GetFromNodeOrReturnDefault( rootNode, "actionMenus", psp ? PSPResources.Binaries.ActionEvents : PSXResources.Binaries.ActionEvents );
            IList<byte> statusAttributes = GetFromNodeOrReturnDefault( rootNode, "statusAttributes", psp ? PSPResources.Binaries.StatusAttributes : PSXResources.Binaries.StatusAttributes );
            IList<byte> inflictStatuses = GetFromNodeOrReturnDefault( rootNode, "inflictStatuses", psp ? PSPResources.Binaries.InflictStatuses : PSXResources.Binaries.InflictStatuses );
            IList<byte> poach = GetFromNodeOrReturnDefault( rootNode, "poaching", psp ? PSPResources.Binaries.PoachProbabilities : PSXResources.Binaries.PoachProbabilities );
            IList<byte> entd1 = GetFromNodeOrReturnDefault( rootNode, "entd1", PSPResources.Binaries.ENTD1 );
            IList<byte> entd2 = GetFromNodeOrReturnDefault( rootNode, "entd2", PSPResources.Binaries.ENTD2 );
            IList<byte> entd3 = GetFromNodeOrReturnDefault( rootNode, "entd3", PSPResources.Binaries.ENTD3 );
            IList<byte> entd4 = GetFromNodeOrReturnDefault( rootNode, "entd4", PSPResources.Binaries.ENTD4 );
            IList<byte> entd5 = GetFromNodeOrReturnDefault( rootNode, "entd5", PSPResources.Binaries.ENTD5 );
            IList<byte> moveFind = GetFromNodeOrReturnDefault( rootNode, "moveFindItems", psp ? PSPResources.Binaries.MoveFind : PSXResources.Binaries.MoveFind );
            IList<byte> inventories = GetFromNodeOrReturnDefault( rootNode, "storeInventories", psp ? PSPResources.Binaries.StoreInventories : PSXResources.Binaries.StoreInventories );
            IList<byte> propositions = GetFromNodeOrReturnDefault( rootNode, "propositions", psp ? PSPResources.Binaries.Propositions : PSXResources.Binaries.Propositions );

            LoadDataFromBytes( abilities, abilityEffects, itemAbilityEffects, reactionAbilityEffects, abilityAnimations,
                oldItems, oldItemAttributes, newItems, newItemAttributes,
                jobs, jobLevels, skillSets, monsterSkills, actionMenus, statusAttributes,
                inflictStatuses, poach, entd1, entd2, entd3, entd4, entd5,
                moveFind, inventories, propositions, true );
        }

        private static string ReadString( FileStream stream, int length )
        {
            byte[] bytes = new byte[length];
            stream.Read( bytes, 0, length );
            StringBuilder result = new StringBuilder();
            foreach( byte b in bytes )
            {
                result.Append( Convert.ToChar( b ) );
            }

            return result.ToString();
        }

        private static void SaveZippedPatch( string path, Context destinationContext )
        {
            using ( ZipOutputStream stream = new ZipOutputStream( File.Open( path, FileMode.Create, FileAccess.ReadWrite ) ) )
            {
                const string fileVersion = "1.0";
                bool psp = destinationContext == Context.US_PSP;

                WriteFileToZip( stream, "version", Encoding.UTF8.GetBytes( fileVersion ) );
                WriteFileToZip( stream, "type", Encoding.UTF8.GetBytes( destinationContext.ToString() ) );

                WriteFileToZip( stream, elementNames[ElementName.Abilities], Abilities.ToByteArray( destinationContext ) );
                WriteFileToZip( stream, elementNames[ElementName.AbilityAnimations], AbilityAnimations.ToByteArray());
                WriteFileToZip( stream, elementNames[ElementName.AbilityEffects], Abilities.ToEffectsByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.ItemAbilityEffects], Abilities.ToItemEffectsByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.ReactionAbilityEffects], Abilities.ToReactionEffectsByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.Items], Items.ToFirstByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.ItemAttributes], ItemAttributes.ToFirstByteArray() );
                if ( psp && Context == Context.US_PSP )
                {
                    WriteFileToZip( stream, elementNames[ElementName.PSPItems], Items.ToSecondByteArray() );
                    WriteFileToZip( stream, elementNames[ElementName.PSPItemAttributes], ItemAttributes.ToSecondByteArray() );
                    WriteFileToZip( stream, elementNames[ElementName.ENTD5], ENTDs.PSPEventsToByteArray() );
                }
                WriteFileToZip( stream, elementNames[ElementName.Jobs], Jobs.ToByteArray( destinationContext ) );
                WriteFileToZip( stream, elementNames[ElementName.JobLevels], JobLevels.ToByteArray( destinationContext ) );
                WriteFileToZip( stream, elementNames[ElementName.MonsterSkills], MonsterSkills.ToByteArray( destinationContext ) );
                WriteFileToZip( stream, elementNames[ElementName.SkillSets], SkillSets.ToByteArray( destinationContext ) );
                WriteFileToZip( stream, elementNames[ElementName.ActionMenus], ActionMenus.ToByteArray( destinationContext ) );
                WriteFileToZip( stream, elementNames[ElementName.StatusAttributes], StatusAttributes.ToByteArray( destinationContext ) );
                WriteFileToZip( stream, elementNames[ElementName.InflictStatuses], InflictStatuses.ToByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.Poaching], PoachProbabilities.ToByteArray( destinationContext ) );
                WriteFileToZip( stream, elementNames[ElementName.ENTD1], ENTDs.ENTDs[0].ToByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.ENTD2], ENTDs.ENTDs[1].ToByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.ENTD3], ENTDs.ENTDs[2].ToByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.ENTD4], ENTDs.ENTDs[3].ToByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.MoveFindItems], MoveFind.ToByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.StoreInventories], StoreInventories.ToByteArray() );
                WriteFileToZip( stream, elementNames[ElementName.Propositions], Propositions.ToByteArray() );
                if (!AllPropositions.CanFixBuggyLevelBonuses( destinationContext ))
                {
                    WriteFileToZip( stream, "BuggyPropositions", new byte[0] );
                }
            }
        }

        private static void WriteFileToZip( ZipOutputStream stream, string filename, byte[] bytes )
        {
            ZipEntry ze = new ZipEntry(filename);
            ze.Size = bytes.Length;
            stream.PutNextEntry( ze );
            stream.Write( bytes, 0, bytes.Length );
        }

		#endregion Private Methods 

        private static IDictionary<ElementName, IList<byte>> DefaultPsxElements = new Dictionary<ElementName, IList<byte>> {
            { ElementName.Propositions, PSXResources.Binaries.Propositions },
            { ElementName.Abilities, PSXResources.Binaries.Abilities },
            { ElementName.ReactionAbilityEffects, PSXResources.Binaries.ReactionAbilityEffects },
            { ElementName.ItemAbilityEffects, PSXResources.Binaries.ItemAbilityEffects },
            { ElementName.AbilityEffects, PSXResources.Binaries.AbilityEffects },
            { ElementName.AbilityAnimations, PSXResources.Binaries.AbilityAnimations },
            { ElementName.Items, PSXResources.Binaries.OldItems },
            { ElementName.ItemAttributes, PSXResources.Binaries.OldItemAttributes },
            { ElementName.PSPItems, null },
            { ElementName.PSPItemAttributes, null },
            { ElementName.Jobs, PSXResources.Binaries.Jobs },
            { ElementName.JobLevels, PSXResources.Binaries.JobLevels},
            { ElementName.SkillSets, PSXResources.Binaries.SkillSets },
            { ElementName.MonsterSkills, PSXResources.Binaries.MonsterSkills},
            { ElementName.ActionMenus, PSXResources.Binaries.ActionEvents},
            { ElementName.InflictStatuses, PSXResources.Binaries.InflictStatuses },
            { ElementName.StatusAttributes, PSXResources.Binaries.StatusAttributes },
            { ElementName.Poaching, PSXResources.Binaries.PoachProbabilities },
            { ElementName.ENTD1, PSXResources.Binaries.ENTD1 },
            { ElementName.ENTD2, PSXResources.Binaries.ENTD2},
            { ElementName.ENTD3, PSXResources.Binaries.ENTD3},
            { ElementName.ENTD4, PSXResources.Binaries.ENTD4},
            { ElementName.ENTD5, null},
            { ElementName.MoveFindItems, PSXResources.Binaries.MoveFind},
            { ElementName.StoreInventories, PSXResources.Binaries.StoreInventories} };
        private static IDictionary<ElementName,IList<byte>> DefaultPspElements = new Dictionary<ElementName, IList<byte>> {
            { ElementName.Propositions, PSPResources.Binaries.Propositions },
            { ElementName.Abilities, PSPResources.Binaries.Abilities },
            { ElementName.AbilityEffects, PSPResources.Binaries.AbilityEffects },
            { ElementName.ItemAbilityEffects, PSPResources.Binaries.ItemAbilityEffects },
            { ElementName.ReactionAbilityEffects, PSPResources.Binaries.ReactionAbilityEffects },
            { ElementName.AbilityAnimations, PSPResources.Binaries.AbilityAnimations },
            { ElementName.Items, PSPResources.Binaries.OldItems },
            { ElementName.ItemAttributes, PSPResources.Binaries.OldItemAttributes },
            { ElementName.PSPItems, PSPResources.Binaries.NewItems },
            { ElementName.PSPItemAttributes, PSPResources.Binaries.NewItemAttributes },
            { ElementName.Jobs, PSPResources.Binaries.Jobs },
            { ElementName.JobLevels, PSPResources.Binaries.JobLevels},
            { ElementName.SkillSets, PSPResources.Binaries.SkillSets },
            { ElementName.MonsterSkills, PSPResources.Binaries.MonsterSkills},
            { ElementName.ActionMenus, PSPResources.Binaries.ActionEvents},
            { ElementName.InflictStatuses, PSPResources.Binaries.InflictStatuses },
            { ElementName.StatusAttributes, PSPResources.Binaries.StatusAttributes },
            { ElementName.Poaching, PSPResources.Binaries.PoachProbabilities },
            { ElementName.ENTD1, PSPResources.Binaries.ENTD1 },
            { ElementName.ENTD2, PSPResources.Binaries.ENTD2},
            { ElementName.ENTD3, PSPResources.Binaries.ENTD3},
            { ElementName.ENTD4, PSPResources.Binaries.ENTD4},
            { ElementName.ENTD5, PSPResources.Binaries.ENTD5},
            { ElementName.MoveFindItems, PSPResources.Binaries.MoveFind},
            { ElementName.StoreInventories, PSPResources.Binaries.StoreInventories} };

        private enum ElementName
        {
            Propositions,
            Abilities,
            ReactionAbilityEffects,
            ItemAbilityEffects,
            AbilityEffects,
            AbilityAnimations,
            Items,
            ItemAttributes,
            PSPItems,
            PSPItemAttributes,
            Jobs,
            JobLevels,
            SkillSets,
            MonsterSkills,
            ActionMenus,
            InflictStatuses,
            StatusAttributes,
            Poaching,
            ENTD1,
            ENTD2,
            ENTD3,
            ENTD4,
            ENTD5,
            MoveFindItems,
            StoreInventories
        }

        public static event EventHandler DataChanged;

        public class LoadPatchException : Exception
        {

        }
    }
}