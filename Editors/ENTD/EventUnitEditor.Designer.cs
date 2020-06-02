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
    partial class EventUnitEditor
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
            System.Windows.Forms.GroupBox spoilsGroupBox;
            System.Windows.Forms.Label x100GilLabel;
            System.Windows.Forms.Label warTrophyLabel;
            System.Windows.Forms.Label bonusMoneyLabel;
            System.Windows.Forms.Label teamColorLabel;
            System.Windows.Forms.Label hLabel2;
            System.Windows.Forms.Label hLabel1;
            System.Windows.Forms.Label initialDirectionLabel;
            System.Windows.Forms.Label jobsUnlockedLabel;
            System.Windows.Forms.Label unitIdLabel;
            System.Windows.Forms.Label jobLabel;
            System.Windows.Forms.Label unitLabel;
            System.Windows.Forms.Label braveryLabel;
            System.Windows.Forms.Label faithLabel;
            System.Windows.Forms.Label birthdayLabel;
            System.Windows.Forms.GroupBox aiGroupBox;
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label levelLabel;
            System.Windows.Forms.Label yLabel;
            System.Windows.Forms.Label paletteLabel;
            System.Windows.Forms.GroupBox equipmentGroupBox;
            System.Windows.Forms.Label accessoryLabel;
            System.Windows.Forms.Label bodyLabel;
            System.Windows.Forms.Label headLabel;
            System.Windows.Forms.Label leftHandLabel;
            System.Windows.Forms.Label rightHandLabel;
            System.Windows.Forms.Label xLabel;
            System.Windows.Forms.GroupBox skillsGroupBox;
            System.Windows.Forms.Label movementSkillLabel;
            System.Windows.Forms.Label supportSkillLabel;
            System.Windows.Forms.Label reactionSkillLabel;
            System.Windows.Forms.Label secondarySkillLabel;
            System.Windows.Forms.Label primarySkillLabel;
            this.bonusMoneySpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.warTrophyComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.clbAIFlags2 = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
            this.clbAIFlags1 = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
            this.labelTargetY = new System.Windows.Forms.Label();
            this.labelTargetX = new System.Windows.Forms.Label();
            this.targetLabel = new System.Windows.Forms.Label();
            this.targetYSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.targetXSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.targetSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.rightHandComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.accessoryComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.leftHandComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.bodyComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.headComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.primarySkillComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.movementComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.secondaryActionComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.supportComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.reactionComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.unknownGroupBox = new System.Windows.Forms.GroupBox();
            this.unknown12Spinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.unknown10Spinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.upperLevelCheckBox = new System.Windows.Forms.CheckBox();
            this.teamColorComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.facingDirectionComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.levelComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.dayComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.faithComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.braveryComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.preRequisiteJobComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.flags2CheckedListBox = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
            this.spriteSetComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.jobComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.unitIDSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.specialNameComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.flags1CheckedListBox = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
            this.jobLevelSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.monthComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.paletteSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.ySpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.xSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.experienceLabel = new System.Windows.Forms.Label();
            this.experienceComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            spoilsGroupBox = new System.Windows.Forms.GroupBox();
            x100GilLabel = new System.Windows.Forms.Label();
            warTrophyLabel = new System.Windows.Forms.Label();
            bonusMoneyLabel = new System.Windows.Forms.Label();
            teamColorLabel = new System.Windows.Forms.Label();
            hLabel2 = new System.Windows.Forms.Label();
            hLabel1 = new System.Windows.Forms.Label();
            initialDirectionLabel = new System.Windows.Forms.Label();
            jobsUnlockedLabel = new System.Windows.Forms.Label();
            unitIdLabel = new System.Windows.Forms.Label();
            jobLabel = new System.Windows.Forms.Label();
            unitLabel = new System.Windows.Forms.Label();
            braveryLabel = new System.Windows.Forms.Label();
            faithLabel = new System.Windows.Forms.Label();
            birthdayLabel = new System.Windows.Forms.Label();
            aiGroupBox = new System.Windows.Forms.GroupBox();
            nameLabel = new System.Windows.Forms.Label();
            levelLabel = new System.Windows.Forms.Label();
            yLabel = new System.Windows.Forms.Label();
            paletteLabel = new System.Windows.Forms.Label();
            equipmentGroupBox = new System.Windows.Forms.GroupBox();
            accessoryLabel = new System.Windows.Forms.Label();
            bodyLabel = new System.Windows.Forms.Label();
            headLabel = new System.Windows.Forms.Label();
            leftHandLabel = new System.Windows.Forms.Label();
            rightHandLabel = new System.Windows.Forms.Label();
            xLabel = new System.Windows.Forms.Label();
            skillsGroupBox = new System.Windows.Forms.GroupBox();
            movementSkillLabel = new System.Windows.Forms.Label();
            supportSkillLabel = new System.Windows.Forms.Label();
            reactionSkillLabel = new System.Windows.Forms.Label();
            secondarySkillLabel = new System.Windows.Forms.Label();
            primarySkillLabel = new System.Windows.Forms.Label();
            spoilsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bonusMoneySpinner)).BeginInit();
            aiGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targetYSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetXSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetSpinner)).BeginInit();
            equipmentGroupBox.SuspendLayout();
            skillsGroupBox.SuspendLayout();
            this.unknownGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unknown12Spinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown10Spinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitIDSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.jobLevelSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ySpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // spoilsGroupBox
            // 
            spoilsGroupBox.AutoSize = true;
            spoilsGroupBox.Controls.Add(x100GilLabel);
            spoilsGroupBox.Controls.Add(warTrophyLabel);
            spoilsGroupBox.Controls.Add(bonusMoneyLabel);
            spoilsGroupBox.Controls.Add(this.bonusMoneySpinner);
            spoilsGroupBox.Controls.Add(this.warTrophyComboBox);
            spoilsGroupBox.Location = new System.Drawing.Point(211, 226);
            spoilsGroupBox.Name = "spoilsGroupBox";
            spoilsGroupBox.Size = new System.Drawing.Size(211, 103);
            spoilsGroupBox.TabIndex = 42;
            spoilsGroupBox.TabStop = false;
            spoilsGroupBox.Text = "Spoils";
            // 
            // x100GilLabel
            // 
            x100GilLabel.AutoSize = true;
            x100GilLabel.Location = new System.Drawing.Point(142, 25);
            x100GilLabel.Name = "x100GilLabel";
            x100GilLabel.Size = new System.Drawing.Size(48, 13);
            x100GilLabel.TabIndex = 44;
            x100GilLabel.Text = "x 100 Gil";
            // 
            // warTrophyLabel
            // 
            warTrophyLabel.AutoSize = true;
            warTrophyLabel.Location = new System.Drawing.Point(7, 47);
            warTrophyLabel.Name = "warTrophyLabel";
            warTrophyLabel.Size = new System.Drawing.Size(63, 13);
            warTrophyLabel.TabIndex = 43;
            warTrophyLabel.Text = "War Trophy";
            // 
            // bonusMoneyLabel
            // 
            bonusMoneyLabel.AutoSize = true;
            bonusMoneyLabel.Location = new System.Drawing.Point(7, 25);
            bonusMoneyLabel.Name = "bonusMoneyLabel";
            bonusMoneyLabel.Size = new System.Drawing.Size(72, 13);
            bonusMoneyLabel.TabIndex = 42;
            bonusMoneyLabel.Text = "Bonus Money";
            // 
            // bonusMoneySpinner
            // 
            this.bonusMoneySpinner.Location = new System.Drawing.Point(84, 23);
            this.bonusMoneySpinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.bonusMoneySpinner.Name = "bonusMoneySpinner";
            this.bonusMoneySpinner.Size = new System.Drawing.Size(52, 20);
            this.bonusMoneySpinner.TabIndex = 5;
            this.bonusMoneySpinner.Tag = "BonusMoney";
            this.bonusMoneySpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // warTrophyComboBox
            // 
            this.warTrophyComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.warTrophyComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.warTrophyComboBox.FormattingEnabled = true;
            this.warTrophyComboBox.Location = new System.Drawing.Point(84, 44);
            this.warTrophyComboBox.Name = "warTrophyComboBox";
            this.warTrophyComboBox.Size = new System.Drawing.Size(121, 21);
            this.warTrophyComboBox.TabIndex = 41;
            this.warTrophyComboBox.Tag = "WarTrophy";
            // 
            // teamColorLabel
            // 
            teamColorLabel.AutoSize = true;
            teamColorLabel.Location = new System.Drawing.Point(292, 136);
            teamColorLabel.Name = "teamColorLabel";
            teamColorLabel.Size = new System.Drawing.Size(61, 13);
            teamColorLabel.TabIndex = 40;
            teamColorLabel.Text = "Team";
            // 
            // hLabel2
            // 
            hLabel2.AutoSize = true;
            hLabel2.Location = new System.Drawing.Point(128, 139);
            hLabel2.Name = "hLabel2";
            hLabel2.Size = new System.Drawing.Size(13, 13);
            hLabel2.TabIndex = 38;
            hLabel2.Text = "h";
            // 
            // hLabel1
            // 
            hLabel1.AutoSize = true;
            hLabel1.Location = new System.Drawing.Point(256, 139);
            hLabel1.Name = "hLabel1";
            hLabel1.Size = new System.Drawing.Size(13, 13);
            hLabel1.TabIndex = 37;
            hLabel1.Text = "h";
            // 
            // initialDirectionLabel
            // 
            initialDirectionLabel.AutoSize = true;
            initialDirectionLabel.Location = new System.Drawing.Point(2, 202);
            initialDirectionLabel.Name = "initialDirectionLabel";
            initialDirectionLabel.Size = new System.Drawing.Size(74, 13);
            initialDirectionLabel.TabIndex = 36;
            initialDirectionLabel.Text = "Initial Direction";
            // 
            // jobsUnlockedLabel
            // 
            jobsUnlockedLabel.AutoSize = true;
            jobsUnlockedLabel.Location = new System.Drawing.Point(2, 180);
            jobsUnlockedLabel.Name = "jobsUnlockedLabel";
            jobsUnlockedLabel.Size = new System.Drawing.Size(76, 13);
            jobsUnlockedLabel.TabIndex = 34;
            jobsUnlockedLabel.Text = "Jobs Unlocked";
            // 
            // unitIdLabel
            // 
            unitIdLabel.AutoSize = true;
            unitIdLabel.Location = new System.Drawing.Point(160, 137);
            unitIdLabel.Name = "unitIdLabel";
            unitIdLabel.Size = new System.Drawing.Size(40, 13);
            unitIdLabel.TabIndex = 32;
            unitIdLabel.Text = "Unit ID";
            // 
            // jobLabel
            // 
            jobLabel.AutoSize = true;
            jobLabel.Location = new System.Drawing.Point(2, 72);
            jobLabel.Name = "jobLabel";
            jobLabel.Size = new System.Drawing.Size(24, 13);
            jobLabel.TabIndex = 31;
            jobLabel.Text = "Current Job";
            // 
            // unitLabel
            // 
            unitLabel.AutoSize = true;
            unitLabel.Location = new System.Drawing.Point(2, 6);
            unitLabel.Name = "unitLabel";
            unitLabel.Size = new System.Drawing.Size(53, 13);
            unitLabel.TabIndex = 17;
            unitLabel.Text = "Unit";
            // 
            // braveryLabel
            // 
            braveryLabel.AutoSize = true;
            braveryLabel.Location = new System.Drawing.Point(160, 117);
            braveryLabel.Name = "braveryLabel";
            braveryLabel.Size = new System.Drawing.Size(43, 13);
            braveryLabel.TabIndex = 20;
            braveryLabel.Text = "Bravery";
            // 
            // faithLabel
            // 
            faithLabel.AutoSize = true;
            faithLabel.Location = new System.Drawing.Point(2, 117);
            faithLabel.Name = "faithLabel";
            faithLabel.Size = new System.Drawing.Size(30, 13);
            faithLabel.TabIndex = 21;
            faithLabel.Text = "Faith";
            // 
            // birthdayLabel
            // 
            birthdayLabel.AutoSize = true;
            birthdayLabel.Location = new System.Drawing.Point(2, 49);
            birthdayLabel.Name = "birthdayLabel";
            birthdayLabel.Size = new System.Drawing.Size(49, 13);
            birthdayLabel.TabIndex = 19;
            birthdayLabel.Text = "Birthdate";
            // 
            // aiGroupBox
            // 
            aiGroupBox.Controls.Add(this.clbAIFlags2);
            aiGroupBox.Controls.Add(this.clbAIFlags1);
            aiGroupBox.Controls.Add(this.labelTargetY);
            aiGroupBox.Controls.Add(this.labelTargetX);
            aiGroupBox.Controls.Add(this.targetLabel);
            aiGroupBox.Controls.Add(this.targetYSpinner);
            aiGroupBox.Controls.Add(this.targetXSpinner);
            aiGroupBox.Controls.Add(this.targetSpinner);
            aiGroupBox.Location = new System.Drawing.Point(211, 346);
            aiGroupBox.Name = "aiGroupBox";
            aiGroupBox.Size = new System.Drawing.Size(367, 162);
            aiGroupBox.TabIndex = 19;
            aiGroupBox.TabStop = false;
            aiGroupBox.Text = "AI";
            // 
            // clbAIFlags2
            // 
            this.clbAIFlags2.FormattingEnabled = true;
            this.clbAIFlags2.Items.AddRange(new object[] {
            "",
            "",
            "",
            "",
            "",
            "Save CT",
            "",
            ""});
            this.clbAIFlags2.Location = new System.Drawing.Point(252, 23);
            this.clbAIFlags2.Name = "clbAIFlags2";
            this.clbAIFlags2.Size = new System.Drawing.Size(109, 124);
            this.clbAIFlags2.TabIndex = 47;
            // 
            // clbAIFlags1
            // 
            this.clbAIFlags1.FormattingEnabled = true;
            this.clbAIFlags1.Items.AddRange(new object[] {
            "",
            "Focus Unit",
            "Stay Near X/Y",
            "Aggressive",
            "Defensive",
            "",
            "",
            ""});
            this.clbAIFlags1.Location = new System.Drawing.Point(138, 23);
            this.clbAIFlags1.Name = "clbAIFlags1";
            this.clbAIFlags1.Size = new System.Drawing.Size(108, 124);
            this.clbAIFlags1.TabIndex = 46;
            // 
            // labelTargetY
            // 
            this.labelTargetY.AutoSize = true;
            this.labelTargetY.Location = new System.Drawing.Point(7, 77);
            this.labelTargetY.Name = "labelTargetY";
            this.labelTargetY.Size = new System.Drawing.Size(48, 13);
            this.labelTargetY.TabIndex = 45;
            this.labelTargetY.Text = "Target Y";
            // 
            // labelTargetX
            // 
            this.labelTargetX.AutoSize = true;
            this.labelTargetX.Location = new System.Drawing.Point(7, 51);
            this.labelTargetX.Name = "labelTargetX";
            this.labelTargetX.Size = new System.Drawing.Size(48, 13);
            this.labelTargetX.TabIndex = 44;
            this.labelTargetX.Text = "Target X";
            // 
            // targetLabel
            // 
            this.targetLabel.AutoSize = true;
            this.targetLabel.Location = new System.Drawing.Point(7, 23);
            this.targetLabel.Name = "targetLabel";
            this.targetLabel.Size = new System.Drawing.Size(74, 13);
            this.targetLabel.TabIndex = 43;
            this.targetLabel.Text = "Target Unit ID";
            // 
            // targetYSpinner
            // 
            this.targetYSpinner.Hexadecimal = true;
            this.targetYSpinner.Location = new System.Drawing.Point(87, 75);
            this.targetYSpinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.targetYSpinner.Name = "targetYSpinner";
            this.targetYSpinner.Size = new System.Drawing.Size(45, 20);
            this.targetYSpinner.TabIndex = 8;
            this.targetYSpinner.Tag = "TargetY";
            this.targetYSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // targetXSpinner
            // 
            this.targetXSpinner.Hexadecimal = true;
            this.targetXSpinner.Location = new System.Drawing.Point(87, 49);
            this.targetXSpinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.targetXSpinner.Name = "targetXSpinner";
            this.targetXSpinner.Size = new System.Drawing.Size(45, 20);
            this.targetXSpinner.TabIndex = 7;
            this.targetXSpinner.Tag = "TargetX";
            this.targetXSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // targetSpinner
            // 
            this.targetSpinner.Hexadecimal = true;
            this.targetSpinner.Location = new System.Drawing.Point(87, 23);
            this.targetSpinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.targetSpinner.Name = "targetSpinner";
            this.targetSpinner.Size = new System.Drawing.Size(45, 20);
            this.targetSpinner.TabIndex = 10;
            this.targetSpinner.Tag = "Target";
            this.targetSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(2, 28);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(35, 13);
            nameLabel.TabIndex = 18;
            nameLabel.Tag = "SpecialName";
            nameLabel.Text = "Name";
            // 
            // levelLabel
            // 
            levelLabel.AutoSize = true;
            levelLabel.Location = new System.Drawing.Point(2, 94);
            levelLabel.Name = "levelLabel";
            levelLabel.Size = new System.Drawing.Size(33, 13);
            levelLabel.TabIndex = 23;
            levelLabel.Text = "Level";
            // 
            // yLabel
            // 
            yLabel.AutoSize = true;
            yLabel.Location = new System.Drawing.Point(160, 158);
            yLabel.Name = "yLabel";
            yLabel.Size = new System.Drawing.Size(14, 13);
            yLabel.TabIndex = 29;
            yLabel.Text = "Y";
            // 
            // paletteLabel
            // 
            paletteLabel.AutoSize = true;
            paletteLabel.Location = new System.Drawing.Point(2, 137);
            paletteLabel.Name = "paletteLabel";
            paletteLabel.Size = new System.Drawing.Size(40, 13);
            paletteLabel.TabIndex = 24;
            paletteLabel.Text = "Palette";
            // 
            // equipmentGroupBox
            // 
            equipmentGroupBox.Controls.Add(accessoryLabel);
            equipmentGroupBox.Controls.Add(bodyLabel);
            equipmentGroupBox.Controls.Add(headLabel);
            equipmentGroupBox.Controls.Add(leftHandLabel);
            equipmentGroupBox.Controls.Add(rightHandLabel);
            equipmentGroupBox.Controls.Add(this.rightHandComboBox);
            equipmentGroupBox.Controls.Add(this.accessoryComboBox);
            equipmentGroupBox.Controls.Add(this.leftHandComboBox);
            equipmentGroupBox.Controls.Add(this.bodyComboBox);
            equipmentGroupBox.Controls.Add(this.headComboBox);
            equipmentGroupBox.Location = new System.Drawing.Point(5, 370);
            equipmentGroupBox.Name = "equipmentGroupBox";
            equipmentGroupBox.Size = new System.Drawing.Size(200, 138);
            equipmentGroupBox.TabIndex = 18;
            equipmentGroupBox.TabStop = false;
            equipmentGroupBox.Text = "Equipment";
            // 
            // accessoryLabel
            // 
            accessoryLabel.AutoSize = true;
            accessoryLabel.Location = new System.Drawing.Point(7, 106);
            accessoryLabel.Name = "accessoryLabel";
            accessoryLabel.Size = new System.Drawing.Size(56, 13);
            accessoryLabel.TabIndex = 34;
            accessoryLabel.Text = "Accessory";
            // 
            // bodyLabel
            // 
            bodyLabel.AutoSize = true;
            bodyLabel.Location = new System.Drawing.Point(7, 84);
            bodyLabel.Name = "bodyLabel";
            bodyLabel.Size = new System.Drawing.Size(31, 13);
            bodyLabel.TabIndex = 33;
            bodyLabel.Text = "Body";
            // 
            // headLabel
            // 
            headLabel.AutoSize = true;
            headLabel.Location = new System.Drawing.Point(7, 62);
            headLabel.Name = "headLabel";
            headLabel.Size = new System.Drawing.Size(33, 13);
            headLabel.TabIndex = 32;
            headLabel.Text = "Head";
            // 
            // leftHandLabel
            // 
            leftHandLabel.AutoSize = true;
            leftHandLabel.Location = new System.Drawing.Point(7, 40);
            leftHandLabel.Name = "leftHandLabel";
            leftHandLabel.Size = new System.Drawing.Size(54, 13);
            leftHandLabel.TabIndex = 31;
            leftHandLabel.Text = "Left Hand";
            // 
            // rightHandLabel
            // 
            rightHandLabel.AutoSize = true;
            rightHandLabel.Location = new System.Drawing.Point(7, 18);
            rightHandLabel.Name = "rightHandLabel";
            rightHandLabel.Size = new System.Drawing.Size(61, 13);
            rightHandLabel.TabIndex = 30;
            rightHandLabel.Text = "Right Hand";
            // 
            // rightHandComboBox
            // 
            this.rightHandComboBox.FormattingEnabled = true;
            this.rightHandComboBox.Location = new System.Drawing.Point(74, 15);
            this.rightHandComboBox.Name = "rightHandComboBox";
            this.rightHandComboBox.Size = new System.Drawing.Size(121, 21);
            this.rightHandComboBox.TabIndex = 0;
            this.rightHandComboBox.Tag = "RightHand";
            // 
            // accessoryComboBox
            // 
            this.accessoryComboBox.FormattingEnabled = true;
            this.accessoryComboBox.Location = new System.Drawing.Point(74, 103);
            this.accessoryComboBox.Name = "accessoryComboBox";
            this.accessoryComboBox.Size = new System.Drawing.Size(121, 21);
            this.accessoryComboBox.TabIndex = 4;
            this.accessoryComboBox.Tag = "Accessory";
            // 
            // leftHandComboBox
            // 
            this.leftHandComboBox.FormattingEnabled = true;
            this.leftHandComboBox.Location = new System.Drawing.Point(74, 37);
            this.leftHandComboBox.Name = "leftHandComboBox";
            this.leftHandComboBox.Size = new System.Drawing.Size(121, 21);
            this.leftHandComboBox.TabIndex = 1;
            this.leftHandComboBox.Tag = "LeftHand";
            // 
            // bodyComboBox
            // 
            this.bodyComboBox.FormattingEnabled = true;
            this.bodyComboBox.Location = new System.Drawing.Point(74, 81);
            this.bodyComboBox.Name = "bodyComboBox";
            this.bodyComboBox.Size = new System.Drawing.Size(121, 21);
            this.bodyComboBox.TabIndex = 3;
            this.bodyComboBox.Tag = "Body";
            // 
            // headComboBox
            // 
            this.headComboBox.FormattingEnabled = true;
            this.headComboBox.Location = new System.Drawing.Point(74, 59);
            this.headComboBox.Name = "headComboBox";
            this.headComboBox.Size = new System.Drawing.Size(121, 21);
            this.headComboBox.TabIndex = 2;
            this.headComboBox.Tag = "Head";
            // 
            // xLabel
            // 
            xLabel.AutoSize = true;
            xLabel.Location = new System.Drawing.Point(2, 158);
            xLabel.Name = "xLabel";
            xLabel.Size = new System.Drawing.Size(14, 13);
            xLabel.TabIndex = 28;
            xLabel.Text = "X";
            // 
            // skillsGroupBox
            // 
            skillsGroupBox.Controls.Add(movementSkillLabel);
            skillsGroupBox.Controls.Add(supportSkillLabel);
            skillsGroupBox.Controls.Add(reactionSkillLabel);
            skillsGroupBox.Controls.Add(secondarySkillLabel);
            skillsGroupBox.Controls.Add(primarySkillLabel);
            skillsGroupBox.Controls.Add(this.primarySkillComboBox);
            skillsGroupBox.Controls.Add(this.movementComboBox);
            skillsGroupBox.Controls.Add(this.secondaryActionComboBox);
            skillsGroupBox.Controls.Add(this.supportComboBox);
            skillsGroupBox.Controls.Add(this.reactionComboBox);
            skillsGroupBox.Location = new System.Drawing.Point(5, 226);
            skillsGroupBox.Name = "skillsGroupBox";
            skillsGroupBox.Size = new System.Drawing.Size(200, 138);
            skillsGroupBox.TabIndex = 17;
            skillsGroupBox.TabStop = false;
            skillsGroupBox.Text = "Skills";
            // 
            // movementSkillLabel
            // 
            movementSkillLabel.AutoSize = true;
            movementSkillLabel.Location = new System.Drawing.Point(7, 106);
            movementSkillLabel.Name = "movementSkillLabel";
            movementSkillLabel.Size = new System.Drawing.Size(57, 13);
            movementSkillLabel.TabIndex = 24;
            movementSkillLabel.Text = "Movement";
            // 
            // supportSkillLabel
            // 
            supportSkillLabel.AutoSize = true;
            supportSkillLabel.Location = new System.Drawing.Point(7, 84);
            supportSkillLabel.Name = "supportSkillLabel";
            supportSkillLabel.Size = new System.Drawing.Size(44, 13);
            supportSkillLabel.TabIndex = 23;
            supportSkillLabel.Text = "Support";
            // 
            // reactionSkillLabel
            // 
            reactionSkillLabel.AutoSize = true;
            reactionSkillLabel.Location = new System.Drawing.Point(7, 62);
            reactionSkillLabel.Name = "reactionSkillLabel";
            reactionSkillLabel.Size = new System.Drawing.Size(50, 13);
            reactionSkillLabel.TabIndex = 22;
            reactionSkillLabel.Text = "Reaction";
            // 
            // secondarySkillLabel
            // 
            secondarySkillLabel.AutoSize = true;
            secondarySkillLabel.Location = new System.Drawing.Point(7, 40);
            secondarySkillLabel.Name = "secondarySkillLabel";
            secondarySkillLabel.Size = new System.Drawing.Size(58, 13);
            secondarySkillLabel.TabIndex = 21;
            secondarySkillLabel.Text = "Secondary";
            // 
            // primarySkillLabel
            // 
            primarySkillLabel.AutoSize = true;
            primarySkillLabel.Location = new System.Drawing.Point(7, 18);
            primarySkillLabel.Name = "primarySkillLabel";
            primarySkillLabel.Size = new System.Drawing.Size(41, 13);
            primarySkillLabel.TabIndex = 20;
            primarySkillLabel.Text = "Primary";
            // 
            // primarySkillComboBox
            // 
            this.primarySkillComboBox.FormattingEnabled = true;
            this.primarySkillComboBox.Location = new System.Drawing.Point(74, 15);
            this.primarySkillComboBox.Name = "primarySkillComboBox";
            this.primarySkillComboBox.Size = new System.Drawing.Size(121, 21);
            this.primarySkillComboBox.TabIndex = 0;
            this.primarySkillComboBox.Tag = "SkillSet";
            // 
            // movementComboBox
            // 
            this.movementComboBox.FormattingEnabled = true;
            this.movementComboBox.Location = new System.Drawing.Point(74, 103);
            this.movementComboBox.Name = "movementComboBox";
            this.movementComboBox.Size = new System.Drawing.Size(121, 21);
            this.movementComboBox.TabIndex = 4;
            this.movementComboBox.Tag = "Movement";
            // 
            // secondaryActionComboBox
            // 
            this.secondaryActionComboBox.FormattingEnabled = true;
            this.secondaryActionComboBox.Location = new System.Drawing.Point(74, 37);
            this.secondaryActionComboBox.Name = "secondaryActionComboBox";
            this.secondaryActionComboBox.Size = new System.Drawing.Size(121, 21);
            this.secondaryActionComboBox.TabIndex = 1;
            this.secondaryActionComboBox.Tag = "SecondaryAction";
            // 
            // supportComboBox
            // 
            this.supportComboBox.FormattingEnabled = true;
            this.supportComboBox.Location = new System.Drawing.Point(74, 81);
            this.supportComboBox.Name = "supportComboBox";
            this.supportComboBox.Size = new System.Drawing.Size(121, 21);
            this.supportComboBox.TabIndex = 3;
            this.supportComboBox.Tag = "Support";
            // 
            // reactionComboBox
            // 
            this.reactionComboBox.FormattingEnabled = true;
            this.reactionComboBox.Location = new System.Drawing.Point(74, 59);
            this.reactionComboBox.Name = "reactionComboBox";
            this.reactionComboBox.Size = new System.Drawing.Size(121, 21);
            this.reactionComboBox.TabIndex = 2;
            this.reactionComboBox.Tag = "Reaction";
            // 
            // unknownGroupBox
            // 
            this.unknownGroupBox.Controls.Add(this.unknown12Spinner);
            this.unknownGroupBox.Controls.Add(this.unknown10Spinner);
            this.unknownGroupBox.Location = new System.Drawing.Point(428, 226);
            this.unknownGroupBox.Name = "unknownGroupBox";
            this.unknownGroupBox.Size = new System.Drawing.Size(150, 103);
            this.unknownGroupBox.TabIndex = 45;
            this.unknownGroupBox.TabStop = false;
            this.unknownGroupBox.Text = "Unknown";
            // 
            // unknown12Spinner
            // 
            this.unknown12Spinner.Hexadecimal = true;
            this.unknown12Spinner.Location = new System.Drawing.Point(70, 25);
            this.unknown12Spinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.unknown12Spinner.Name = "unknown12Spinner";
            this.unknown12Spinner.Size = new System.Drawing.Size(45, 20);
            this.unknown12Spinner.TabIndex = 13;
            this.unknown12Spinner.Tag = "Unknown12";
            this.unknown12Spinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // unknown10Spinner
            // 
            this.unknown10Spinner.Hexadecimal = true;
            this.unknown10Spinner.Location = new System.Drawing.Point(14, 25);
            this.unknown10Spinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.unknown10Spinner.Name = "unknown10Spinner";
            this.unknown10Spinner.Size = new System.Drawing.Size(45, 20);
            this.unknown10Spinner.TabIndex = 11;
            this.unknown10Spinner.Tag = "Unknown10";
            this.unknown10Spinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // upperLevelCheckBox
            // 
            this.upperLevelCheckBox.AutoSize = true;
            this.upperLevelCheckBox.Location = new System.Drawing.Point(209, 201);
            this.upperLevelCheckBox.Name = "upperLevelCheckBox";
            this.upperLevelCheckBox.Size = new System.Drawing.Size(80, 17);
            this.upperLevelCheckBox.TabIndex = 44;
            this.upperLevelCheckBox.Text = "Upper Level";
            this.upperLevelCheckBox.UseVisualStyleBackColor = true;
            // 
            // teamColorComboBox
            // 
            this.teamColorComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.teamColorComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.teamColorComboBox.FormattingEnabled = true;
            this.teamColorComboBox.Location = new System.Drawing.Point(366, 133);
            this.teamColorComboBox.Name = "teamColorComboBox";
            this.teamColorComboBox.Size = new System.Drawing.Size(121, 21);
            this.teamColorComboBox.TabIndex = 39;
            this.teamColorComboBox.Tag = "TeamColor";
            // 
            // facingDirectionComboBox
            // 
            this.facingDirectionComboBox.FormattingEnabled = true;
            this.facingDirectionComboBox.Location = new System.Drawing.Point(82, 199);
            this.facingDirectionComboBox.Name = "facingDirectionComboBox";
            this.facingDirectionComboBox.Size = new System.Drawing.Size(121, 21);
            this.facingDirectionComboBox.TabIndex = 14;
            this.facingDirectionComboBox.Tag = "FacingDirection";
            // 
            // levelComboBox
            // 
            this.levelComboBox.FormattingEnabled = true;
            this.levelComboBox.Location = new System.Drawing.Point(82, 91);
            this.levelComboBox.Name = "levelComboBox";
            this.levelComboBox.Size = new System.Drawing.Size(142, 21);
            this.levelComboBox.TabIndex = 5;
            this.levelComboBox.Tag = "Level";
            // 
            // dayComboBox
            // 
            this.dayComboBox.FormattingEnabled = true;
            this.dayComboBox.Location = new System.Drawing.Point(225, 47);
            this.dayComboBox.Name = "dayComboBox";
            this.dayComboBox.Size = new System.Drawing.Size(68, 21);
            this.dayComboBox.TabIndex = 3;
            this.dayComboBox.Tag = "Day";
            // 
            // faithComboBox
            // 
            this.faithComboBox.FormattingEnabled = true;
            this.faithComboBox.Location = new System.Drawing.Point(82, 113);
            this.faithComboBox.Name = "faithComboBox";
            this.faithComboBox.Size = new System.Drawing.Size(65, 21);
            this.faithComboBox.TabIndex = 6;
            this.faithComboBox.Tag = "Faith";
            // 
            // braveryComboBox
            // 
            this.braveryComboBox.FormattingEnabled = true;
            this.braveryComboBox.Location = new System.Drawing.Point(209, 113);
            this.braveryComboBox.Name = "braveryComboBox";
            this.braveryComboBox.Size = new System.Drawing.Size(65, 21);
            this.braveryComboBox.TabIndex = 7;
            this.braveryComboBox.Tag = "Bravery";
            // 
            // preRequisiteJobComboBox
            // 
            this.preRequisiteJobComboBox.FormattingEnabled = true;
            this.preRequisiteJobComboBox.Location = new System.Drawing.Point(82, 177);
            this.preRequisiteJobComboBox.Name = "preRequisiteJobComboBox";
            this.preRequisiteJobComboBox.Size = new System.Drawing.Size(121, 21);
            this.preRequisiteJobComboBox.TabIndex = 12;
            this.preRequisiteJobComboBox.Tag = "PrerequisiteJob";
            // 
            // flags2CheckedListBox
            // 
            this.flags2CheckedListBox.FormattingEnabled = true;
            this.flags2CheckedListBox.Items.AddRange(new object[] {
            "Always Present",
            "Randomly Present",
            "Control",
            "Immortal",
            "",
            ""});
            this.flags2CheckedListBox.Location = new System.Drawing.Point(422, 3);
            this.flags2CheckedListBox.Name = "flags2CheckedListBox";
            this.flags2CheckedListBox.Size = new System.Drawing.Size(121, 124);
            this.flags2CheckedListBox.TabIndex = 16;
            this.flags2CheckedListBox.TabStop = false;
            // 
            // spriteSetComboBox
            // 
            this.spriteSetComboBox.FormattingEnabled = true;
            this.spriteSetComboBox.Location = new System.Drawing.Point(82, 3);
            this.spriteSetComboBox.Name = "spriteSetComboBox";
            this.spriteSetComboBox.Size = new System.Drawing.Size(142, 21);
            this.spriteSetComboBox.TabIndex = 0;
            this.spriteSetComboBox.Tag = "SpriteSet";
            // 
            // jobComboBox
            // 
            this.jobComboBox.FormattingEnabled = true;
            this.jobComboBox.Location = new System.Drawing.Point(82, 69);
            this.jobComboBox.Name = "jobComboBox";
            this.jobComboBox.Size = new System.Drawing.Size(142, 21);
            this.jobComboBox.TabIndex = 4;
            this.jobComboBox.Tag = "Job";
            // 
            // unitIDSpinner
            // 
            this.unitIDSpinner.Hexadecimal = true;
            this.unitIDSpinner.Location = new System.Drawing.Point(209, 135);
            this.unitIDSpinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.unitIDSpinner.Name = "unitIDSpinner";
            this.unitIDSpinner.Size = new System.Drawing.Size(45, 20);
            this.unitIDSpinner.TabIndex = 9;
            this.unitIDSpinner.Tag = "UnitID";
            this.unitIDSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // specialNameComboBox
            // 
            this.specialNameComboBox.FormattingEnabled = true;
            this.specialNameComboBox.Location = new System.Drawing.Point(82, 25);
            this.specialNameComboBox.Name = "specialNameComboBox";
            this.specialNameComboBox.Size = new System.Drawing.Size(142, 21);
            this.specialNameComboBox.TabIndex = 1;
            this.specialNameComboBox.Tag = "SpecialName";
            // 
            // flags1CheckedListBox
            // 
            this.flags1CheckedListBox.FormattingEnabled = true;
            this.flags1CheckedListBox.Items.AddRange(new object[] {
            "Male",
            "Female",
            "Monster",
            "Join After Event",
            "Load Formation",
            "Has ??? stats",
            "",
            "Save as Guest"});
            this.flags1CheckedListBox.Location = new System.Drawing.Point(295, 3);
            this.flags1CheckedListBox.Name = "flags1CheckedListBox";
            this.flags1CheckedListBox.Size = new System.Drawing.Size(121, 124);
            this.flags1CheckedListBox.TabIndex = 15;
            this.flags1CheckedListBox.TabStop = false;
            // 
            // jobLevelSpinner
            // 
            this.jobLevelSpinner.Location = new System.Drawing.Point(209, 177);
            this.jobLevelSpinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.jobLevelSpinner.Name = "jobLevelSpinner";
            this.jobLevelSpinner.Size = new System.Drawing.Size(45, 20);
            this.jobLevelSpinner.TabIndex = 13;
            this.jobLevelSpinner.Tag = "PrerequisiteJobLevel";
            this.jobLevelSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // monthComboBox
            // 
            this.monthComboBox.FormattingEnabled = true;
            this.monthComboBox.Location = new System.Drawing.Point(82, 47);
            this.monthComboBox.Name = "monthComboBox";
            this.monthComboBox.Size = new System.Drawing.Size(142, 21);
            this.monthComboBox.TabIndex = 2;
            this.monthComboBox.Tag = "Month";
            // 
            // paletteSpinner
            // 
            this.paletteSpinner.Hexadecimal = true;
            this.paletteSpinner.Location = new System.Drawing.Point(82, 135);
            this.paletteSpinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.paletteSpinner.Name = "paletteSpinner";
            this.paletteSpinner.Size = new System.Drawing.Size(45, 20);
            this.paletteSpinner.TabIndex = 8;
            this.paletteSpinner.Tag = "Palette";
            this.paletteSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ySpinner
            // 
            this.ySpinner.Location = new System.Drawing.Point(209, 156);
            this.ySpinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ySpinner.Name = "ySpinner";
            this.ySpinner.Size = new System.Drawing.Size(45, 20);
            this.ySpinner.TabIndex = 11;
            this.ySpinner.Tag = "Y";
            this.ySpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // xSpinner
            // 
            this.xSpinner.Location = new System.Drawing.Point(82, 156);
            this.xSpinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.xSpinner.Name = "xSpinner";
            this.xSpinner.Size = new System.Drawing.Size(45, 20);
            this.xSpinner.TabIndex = 10;
            this.xSpinner.Tag = "X";
            this.xSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // experienceLabel
            // 
            this.experienceLabel.AutoSize = true;
            this.experienceLabel.Location = new System.Drawing.Point(293, 163);
            this.experienceLabel.Name = "experienceLabel";
            this.experienceLabel.Size = new System.Drawing.Size(60, 13);
            this.experienceLabel.TabIndex = 46;
            this.experienceLabel.Text = "Experience";
            // 
            // experienceComboBox
            // 
            this.experienceComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.experienceComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.experienceComboBox.FormattingEnabled = true;
            this.experienceComboBox.Location = new System.Drawing.Point(366, 159);
            this.experienceComboBox.Name = "experienceComboBox";
            this.experienceComboBox.Size = new System.Drawing.Size(91, 21);
            this.experienceComboBox.TabIndex = 47;
            this.experienceComboBox.Tag = "Experience";
            // 
            // EventUnitEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.experienceComboBox);
            this.Controls.Add(this.experienceLabel);
            this.Controls.Add(this.unknownGroupBox);
            this.Controls.Add(this.upperLevelCheckBox);
            this.Controls.Add(spoilsGroupBox);
            this.Controls.Add(teamColorLabel);
            this.Controls.Add(this.teamColorComboBox);
            this.Controls.Add(hLabel2);
            this.Controls.Add(hLabel1);
            this.Controls.Add(initialDirectionLabel);
            this.Controls.Add(this.facingDirectionComboBox);
            this.Controls.Add(this.levelComboBox);
            this.Controls.Add(this.dayComboBox);
            this.Controls.Add(this.faithComboBox);
            this.Controls.Add(this.braveryComboBox);
            this.Controls.Add(jobsUnlockedLabel);
            this.Controls.Add(this.preRequisiteJobComboBox);
            this.Controls.Add(unitIdLabel);
            this.Controls.Add(this.flags2CheckedListBox);
            this.Controls.Add(jobLabel);
            this.Controls.Add(unitLabel);
            this.Controls.Add(this.spriteSetComboBox);
            this.Controls.Add(braveryLabel);
            this.Controls.Add(this.jobComboBox);
            this.Controls.Add(faithLabel);
            this.Controls.Add(this.unitIDSpinner);
            this.Controls.Add(this.specialNameComboBox);
            this.Controls.Add(birthdayLabel);
            this.Controls.Add(aiGroupBox);
            this.Controls.Add(this.flags1CheckedListBox);
            this.Controls.Add(this.jobLevelSpinner);
            this.Controls.Add(nameLabel);
            this.Controls.Add(levelLabel);
            this.Controls.Add(yLabel);
            this.Controls.Add(paletteLabel);
            this.Controls.Add(this.monthComboBox);
            this.Controls.Add(equipmentGroupBox);
            this.Controls.Add(xLabel);
            this.Controls.Add(this.paletteSpinner);
            this.Controls.Add(skillsGroupBox);
            this.Controls.Add(this.ySpinner);
            this.Controls.Add(this.xSpinner);
            this.Name = "EventUnitEditor";
            this.Size = new System.Drawing.Size(581, 511);
            spoilsGroupBox.ResumeLayout(false);
            spoilsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bonusMoneySpinner)).EndInit();
            aiGroupBox.ResumeLayout(false);
            aiGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targetYSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetXSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetSpinner)).EndInit();
            equipmentGroupBox.ResumeLayout(false);
            equipmentGroupBox.PerformLayout();
            skillsGroupBox.ResumeLayout(false);
            skillsGroupBox.PerformLayout();
            this.unknownGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.unknown12Spinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown10Spinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitIDSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.jobLevelSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ySpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xSpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FFTPatcher.Controls.ComboBoxWithDefault spriteSetComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault specialNameComboBox;
        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault flags1CheckedListBox;
        private FFTPatcher.Controls.ComboBoxWithDefault monthComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault primarySkillComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault movementComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault secondaryActionComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault supportComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault reactionComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault rightHandComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault accessoryComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault leftHandComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault bodyComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault headComboBox;
        private FFTPatcher.Controls.NumericUpDownWithDefault paletteSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault xSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault ySpinner;
        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault flags2CheckedListBox;
        private FFTPatcher.Controls.NumericUpDownWithDefault unknown12Spinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault unknown10Spinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault targetSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault targetYSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault targetXSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault unitIDSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault bonusMoneySpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault jobLevelSpinner;
        private FFTPatcher.Controls.ComboBoxWithDefault jobComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault preRequisiteJobComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault braveryComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault faithComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault dayComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault levelComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault facingDirectionComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault teamColorComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault warTrophyComboBox;
        private System.Windows.Forms.Label targetLabel;
        private System.Windows.Forms.CheckBox upperLevelCheckBox;
        private System.Windows.Forms.Label labelTargetY;
        private System.Windows.Forms.Label labelTargetX;
        private System.Windows.Forms.GroupBox unknownGroupBox;
        private Controls.CheckedListBoxNoHighlightWithDefault clbAIFlags1;
        private Controls.CheckedListBoxNoHighlightWithDefault clbAIFlags2;
        private System.Windows.Forms.Label experienceLabel;
        private Controls.ComboBoxWithDefault experienceComboBox;
    }
}
