namespace EntryEdit.Editors
{
    partial class EventsEditor
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
            this.cmb_Event = new System.Windows.Forms.ComboBox();
            this.eventEditor1 = new EntryEdit.Editors.EventEditor();
            this.SuspendLayout();
            // 
            // cmb_Event
            // 
            this.cmb_Event.FormattingEnabled = true;
            this.cmb_Event.Location = new System.Drawing.Point(11, 11);
            this.cmb_Event.Name = "cmb_Event";
            this.cmb_Event.Size = new System.Drawing.Size(249, 21);
            this.cmb_Event.TabIndex = 0;
            // 
            // eventEditor1
            // 
            this.eventEditor1.Location = new System.Drawing.Point(15, 48);
            this.eventEditor1.Name = "eventEditor1";
            this.eventEditor1.Size = new System.Drawing.Size(380, 220);
            this.eventEditor1.TabIndex = 1;
            // 
            // EventsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventEditor1);
            this.Controls.Add(this.cmb_Event);
            this.Name = "EventsEditor";
            this.Size = new System.Drawing.Size(410, 280);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_Event;
        private EventEditor eventEditor1;
    }
}
