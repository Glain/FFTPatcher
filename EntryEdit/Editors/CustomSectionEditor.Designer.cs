namespace EntryEdit.Editors
{
    partial class CustomSectionEditor
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
            this.cmb_Section = new System.Windows.Forms.ComboBox();
            this.cmb_Entry = new System.Windows.Forms.ComboBox();
            this.entryEditor = new EntryEdit.Editors.CustomEntryEditor();
            this.SuspendLayout();
            // 
            // cmb_Section
            // 
            this.cmb_Section.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Section.FormattingEnabled = true;
            this.cmb_Section.Location = new System.Drawing.Point(1, 1);
            this.cmb_Section.Name = "cmb_Section";
            this.cmb_Section.Size = new System.Drawing.Size(73, 21);
            this.cmb_Section.TabIndex = 3;
            this.cmb_Section.SelectedIndexChanged += new System.EventHandler(this.cmb_Section_SelectedIndexChanged);
            // 
            // cmb_Entry
            // 
            this.cmb_Entry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Entry.FormattingEnabled = true;
            this.cmb_Entry.Location = new System.Drawing.Point(80, 1);
            this.cmb_Entry.Name = "cmb_Entry";
            this.cmb_Entry.Size = new System.Drawing.Size(73, 21);
            this.cmb_Entry.TabIndex = 5;
            this.cmb_Entry.SelectedIndexChanged += new System.EventHandler(this.cmb_Entry_SelectedIndexChanged);
            // 
            // entryEditor
            // 
            this.entryEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entryEditor.Location = new System.Drawing.Point(1, 24);
            this.entryEditor.Name = "entryEditor";
            this.entryEditor.Size = new System.Drawing.Size(874, 100);
            this.entryEditor.TabIndex = 4;
            // 
            // CustomSectionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmb_Entry);
            this.Controls.Add(this.entryEditor);
            this.Controls.Add(this.cmb_Section);
            this.Name = "CustomSectionEditor";
            this.Size = new System.Drawing.Size(876, 125);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_Section;
        private CustomEntryEditor entryEditor;
        private System.Windows.Forms.ComboBox cmb_Entry;
    }
}
