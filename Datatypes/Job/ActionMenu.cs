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

using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using PatcherLib;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    public enum MenuAction
    {
        [Description( "<Default>" )]
        Default,
        [Description( "Item Inventory" )]
        ItemInventory,
        [Description( "Weapon Inventory" )]
        WeaponInventory,
        [PSXDescription( "Mathematics" )]
        [PSPDescription( "Arithmeticks" )]
        Arithmeticks,
        [Description( "Elements" )]
        Elements,
        [Description( "Blank " )] // extra space is a hack to fix DataGridViewComboBoxColumn
        Blank1,
        [Description( "Monster" )]
        Monster,
        [Description( "Katana Inventory" )]
        KatanaInventory,
        [Description( "Attack" )]
        Attack,
        [Description( "Jump" )]
        Jump,
        [PSXDescription( "Aim" )]
        [PSPDescription( "Charge" )]
        Charge,
        [Description( "Defend" )]
        Defend,
        [Description( "Change Equipment" )]
        ChangeEquip,
        [Description( "Unknown " )] // extra space is a hack to fix DataGridViewComboBoxColumn
        Unknown2,
        [Description( "Blank  " )] // extra space is a hack to fix DataGridViewComboBoxColumn
        Blank2,
        [Description( "Unknown  " )] // extra space is a hack to fix DataGridViewComboBoxColumn
        Unknown3
    }

    public class ActionMenuEntry
    {
		#region Instance Variables (1) 

        private static List<ActionMenuEntry> allActionMenuEntries;

		#endregion Instance Variables 

		#region Public Properties (2) 

        public static List<ActionMenuEntry> AllActionMenuEntries
        {
            get
            {
                if( allActionMenuEntries == null )
                {
                    allActionMenuEntries = new List<ActionMenuEntry>( 16 );
                    for( byte i = 0; i < 16; i++ )
                    {
                        allActionMenuEntries.Add( new ActionMenuEntry( i ) );
                    }
                }

                return allActionMenuEntries;
            }
        }

        public MenuAction MenuAction { get; private set; }

		#endregion Public Properties 

		#region Constructors (2) 

        private ActionMenuEntry( byte b )
        {
            MenuAction = (MenuAction)b;
        }

        private ActionMenuEntry( MenuAction a )
        {
            MenuAction = a;
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public override string ToString()
        {
            MemberInfo[] memInfo = typeof( MenuAction ).GetMember( MenuAction.ToString() );
            if( (memInfo != null) && (memInfo.Length > 0) )
            {
                object[] attrs = memInfo[0].GetCustomAttributes( typeof( DescriptionAttribute ), false );
                if( (attrs != null) && (attrs.Length > 0) )
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return base.ToString();
        }

		#endregion Public Methods 
    }

    public class ActionMenu : ISupportDigest, ISupportDefault<ActionMenu>
    {
		#region Instance Variables (1) 

        public static readonly string[] digestableProperties = new string[] { "MenuAction" };

		#endregion Instance Variables 

		#region Public Properties (8) 

        public string ActionName
        {
            get { return MenuAction.ToString(); }
        }

        public ActionMenu Default { get; private set; }

        public IList<string> DigestableProperties
        {
            get { return digestableProperties; }
        }

        public bool HasChanged
        {
            get { return Default != null && MenuAction.MenuAction != Default.MenuAction.MenuAction; }
        }

        private ActionMenuEntry menuAction;
        public ActionMenuEntry MenuAction 
        {
            get { return menuAction; }
            set { menuAction = value; }
        }

        public string Name { get; private set; }

        public ActionMenu Self { get { return this; } }

        public byte Value { get; private set; }

		#endregion Public Properties 

		#region Constructors (2) 

        public ActionMenu( byte value, string name, MenuAction action )
            : this( value, name, action, null )
        {
        }

        public ActionMenu( byte value, string name, MenuAction action, ActionMenu defaults )
        {
            Default = defaults;
            MenuAction = ActionMenuEntry.AllActionMenuEntries[(byte)action];
            Name = name;
            Value = value;
        }

		#endregion Constructors 

        #region Public Methods (2)

        public static void CopyAll(ActionMenu source, ActionMenu destination)
        {
            destination.MenuAction = source.MenuAction;
        }

        public void CopyAllTo(ActionMenu destination)
        {
            CopyAll(this, destination);
        }

        #endregion
    }

    public class AllActionMenus : PatchableFile, IXmlDigest, IGenerateCodes
    {
		#region Public Properties (2) 

        public ActionMenu[] ActionMenus { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public override bool HasChanged
        {
            get
            {
                foreach( ActionMenu a in ActionMenus )
                {
                    if( a.Default != null && a.MenuAction.MenuAction != a.Default.MenuAction.MenuAction )
                        return true;
                }

                return false;
            }
        }

		#endregion Public Properties 

		#region Constructors 

        public AllActionMenus(IList<byte> bytes, Context context) : this(bytes, null, context) { }

        public AllActionMenus( IList<byte> bytes, IList<byte> defaultBytes, Context context )
        {
            defaultBytes = defaultBytes ?? (context == Context.US_PSP ? PSPResources.Binaries.ActionEvents : PSXResources.Binaries.ActionEvents);

            List<ActionMenu> tempActions = new List<ActionMenu>();
            SkillSet[] dummySkillSets = SkillSet.GetDummySkillSets(context);

            for( int i = 0; i < 0xE0; i++ )
            {
                tempActions.Add(new ActionMenu((byte)i, dummySkillSets[i].Name, (MenuAction)bytes[i],
                    new ActionMenu((byte)i, dummySkillSets[i].Name, (MenuAction)defaultBytes[i])));
            }
            if( context == Context.US_PSP )
            {
                tempActions.Add(new ActionMenu(0xE0, dummySkillSets[0xE0].Name, (MenuAction)bytes[0xE0],
                    new ActionMenu((byte)0xE0, dummySkillSets[0xE0].Name, (MenuAction)defaultBytes[0xE0])));
                tempActions.Add(new ActionMenu(0xE1, dummySkillSets[0xE1].Name, (MenuAction)bytes[0xE1],
                    new ActionMenu((byte)0xE1, dummySkillSets[0xE1].Name, (MenuAction)defaultBytes[0xE1])));
                tempActions.Add(new ActionMenu(0xE2, dummySkillSets[0xE2].Name, (MenuAction)bytes[0xE2],
                    new ActionMenu((byte)0xE2, dummySkillSets[0xE2].Name, (MenuAction)defaultBytes[0xE2])));
            }

            ActionMenus = tempActions.ToArray();
        }

		#endregion Constructors 

		#region Public Methods (5) 

        public override IList<PatchedByteArray> GetPatches( Context context )
        {
            var result = new List<PatchedByteArray>( 2 );

            var bytes = ToByteArray( context );
            if ( context == Context.US_PSX )
            {
                result.Add(PatcherLib.Iso.PsxIso.ActionEvents.GetPatchedByteArray(bytes));
            }
            else if ( context == Context.US_PSP )
            {
                PatcherLib.Iso.PspIso.ActionEvents.ForEach(
                    kl => result.Add(kl.GetPatchedByteArray(bytes)));
            }

            return result;
        }

        public byte[] ToByteArray()
        {
            byte[] result = new byte[ActionMenus.Length];

            for( int i = 0; i < ActionMenus.Length; i++ )
            {
                result[i] = (byte)ActionMenus[i].MenuAction.MenuAction;
            }

            return result;
        }

        public byte[] ToByteArray( Context context )
        {
            return ToByteArray();
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            if( HasChanged )
            {
                writer.WriteStartElement( this.GetType().Name );
                writer.WriteAttributeString( "changed", HasChanged.ToString() );
                foreach( ActionMenu a in ActionMenus )
                {
                    if( a.HasChanged )
                    {
                        writer.WriteStartElement( a.GetType().Name );
                        writer.WriteAttributeString( "value", a.Value.ToString( "X2" ) );
                        writer.WriteAttributeString( "name", a.Name );
                        DigestGenerator.WriteXmlDigest( a, writer, false, true );
                    }
                }

                writer.WriteEndElement();
            }
        }

		#endregion Public Methods 
    
        #region IGenerateCodes Members

        public string GetCodeHeader(Context context)
        {
            return context == Context.US_PSP ? "_C0 Action Menus" : "\"Action Menus";
        }

        public IList<string> GenerateCodes(Context context, FFTPatch fftPatch)
        {
            if (context == Context.US_PSP)
            {
                return Codes.GenerateCodes( Context.US_PSP, fftPatch.Defaults[FFTPatch.ElementName.ActionMenus], this.ToByteArray(), 0x27AC50 );
            }
            else
            {
                return Codes.GenerateCodes(Context.US_PSX, fftPatch.Defaults[FFTPatch.ElementName.ActionMenus], this.ToByteArray(), 0x065CB4);
            }
        }

        #endregion
    }
}
