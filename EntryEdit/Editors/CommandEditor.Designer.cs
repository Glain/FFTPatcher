namespace EntryEdit.Editors
{
    partial class CommandEditor
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
            this.cmb_Command = new System.Windows.Forms.ComboBox();
            this.flp_Parameters = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // cmb_Command
            // 
            this.cmb_Command.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Command.FormattingEnabled = true;
            this.cmb_Command.Location = new System.Drawing.Point(1, 1);
            this.cmb_Command.Name = "cmb_Command";
            this.cmb_Command.Size = new System.Drawing.Size(172, 21);
            this.cmb_Command.TabIndex = 0;
            // 
            // flp_Parameters
            // 
            this.flp_Parameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flp_Parameters.Location = new System.Drawing.Point(180, 1);
            this.flp_Parameters.Name = "flp_Parameters";
            this.flp_Parameters.Size = new System.Drawing.Size(268, 27);
            this.flp_Parameters.TabIndex = 1;
            // 
            // CommandEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flp_Parameters);
            this.Controls.Add(this.cmb_Command);
            this.Name = "CommandEditor";
            this.Size = new System.Drawing.Size(450, 30);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_Command;
        private System.Windows.Forms.FlowLayoutPanel flp_Parameters;
    }
}
