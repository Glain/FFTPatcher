namespace FFTPatcher.SpriteEditor
{
    partial class AllSpritesEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.spriteEditor1 = new FFTPatcher.SpriteEditor.SpriteEditor();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 3);
            this.comboBox1.Name = "comboBox1";
            //this.comboBox1.Size = new System.Drawing.Size(739, 21);
            this.comboBox1.Size = new System.Drawing.Size(663, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // spriteEditor1
            // 
            this.spriteEditor1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            //this.spriteEditor1.AutoSize = true;
            //this.spriteEditor1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spriteEditor1.Location = new System.Drawing.Point(3, 30);
            this.spriteEditor1.Name = "spriteEditor1";
            //this.spriteEditor1.Size = new System.Drawing.Size(630, 662);
            //this.spriteEditor1.Size = new System.Drawing.Size(663, 662);
            //this.spriteEditor1.Size = new System.Drawing.Size(663, 695);
            this.spriteEditor1.Size = new System.Drawing.Size(663, 735);
            this.spriteEditor1.TabIndex = 1;
            // 
            // AllSpritesEditor
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spriteEditor1);
            this.Controls.Add(this.comboBox1);
            this.Name = "AllSpritesEditor";
            //this.Size = new System.Drawing.Size(745, 695);
            //this.Size = new System.Drawing.Size(663, 695);
            this.Size = new System.Drawing.Size(663, 735);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private SpriteEditor spriteEditor1;
    }
}
