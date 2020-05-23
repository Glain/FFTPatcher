using System;
using System.Collections.Generic;
using System.Text;

using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    public class AllAnimations : PatchableFile, IXmlDigest, ISupportDefault<AllAnimations>, IGenerateCodes
    {
        private Animation[] animations;
        private IList<Animation> readOnlyAnimations;
        private Context context;

        public Animation this[int i]
        {
            get { return animations[i]; }
        }

        public IList<Animation> Animations { get { return readOnlyAnimations; } }

        private AllAnimations(IList<byte> bytes)
        {
            // Support and Movement abilities not included
            //animations = new Animation[512];
            //for (int i = 0; i < 512; i++)

            animations = new Animation[454];
            for (int i = 0; i < 454; i++)
            {
                animations[i] = new Animation(bytes.Sub(i * 3, i * 3 + 3 - 1));
            }
            readOnlyAnimations = animations.AsReadOnly();
        }

        public AllAnimations(Context context, IList<byte> bytes, IList<byte> defaultBytes)
        {
            this.context = context;
            IList<string> names = context == Context.US_PSP ? AllAbilities.PSPNames : AllAbilities.PSXNames;

            // Support and Movement abilities not included
            //animations = new Animation[512];
            //for (int i = 0; i < 512; i++)

            animations = new Animation[454];
            for (int i = 0; i < 454; i++)
            {
                animations[i] = new Animation(
                    (ushort)i,
                    names[i],
                    bytes.Sub(i * 3, i * 3 + 3 - 1),
                    defaultBytes.Sub(i * 3, i * 3 + 3 - 1));
            }
            Default = new AllAnimations(defaultBytes);
            readOnlyAnimations = animations.AsReadOnly();
        }

        public byte[] ToByteArray()
        {
            //byte[] result = new byte[512 * 3];
            //for (int i = 0; i < 512; i++)
            
            byte[] result = new byte[454 * 3];
            for (int i = 0; i < 454; i++)
            {
                this[i].ToByteArray().CopyTo(result, i * 3);
            }
            return result;
        }

        public AllAnimations Default
        {
            get; private set;
        }

        public override IList<PatcherLib.Datatypes.PatchedByteArray> GetPatches(PatcherLib.Datatypes.Context context)
        {
            List<PatchedByteArray> result = new List<PatchedByteArray>();
            byte[] bytes = ToByteArray();
            if (context == Context.US_PSP)
            {
                PatcherLib.Iso.PspIso.AbilityAnimations.ForEach(kp => result.Add(kp.GetPatchedByteArray(bytes)));
            }
            else if (context == Context.US_PSX)
            {
                result.Add(PatcherLib.Iso.PsxIso.AbilityAnimations.GetPatchedByteArray(bytes));
            }

            return result;
        }

        public override bool HasChanged
        {
            get { return animations.Exists(a => a.HasChanged); }
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            throw new NotImplementedException();
        }

        #region IGenerateCodes Members

        string IGenerateCodes.GetCodeHeader(Context context)
        {
            return context == Context.US_PSP ? "_C0 Ability Animations" : "\"Ability Animations";
        }

        IList<string> IGenerateCodes.GenerateCodes(Context context)
        {
            if (context == Context.US_PSP)
            {
                return Codes.GenerateCodes( Context.US_PSP, PatcherLib.PSPResources.Binaries.AbilityAnimations, this.ToByteArray(), 0x3278F8 );
            }
            else
            {
                return Codes.GenerateCodes( Context.US_PSX, PatcherLib.PSXResources.Binaries.AbilityAnimations, this.ToByteArray(), 0x93E10, Codes.CodeEnabledOnlyWhen.Battle );
            }
        }

        #endregion
    }
}
