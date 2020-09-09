namespace EntryEdit.Editors
{
    partial class ConditionalSetEditor
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
            this.cmb_Block = new System.Windows.Forms.ComboBox();
            this.commandListEditor = new EntryEdit.Editors.CommandListEditor();
            this.SuspendLayout();
            // 
            // cmb_Block
            // 
            this.cmb_Block.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Block.FormattingEnabled = true;
            this.cmb_Block.Location = new System.Drawing.Point(1, 7);
            this.cmb_Block.Name = "cmb_Block";
            this.cmb_Block.Size = new System.Drawing.Size(73, 21);
            this.cmb_Block.TabIndex = 2;
            this.cmb_Block.SelectedIndexChanged += new System.EventHandler(this.cmb_Block_SelectedIndexChanged);
            // 
            // commandListEditor
            // 
            this.commandListEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandListEditor.Location = new System.Drawing.Point(0, 32);
            this.commandListEditor.Name = "commandListEditor";
            this.commandListEditor.Size = new System.Drawing.Size(876, 553);
            this.commandListEditor.TabIndex = 3;
            // 
            // ConditionalSetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.commandListEditor);
            this.Controls.Add(this.cmb_Block);
            this.Name = "ConditionalSetEditor";
            this.Size = new System.Drawing.Size(877, 682);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_Block;
        private CommandListEditor commandListEditor;
    }
}
