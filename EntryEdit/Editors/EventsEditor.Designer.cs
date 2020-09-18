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
            this.btn_Up = new System.Windows.Forms.Button();
            this.btn_Down = new System.Windows.Forms.Button();
            this.eventEditor = new EntryEdit.Editors.EventEditor();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.btn_Reload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmb_Event
            // 
            this.cmb_Event.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Event.FormattingEnabled = true;
            this.cmb_Event.Location = new System.Drawing.Point(1, 1);
            this.cmb_Event.Name = "cmb_Event";
            this.cmb_Event.Size = new System.Drawing.Size(550, 21);
            this.cmb_Event.TabIndex = 0;
            this.cmb_Event.SelectedIndexChanged += new System.EventHandler(this.cmb_Event_SelectedIndexChanged);
            // 
            // btn_Up
            // 
            this.btn_Up.Location = new System.Drawing.Point(581, 0);
            this.btn_Up.Name = "btn_Up";
            this.btn_Up.Size = new System.Drawing.Size(30, 24);
            this.btn_Up.TabIndex = 14;
            this.btn_Up.Text = "↑";
            this.btn_Up.UseVisualStyleBackColor = true;
            this.btn_Up.Click += new System.EventHandler(this.btn_Up_Click);
            // 
            // btn_Down
            // 
            this.btn_Down.Location = new System.Drawing.Point(614, 0);
            this.btn_Down.Name = "btn_Down";
            this.btn_Down.Size = new System.Drawing.Size(30, 24);
            this.btn_Down.TabIndex = 15;
            this.btn_Down.Text = "↓";
            this.btn_Down.UseVisualStyleBackColor = true;
            this.btn_Down.Click += new System.EventHandler(this.btn_Down_Click);
            // 
            // eventEditor
            // 
            this.eventEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventEditor.Location = new System.Drawing.Point(0, 25);
            this.eventEditor.Name = "eventEditor";
            this.eventEditor.Size = new System.Drawing.Size(877, 682);
            this.eventEditor.TabIndex = 1;
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(674, 0);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(50, 24);
            this.btn_Clear.TabIndex = 17;
            this.btn_Clear.Text = "Clear";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btn_Reload
            // 
            this.btn_Reload.Location = new System.Drawing.Point(727, 0);
            this.btn_Reload.Name = "btn_Reload";
            this.btn_Reload.Size = new System.Drawing.Size(50, 24);
            this.btn_Reload.TabIndex = 18;
            this.btn_Reload.Text = "Reload";
            this.btn_Reload.UseVisualStyleBackColor = true;
            this.btn_Reload.Click += new System.EventHandler(this.btn_Reload_Click);
            // 
            // EventsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_Reload);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_Down);
            this.Controls.Add(this.btn_Up);
            this.Controls.Add(this.eventEditor);
            this.Controls.Add(this.cmb_Event);
            this.Name = "EventsEditor";
            this.Size = new System.Drawing.Size(877, 710);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_Event;
        private EventEditor eventEditor;
        private System.Windows.Forms.Button btn_Up;
        private System.Windows.Forms.Button btn_Down;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Button btn_Reload;
    }
}
