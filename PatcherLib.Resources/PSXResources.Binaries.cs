using System;
using System.Collections.Generic;
using System.Text;

namespace PatcherLib
{
    using PatcherLib.Datatypes;
    using PatcherLib.Utilities;
    using Paths = ResourcesClass.Paths.PSX;

    public static partial class PSXResources
    {
        public static class Binaries
        {
            public static IList<byte> Propositions { get; internal set; }
            public static IList<byte> Abilities { get; internal set; }
            public static IList<byte> AbilityAnimations { get; internal set; }
            public static IList<byte> AbilityEffects { get; internal set; }
            public static IList<byte> ItemAbilityEffects { get; internal set; }
            public static IList<byte> ReactionAbilityEffects { get; internal set; }
            public static IList<byte> ActionEvents { get; internal set; }
            public static IList<byte> ENTD1 { get; internal set; }
            public static IList<byte> ENTD2 { get; internal set; }
            public static IList<byte> ENTD3 { get; internal set; }
            public static IList<byte> ENTD4 { get; internal set; }
            public static IList<byte> Font { get; internal set; }
            public static IList<byte> FontWidths { get; internal set; }
            public static IList<byte> InflictStatuses { get; internal set; }
            public static IList<byte> JobLevels { get; internal set; }
            public static IList<byte> Jobs { get; internal set; }
            public static IList<byte> MonsterSkills { get; internal set; }
            public static IList<byte> MoveFind { get; internal set; }
            public static IList<byte> OldItemAttributes { get; internal set; }
            public static IList<byte> OldItems { get; internal set; }
            public static IList<byte> PoachProbabilities { get; internal set; }
            public static IList<byte> SCEAPDAT { get; internal set; }
            public static IList<byte> SkillSets { get; internal set; }
            public static IList<byte> StatusAttributes { get; internal set; }
            public static IList<byte> StoreInventories { get; internal set; }
        }

    }
}
