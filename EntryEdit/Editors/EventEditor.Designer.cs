namespace EntryEdit.Editors
{
    partial class EventEditor
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
            this.textSectionEditor = new EntryEdit.Editors.CustomSectionEditor();
            this.commandListEditor = new EntryEdit.Editors.CommandListEditor();
            this.SuspendLayout();
            // 
            // textSectionEditor
            // 
            this.textSectionEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSectionEditor.Location = new System.Drawing.Point(0, 556);
            this.textSectionEditor.Name = "textSectionEditor";
            this.textSectionEditor.Size = new System.Drawing.Size(876, 125);
            this.textSectionEditor.TabIndex = 2;
            // 
            // commandListEditor
            // 
            this.commandListEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandListEditor.Location = new System.Drawing.Point(0, 0);
            this.commandListEditor.Name = "commandListEditor";
            this.commandListEditor.Size = new System.Drawing.Size(876, 553);
            this.commandListEditor.TabIndex = 0;
            // 
            // EventEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textSectionEditor);
            this.Controls.Add(this.commandListEditor);
            this.Name = "EventEditor";
            this.Size = new System.Drawing.Size(877, 682);
            this.ResumeLayout(false);

        }

        #endregion

        private CommandListEditor commandListEditor;
        private CustomSectionEditor textSectionEditor;
    }
}
