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

namespace FFTPatcher.Editors
{
    partial class ItemEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	System.Windows.Forms.Label hLabel5;
        	System.Windows.Forms.Label hLabel1;
        	System.Windows.Forms.Label hLabel2;
        	System.Windows.Forms.Label paletteLabel;
        	System.Windows.Forms.Label graphicLabel;
        	System.Windows.Forms.Label enemyLevelLabel;
        	System.Windows.Forms.Label itemIconLabel;
        	System.Windows.Forms.Label priceLabel;
        	System.Windows.Forms.Label shopAvailabilityLabel;
        	System.Windows.Forms.Label formulaLabel;
        	System.Windows.Forms.Label rangeLabel;
        	System.Windows.Forms.Label weaponPowerLabel;
        	System.Windows.Forms.Label evadeLabel;
        	System.Windows.Forms.Label percentLabel2;
        	System.Windows.Forms.Label percentLabel3;
        	System.Windows.Forms.Label percentLabel1;
        	System.Windows.Forms.Label percentLabel4;
        	System.Windows.Forms.Label mpBonusLabel;
        	System.Windows.Forms.Label hpBonusLabel;
        	System.Windows.Forms.Label percentLabel6;
        	System.Windows.Forms.Label percentLabel5;
        	System.Windows.Forms.Label magicEvadeRateLabel;
        	System.Windows.Forms.Label physicalEvadeRateLabel;
        	System.Windows.Forms.Label zLabel;
        	System.Windows.Forms.Label itemFormulaLabel;
        	System.Windows.Forms.Label label1;
        	System.Windows.Forms.Label hLabel6;
        	System.Windows.Forms.Label hLabel7;
        	System.Windows.Forms.Label unknown1Label;
        	System.Windows.Forms.Label unknown2Label;
        	System.Windows.Forms.Label weaponUnknownLabel;
        	System.Windows.Forms.Label hLabel8;
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemEditor));
        	this.hLabel4 = new System.Windows.Forms.Label();
        	this.weaponPanel = new System.Windows.Forms.Panel();
        	this.weaponUnknownSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.formulasHelpLabel = new System.Windows.Forms.Label();
        	this.weaponFormulaComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
        	this.weaponCastSpellLabel = new System.Windows.Forms.Label();
        	this.weaponSpellStatusLabel = new System.Windows.Forms.LinkLabel();
        	this.weaponAttributesCheckedListBox = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
        	this.weaponElementsEditor = new FFTPatcher.Editors.ElementsEditor();
        	this.weaponSpellStatusSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.weaponEvadePercentageSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.weaponPowerSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.weaponRangeSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.weaponCastSpellComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
        	this.shieldPanel = new System.Windows.Forms.Panel();
        	this.shieldMagicBlockRateSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.shieldPhysicalBlockRateSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.armorPanel = new System.Windows.Forms.Panel();
        	this.armorMPBonusSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.armorHPBonusSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.accessoryPanel = new System.Windows.Forms.Panel();
        	this.accessoryMagicEvadeRateSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.accessoryPhysicalEvadeRateSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.chemistItemPanel = new System.Windows.Forms.Panel();
        	this.chemistItemFormulaComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
        	this.chemistItemSpellStatusLabel = new System.Windows.Forms.LinkLabel();
        	this.chemistItemSpellStatusSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.chemistItemXSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.itemAttributesLabel = new System.Windows.Forms.LinkLabel();
        	this.secondTableLabel = new System.Windows.Forms.Label();
        	this.weaponJumpLabel = new System.Windows.Forms.LinkLabel();
        	this.shieldJumpLabel = new System.Windows.Forms.LinkLabel();
        	this.headBodyJumpLabel = new System.Windows.Forms.LinkLabel();
        	this.accessoryJumpLabel = new System.Windows.Forms.LinkLabel();
        	this.chemistItemJumpLabel = new System.Windows.Forms.LinkLabel();
        	this.secondTableIdSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.itemAttributesCheckedListBox = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
        	this.shopAvailabilityComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
        	this.priceSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.itemTypeComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
        	this.enemyLevelSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.itemAttributesSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.graphicSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.paletteSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.storeInventoryCheckedListBox = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
        	this.unknown1Spinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.unknown2Spinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	hLabel5 = new System.Windows.Forms.Label();
        	hLabel1 = new System.Windows.Forms.Label();
        	hLabel2 = new System.Windows.Forms.Label();
        	paletteLabel = new System.Windows.Forms.Label();
        	graphicLabel = new System.Windows.Forms.Label();
        	enemyLevelLabel = new System.Windows.Forms.Label();
        	itemIconLabel = new System.Windows.Forms.Label();
        	priceLabel = new System.Windows.Forms.Label();
        	shopAvailabilityLabel = new System.Windows.Forms.Label();
        	formulaLabel = new System.Windows.Forms.Label();
        	rangeLabel = new System.Windows.Forms.Label();
        	weaponPowerLabel = new System.Windows.Forms.Label();
        	evadeLabel = new System.Windows.Forms.Label();
        	percentLabel2 = new System.Windows.Forms.Label();
        	percentLabel3 = new System.Windows.Forms.Label();
        	percentLabel1 = new System.Windows.Forms.Label();
        	percentLabel4 = new System.Windows.Forms.Label();
        	mpBonusLabel = new System.Windows.Forms.Label();
        	hpBonusLabel = new System.Windows.Forms.Label();
        	percentLabel6 = new System.Windows.Forms.Label();
        	percentLabel5 = new System.Windows.Forms.Label();
        	magicEvadeRateLabel = new System.Windows.Forms.Label();
        	physicalEvadeRateLabel = new System.Windows.Forms.Label();
        	zLabel = new System.Windows.Forms.Label();
        	itemFormulaLabel = new System.Windows.Forms.Label();
        	label1 = new System.Windows.Forms.Label();
        	hLabel6 = new System.Windows.Forms.Label();
        	hLabel7 = new System.Windows.Forms.Label();
        	unknown1Label = new System.Windows.Forms.Label();
        	unknown2Label = new System.Windows.Forms.Label();
        	weaponUnknownLabel = new System.Windows.Forms.Label();
        	hLabel8 = new System.Windows.Forms.Label();
        	this.weaponPanel.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.weaponUnknownSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.weaponSpellStatusSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.weaponEvadePercentageSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.weaponPowerSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.weaponRangeSpinner)).BeginInit();
        	this.shieldPanel.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.shieldMagicBlockRateSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.shieldPhysicalBlockRateSpinner)).BeginInit();
        	this.armorPanel.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.armorMPBonusSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.armorHPBonusSpinner)).BeginInit();
        	this.accessoryPanel.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.accessoryMagicEvadeRateSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.accessoryPhysicalEvadeRateSpinner)).BeginInit();
        	this.chemistItemPanel.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.chemistItemSpellStatusSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.chemistItemXSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.secondTableIdSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.priceSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.enemyLevelSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.itemAttributesSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.graphicSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.paletteSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.unknown1Spinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.unknown2Spinner)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// hLabel5
        	// 
        	hLabel5.AutoSize = true;
        	hLabel5.Location = new System.Drawing.Point(131, 4);
        	hLabel5.Name = "hLabel5";
        	hLabel5.Size = new System.Drawing.Size(13, 13);
        	hLabel5.TabIndex = 1;
        	hLabel5.Text = "h";
        	// 
        	// hLabel1
        	// 
        	hLabel1.AutoSize = true;
        	hLabel1.Location = new System.Drawing.Point(131, 25);
        	hLabel1.Name = "hLabel1";
        	hLabel1.Size = new System.Drawing.Size(13, 13);
        	hLabel1.TabIndex = 3;
        	hLabel1.Text = "h";
        	// 
        	// hLabel2
        	// 
        	hLabel2.AutoSize = true;
        	hLabel2.Location = new System.Drawing.Point(131, 89);
        	hLabel2.Name = "hLabel2";
        	hLabel2.Size = new System.Drawing.Size(13, 13);
        	hLabel2.TabIndex = 5;
        	hLabel2.Text = "h";
        	// 
        	// paletteLabel
        	// 
        	paletteLabel.AutoSize = true;
        	paletteLabel.Location = new System.Drawing.Point(4, 4);
        	paletteLabel.Name = "paletteLabel";
        	paletteLabel.Size = new System.Drawing.Size(40, 13);
        	paletteLabel.TabIndex = 6;
        	paletteLabel.Text = "Palette";
        	// 
        	// graphicLabel
        	// 
        	graphicLabel.AutoSize = true;
        	graphicLabel.Location = new System.Drawing.Point(4, 25);
        	graphicLabel.Name = "graphicLabel";
        	graphicLabel.Size = new System.Drawing.Size(44, 13);
        	graphicLabel.TabIndex = 7;
        	graphicLabel.Text = "Graphic";
        	// 
        	// enemyLevelLabel
        	// 
        	enemyLevelLabel.AutoSize = true;
        	enemyLevelLabel.Location = new System.Drawing.Point(4, 46);
        	enemyLevelLabel.Name = "enemyLevelLabel";
        	enemyLevelLabel.Size = new System.Drawing.Size(64, 13);
        	enemyLevelLabel.TabIndex = 9;
        	enemyLevelLabel.Text = "Enemy level";
        	// 
        	// itemIconLabel
        	// 
        	itemIconLabel.AutoSize = true;
        	itemIconLabel.Location = new System.Drawing.Point(4, 68);
        	itemIconLabel.Name = "itemIconLabel";
        	itemIconLabel.Size = new System.Drawing.Size(54, 13);
        	itemIconLabel.TabIndex = 13;
        	itemIconLabel.Text = "Item Type";
        	// 
        	// priceLabel
        	// 
        	priceLabel.AutoSize = true;
        	priceLabel.Location = new System.Drawing.Point(4, 110);
        	priceLabel.Name = "priceLabel";
        	priceLabel.Size = new System.Drawing.Size(31, 13);
        	priceLabel.TabIndex = 15;
        	priceLabel.Text = "Price";
        	// 
        	// shopAvailabilityLabel
        	// 
        	shopAvailabilityLabel.AutoSize = true;
        	shopAvailabilityLabel.Location = new System.Drawing.Point(4, 132);
        	shopAvailabilityLabel.Name = "shopAvailabilityLabel";
        	shopAvailabilityLabel.Size = new System.Drawing.Size(84, 13);
        	shopAvailabilityLabel.TabIndex = 16;
        	shopAvailabilityLabel.Text = "Shop Availability";
        	// 
        	// formulaLabel
        	// 
        	formulaLabel.AutoSize = true;
        	formulaLabel.Location = new System.Drawing.Point(0, 44);
        	formulaLabel.Name = "formulaLabel";
        	formulaLabel.Size = new System.Drawing.Size(44, 13);
        	formulaLabel.TabIndex = 17;
        	formulaLabel.Text = "Formula";
        	// 
        	// rangeLabel
        	// 
        	rangeLabel.AutoSize = true;
        	rangeLabel.Location = new System.Drawing.Point(0, 23);
        	rangeLabel.Name = "rangeLabel";
        	rangeLabel.Size = new System.Drawing.Size(39, 13);
        	rangeLabel.TabIndex = 18;
        	rangeLabel.Text = "Range";
        	// 
        	// weaponPowerLabel
        	// 
        	weaponPowerLabel.AutoSize = true;
        	weaponPowerLabel.Location = new System.Drawing.Point(0, 66);
        	weaponPowerLabel.Name = "weaponPowerLabel";
        	weaponPowerLabel.Size = new System.Drawing.Size(81, 13);
        	weaponPowerLabel.TabIndex = 19;
        	weaponPowerLabel.Text = "Weapon Power";
        	// 
        	// evadeLabel
        	// 
        	evadeLabel.AutoSize = true;
        	evadeLabel.Location = new System.Drawing.Point(0, 87);
        	evadeLabel.Name = "evadeLabel";
        	evadeLabel.Size = new System.Drawing.Size(46, 13);
        	evadeLabel.TabIndex = 20;
        	evadeLabel.Text = "Evade%";
        	// 
        	// percentLabel2
        	// 
        	percentLabel2.AutoSize = true;
        	percentLabel2.Location = new System.Drawing.Point(4, 6);
        	percentLabel2.Name = "percentLabel2";
        	percentLabel2.Size = new System.Drawing.Size(73, 13);
        	percentLabel2.TabIndex = 2;
        	percentLabel2.Text = "P. Block Rate";
        	// 
        	// percentLabel3
        	// 
        	percentLabel3.AutoSize = true;
        	percentLabel3.Location = new System.Drawing.Point(4, 26);
        	percentLabel3.Name = "percentLabel3";
        	percentLabel3.Size = new System.Drawing.Size(75, 13);
        	percentLabel3.TabIndex = 3;
        	percentLabel3.Text = "M. Block Rate";
        	// 
        	// percentLabel1
        	// 
        	percentLabel1.AutoSize = true;
        	percentLabel1.Location = new System.Drawing.Point(130, 9);
        	percentLabel1.Name = "percentLabel1";
        	percentLabel1.Size = new System.Drawing.Size(15, 13);
        	percentLabel1.TabIndex = 4;
        	percentLabel1.Text = "%";
        	// 
        	// percentLabel4
        	// 
        	percentLabel4.AutoSize = true;
        	percentLabel4.Location = new System.Drawing.Point(130, 26);
        	percentLabel4.Name = "percentLabel4";
        	percentLabel4.Size = new System.Drawing.Size(15, 13);
        	percentLabel4.TabIndex = 5;
        	percentLabel4.Text = "%";
        	// 
        	// mpBonusLabel
        	// 
        	mpBonusLabel.AutoSize = true;
        	mpBonusLabel.Location = new System.Drawing.Point(3, 26);
        	mpBonusLabel.Name = "mpBonusLabel";
        	mpBonusLabel.Size = new System.Drawing.Size(56, 13);
        	mpBonusLabel.TabIndex = 9;
        	mpBonusLabel.Text = "MP Bonus";
        	// 
        	// hpBonusLabel
        	// 
        	hpBonusLabel.AutoSize = true;
        	hpBonusLabel.Location = new System.Drawing.Point(3, 6);
        	hpBonusLabel.Name = "hpBonusLabel";
        	hpBonusLabel.Size = new System.Drawing.Size(55, 13);
        	hpBonusLabel.TabIndex = 8;
        	hpBonusLabel.Text = "HP Bonus";
        	// 
        	// percentLabel6
        	// 
        	percentLabel6.AutoSize = true;
        	percentLabel6.Location = new System.Drawing.Point(130, 26);
        	percentLabel6.Name = "percentLabel6";
        	percentLabel6.Size = new System.Drawing.Size(15, 13);
        	percentLabel6.TabIndex = 5;
        	percentLabel6.Text = "%";
        	// 
        	// percentLabel5
        	// 
        	percentLabel5.AutoSize = true;
        	percentLabel5.Location = new System.Drawing.Point(130, 9);
        	percentLabel5.Name = "percentLabel5";
        	percentLabel5.Size = new System.Drawing.Size(15, 13);
        	percentLabel5.TabIndex = 4;
        	percentLabel5.Text = "%";
        	// 
        	// magicEvadeRateLabel
        	// 
        	magicEvadeRateLabel.AutoSize = true;
        	magicEvadeRateLabel.Location = new System.Drawing.Point(4, 26);
        	magicEvadeRateLabel.Name = "magicEvadeRateLabel";
        	magicEvadeRateLabel.Size = new System.Drawing.Size(79, 13);
        	magicEvadeRateLabel.TabIndex = 3;
        	magicEvadeRateLabel.Text = "M. Evade Rate";
        	// 
        	// physicalEvadeRateLabel
        	// 
        	physicalEvadeRateLabel.AutoSize = true;
        	physicalEvadeRateLabel.Location = new System.Drawing.Point(4, 6);
        	physicalEvadeRateLabel.Name = "physicalEvadeRateLabel";
        	physicalEvadeRateLabel.Size = new System.Drawing.Size(77, 13);
        	physicalEvadeRateLabel.TabIndex = 2;
        	physicalEvadeRateLabel.Text = "P. Evade Rate";
        	// 
        	// zLabel
        	// 
        	zLabel.AutoSize = true;
        	zLabel.Location = new System.Drawing.Point(4, 27);
        	zLabel.Name = "zLabel";
        	zLabel.Size = new System.Drawing.Size(14, 13);
        	zLabel.TabIndex = 3;
        	zLabel.Text = "Z";
        	// 
        	// itemFormulaLabel
        	// 
        	itemFormulaLabel.AutoSize = true;
        	itemFormulaLabel.Location = new System.Drawing.Point(4, 6);
        	itemFormulaLabel.Name = "itemFormulaLabel";
        	itemFormulaLabel.Size = new System.Drawing.Size(44, 13);
        	itemFormulaLabel.TabIndex = 2;
        	itemFormulaLabel.Text = "Formula";
        	// 
        	// label1
        	// 
        	label1.AutoSize = true;
        	label1.Location = new System.Drawing.Point(427, 132);
        	label1.Name = "label1";
        	label1.Size = new System.Drawing.Size(13, 13);
        	label1.TabIndex = 19;
        	label1.Text = "h";
        	// 
        	// hLabel6
        	// 
        	hLabel6.AutoSize = true;
        	hLabel6.Location = new System.Drawing.Point(333, 4);
        	hLabel6.Name = "hLabel6";
        	hLabel6.Size = new System.Drawing.Size(13, 13);
        	hLabel6.TabIndex = 30;
        	hLabel6.Text = "h";
        	// 
        	// hLabel7
        	// 
        	hLabel7.AutoSize = true;
        	hLabel7.Location = new System.Drawing.Point(333, 25);
        	hLabel7.Name = "hLabel7";
        	hLabel7.Size = new System.Drawing.Size(13, 13);
        	hLabel7.TabIndex = 31;
        	hLabel7.Text = "h";
        	// 
        	// unknown1Label
        	// 
        	unknown1Label.AutoSize = true;
        	unknown1Label.Location = new System.Drawing.Point(224, 4);
        	unknown1Label.Name = "unknown1Label";
        	unknown1Label.Size = new System.Drawing.Size(62, 13);
        	unknown1Label.TabIndex = 32;
        	unknown1Label.Text = "Unknown 1";
        	// 
        	// unknown2Label
        	// 
        	unknown2Label.AutoSize = true;
        	unknown2Label.Location = new System.Drawing.Point(224, 25);
        	unknown2Label.Name = "unknown2Label";
        	unknown2Label.Size = new System.Drawing.Size(62, 13);
        	unknown2Label.TabIndex = 33;
        	unknown2Label.Text = "Unknown 2";
        	// 
        	// weaponUnknownLabel
        	// 
        	weaponUnknownLabel.AutoSize = true;
        	weaponUnknownLabel.Location = new System.Drawing.Point(221, 23);
        	weaponUnknownLabel.Name = "weaponUnknownLabel";
        	weaponUnknownLabel.Size = new System.Drawing.Size(53, 13);
        	weaponUnknownLabel.TabIndex = 29;
        	weaponUnknownLabel.Text = "Unknown";
        	// 
        	// hLabel8
        	// 
        	hLabel8.AutoSize = true;
        	hLabel8.Location = new System.Drawing.Point(353, 23);
        	hLabel8.Name = "hLabel8";
        	hLabel8.Size = new System.Drawing.Size(13, 13);
        	hLabel8.TabIndex = 34;
        	hLabel8.Text = "h";
        	// 
        	// hLabel4
        	// 
        	this.hLabel4.AutoSize = true;
        	this.hLabel4.Location = new System.Drawing.Point(162, 108);
        	this.hLabel4.Name = "hLabel4";
        	this.hLabel4.Size = new System.Drawing.Size(13, 13);
        	this.hLabel4.TabIndex = 16;
        	this.hLabel4.Text = "h";
        	// 
        	// weaponPanel
        	// 
        	this.weaponPanel.AutoSize = true;
        	this.weaponPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.weaponPanel.Controls.Add(hLabel8);
        	this.weaponPanel.Controls.Add(this.weaponUnknownSpinner);
        	this.weaponPanel.Controls.Add(weaponUnknownLabel);
        	this.weaponPanel.Controls.Add(this.formulasHelpLabel);
        	this.weaponPanel.Controls.Add(this.weaponFormulaComboBox);
        	this.weaponPanel.Controls.Add(this.weaponCastSpellLabel);
        	this.weaponPanel.Controls.Add(this.weaponSpellStatusLabel);
        	this.weaponPanel.Controls.Add(this.weaponAttributesCheckedListBox);
        	this.weaponPanel.Controls.Add(this.weaponElementsEditor);
        	this.weaponPanel.Controls.Add(evadeLabel);
        	this.weaponPanel.Controls.Add(weaponPowerLabel);
        	this.weaponPanel.Controls.Add(rangeLabel);
        	this.weaponPanel.Controls.Add(formulaLabel);
        	this.weaponPanel.Controls.Add(this.hLabel4);
        	this.weaponPanel.Controls.Add(this.weaponSpellStatusSpinner);
        	this.weaponPanel.Controls.Add(this.weaponEvadePercentageSpinner);
        	this.weaponPanel.Controls.Add(this.weaponPowerSpinner);
        	this.weaponPanel.Controls.Add(this.weaponRangeSpinner);
        	this.weaponPanel.Controls.Add(this.weaponCastSpellComboBox);
        	this.weaponPanel.Location = new System.Drawing.Point(4, 284);
        	this.weaponPanel.Name = "weaponPanel";
        	this.weaponPanel.Size = new System.Drawing.Size(482, 304);
        	this.weaponPanel.TabIndex = 12;
        	// 
        	// weaponUnknownSpinner
        	// 
        	this.weaponUnknownSpinner.Hexadecimal = true;
        	this.weaponUnknownSpinner.Location = new System.Drawing.Point(312, 21);
        	this.weaponUnknownSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.weaponUnknownSpinner.Name = "weaponUnknownSpinner";
        	this.weaponUnknownSpinner.Size = new System.Drawing.Size(41, 20);
        	this.weaponUnknownSpinner.TabIndex = 30;
        	this.weaponUnknownSpinner.Tag = "Unknown";
        	this.weaponUnknownSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// formulasHelpLabel
        	// 
        	this.formulasHelpLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        	this.formulasHelpLabel.Location = new System.Drawing.Point(216, 130);
        	this.formulasHelpLabel.Name = "formulasHelpLabel";
        	this.formulasHelpLabel.Size = new System.Drawing.Size(263, 174);
        	this.formulasHelpLabel.TabIndex = 28;
        	this.formulasHelpLabel.Text = resources.GetString("formulasHelpLabel.Text");
        	// 
        	// weaponFormulaComboBox
        	// 
        	this.weaponFormulaComboBox.BackColor = System.Drawing.SystemColors.Window;
        	this.weaponFormulaComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
        	this.weaponFormulaComboBox.FormattingEnabled = true;
        	this.weaponFormulaComboBox.Location = new System.Drawing.Point(121, 42);
        	this.weaponFormulaComboBox.Name = "weaponFormulaComboBox";
        	this.weaponFormulaComboBox.Size = new System.Drawing.Size(252, 21);
        	this.weaponFormulaComboBox.TabIndex = 1;
        	// 
        	// weaponCastSpellLabel
        	// 
        	this.weaponCastSpellLabel.AutoSize = true;
        	this.weaponCastSpellLabel.Location = new System.Drawing.Point(0, 108);
        	this.weaponCastSpellLabel.Name = "weaponCastSpellLabel";
        	this.weaponCastSpellLabel.Size = new System.Drawing.Size(34, 13);
        	this.weaponCastSpellLabel.TabIndex = 27;
        	this.weaponCastSpellLabel.Text = "Ability";
        	// 
        	// weaponSpellStatusLabel
        	// 
        	this.weaponSpellStatusLabel.AutoSize = true;
        	this.weaponSpellStatusLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
        	this.weaponSpellStatusLabel.Location = new System.Drawing.Point(0, 108);
        	this.weaponSpellStatusLabel.Name = "weaponSpellStatusLabel";
        	this.weaponSpellStatusLabel.Size = new System.Drawing.Size(117, 13);
        	this.weaponSpellStatusLabel.TabIndex = 23;
        	this.weaponSpellStatusLabel.TabStop = true;
        	this.weaponSpellStatusLabel.Text = "Inflict Status/Cast Spell";
        	// 
        	// weaponAttributesCheckedListBox
        	// 
        	this.weaponAttributesCheckedListBox.FormattingEnabled = true;
        	this.weaponAttributesCheckedListBox.Items.AddRange(new object[] {
        	        	        	"Striking",
        	        	        	"Lunging",
        	        	        	"Direct",
        	        	        	"Arc",
        	        	        	"2 Swords",
        	        	        	"2 Hands",
        	        	        	"Throwable",
        	        	        	"Forced 2 Hands"});
        	this.weaponAttributesCheckedListBox.Location = new System.Drawing.Point(103, 143);
        	this.weaponAttributesCheckedListBox.Name = "weaponAttributesCheckedListBox";
        	this.weaponAttributesCheckedListBox.Size = new System.Drawing.Size(107, 124);
        	this.weaponAttributesCheckedListBox.TabIndex = 5;
        	this.weaponAttributesCheckedListBox.TabStop = false;
        	// 
        	// weaponElementsEditor
        	// 
        	this.weaponElementsEditor.AutoSize = true;
        	this.weaponElementsEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.weaponElementsEditor.GroupBoxText = "Elements";
        	this.weaponElementsEditor.Location = new System.Drawing.Point(3, 124);
        	this.weaponElementsEditor.Name = "weaponElementsEditor";
        	this.weaponElementsEditor.Size = new System.Drawing.Size(94, 162);
        	this.weaponElementsEditor.TabIndex = 22;
        	this.weaponElementsEditor.TabStop = false;
        	// 
        	// weaponSpellStatusSpinner
        	// 
        	this.weaponSpellStatusSpinner.Hexadecimal = true;
        	this.weaponSpellStatusSpinner.Location = new System.Drawing.Point(121, 106);
        	this.weaponSpellStatusSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.weaponSpellStatusSpinner.Name = "weaponSpellStatusSpinner";
        	this.weaponSpellStatusSpinner.Size = new System.Drawing.Size(41, 20);
        	this.weaponSpellStatusSpinner.TabIndex = 4;
        	this.weaponSpellStatusSpinner.Tag = "InflictStatus";
        	this.weaponSpellStatusSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// weaponEvadePercentageSpinner
        	// 
        	this.weaponEvadePercentageSpinner.Location = new System.Drawing.Point(121, 85);
        	this.weaponEvadePercentageSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.weaponEvadePercentageSpinner.Name = "weaponEvadePercentageSpinner";
        	this.weaponEvadePercentageSpinner.Size = new System.Drawing.Size(41, 20);
        	this.weaponEvadePercentageSpinner.TabIndex = 3;
        	this.weaponEvadePercentageSpinner.Tag = "EvadePercentage";
        	this.weaponEvadePercentageSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// weaponPowerSpinner
        	// 
        	this.weaponPowerSpinner.Location = new System.Drawing.Point(121, 64);
        	this.weaponPowerSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.weaponPowerSpinner.Name = "weaponPowerSpinner";
        	this.weaponPowerSpinner.Size = new System.Drawing.Size(41, 20);
        	this.weaponPowerSpinner.TabIndex = 2;
        	this.weaponPowerSpinner.Tag = "WeaponPower";
        	this.weaponPowerSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// weaponRangeSpinner
        	// 
        	this.weaponRangeSpinner.Location = new System.Drawing.Point(121, 21);
        	this.weaponRangeSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.weaponRangeSpinner.Name = "weaponRangeSpinner";
        	this.weaponRangeSpinner.Size = new System.Drawing.Size(41, 20);
        	this.weaponRangeSpinner.TabIndex = 0;
        	this.weaponRangeSpinner.Tag = "Range";
        	this.weaponRangeSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// weaponCastSpellComboBox
        	// 
        	this.weaponCastSpellComboBox.BackColor = System.Drawing.SystemColors.Window;
        	this.weaponCastSpellComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
        	this.weaponCastSpellComboBox.FormattingEnabled = true;
        	this.weaponCastSpellComboBox.Location = new System.Drawing.Point(121, 106);
        	this.weaponCastSpellComboBox.Name = "weaponCastSpellComboBox";
        	this.weaponCastSpellComboBox.Size = new System.Drawing.Size(159, 21);
        	this.weaponCastSpellComboBox.TabIndex = 6;
        	// 
        	// shieldPanel
        	// 
        	this.shieldPanel.Controls.Add(percentLabel4);
        	this.shieldPanel.Controls.Add(percentLabel1);
        	this.shieldPanel.Controls.Add(percentLabel3);
        	this.shieldPanel.Controls.Add(percentLabel2);
        	this.shieldPanel.Controls.Add(this.shieldMagicBlockRateSpinner);
        	this.shieldPanel.Controls.Add(this.shieldPhysicalBlockRateSpinner);
        	this.shieldPanel.Location = new System.Drawing.Point(4, 284);
        	this.shieldPanel.Name = "shieldPanel";
        	this.shieldPanel.Size = new System.Drawing.Size(376, 167);
        	this.shieldPanel.TabIndex = 11;
        	// 
        	// shieldMagicBlockRateSpinner
        	// 
        	this.shieldMagicBlockRateSpinner.Location = new System.Drawing.Point(86, 24);
        	this.shieldMagicBlockRateSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.shieldMagicBlockRateSpinner.Name = "shieldMagicBlockRateSpinner";
        	this.shieldMagicBlockRateSpinner.Size = new System.Drawing.Size(42, 20);
        	this.shieldMagicBlockRateSpinner.TabIndex = 1;
        	this.shieldMagicBlockRateSpinner.Tag = "MagicBlockRate";
        	this.shieldMagicBlockRateSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// shieldPhysicalBlockRateSpinner
        	// 
        	this.shieldPhysicalBlockRateSpinner.Location = new System.Drawing.Point(86, 3);
        	this.shieldPhysicalBlockRateSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.shieldPhysicalBlockRateSpinner.Name = "shieldPhysicalBlockRateSpinner";
        	this.shieldPhysicalBlockRateSpinner.Size = new System.Drawing.Size(42, 20);
        	this.shieldPhysicalBlockRateSpinner.TabIndex = 0;
        	this.shieldPhysicalBlockRateSpinner.Tag = "PhysicalBlockRate";
        	this.shieldPhysicalBlockRateSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// armorPanel
        	// 
        	this.armorPanel.Controls.Add(mpBonusLabel);
        	this.armorPanel.Controls.Add(hpBonusLabel);
        	this.armorPanel.Controls.Add(this.armorMPBonusSpinner);
        	this.armorPanel.Controls.Add(this.armorHPBonusSpinner);
        	this.armorPanel.Location = new System.Drawing.Point(4, 284);
        	this.armorPanel.Name = "armorPanel";
        	this.armorPanel.Size = new System.Drawing.Size(376, 167);
        	this.armorPanel.TabIndex = 10;
        	// 
        	// armorMPBonusSpinner
        	// 
        	this.armorMPBonusSpinner.Location = new System.Drawing.Point(85, 24);
        	this.armorMPBonusSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.armorMPBonusSpinner.Name = "armorMPBonusSpinner";
        	this.armorMPBonusSpinner.Size = new System.Drawing.Size(42, 20);
        	this.armorMPBonusSpinner.TabIndex = 1;
        	this.armorMPBonusSpinner.Tag = "MPBonus";
        	this.armorMPBonusSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// armorHPBonusSpinner
        	// 
        	this.armorHPBonusSpinner.Location = new System.Drawing.Point(85, 3);
        	this.armorHPBonusSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.armorHPBonusSpinner.Name = "armorHPBonusSpinner";
        	this.armorHPBonusSpinner.Size = new System.Drawing.Size(42, 20);
        	this.armorHPBonusSpinner.TabIndex = 0;
        	this.armorHPBonusSpinner.Tag = "HPBonus";
        	this.armorHPBonusSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// accessoryPanel
        	// 
        	this.accessoryPanel.Controls.Add(percentLabel6);
        	this.accessoryPanel.Controls.Add(percentLabel5);
        	this.accessoryPanel.Controls.Add(magicEvadeRateLabel);
        	this.accessoryPanel.Controls.Add(physicalEvadeRateLabel);
        	this.accessoryPanel.Controls.Add(this.accessoryMagicEvadeRateSpinner);
        	this.accessoryPanel.Controls.Add(this.accessoryPhysicalEvadeRateSpinner);
        	this.accessoryPanel.Location = new System.Drawing.Point(4, 284);
        	this.accessoryPanel.Name = "accessoryPanel";
        	this.accessoryPanel.Size = new System.Drawing.Size(376, 167);
        	this.accessoryPanel.TabIndex = 9;
        	// 
        	// accessoryMagicEvadeRateSpinner
        	// 
        	this.accessoryMagicEvadeRateSpinner.Location = new System.Drawing.Point(86, 24);
        	this.accessoryMagicEvadeRateSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.accessoryMagicEvadeRateSpinner.Name = "accessoryMagicEvadeRateSpinner";
        	this.accessoryMagicEvadeRateSpinner.Size = new System.Drawing.Size(42, 20);
        	this.accessoryMagicEvadeRateSpinner.TabIndex = 1;
        	this.accessoryMagicEvadeRateSpinner.Tag = "MagicEvade";
        	this.accessoryMagicEvadeRateSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// accessoryPhysicalEvadeRateSpinner
        	// 
        	this.accessoryPhysicalEvadeRateSpinner.Location = new System.Drawing.Point(86, 3);
        	this.accessoryPhysicalEvadeRateSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.accessoryPhysicalEvadeRateSpinner.Name = "accessoryPhysicalEvadeRateSpinner";
        	this.accessoryPhysicalEvadeRateSpinner.Size = new System.Drawing.Size(42, 20);
        	this.accessoryPhysicalEvadeRateSpinner.TabIndex = 0;
        	this.accessoryPhysicalEvadeRateSpinner.Tag = "PhysicalEvade";
        	this.accessoryPhysicalEvadeRateSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// chemistItemPanel
        	// 
        	this.chemistItemPanel.Controls.Add(this.chemistItemFormulaComboBox);
        	this.chemistItemPanel.Controls.Add(this.chemistItemSpellStatusLabel);
        	this.chemistItemPanel.Controls.Add(this.chemistItemSpellStatusSpinner);
        	this.chemistItemPanel.Controls.Add(zLabel);
        	this.chemistItemPanel.Controls.Add(itemFormulaLabel);
        	this.chemistItemPanel.Controls.Add(this.chemistItemXSpinner);
        	this.chemistItemPanel.Location = new System.Drawing.Point(4, 284);
        	this.chemistItemPanel.Name = "chemistItemPanel";
        	this.chemistItemPanel.Size = new System.Drawing.Size(376, 167);
        	this.chemistItemPanel.TabIndex = 8;
        	// 
        	// chemistItemFormulaComboBox
        	// 
        	this.chemistItemFormulaComboBox.BackColor = System.Drawing.SystemColors.Window;
        	this.chemistItemFormulaComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
        	this.chemistItemFormulaComboBox.FormattingEnabled = true;
        	this.chemistItemFormulaComboBox.Location = new System.Drawing.Point(86, 3);
        	this.chemistItemFormulaComboBox.Name = "chemistItemFormulaComboBox";
        	this.chemistItemFormulaComboBox.Size = new System.Drawing.Size(211, 21);
        	this.chemistItemFormulaComboBox.TabIndex = 0;
        	// 
        	// chemistItemSpellStatusLabel
        	// 
        	this.chemistItemSpellStatusLabel.AutoSize = true;
        	this.chemistItemSpellStatusLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
        	this.chemistItemSpellStatusLabel.Location = new System.Drawing.Point(4, 49);
        	this.chemistItemSpellStatusLabel.Name = "chemistItemSpellStatusLabel";
        	this.chemistItemSpellStatusLabel.Size = new System.Drawing.Size(65, 13);
        	this.chemistItemSpellStatusLabel.TabIndex = 6;
        	this.chemistItemSpellStatusLabel.TabStop = true;
        	this.chemistItemSpellStatusLabel.Text = "Inflict Status";
        	// 
        	// chemistItemSpellStatusSpinner
        	// 
        	this.chemistItemSpellStatusSpinner.Hexadecimal = true;
        	this.chemistItemSpellStatusSpinner.Location = new System.Drawing.Point(86, 46);
        	this.chemistItemSpellStatusSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.chemistItemSpellStatusSpinner.Name = "chemistItemSpellStatusSpinner";
        	this.chemistItemSpellStatusSpinner.Size = new System.Drawing.Size(42, 20);
        	this.chemistItemSpellStatusSpinner.TabIndex = 2;
        	this.chemistItemSpellStatusSpinner.Tag = "InflictStatus";
        	this.chemistItemSpellStatusSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// chemistItemXSpinner
        	// 
        	this.chemistItemXSpinner.Location = new System.Drawing.Point(86, 25);
        	this.chemistItemXSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.chemistItemXSpinner.Name = "chemistItemXSpinner";
        	this.chemistItemXSpinner.Size = new System.Drawing.Size(42, 20);
        	this.chemistItemXSpinner.TabIndex = 1;
        	this.chemistItemXSpinner.Tag = "X";
        	this.chemistItemXSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// itemAttributesLabel
        	// 
        	this.itemAttributesLabel.AutoSize = true;
        	this.itemAttributesLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
        	this.itemAttributesLabel.Location = new System.Drawing.Point(4, 89);
        	this.itemAttributesLabel.Name = "itemAttributesLabel";
        	this.itemAttributesLabel.Size = new System.Drawing.Size(74, 13);
        	this.itemAttributesLabel.TabIndex = 17;
        	this.itemAttributesLabel.TabStop = true;
        	this.itemAttributesLabel.Text = "Item Attributes";
        	// 
        	// secondTableLabel
        	// 
        	this.secondTableLabel.AutoSize = true;
        	this.secondTableLabel.Location = new System.Drawing.Point(306, 132);
        	this.secondTableLabel.Name = "secondTableLabel";
        	this.secondTableLabel.Size = new System.Drawing.Size(74, 13);
        	this.secondTableLabel.TabIndex = 21;
        	this.secondTableLabel.Text = "Second Table";
        	// 
        	// weaponJumpLabel
        	// 
        	this.weaponJumpLabel.AutoSize = true;
        	this.weaponJumpLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
        	this.weaponJumpLabel.Location = new System.Drawing.Point(383, 153);
        	this.weaponJumpLabel.Name = "weaponJumpLabel";
        	this.weaponJumpLabel.Size = new System.Drawing.Size(88, 13);
        	this.weaponJumpLabel.TabIndex = 22;
        	this.weaponJumpLabel.TabStop = true;
        	this.weaponJumpLabel.Text = "Jump to Weapon";
        	// 
        	// shieldJumpLabel
        	// 
        	this.shieldJumpLabel.AutoSize = true;
        	this.shieldJumpLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
        	this.shieldJumpLabel.Location = new System.Drawing.Point(383, 172);
        	this.shieldJumpLabel.Name = "shieldJumpLabel";
        	this.shieldJumpLabel.Size = new System.Drawing.Size(76, 13);
        	this.shieldJumpLabel.TabIndex = 23;
        	this.shieldJumpLabel.TabStop = true;
        	this.shieldJumpLabel.Text = "Jump to Shield";
        	// 
        	// headBodyJumpLabel
        	// 
        	this.headBodyJumpLabel.AutoSize = true;
        	this.headBodyJumpLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
        	this.headBodyJumpLabel.Location = new System.Drawing.Point(383, 191);
        	this.headBodyJumpLabel.Name = "headBodyJumpLabel";
        	this.headBodyJumpLabel.Size = new System.Drawing.Size(102, 13);
        	this.headBodyJumpLabel.TabIndex = 24;
        	this.headBodyJumpLabel.TabStop = true;
        	this.headBodyJumpLabel.Text = "Jump to Head/Body";
        	// 
        	// accessoryJumpLabel
        	// 
        	this.accessoryJumpLabel.AutoSize = true;
        	this.accessoryJumpLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
        	this.accessoryJumpLabel.Location = new System.Drawing.Point(383, 210);
        	this.accessoryJumpLabel.Name = "accessoryJumpLabel";
        	this.accessoryJumpLabel.Size = new System.Drawing.Size(96, 13);
        	this.accessoryJumpLabel.TabIndex = 25;
        	this.accessoryJumpLabel.TabStop = true;
        	this.accessoryJumpLabel.Text = "Jump to Accessory";
        	// 
        	// chemistItemJumpLabel
        	// 
        	this.chemistItemJumpLabel.AutoSize = true;
        	this.chemistItemJumpLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
        	this.chemistItemJumpLabel.Location = new System.Drawing.Point(383, 229);
        	this.chemistItemJumpLabel.Name = "chemistItemJumpLabel";
        	this.chemistItemJumpLabel.Size = new System.Drawing.Size(107, 13);
        	this.chemistItemJumpLabel.TabIndex = 26;
        	this.chemistItemJumpLabel.TabStop = true;
        	this.chemistItemJumpLabel.Text = "Jump to Chemist Item";
        	// 
        	// secondTableIdSpinner
        	// 
        	this.secondTableIdSpinner.Hexadecimal = true;
        	this.secondTableIdSpinner.Location = new System.Drawing.Point(386, 130);
        	this.secondTableIdSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.secondTableIdSpinner.Name = "secondTableIdSpinner";
        	this.secondTableIdSpinner.Size = new System.Drawing.Size(41, 20);
        	this.secondTableIdSpinner.TabIndex = 8;
        	this.secondTableIdSpinner.Tag = "SecondTableId";
        	this.secondTableIdSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// itemAttributesCheckedListBox
        	// 
        	this.itemAttributesCheckedListBox.FormattingEnabled = true;
        	this.itemAttributesCheckedListBox.Items.AddRange(new object[] {
        	        	        	"Weapon",
        	        	        	"Shield",
        	        	        	"Head",
        	        	        	"Body",
        	        	        	"Accessory",
        	        	        	"",
        	        	        	"Rare",
        	        	        	""});
        	this.itemAttributesCheckedListBox.Location = new System.Drawing.Point(372, 3);
        	this.itemAttributesCheckedListBox.Name = "itemAttributesCheckedListBox";
        	this.itemAttributesCheckedListBox.Size = new System.Drawing.Size(107, 124);
        	this.itemAttributesCheckedListBox.TabIndex = 7;
        	this.itemAttributesCheckedListBox.TabStop = false;
        	// 
        	// shopAvailabilityComboBox
        	// 
        	this.shopAvailabilityComboBox.BackColor = System.Drawing.SystemColors.Window;
        	this.shopAvailabilityComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
        	this.shopAvailabilityComboBox.FormattingEnabled = true;
        	this.shopAvailabilityComboBox.Location = new System.Drawing.Point(90, 129);
        	this.shopAvailabilityComboBox.Name = "shopAvailabilityComboBox";
        	this.shopAvailabilityComboBox.Size = new System.Drawing.Size(174, 21);
        	this.shopAvailabilityComboBox.TabIndex = 6;
        	this.shopAvailabilityComboBox.Tag = "ShopAvailability";
        	// 
        	// priceSpinner
        	// 
        	this.priceSpinner.Location = new System.Drawing.Point(90, 108);
        	this.priceSpinner.Maximum = new decimal(new int[] {
        	        	        	65535,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.priceSpinner.Name = "priceSpinner";
        	this.priceSpinner.Size = new System.Drawing.Size(54, 20);
        	this.priceSpinner.TabIndex = 5;
        	this.priceSpinner.Tag = "Price";
        	this.priceSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// itemTypeComboBox
        	// 
        	this.itemTypeComboBox.BackColor = System.Drawing.SystemColors.Window;
        	this.itemTypeComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
        	this.itemTypeComboBox.FormattingEnabled = true;
        	this.itemTypeComboBox.Location = new System.Drawing.Point(90, 65);
        	this.itemTypeComboBox.Name = "itemTypeComboBox";
        	this.itemTypeComboBox.Size = new System.Drawing.Size(121, 21);
        	this.itemTypeComboBox.TabIndex = 3;
        	this.itemTypeComboBox.Tag = "ItemType";
        	// 
        	// enemyLevelSpinner
        	// 
        	this.enemyLevelSpinner.Location = new System.Drawing.Point(90, 44);
        	this.enemyLevelSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.enemyLevelSpinner.Name = "enemyLevelSpinner";
        	this.enemyLevelSpinner.Size = new System.Drawing.Size(41, 20);
        	this.enemyLevelSpinner.TabIndex = 2;
        	this.enemyLevelSpinner.Tag = "EnemyLevel";
        	this.enemyLevelSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// itemAttributesSpinner
        	// 
        	this.itemAttributesSpinner.Hexadecimal = true;
        	this.itemAttributesSpinner.Location = new System.Drawing.Point(90, 87);
        	this.itemAttributesSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.itemAttributesSpinner.Name = "itemAttributesSpinner";
        	this.itemAttributesSpinner.Size = new System.Drawing.Size(41, 20);
        	this.itemAttributesSpinner.TabIndex = 4;
        	this.itemAttributesSpinner.Tag = "SIA";
        	this.itemAttributesSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// graphicSpinner
        	// 
        	this.graphicSpinner.Hexadecimal = true;
        	this.graphicSpinner.Location = new System.Drawing.Point(90, 23);
        	this.graphicSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.graphicSpinner.Name = "graphicSpinner";
        	this.graphicSpinner.Size = new System.Drawing.Size(41, 20);
        	this.graphicSpinner.TabIndex = 1;
        	this.graphicSpinner.Tag = "Graphic";
        	this.graphicSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// paletteSpinner
        	// 
        	this.paletteSpinner.Hexadecimal = true;
        	this.paletteSpinner.Location = new System.Drawing.Point(90, 2);
        	this.paletteSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.paletteSpinner.Name = "paletteSpinner";
        	this.paletteSpinner.Size = new System.Drawing.Size(41, 20);
        	this.paletteSpinner.TabIndex = 0;
        	this.paletteSpinner.Tag = "Palette";
        	this.paletteSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// storeInventoryCheckedListBox
        	// 
        	this.storeInventoryCheckedListBox.ColumnWidth = 165;
        	this.storeInventoryCheckedListBox.FormattingEnabled = true;
        	this.storeInventoryCheckedListBox.Location = new System.Drawing.Point(7, 156);
        	this.storeInventoryCheckedListBox.MultiColumn = true;
        	this.storeInventoryCheckedListBox.Name = "storeInventoryCheckedListBox";
        	this.storeInventoryCheckedListBox.Size = new System.Drawing.Size(370, 124);
        	this.storeInventoryCheckedListBox.TabIndex = 27;
        	// 
        	// unknown1Spinner
        	// 
        	this.unknown1Spinner.Hexadecimal = true;
        	this.unknown1Spinner.Location = new System.Drawing.Point(292, 2);
        	this.unknown1Spinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.unknown1Spinner.Name = "unknown1Spinner";
        	this.unknown1Spinner.Size = new System.Drawing.Size(41, 20);
        	this.unknown1Spinner.TabIndex = 28;
        	this.unknown1Spinner.Tag = "Unknown1";
        	this.unknown1Spinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// unknown2Spinner
        	// 
        	this.unknown2Spinner.Hexadecimal = true;
        	this.unknown2Spinner.Location = new System.Drawing.Point(292, 23);
        	this.unknown2Spinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.unknown2Spinner.Name = "unknown2Spinner";
        	this.unknown2Spinner.Size = new System.Drawing.Size(41, 20);
        	this.unknown2Spinner.TabIndex = 29;
        	this.unknown2Spinner.Tag = "Unknown2";
        	this.unknown2Spinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// ItemEditor
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.AutoSize = true;
        	this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.Controls.Add(unknown2Label);
        	this.Controls.Add(unknown1Label);
        	this.Controls.Add(hLabel7);
        	this.Controls.Add(hLabel6);
        	this.Controls.Add(this.unknown2Spinner);
        	this.Controls.Add(this.unknown1Spinner);
        	this.Controls.Add(this.storeInventoryCheckedListBox);
        	this.Controls.Add(this.chemistItemJumpLabel);
        	this.Controls.Add(this.accessoryJumpLabel);
        	this.Controls.Add(this.headBodyJumpLabel);
        	this.Controls.Add(this.shieldJumpLabel);
        	this.Controls.Add(this.weaponJumpLabel);
        	this.Controls.Add(this.secondTableLabel);
        	this.Controls.Add(label1);
        	this.Controls.Add(this.secondTableIdSpinner);
        	this.Controls.Add(this.itemAttributesLabel);
        	this.Controls.Add(this.itemAttributesCheckedListBox);
        	this.Controls.Add(shopAvailabilityLabel);
        	this.Controls.Add(priceLabel);
        	this.Controls.Add(itemIconLabel);
        	this.Controls.Add(this.shopAvailabilityComboBox);
        	this.Controls.Add(this.priceSpinner);
        	this.Controls.Add(this.itemTypeComboBox);
        	this.Controls.Add(enemyLevelLabel);
        	this.Controls.Add(this.enemyLevelSpinner);
        	this.Controls.Add(graphicLabel);
        	this.Controls.Add(paletteLabel);
        	this.Controls.Add(hLabel2);
        	this.Controls.Add(this.itemAttributesSpinner);
        	this.Controls.Add(hLabel1);
        	this.Controls.Add(this.graphicSpinner);
        	this.Controls.Add(hLabel5);
        	this.Controls.Add(this.paletteSpinner);
        	this.Controls.Add(this.weaponPanel);
        	this.Controls.Add(this.chemistItemPanel);
        	this.Controls.Add(this.accessoryPanel);
        	this.Controls.Add(this.armorPanel);
        	this.Controls.Add(this.shieldPanel);
        	this.Name = "ItemEditor";
        	this.Size = new System.Drawing.Size(493, 591);
        	this.weaponPanel.ResumeLayout(false);
        	this.weaponPanel.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.weaponUnknownSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.weaponSpellStatusSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.weaponEvadePercentageSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.weaponPowerSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.weaponRangeSpinner)).EndInit();
        	this.shieldPanel.ResumeLayout(false);
        	this.shieldPanel.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.shieldMagicBlockRateSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.shieldPhysicalBlockRateSpinner)).EndInit();
        	this.armorPanel.ResumeLayout(false);
        	this.armorPanel.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.armorMPBonusSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.armorHPBonusSpinner)).EndInit();
        	this.accessoryPanel.ResumeLayout(false);
        	this.accessoryPanel.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.accessoryMagicEvadeRateSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.accessoryPhysicalEvadeRateSpinner)).EndInit();
        	this.chemistItemPanel.ResumeLayout(false);
        	this.chemistItemPanel.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.chemistItemSpellStatusSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.chemistItemXSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.secondTableIdSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.priceSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.enemyLevelSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.itemAttributesSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.graphicSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.paletteSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.unknown1Spinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.unknown2Spinner)).EndInit();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private FFTPatcher.Controls.NumericUpDownWithDefault weaponUnknownSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault unknown2Spinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault unknown1Spinner;

        #endregion

        private FFTPatcher.Controls.NumericUpDownWithDefault paletteSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault graphicSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault itemAttributesSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault enemyLevelSpinner;
        private FFTPatcher.Controls.ComboBoxWithDefault itemTypeComboBox;
        private FFTPatcher.Controls.NumericUpDownWithDefault priceSpinner;
        private FFTPatcher.Controls.ComboBoxWithDefault shopAvailabilityComboBox;
        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault itemAttributesCheckedListBox;
        private FFTPatcher.Controls.NumericUpDownWithDefault weaponEvadePercentageSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault weaponPowerSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault weaponRangeSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault weaponSpellStatusSpinner;
        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault weaponAttributesCheckedListBox;
        private ElementsEditor weaponElementsEditor;
        private System.Windows.Forms.Panel shieldPanel;
        private FFTPatcher.Controls.NumericUpDownWithDefault shieldMagicBlockRateSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault shieldPhysicalBlockRateSpinner;
        private System.Windows.Forms.Panel armorPanel;
        private FFTPatcher.Controls.NumericUpDownWithDefault armorMPBonusSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault armorHPBonusSpinner;
        private System.Windows.Forms.Panel accessoryPanel;
        private FFTPatcher.Controls.NumericUpDownWithDefault accessoryMagicEvadeRateSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault accessoryPhysicalEvadeRateSpinner;
        private System.Windows.Forms.Panel chemistItemPanel;
        private FFTPatcher.Controls.NumericUpDownWithDefault chemistItemXSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault chemistItemSpellStatusSpinner;
        private System.Windows.Forms.Panel weaponPanel;
        private System.Windows.Forms.LinkLabel chemistItemSpellStatusLabel;
        private System.Windows.Forms.LinkLabel itemAttributesLabel;
        private System.Windows.Forms.LinkLabel weaponSpellStatusLabel;
        private FFTPatcher.Controls.ComboBoxWithDefault weaponFormulaComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault chemistItemFormulaComboBox;
        private FFTPatcher.Controls.NumericUpDownWithDefault secondTableIdSpinner;
        private System.Windows.Forms.Label secondTableLabel;
        private System.Windows.Forms.LinkLabel weaponJumpLabel;
        private System.Windows.Forms.LinkLabel shieldJumpLabel;
        private System.Windows.Forms.LinkLabel headBodyJumpLabel;
        private System.Windows.Forms.LinkLabel accessoryJumpLabel;
        private System.Windows.Forms.LinkLabel chemistItemJumpLabel;
        private System.Windows.Forms.Label weaponCastSpellLabel;
        private System.Windows.Forms.Label hLabel4;
        private FFTPatcher.Controls.ComboBoxWithDefault weaponCastSpellComboBox;
        private System.Windows.Forms.Label formulasHelpLabel;
        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault storeInventoryCheckedListBox;

    }
}
