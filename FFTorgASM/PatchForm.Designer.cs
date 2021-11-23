namespace FFTorgASM
{
    partial class PatchForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_Description = new System.Windows.Forms.Label();
            this.clb_Patches = new PatcherLib.Controls.ModifiedBGCheckedListBox();
            this.btn_Patch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_Description
            // 
            this.lbl_Description.AutoSize = true;
            this.lbl_Description.Location = new System.Drawing.Point(15, 11);
            this.lbl_Description.Name = "lbl_Description";
            this.lbl_Description.Size = new System.Drawing.Size(226, 13);
            this.lbl_Description.TabIndex = 0;
            this.lbl_Description.Text = "The following selected patches will be applied:";
            // 
            // clb_Patches
            // 
            this.clb_Patches.BackColors = null;
            this.clb_Patches.FormattingEnabled = true;
            this.clb_Patches.IncludePrefix = false;
            this.clb_Patches.Location = new System.Drawing.Point(25, 42);
            this.clb_Patches.Name = "clb_Patches";
            this.clb_Patches.Size = new System.Drawing.Size(398, 349);
            this.clb_Patches.TabIndex = 1;
            this.clb_Patches.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clb_Patches_ItemCheck);
            // 
            // btn_Patch
            // 
            this.btn_Patch.Location = new System.Drawing.Point(348, 410);
            this.btn_Patch.Name = "btn_Patch";
            this.btn_Patch.Size = new System.Drawing.Size(74, 29);
            this.btn_Patch.TabIndex = 2;
            this.btn_Patch.Text = "Patch...";
            this.btn_Patch.UseVisualStyleBackColor = true;
            this.btn_Patch.Click += new System.EventHandler(this.btn_Patch_Click);
            // 
            // PatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 452);
            this.Controls.Add(this.btn_Patch);
            this.Controls.Add(this.clb_Patches);
            this.Controls.Add(this.lbl_Description);
            this.Name = "PatchForm";
            this.Text = "Patch";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Description;
        private PatcherLib.Controls.ModifiedBGCheckedListBox clb_Patches;
        private System.Windows.Forms.Button btn_Patch;
    }
}