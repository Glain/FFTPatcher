namespace EntryEdit.Editors
{
    partial class ConditionalSetsEditor
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
            this.cmb_ConditionalSet = new System.Windows.Forms.ComboBox();
            this.conditionalSetEditor = new EntryEdit.Editors.ConditionalSetEditor();
            this.SuspendLayout();
            // 
            // cmb_ConditionalSet
            // 
            this.cmb_ConditionalSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_ConditionalSet.FormattingEnabled = true;
            this.cmb_ConditionalSet.Location = new System.Drawing.Point(11, 11);
            this.cmb_ConditionalSet.Name = "cmb_ConditionalSet";
            this.cmb_ConditionalSet.Size = new System.Drawing.Size(550, 21);
            this.cmb_ConditionalSet.TabIndex = 1;
            this.cmb_ConditionalSet.SelectedIndexChanged += new System.EventHandler(this.cmb_Event_SelectedIndexChanged);
            // 
            // conditionalSetEditor
            // 
            this.conditionalSetEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.conditionalSetEditor.Location = new System.Drawing.Point(15, 48);
            this.conditionalSetEditor.Name = "conditionalSetEditor";
            this.conditionalSetEditor.Size = new System.Drawing.Size(620, 350);
            this.conditionalSetEditor.TabIndex = 2;
            // 
            // ConditionalSetsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.conditionalSetEditor);
            this.Controls.Add(this.cmb_ConditionalSet);
            this.Name = "ConditionalSetsEditor";
            this.Size = new System.Drawing.Size(650, 410);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_ConditionalSet;
        private ConditionalSetEditor conditionalSetEditor;
    }
}
