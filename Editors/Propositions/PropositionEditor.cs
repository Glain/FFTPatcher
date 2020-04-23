using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using PatcherLib;

namespace FFTPatcher.Editors
{
    public partial class PropositionEditor : BaseEditor
    {
        private Proposition proposition;
        private Context ourContext = Context.Default;
        private UInt16[] prices;
        public void BindTo( Proposition prop, IList<UInt16> prices, IList<UInt16> baseJpRewards, IList<UInt16> baseGilRewards )
        {
            if (ourContext != FFTPatch.Context)
            {
                ourContext = FFTPatch.Context;
                RegenerateContextSensitiveData();
            }

            if (this.prices == null || !Utilities.CompareArrays( prices, this.prices ) ||
                this.jpRewards == null || !Utilities.CompareArrays(baseJpRewards, this.jpRewards) ||
                this.gilRewards == null || !Utilities.CompareArrays(baseGilRewards, this.gilRewards) )
            {
                NotifyNewPrices( prices, baseJpRewards, baseGilRewards );
            }

            ignoreChanges = true;

            preferredStatsComboBox.SetIndexAndDefault( (int)prop.BraveFaith, (int)prop.Default.BraveFaith );
            maxDaysSpinner.SetValueAndDefault( prop.MaxDays, prop.Default.MaxDays );
            minDaysSpinner.SetValueAndDefault( prop.MinDays, prop.Default.MinDays );
            prereqByteSpinner.SetValueAndDefault( prop.PrereqByte, prop.Default.PrereqByte );
            completeJobComboBox.SetIndexAndDefault( (int)prop.PrereqProp, (int)prop.Default.PrereqProp );
            prereqTypeComboBox.SetIndexAndDefault( (int)prop.PrereqType, (int)prop.Default.PrereqType );
            certainDateComboBox.SetIndexAndDefault( (int)prop.PrereqZodiac, (int)prop.Default.PrereqZodiac );
            price1ComboBox.SetIndexAndDefault( prop.PriceIndex1 - 1, prop.Default.PriceIndex1 - 1 );
            price2ComboBox.SetIndexAndDefault( prop.PriceIndex2 - 1, prop.Default.PriceIndex2 - 1 );
            baseJpRewardComboBox.SetIndexAndDefault( (int)prop.BaseSmallReward, (int)prop.Default.BaseSmallReward );
            baseGilRewardComboBox.SetIndexAndDefault( (int)prop.BaseLargeReward, (int)prop.Default.BaseLargeReward );
            rewardComboBox.SetIndexAndDefault( (int)prop.Reward+1, (int)prop.Default.Reward +1);
            townComboBox.SetIndexAndDefault( (int)prop.Town, (int)prop.Default.Town );
            jobTypeComboBox.SetIndexAndDefault( (int)prop.Type, (int)prop.Default.Type );
            randomSuccessClassComboBox.SetIndexAndDefault( (int)prop.RandomSuccessClass, (int)prop.Default.RandomSuccessClass );

            //unknown0x09Spinner.SetValueAndDefault( prop.Unknown0x09, prop.Default.Unknown0x09 );
            //unknown0x0BSpinner.SetValueAndDefault( prop.Unknown0x0B, prop.Default.Unknown0x0B );
            //unknown0x0CSpinner.SetValueAndDefault( prop.Unknown0x0C, prop.Default.Unknown0x0C );
            unknown0x0FSpinner.SetValueAndDefault( prop.Unknown0x0F, prop.Default.Unknown0x0F );
            //unknown0x14Spinner.SetValueAndDefault( prop.Unknown0x14, prop.Default.Unknown0x14 );
            unlockedComboBox.SetValueAndDefault( prop.WhenUnlocked, prop.Default.WhenUnlocked );

            rawByteLabel.Enabled = prop.PrereqType != PrereqType.None;
            completeJobLabel.Enabled = prop.PrereqType == PrereqType.FinishedJob;
            certainDateLabel.Enabled = prop.PrereqType == PrereqType.CertainMonth;
            dateRangeLabel.Enabled = prop.PrereqType == PrereqType.CertainMonth;

            proposition = prop;
            ignoreChanges = false;

            //FireDataChanged();
        }

        private void GenerateContextInsensitiveData()
        {
            string[] jobTypeStrings = new string[256];
            string[] prereqTypeStrings = new string[256];
            string[] dateStrings = new string[256];

            for (int i = 0; i < 256; i++)
            {
                string unknownString = string.Format( "{0:X2} Unknown", i );
                jobTypeStrings[i] = unknownString;
                prereqTypeStrings[i] = unknownString;
                dateStrings[i] = unknownString;
            }
            jobTypeStrings[(int)PropositionType.Combat] = "Combat";
            jobTypeStrings[(int)PropositionType.Mining] = "Mining";
            jobTypeStrings[(int)PropositionType.Exploration] = "Exploration";
            jobTypeStrings[(int)PropositionType.Salvage] = "Salvage";
            jobTypeStrings[(int)PropositionType.Investigation1] = "Investigation";
            jobTypeStrings[(int)PropositionType.Investigation2] = "Investigation 2";
            jobTypeStrings[(int)PropositionType.OddJobs] = "Odd Jobs";
            jobTypeStrings[(int)PropositionType.Contest] = "Contest";

            prereqTypeStrings[(int)PrereqType.CertainMonth] = "Certain Date";
            prereqTypeStrings[(int)PrereqType.FinishedJob] = "Complete Another Job";
            prereqTypeStrings[(int)PrereqType.None] = "Nothing";

            for (int i = (int)Zodiac.Aries; i <= (int)Zodiac.Pisces; i++)
            {
                dateStrings[i] = ((Zodiac)i).ToString();
            }

            certainDateComboBox.BeginUpdate();
            certainDateComboBox.Items.Clear();
            certainDateComboBox.Items.AddRange( dateStrings );
            certainDateComboBox.EndUpdate();

            jobTypeComboBox.BeginUpdate();
            jobTypeComboBox.Items.Clear();
            jobTypeComboBox.Items.AddRange( jobTypeStrings );
            jobTypeComboBox.EndUpdate();


            prereqTypeComboBox.BeginUpdate();
            prereqTypeComboBox.Items.Clear();
            prereqTypeComboBox.Items.AddRange( prereqTypeStrings );
            prereqTypeComboBox.EndUpdate();

        }

        private void RegenerateContextSensitiveData()
        {
            string[] rewardStrings = new string[256];
            string[] preferredStatsStrings = new string[256];
            string[] townStrings = new string[256];
            string[] propNames = new string[256];

            for (int i = 0; i < 256; i++)
            {
                string unknownString = string.Format( "{0:X2} Unknown", i );
                rewardStrings[i] = unknownString;
                preferredStatsStrings[i] = unknownString;
                townStrings[i] = unknownString;
                propNames[i] = unknownString;
            }
            townStrings[0] = "Any Town";

            rewardStrings[(int)Reward.Gil] = "Gil";

            preferredStatsStrings[(int)BraveFaithNeutral.Faith] = "Faith";
            preferredStatsStrings[(int)BraveFaithNeutral.Neutral] = "Neutral";

            if (ourContext == Context.US_PSP)
            {
                preferredStatsStrings[(int)BraveFaithNeutral.Brave] = "Bravery";
                rewardStrings[(int)Reward.Land] = "Wonder";
                rewardStrings[(int)Reward.Treasure] = "Artefact";
                PSPResources.Lists.TownNames.ForEach( kvp => townStrings[(int)kvp.Key] = kvp.Value );
                var props = PSPResources.Lists.Propositions;
                for (int i = 0; i < props.Count; i++)
                    propNames[i] = props[i];

            }
            else if (ourContext == Context.US_PSX)
            {
                preferredStatsStrings[(int)BraveFaithNeutral.Brave] = "Brave";
                rewardStrings[(int)Reward.Land] = "Unexplored Land";
                rewardStrings[(int)Reward.Treasure] = "Treasure";
                PSXResources.Lists.TownNames.ForEach( kvp => townStrings[(int)kvp.Key] = kvp.Value );
                var props = PSXResources.Lists.Propositions;
                for (int i = 0; i < props.Count; i++)
                    propNames[i] = props[i];
            }

            completeJobComboBox.BeginUpdate();
            completeJobComboBox.Items.Clear();
            completeJobComboBox.Items.AddRange( propNames );
            completeJobComboBox.EndUpdate();

            unlockedComboBox.BeginUpdate();
            unlockedComboBox.Items.Clear();
            unlockedComboBox.Items.AddRange( ShopAvailability.AllAvailabilities.ToArray() );
            unlockedComboBox.EndUpdate();

            preferredStatsComboBox.BeginUpdate();
            preferredStatsComboBox.Items.Clear();
            preferredStatsComboBox.Items.AddRange( preferredStatsStrings );
            preferredStatsComboBox.EndUpdate();

            rewardComboBox.BeginUpdate();
            rewardComboBox.Items.Clear();
            rewardComboBox.Items.Add( "Nothing" );
            rewardComboBox.Items.AddRange( rewardStrings);
            rewardComboBox.EndUpdate();

            townComboBox.BeginUpdate();
            townComboBox.Items.Clear();
            townComboBox.Items.AddRange( townStrings );
            townComboBox.EndUpdate();

            randomSuccessClassComboBox.BeginUpdate();
            randomSuccessClassComboBox.Items.Clear();
            randomSuccessClassComboBox.Items.Add( "None" );
            var jobNames = ourContext == Context.US_PSP ? PSPResources.Lists.JobNames : PSXResources.Lists.JobNames;
            randomSuccessClassComboBox.Items.AddRange( jobNames.Sub( 0x4A, 0x4A + 19 ).ToArray() );
            randomSuccessClassComboBox.EndUpdate();

        }

        public void NotifyNewPrices( IList<UInt16> prices, IList<UInt16> baseJpRewards, IList<UInt16> baseGilRewards )
        {
            ignoreChanges = true;
            int price1Index = price1ComboBox.SelectedIndex;
            int price2Index = price2ComboBox.SelectedIndex;

            this.prices = prices.ToArray();
            price1ComboBox.BeginUpdate();
            price2ComboBox.BeginUpdate();
            price1ComboBox.Items.Clear();
            price2ComboBox.Items.Clear();
            foreach (ushort p in prices)
            {
                price1ComboBox.Items.Add(p);
                price2ComboBox.Items.Add(p);
            }
            price1ComboBox.SelectedIndex = price1Index;
            price2ComboBox.SelectedIndex = price2Index;

            price1ComboBox.EndUpdate();
            price2ComboBox.EndUpdate();

            int jpIndex = baseJpRewardComboBox.SelectedIndex;
            this.jpRewards = baseJpRewards.ToArray();
            baseJpRewardComboBox.BeginUpdate();
            baseJpRewardComboBox.Items.Clear();
            baseJpRewardComboBox.Items.Add( 0 );
            foreach (ushort jp in baseJpRewards)
            {
                baseJpRewardComboBox.Items.Add( jp );
            }
            baseJpRewardComboBox.SelectedIndex = jpIndex;
            baseJpRewardComboBox.EndUpdate();

            int gilIndex = baseGilRewardComboBox.SelectedIndex;
            this.gilRewards = baseGilRewards.ToArray();
            baseGilRewardComboBox.BeginUpdate();
            baseGilRewardComboBox.Items.Clear();
            baseGilRewardComboBox.Items.Add( 0 );
            foreach (ushort gil in baseGilRewards)
            {
                baseGilRewardComboBox.Items.Add( gil );
            }
            baseGilRewardComboBox.SelectedIndex = gilIndex;
            baseGilRewardComboBox.EndUpdate();
            ignoreChanges = false;


        }

        ushort[] jpRewards;
        ushort[] gilRewards;

        public PropositionEditor()
        {
            InitializeComponent();
            GenerateContextInsensitiveData();
        }

        bool ignoreChanges = false;

        private void prereqByteSpinner_ValueChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                ignoreChanges = true;
                proposition.PrereqByte = (byte)prereqByteSpinner.Value;
                completeJobComboBox.SetIndexAndDefault( (int)proposition.PrereqProp, (int)proposition.Default.PrereqProp );
                certainDateComboBox.SetIndexAndDefault( (int)proposition.PrereqZodiac, (int)proposition.Default.PrereqZodiac );
                ignoreChanges = false;
                FireDataChanged();
            }
        }

        private void spinner_ValueChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                NumericUpDown spinner = sender as NumericUpDown;
                PatcherLib.ReflectionHelpers.SetFieldOrProperty( proposition, (string)spinner.Tag, (byte)spinner.Value );
                FireDataChanged();
            }
        }

        private void priceComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                ComboBox box = sender as ComboBox;
                PatcherLib.ReflectionHelpers.SetFieldOrProperty( proposition, (string)box.Tag, (byte)(box.SelectedIndex + 1) );
                FireDataChanged();
            }
        }


        private void completeJobComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                ignoreChanges = true;
                proposition.PrereqByte = (byte)completeJobComboBox.SelectedIndex;
                prereqByteSpinner.SetValueAndDefault( proposition.PrereqByte, proposition.Default.PrereqByte );
                certainDateComboBox.SetIndexAndDefault( (int)proposition.PrereqZodiac, (int)proposition.Default.PrereqZodiac );
                ignoreChanges = false;
                FireDataChanged();
            }
        }

        private IDictionary<Zodiac, string> zodiacToDateRange = new Dictionary<Zodiac,string> {
            { Zodiac.Aries, "March 21 - April 19" },
            { Zodiac.Taurus, "April 20 - May 20" },
            { Zodiac.Gemini, "May 21 - June 21" },
            { Zodiac.Cancer, "June 22 - July 22" },
            { Zodiac.Leo, "July 23 - August 22" },
            { Zodiac.Virgo, "August 23 - September 22" },
            { Zodiac.Libra, "September 23 - October 23" },
            { Zodiac.Scorpio, "October 24 - November 22" },
            { Zodiac.Sagittarius, "November 23 - December 22" },
            { Zodiac.Capricorn, "December 23 - January 19" },
            { Zodiac.Aquarius, "January 20 - February 18" },
            { Zodiac.Pisces, "February 19 - March 20" },
        }.AsReadOnly();

        private void certainDateComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                ignoreChanges = true;
                proposition.PrereqByte = (byte)certainDateComboBox.SelectedIndex;
                completeJobComboBox.SetIndexAndDefault( (int)proposition.PrereqProp, (int)proposition.Default.PrereqProp );
                prereqByteSpinner.SetValueAndDefault( proposition.PrereqByte, proposition.Default.PrereqByte );
                ignoreChanges = false;
                FireDataChanged();
            }
            if (zodiacToDateRange.ContainsKey( (Zodiac)certainDateComboBox.SelectedIndex ))
            {
                dateRangeLabel.Text = zodiacToDateRange[(Zodiac)certainDateComboBox.SelectedIndex];
            }
            else
            {
                dateRangeLabel.Text = string.Empty;
            }
        }

        private void unlockedComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                proposition.WhenUnlocked = (ShopAvailability)unlockedComboBox.SelectedItem;
                FireDataChanged();
            }
        }

        private void prereqTypeComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                proposition.PrereqType = (PrereqType)prereqTypeComboBox.SelectedIndex;
                rawByteLabel.Enabled = proposition.PrereqType != PrereqType.None;
                completeJobLabel.Enabled = proposition.PrereqType == PrereqType.FinishedJob;
                certainDateLabel.Enabled = proposition.PrereqType == PrereqType.CertainMonth;
                dateRangeLabel.Enabled = proposition.PrereqType == PrereqType.CertainMonth;
                FireDataChanged();
            }
        }

        private void enumComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                ComboBox box = sender as ComboBox;
                ReflectionHelpers.SetFieldOrProperty( proposition, (string)box.Tag, box.SelectedIndex );
                FireDataChanged();
            }
        }

        private void rewardComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                ComboBox box = sender as ComboBox;
                proposition.Reward = (Reward)(box.SelectedIndex - 1);
            }
        }

        private void randomSuccessClassComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges && randomSuccessClassComboBox.SelectedIndex != -1)
            {
                proposition.RandomSuccessClass = (RandomSuccessClass)randomSuccessClassComboBox.SelectedIndex;
            }
        }

        private void baseJpRewardComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges && baseJpRewardComboBox.SelectedIndex != -1)
            {
                proposition.BaseSmallReward = (BonusIndex)baseJpRewardComboBox.SelectedIndex;
            }
        }

        private void baseGilRewardComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges && baseGilRewardComboBox.SelectedIndex != -1)
            {
                proposition.BaseLargeReward = (BonusIndex)baseGilRewardComboBox.SelectedIndex;
            }
        }


    }
}
