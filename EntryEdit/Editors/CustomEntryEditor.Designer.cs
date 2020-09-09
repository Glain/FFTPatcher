namespace EntryEdit.Editors
{
    partial class CustomEntryEditor
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
            this.txt_Entry = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_Entry
            // 
            this.txt_Entry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Entry.Location = new System.Drawing.Point(0, 0);
            this.txt_Entry.Multiline = true;
            this.txt_Entry.Name = "txt_Entry";
            this.txt_Entry.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_Entry.Size = new System.Drawing.Size(874, 100);
            this.txt_Entry.TabIndex = 0;
            // 
            // CustomEntryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt_Entry);
            this.Name = "CustomEntryEditor";
            this.Size = new System.Drawing.Size(874, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Entry;
    }
}
