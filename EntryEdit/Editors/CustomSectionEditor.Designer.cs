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
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.entryEditor = new EntryEdit.Editors.CustomEntryEditor();
            this.btn_Add_UseDefault = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.btn_Reload = new System.Windows.Forms.Button();
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
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(193, 0);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(30, 24);
            this.btn_Add.TabIndex = 6;
            this.btn_Add.Text = "+";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(259, 0);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(30, 24);
            this.btn_Delete.TabIndex = 7;
            this.btn_Delete.Text = "-";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
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
            // btn_Add_UseDefault
            // 
            this.btn_Add_UseDefault.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_Add_UseDefault.Location = new System.Drawing.Point(226, 0);
            this.btn_Add_UseDefault.Name = "btn_Add_UseDefault";
            this.btn_Add_UseDefault.Size = new System.Drawing.Size(30, 24);
            this.btn_Add_UseDefault.TabIndex = 8;
            this.btn_Add_UseDefault.Text = "+";
            this.btn_Add_UseDefault.UseVisualStyleBackColor = false;
            this.btn_Add_UseDefault.Click += new System.EventHandler(this.btn_Add_UseDefault_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(319, 0);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(50, 24);
            this.btn_Clear.TabIndex = 14;
            this.btn_Clear.Text = "Clear";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btn_Reload
            // 
            this.btn_Reload.Location = new System.Drawing.Point(372, 0);
            this.btn_Reload.Name = "btn_Reload";
            this.btn_Reload.Size = new System.Drawing.Size(50, 24);
            this.btn_Reload.TabIndex = 15;
            this.btn_Reload.Text = "Reload";
            this.btn_Reload.UseVisualStyleBackColor = true;
            this.btn_Reload.Click += new System.EventHandler(this.btn_Reload_Click);
            // 
            // CustomSectionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_Reload);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_Add_UseDefault);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.btn_Add);
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
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Add_UseDefault;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Button btn_Reload;
    }
}
