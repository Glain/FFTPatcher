using System.Collections.Generic;
using System.Collections.ObjectModel;

using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace PatcherLib
{
    public static partial class PSPResources
    {
        public static class Binaries
        {
            private static IList<byte> _propositions = null;
            public static IList<byte> Propositions 
            { 
                get
                {
                    if (_propositions == null)
                    {
                        _propositions = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.Propositions].AsReadOnly();
                        var defaultProps = ResourcesClass.DefaultZipFileContents[ResourcesClass.Paths.PSP.Binaries.Propositions].AsReadOnly();
                        if (_propositions.Count < defaultProps.Count)
                        {
                            List<byte> newProps = new List<byte>( defaultProps.Count );
                            newProps.AddRange( _propositions );
                            newProps.AddRange(defaultProps.Sub( _propositions.Count ) );
                            _propositions = newProps.AsReadOnly();
                        }
                    }

                    return _propositions;
                }
            }

            public static readonly IList<byte> StoreInventories = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.StoreInventories].AsReadOnly();
            public static readonly IList<byte> Abilities = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.Abilities].AsReadOnly();
            public static readonly IList<byte> AbilityAnimations = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.AbilityAnimations].AsReadOnly();
            public static readonly IList<byte> AbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.AbilityEffects].AsReadOnly();
            public static readonly IList<byte> ItemAbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ItemAbilityEffects].AsReadOnly();
            public static readonly IList<byte> ReactionAbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ReactionAbilityEffects].AsReadOnly();
            public static readonly IList<byte> ActionEvents = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ActionEvents].AsReadOnly();
            public static readonly IList<byte> ENTD1 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ENTD1].AsReadOnly();
            public static readonly IList<byte> ENTD2 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ENTD2].AsReadOnly();
            public static readonly IList<byte> ENTD3 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ENTD3].AsReadOnly();
            public static readonly IList<byte> ENTD4 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ENTD4].AsReadOnly();
            public static readonly IList<byte> ENTD5 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ENTD5].AsReadOnly();
            public static readonly IList<byte> Font = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.Font].AsReadOnly();
            public static readonly IList<byte> FontWidths = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.FontWidths].AsReadOnly();
            public static readonly IList<byte> ICON0 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ICON0].AsReadOnly();
            public static readonly IList<byte> InflictStatuses = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.InflictStatuses].AsReadOnly();
            public static readonly IList<byte> JobFormationSprites1 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.JobFormationSprites1].AsReadOnly();
            public static readonly IList<byte> JobFormationSprites2 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.JobFormationSprites2].AsReadOnly();
            public static readonly IList<byte> JobLevels = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.JobLevels].AsReadOnly();
            public static readonly IList<byte> Jobs = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.Jobs].AsReadOnly();
            public static readonly IList<byte> MonsterSkills = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.MonsterSkills].AsReadOnly();
            public static readonly IList<byte> MoveFind = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.MoveFind].AsReadOnly();
            public static readonly IList<byte> NewItemAttributes = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.NewItemAttributes].AsReadOnly();
            public static readonly IList<byte> NewItems = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.NewItems].AsReadOnly();
            public static readonly IList<byte> OldItemAttributes = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.OldItemAttributes].AsReadOnly();
            public static readonly IList<byte> OldItems = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.OldItems].AsReadOnly();
            public static readonly IList<byte> PoachProbabilities = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.PoachProbabilities].AsReadOnly();
            public static readonly IList<byte> SkillSets = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.SkillSets].AsReadOnly();
            public static readonly IList<byte> StatusAttributes = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.StatusAttributes].AsReadOnly();
        }
    }
}