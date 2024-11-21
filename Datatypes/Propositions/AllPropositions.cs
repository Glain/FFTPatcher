using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Utilities;
using PatcherLib.Datatypes;
using PatcherLib;
using Lokad;

namespace FFTPatcher.Datatypes
{
    public enum PropositionClass
    {
        Squire = 0,
        Chemist,
        Knight,
        Archer,
        Monk,
        Priest,
        Wizard,
        TimeMage,
        Summoner,
        Thief,
        Mediator,
        Oracle,
        Geomancer,
        Lancer,
        Samurai,
        Ninja,
        Calculator,
        Bard,
        Dancer,
        Mime,
        Specials,
        Monsters
    }

    public enum RandomSuccessClass
    {
        None = 0,
        Squire,
        Chemist,
        Knight,
        Archer,
        Monk,
        Priest,
        Wizard,
        TimeMage,
        Summoner,
        Thief,
        Mediator,
        Oracle,
        Geomancer,
        Lancer,
        Samurai,
        Ninja,
        Calculator,
        Bard,
        Dancer,
        Mime,
    }

    public enum BraveFaithRange
    {
        _1to20 = 0,
        _21to40,
        _41to60,
        _61to80,
        _81to100
    }

    public enum LevelRange
    {
        _1to10 = 0,
        _11to20,
        _21to30,
        _31to40,
        _41to50,
        _51to60,
        _61to70,
        _71to80,
        _81to90,
        _91to100,
    }

    public enum BonusPercent
    {
        _50Percent = 0,
        _40Percent,
        _10Percent
    }

    public enum BonusIndex
    {
        Nothing = 0,
        _One,
        _Two,
        _Three,
        _Four,
        _Five,
        _Six,
        _Seven,
        _Eight,
    }
    public class AllPropositions : PatchableFile, IXmlDigest, IGenerateCodes
    {
        public const int NumPropositions = 96;
        public const int propLength = 23;

        public AllPropositions( IList<byte> bytes, bool brokenLevelBonuses, Context context )
            : this( bytes, null as AllPropositions, brokenLevelBonuses, context )
        {
        }

        public AllPropositions( IList<byte> bytes, IList<byte> defaultBytes, bool brokenLevelBonuses, Context context )
            : this( bytes, new AllPropositions( defaultBytes, brokenLevelBonuses, context ), brokenLevelBonuses, context )
        {
        }

        //public bool MirrorLevelRanges { get { return !CanFixBuggyLevelBonuses( FFTPatch.Context ); } }
        public bool MirrorLevelRanges { get { return false; } }

        public IList<UInt16> Prices { get; private set; }
        public IList<UInt16> SmallBonuses { get; private set; }
        public IList<UInt16> LargeBonuses { get; private set; }
        public IList<UInt16> JPMultipliers { get; private set; }
        public IList<UInt16> GilMultipliers { get; private set; }

        private TupleDictionary<PropositionType, PropositionClass, byte> propTypeBonuses;
        public TupleDictionary<PropositionType, PropositionClass, byte> PropositionTypeBonuses { get { return propTypeBonuses; } }

        private TupleDictionary<BraveFaithNeutral, PropositionClass, byte> bfBonuses;
        public TupleDictionary<BraveFaithNeutral, PropositionClass, byte> BraveFaithBonuses { get { return bfBonuses; } }

        private TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte> braveBonuses;
        public TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte> BraveStatBonuses { get { return braveBonuses; } }

        private TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte> faithBonuses;
        public TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte> FaithStatBonuses { get { return faithBonuses; } }

        private TupleDictionary<BraveFaithNeutral, LevelRange, byte> levelBonuses;
        public TupleDictionary<BraveFaithNeutral, LevelRange, byte> LevelBonuses { get { return levelBonuses; } }

        private IDictionary<PropositionType, BonusIndex> treasureLandJpBonuses;
        public IDictionary<PropositionType, BonusIndex> TreasureLandJpBonuses { get { return treasureLandJpBonuses; } }

        private IDictionary<PropositionType, BonusIndex> treasureLandGilBonuses;
        public IDictionary<PropositionType, BonusIndex> TreasureLandGilBonuses { get { return treasureLandGilBonuses; } }

        private TupleDictionary<PropositionType, BonusPercent, BonusIndex> bonusCashGilBonuses;
        public TupleDictionary<PropositionType, BonusPercent, BonusIndex> BonusCashGilBonuses { get { return bonusCashGilBonuses; } }

        private TupleDictionary<PropositionType, BonusPercent, BonusIndex> bonusCashJpBonuses;
        public TupleDictionary<PropositionType, BonusPercent, BonusIndex> BonusCashJpBonuses { get { return bonusCashJpBonuses; } }

        IList<byte> realLevelBonusBytes;

        public void SetPropositionTypeBonus( PropositionType type, PropositionClass _class, byte value )
        {
            propTypeBonuses[type, _class] = value;
        }

        public void SetBraveFaithBonus( BraveFaithNeutral bfn, PropositionClass _class, byte value )
        {
            bfBonuses[bfn, _class] = value;
        }

        public AllPropositions Default { get; private set; }

        public AllPropositions( IList<byte> bytes, AllPropositions defaults, bool brokenLevelBonuses, Context context )
        {
            Default = defaults;
            var names = context == Context.US_PSP ? PSPResources.Lists.Propositions : PSXResources.Lists.Propositions;

            List<Proposition> props = new List<Proposition>();
            if (defaults != null)
            {
                for (int i = 0; i < NumPropositions; i++)
                {
                    props.Add( new Proposition( names[i], bytes.Sub( i * propLength, (i + 1) * propLength - 1 ), defaults.Propositions[i], context ) );
                }
            }
            else
            {
                for (int i = 0; i < NumPropositions; i++)
                {
                    props.Add( new Proposition( names[i], bytes.Sub( i * propLength, (i + 1) * propLength - 1 ), context ) );
                }
            }

            Prices = new UInt16[8];
            for (int i = 0; i < 8; i++)
            {
                var b = bytes.Sub( propLength * NumPropositions + i * 2 + 2, propLength * NumPropositions + i * 2 + 2 + 1 );
                Prices[i] = Utilities.BytesToUShort( b[0], b[1] );
            }

            unknownBytes = bytes.Sub( 0x8b2, 0x8bf ).ToArray();

            propTypeBonuses = new TupleDictionary<PropositionType, PropositionClass, byte>();

            foreach (PropositionType type in (PropositionType[])Enum.GetValues( typeof( PropositionType ) ))
            {
                foreach (PropositionClass _class in (PropositionClass[])Enum.GetValues( typeof( PropositionClass ) ))
                {
                    propTypeBonuses[type, _class] = bytes[0x8C0 + ((int)type - 1) * 22 + (int)_class];
                }
            }
            propTypeBonuses = new TupleDictionary<PropositionType, PropositionClass, byte>( propTypeBonuses, false, true );

            bfBonuses = new TupleDictionary<BraveFaithNeutral, PropositionClass, byte>();
            foreach (BraveFaithNeutral bfn in (BraveFaithNeutral[])Enum.GetValues( typeof( BraveFaithNeutral ) ))
            {
                foreach (PropositionClass _class in (PropositionClass[])Enum.GetValues( typeof( PropositionClass ) ))
                {
                    bfBonuses[bfn, _class] = bytes[0x970 + ((int)bfn - 1) * 22 + (int)_class];
                }
            }
            bfBonuses = new TupleDictionary<BraveFaithNeutral, PropositionClass, byte>( bfBonuses, false, true );

            braveBonuses = new TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte>();
            faithBonuses = new TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte>();
            levelBonuses = new TupleDictionary<BraveFaithNeutral, LevelRange, byte>();

            //int levelBonusesOffset = brokenLevelBonuses ? 0x9B4 : 0x9D4;
            int levelBonusesOffset = 0x9D4;
            
            foreach (BraveFaithNeutral bfn in (BraveFaithNeutral[])Enum.GetValues( typeof( BraveFaithNeutral ) ))
            {
                foreach (BraveFaithRange range in (BraveFaithRange[])Enum.GetValues( typeof( BraveFaithRange ) ))
                {
                    braveBonuses[bfn, range] = bytes[0x9B4 + ((int)bfn - 1) * 5 + (int)range];
                    faithBonuses[bfn, range] = bytes[0x9C4 + ((int)bfn - 1) * 5 + (int)range];
                }

                foreach (LevelRange range in (LevelRange[])Enum.GetValues( typeof( LevelRange ) ))
                {
                    levelBonuses[bfn, range] = bytes[levelBonusesOffset + ((int)bfn - 1) * 10 + (int)range];
                }
            }


            braveBonuses = new TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte>( braveBonuses, false, true );
            faithBonuses = new TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte>( faithBonuses, false, true );
            levelBonuses = new TupleDictionary<BraveFaithNeutral, LevelRange, byte>( levelBonuses, false, true );

            treasureLandJpBonuses = new Dictionary<PropositionType, BonusIndex>();
            treasureLandGilBonuses = new Dictionary<PropositionType, BonusIndex>();
            bonusCashGilBonuses = new TupleDictionary<PropositionType, BonusPercent, BonusIndex>();
            bonusCashJpBonuses = new TupleDictionary<PropositionType, BonusPercent, BonusIndex>();

            foreach (PropositionType type in (PropositionType[])Enum.GetValues( typeof( PropositionType ) ))
            {
                treasureLandGilBonuses[type] = (BonusIndex)bytes[0x9F4 + 2 * ((int)type - 1)];
                treasureLandJpBonuses[type] = (BonusIndex)bytes[0x9F4 + 2 * ((int)type - 1) + 1];

                bonusCashGilBonuses[type, BonusPercent._10Percent] = (BonusIndex)bytes[0xA04 + ((int)type - 1) * 6 + 2 * 2];
                bonusCashGilBonuses[type, BonusPercent._40Percent] = (BonusIndex)bytes[0xA04 + ((int)type - 1) * 6 + 2 * 1];
                bonusCashGilBonuses[type, BonusPercent._50Percent] = (BonusIndex)bytes[0xA04 + ((int)type - 1) * 6 + 2 * 0];

                bonusCashJpBonuses[type, BonusPercent._10Percent] = (BonusIndex)bytes[0xA04 + ((int)type - 1) * 6 + 2 * 2 + 1];
                bonusCashJpBonuses[type, BonusPercent._40Percent] = (BonusIndex)bytes[0xA04 + ((int)type - 1) * 6 + 2 * 1 + 1];
                bonusCashJpBonuses[type, BonusPercent._50Percent] = (BonusIndex)bytes[0xA04 + ((int)type - 1) * 6 + 2 * 0 + 1];
            }
            bonusCashGilBonuses = new TupleDictionary<PropositionType, BonusPercent, BonusIndex>( bonusCashGilBonuses, false, true );
            bonusCashJpBonuses = new TupleDictionary<PropositionType, BonusPercent, BonusIndex>( bonusCashJpBonuses, false, true );

            SmallBonuses = new UInt16[8];
            LargeBonuses = new UInt16[8];
            for (int i = 0; i < 8; i++)
            {
                SmallBonuses[i] = Utilities.BytesToUShort( bytes[0xA34 + i * 2], bytes[0xA34 + i * 2 + 1] );
                LargeBonuses[i] = Utilities.BytesToUShort( bytes[0xA44 + i * 2], bytes[0xA44 + i * 2 + 1] );
            }

            JPMultipliers = new UInt16[10];
            GilMultipliers = new UInt16[10];

            for (int i = 0; i < 10; i++)
            {
                JPMultipliers[i] = Utilities.BytesToUShort( bytes[0xA54 + i * 2], bytes[0xA54 + i * 2 + 1] );
                GilMultipliers[i] = Utilities.BytesToUShort( bytes[0xA68 + i * 2], bytes[0xA68 + i * 2 + 1] );
            }


            Propositions = props.AsReadOnly();
        }

        private IList<byte> unknownBytes;

        public IList<Proposition> Propositions
        {
            get;
            private set;
        }

        public string GetCodeHeader( PatcherLib.Datatypes.Context context )
        {
            return context == Context.US_PSP ? "_C0 Propositions" : "\"Propositions";

        }

        public IList<string> GenerateCodes( PatcherLib.Datatypes.Context context, FFTPatch fftPatch )
        {
            if (context == Context.US_PSP)
            {
                return Codes.GenerateCodes( Context.US_PSP, fftPatch.Defaults[FFTPatch.ElementName.Propositions], this.ToByteArray(), 0x2E9634 );
            }
            else
            {
                return Codes.GenerateCodes(Context.US_PSX, fftPatch.Defaults[FFTPatch.ElementName.Propositions], this.ToByteArray(), 0x9D380, Codes.CodeEnabledOnlyWhen.World);
            }
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            throw new NotImplementedException();
        }

        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>( 0xA7C );
            Propositions.ForEach( p => result.AddRange( p.ToByteArray() ) );
            result.Add( 0x00 );
            result.Add( 0x00 );
            Prices.ForEach( p => result.AddRange( p.ToBytes() ) );
            result.AddRange( unknownBytes );

            PropositionClass[] classes = (PropositionClass[])Enum.GetValues( typeof( PropositionClass ) );
            PropositionType[] propTypes = (PropositionType[])Enum.GetValues( typeof( PropositionType ) );
            foreach (PropositionType type in propTypes)
            {
                foreach (PropositionClass _class in classes)
                {
                    result.Add( propTypeBonuses[type, _class] );
                }
            }
            BraveFaithNeutral[] bfnVals = (BraveFaithNeutral[])Enum.GetValues( typeof( BraveFaithNeutral ) );
            foreach (BraveFaithNeutral bfn in bfnVals)
            {
                foreach (PropositionClass _class in classes)
                {
                    result.Add( bfBonuses[bfn, _class] );
                }
            }

            result.Add( 0x00 );
            result.Add( 0x00 );
            BraveFaithRange[] bfnRanges = (BraveFaithRange[])Enum.GetValues( typeof( BraveFaithRange ) );

            foreach (BraveFaithNeutral bfn in bfnVals)
            {
                foreach (BraveFaithRange range in bfnRanges)
                {
                    result.Add( braveBonuses[bfn, range] );
                }
            }

            result.Add( 0x00 );

            foreach (BraveFaithNeutral bfn in bfnVals)
            {
                foreach (BraveFaithRange range in bfnRanges)
                {
                    result.Add( faithBonuses[bfn, range] );
                }
            }

            result.Add( 0x00 );

            LevelRange[] levelRanges = (LevelRange[])Enum.GetValues( typeof( LevelRange ) );
            foreach (BraveFaithNeutral bfn in bfnVals)
            {
                foreach (LevelRange range in levelRanges)
                {
                    result.Add( levelBonuses[bfn, range] );
                }
            }

            result.Add( 0x00 );
            result.Add( 0x00 );

            foreach (PropositionType type in propTypes)
            {
                result.Add( (byte)treasureLandGilBonuses[type] );
                result.Add( (byte)treasureLandJpBonuses[type] );
            }

            foreach (PropositionType type in propTypes)
            {
                for (int i = 0; i < 3; i++)
                {
                    result.Add( (byte)bonusCashGilBonuses[type, (BonusPercent)i] );
                    result.Add( (byte)bonusCashJpBonuses[type, (BonusPercent)i] );
                }
            }

            SmallBonuses.ForEach( b => result.AddRange( b.ToBytes() ) );
            LargeBonuses.ForEach( b => result.AddRange( b.ToBytes() ) );

            JPMultipliers.ForEach( b => result.AddRange( b.ToBytes() ) );
            GilMultipliers.ForEach( b => result.AddRange( b.ToBytes() ) );

            System.Diagnostics.Debug.Assert( result.Count == 0xA7C );
            return result.ToArray();
        }

        public override IList<PatchedByteArray> GetPatches( PatcherLib.Datatypes.Context context )
        {
            var result = new List<PatchedByteArray>( 2 );
            var bytes = ToByteArray();
            if (context == Context.US_PSX)
            {
                result.Add( PatcherLib.Iso.PsxIso.Propositions.GetPatchedByteArray( bytes ) );
                foreach (var good in goodPsxInstructions)
                {
                    result.Add( new PatchedByteArray( PatcherLib.Iso.PsxIso.Sectors.WORLD_WLDCORE_BIN,
                        good.Item1, good.Item2.ToArray() ) );
                }
            }
            else if (context == Context.US_PSP)
            {
                PatcherLib.Iso.PspIso.Propositions.ForEach( kp => result.Add( kp.GetPatchedByteArray( bytes ) ) );
            }

            return result;
        }

        public override bool HasChanged
        {
            get
            {
                return
                    Default != null &&
                    (!Utilities.CompareArrays( Prices, Default.Prices ) ||
                     !Utilities.CompareArrays( SmallBonuses, Default.SmallBonuses ) ||
                     !Utilities.CompareArrays( LargeBonuses, Default.LargeBonuses ) ||
                     !Utilities.CompareArrays(JPMultipliers, Default.JPMultipliers)||
                     !Utilities.CompareArrays(GilMultipliers, Default.GilMultipliers)||
                     !propTypeBonuses.Keys.TrueForAll( k => propTypeBonuses[k] == Default.propTypeBonuses[k] ) ||
                     !bfBonuses.Keys.TrueForAll( k => bfBonuses[k] == Default.bfBonuses[k] ) ||
                     !braveBonuses.Keys.TrueForAll( k => braveBonuses[k] == Default.braveBonuses[k] ) ||
                     !faithBonuses.Keys.TrueForAll( k => faithBonuses[k] == Default.faithBonuses[k] ) ||
                     !levelBonuses.Keys.TrueForAll( k => levelBonuses[k] == Default.levelBonuses[k] ) ||
                     !treasureLandJpBonuses.Keys.TrueForAll( k => treasureLandJpBonuses[k] == Default.treasureLandJpBonuses[k] ) ||
                     !treasureLandGilBonuses.Keys.TrueForAll( k => treasureLandGilBonuses[k] == Default.treasureLandGilBonuses[k] ) ||
                     !bonusCashGilBonuses.Keys.TrueForAll( k => bonusCashGilBonuses[k] == Default.bonusCashGilBonuses[k] ) ||
                     !bonusCashJpBonuses.Keys.TrueForAll( k => bonusCashJpBonuses[k] == Default.bonusCashJpBonuses[k] ) ||
                    !Propositions.TrueForAll( p => !p.HasChanged ));
            }
        }

        private static readonly IList<Tuple<int, IList<byte>>> badPsxInstructions = new List<Tuple<int, IList<byte>>> {
            Tuple.From(0x128C0, (IList<byte>)(new byte[] { 0x21, 0xB0, 0x40, 0x00 }.AsReadOnly())), // 0x128C0 move $s6, $v0
            Tuple.From(0x12A64, (IList<byte>)(new byte[] { 0x00, 0x00, 0x00, 0x00 }.AsReadOnly())), // 0x12A64 nop
            Tuple.From(0x12A74, (IList<byte>)(new byte[] { 0x21, 0x10, 0x56, 0x00 }.AsReadOnly())), // 0x12A74 addu $v0,$s6
            Tuple.From(0x1287C, (IList<byte>)(new byte[] { 0x00, 0x00, 0x00, 0x00 }.AsReadOnly())), // 0x1287C nop
            Tuple.From(0x128C8, (IList<byte>)(new byte[] { 0x00, 0x00, 0x00, 0x00 }.AsReadOnly())), // 0x128C8 nop
        }.AsReadOnly();

        private static readonly IList<Tuple<int, IList<byte>>> goodPsxInstructions = new List<Tuple<int, IList<byte>>> {
            Tuple.From(0x128C0, (IList<byte>)(new byte[] { 0x3C, 0x00, 0xA2, 0xAF }.AsReadOnly())), // 0x128C0 sw $v0, 0x68+var_2C($sp)
            Tuple.From(0x12A64, (IList<byte>)(new byte[] { 0x3C, 0x00, 0xA5, 0x8F }.AsReadOnly())), // 0x12A64 lw $a1, 0x68+var_2C($sp)
            Tuple.From(0x12A74, (IList<byte>)(new byte[] { 0x21, 0x10, 0x45, 0x00 }.AsReadOnly())), // 0x12A74 addu $v0,$a1
            Tuple.From(0x1287C, (IList<byte>)(new byte[] { 0x0A, 0x80, 0x16, 0x3C }.AsReadOnly())), // 0x1287C lui $s6, 0x800a
            Tuple.From(0x128C8, (IList<byte>)(new byte[] { 0x54, 0xDD, 0xD6, 0x26 }.AsReadOnly())), // 0x128C8 addiu $s6, 0xDD54
        }.AsReadOnly();

        public static bool CanFixBuggyLevelBonuses( Context context )
        {
            return context == Context.US_PSX;
            //return true;
        }

        public static bool IsoHasBuggyLevelBonuses( System.IO.Stream iso, Context context )
        {
            if (context == Context.US_PSP)
            {
                //return false;
                return true;
            }
            else
            {
                bool allBad = true;
                foreach (var badInstruction in badPsxInstructions)
                {
                    var kp = new PatcherLib.Iso.PsxIso.KnownPosition( PatcherLib.Iso.PsxIso.Sectors.WORLD_WLDCORE_BIN, badInstruction.Item1, 4 );
                    var bytes = kp.ReadIso( iso );
                    allBad &= Utilities.CompareArrays( bytes, badInstruction.Item2 );
                }

                if (allBad)
                    return true;

                bool allGood = true;
                foreach (var goodInstruction in goodPsxInstructions)
                {
                    var kp = new PatcherLib.Iso.PsxIso.KnownPosition( PatcherLib.Iso.PsxIso.Sectors.WORLD_WLDCORE_BIN, goodInstruction.Item1, 4 );
                    var bytes = kp.ReadIso( iso );
                    allGood &= Utilities.CompareArrays( bytes, goodInstruction.Item2 );
                }

                if (allGood)
                    return false;

                return true;
            }
        }

        public static bool PsxSavestateHasBuggyLevelBonuses(System.IO.Stream stream)
        {
            bool allBad = true;
            foreach (var badInstruction in badPsxInstructions)
            {
                byte[] bytes = PatcherLib.Iso.PsxIso.LoadFromPsxSaveState(stream, PatcherLib.Iso.PsxIso.Sectors.WORLD_WLDCORE_BIN, badInstruction.Item1, 4, null);
                if (bytes != null)
                    allBad &= Utilities.CompareArrays(bytes, badInstruction.Item2);
            }

            if (allBad)
                return true;

            bool allGood = true;
            foreach (var goodInstruction in goodPsxInstructions)
            {
                byte[] bytes = PatcherLib.Iso.PsxIso.LoadFromPsxSaveState(stream, PatcherLib.Iso.PsxIso.Sectors.WORLD_WLDCORE_BIN, goodInstruction.Item1, 4, null);
                if (bytes != null)
                    allGood &= Utilities.CompareArrays(bytes, goodInstruction.Item2);
            }

            if (allGood)
                return false;

            return true;
        }
    }
}
