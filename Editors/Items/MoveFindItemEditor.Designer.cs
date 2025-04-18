﻿namespace FFTPatcher.Editors
{
    partial class MoveFindItemEditor
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
            if ( disposing && ( components != null ) )
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
            System.Windows.Forms.Label yLabel;
            System.Windows.Forms.Label xLabel;
            System.Windows.Forms.Label commonLabel;
            System.Windows.Forms.Label rareLabel;
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.xSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.ySpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
            this.trapsCheckedListBox = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
            this.commonComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            this.rareComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
            yLabel = new System.Windows.Forms.Label();
            xLabel = new System.Windows.Forms.Label();
            commonLabel = new System.Windows.Forms.Label();
            rareLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.xSpinner ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ySpinner ) ).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add( rareLabel );
            this.groupBox1.Controls.Add( commonLabel );
            this.groupBox1.Controls.Add( this.rareComboBox );
            this.groupBox1.Controls.Add( this.commonComboBox );
            this.groupBox1.Controls.Add( this.trapsCheckedListBox );
            this.groupBox1.Controls.Add( this.ySpinner );
            this.groupBox1.Controls.Add( this.xSpinner );
            this.groupBox1.Controls.Add( yLabel );
            this.groupBox1.Controls.Add( xLabel );
            this.groupBox1.Location = new System.Drawing.Point( 0, 0 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 364, 161 );
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // xSpinner
            // 
            this.xSpinner.Location = new System.Drawing.Point( 61, 18 );
            this.xSpinner.Maximum = new decimal( new int[] {
            15,
            0,
            0,
            0} );
            this.xSpinner.Name = "xSpinner";
            this.xSpinner.Size = new System.Drawing.Size( 44, 20 );
            this.xSpinner.TabIndex = 0;
            this.xSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // yLabel
            // 
            yLabel.AutoSize = true;
            yLabel.Location = new System.Drawing.Point( 7, 42 );
            yLabel.Name = "yLabel";
            yLabel.Size = new System.Drawing.Size( 14, 13 );
            yLabel.TabIndex = 1;
            yLabel.Text = "Y";
            // 
            // xLabel
            // 
            xLabel.AutoSize = true;
            xLabel.Location = new System.Drawing.Point( 7, 20 );
            xLabel.Name = "xLabel";
            xLabel.Size = new System.Drawing.Size( 14, 13 );
            xLabel.TabIndex = 0;
            xLabel.Text = "X";
            // 
            // ySpinner
            // 
            this.ySpinner.Location = new System.Drawing.Point( 61, 40 );
            this.ySpinner.Maximum = new decimal( new int[] {
            15,
            0,
            0,
            0} );
            this.ySpinner.Name = "ySpinner";
            this.ySpinner.Size = new System.Drawing.Size( 44, 20 );
            this.ySpinner.TabIndex = 1;
            this.ySpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // trapsCheckedListBox
            // 
            this.trapsCheckedListBox.FormattingEnabled = true;
            this.trapsCheckedListBox.Items.AddRange( new object[] {
            "Upper Level",
            "",
            "Always Trap",
            "Disable Trap",
            "Steel Needle",
            "Sleeping Gas",
            "Deathtrap",
            "Degenerator"} );
            this.trapsCheckedListBox.Location = new System.Drawing.Point( 252, 18 );
            this.trapsCheckedListBox.MultiColumn = true;
            this.trapsCheckedListBox.Name = "trapsCheckedListBox";
            this.trapsCheckedListBox.Size = new System.Drawing.Size( 106, 124 );
            this.trapsCheckedListBox.TabIndex = 4;
            // 
            // commonComboBox
            // 
            this.commonComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.commonComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.commonComboBox.FormattingEnabled = true;
            this.commonComboBox.Location = new System.Drawing.Point( 61, 66 );
            this.commonComboBox.Name = "commonComboBox";
            this.commonComboBox.Size = new System.Drawing.Size( 185, 21 );
            this.commonComboBox.TabIndex = 2;
            // 
            // rareComboBox
            // 
            this.rareComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.rareComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.rareComboBox.FormattingEnabled = true;
            this.rareComboBox.Location = new System.Drawing.Point( 61, 93 );
            this.rareComboBox.Name = "rareComboBox";
            this.rareComboBox.Size = new System.Drawing.Size( 185, 21 );
            this.rareComboBox.TabIndex = 3;
            // 
            // commonLabel
            // 
            commonLabel.AutoSize = true;
            commonLabel.Location = new System.Drawing.Point( 7, 69 );
            commonLabel.Name = "commonLabel";
            commonLabel.Size = new System.Drawing.Size( 48, 13 );
            commonLabel.TabIndex = 7;
            commonLabel.Text = "Common";
            // 
            // rareLabel
            // 
            rareLabel.AutoSize = true;
            rareLabel.Location = new System.Drawing.Point( 7, 93 );
            rareLabel.Name = "rareLabel";
            rareLabel.Size = new System.Drawing.Size( 30, 13 );
            rareLabel.TabIndex = 8;
            rareLabel.Text = "Rare";
            // 
            // MoveFindItemEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add( this.groupBox1 );
            this.Name = "MoveFindItemEditor";
            this.Size = new System.Drawing.Size( 367, 164 );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.xSpinner ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.ySpinner ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private FFTPatcher.Controls.NumericUpDownWithDefault xSpinner;
        private FFTPatcher.Controls.ComboBoxWithDefault rareComboBox;
        private FFTPatcher.Controls.ComboBoxWithDefault commonComboBox;
        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault trapsCheckedListBox;
        private FFTPatcher.Controls.NumericUpDownWithDefault ySpinner;
    }
}
