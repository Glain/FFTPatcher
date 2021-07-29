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
            private static IList<byte> _propositions = null;
            public static IList<byte> Propositions {
                get 
                {
                    if (_propositions == null)
                    {
                        _propositions = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.Propositions].AsReadOnly();
                        IList<byte> defaultProps = ResourcesClass.DefaultZipFileContents[ResourcesClass.Paths.PSX.Binaries.Propositions].AsReadOnly();
                        if (_propositions.Count < defaultProps.Count)
                        {
                            List<byte> newProps = new List<byte>(defaultProps.Count);
                            newProps.AddRange(_propositions);
                            newProps.AddRange(defaultProps.Sub(_propositions.Count));
                            _propositions = newProps.AsReadOnly();
                        }
                    }

                    return _propositions; 
                }
            }

            public static readonly IList<byte> Abilities = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.Abilities].AsReadOnly();
            public static readonly IList<byte> AbilityAnimations = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.AbilityAnimations].AsReadOnly();
            public static readonly IList<byte> AbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.AbilityEffects].AsReadOnly();
            public static readonly IList<byte> ItemAbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ItemAbilityEffects].AsReadOnly();
            public static readonly IList<byte> ReactionAbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ReactionAbilityEffects].AsReadOnly();
            public static readonly IList<byte> ActionEvents = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ActionEvents].AsReadOnly();
            public static readonly IList<byte> ENTD1 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ENTD1].AsReadOnly();
            public static readonly IList<byte> ENTD2 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ENTD2].AsReadOnly();
            public static readonly IList<byte> ENTD3 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ENTD3].AsReadOnly();
            public static readonly IList<byte> ENTD4 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ENTD4].AsReadOnly();
            public static readonly IList<byte> Font = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.Font].AsReadOnly();
            public static readonly IList<byte> FontWidths = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.FontWidths].AsReadOnly();
            public static readonly IList<byte> InflictStatuses = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.InflictStatuses].AsReadOnly();
            public static readonly IList<byte> JobFormationSprites1 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.JobFormationSprites1].AsReadOnly();
            public static readonly IList<byte> JobFormationSprites2 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.JobFormationSprites2].AsReadOnly();
            public static readonly IList<byte> JobLevels = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.JobLevels].AsReadOnly();
            public static readonly IList<byte> Jobs = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.Jobs].AsReadOnly();
            public static readonly IList<byte> MonsterSkills = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.MonsterSkills].AsReadOnly();
            public static readonly IList<byte> MoveFind = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.MoveFind].AsReadOnly();
            public static readonly IList<byte> OldItemAttributes = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.OldItemAttributes].AsReadOnly();
            public static readonly IList<byte> OldItems = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.OldItems].AsReadOnly();
            public static readonly IList<byte> PoachProbabilities = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.PoachProbabilities].AsReadOnly();
            public static readonly IList<byte> SCEAPDAT = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.SCEAP]; 
            public static readonly IList<byte> SkillSets = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.SkillSets].AsReadOnly();
            public static readonly IList<byte> StatusAttributes = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.StatusAttributes].AsReadOnly();
            public static readonly IList<byte> StoreInventories = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.StoreInventories].AsReadOnly();
        }
    }
}
