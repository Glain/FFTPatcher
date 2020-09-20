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
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Up = new System.Windows.Forms.Button();
            this.btn_Down = new System.Windows.Forms.Button();
            this.commandListEditor = new EntryEdit.Editors.CommandListEditor();
            this.SuspendLayout();
            // 
            // cmb_Block
            // 
            this.cmb_Block.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Block.FormattingEnabled = true;
            this.cmb_Block.Location = new System.Drawing.Point(1, 7);
            this.cmb_Block.Name = "cmb_Block";
            this.cmb_Block.Size = new System.Drawing.Size(550, 21);
            this.cmb_Block.TabIndex = 2;
            this.cmb_Block.SelectedIndexChanged += new System.EventHandler(this.cmb_Block_SelectedIndexChanged);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(674, 6);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(30, 24);
            this.btn_Add.TabIndex = 4;
            this.btn_Add.Text = "+";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(707, 6);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(30, 24);
            this.btn_Delete.TabIndex = 5;
            this.btn_Delete.Text = "-";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Up
            // 
            this.btn_Up.Location = new System.Drawing.Point(581, 6);
            this.btn_Up.Name = "btn_Up";
            this.btn_Up.Size = new System.Drawing.Size(30, 24);
            this.btn_Up.TabIndex = 12;
            this.btn_Up.Text = "↑";
            this.btn_Up.UseVisualStyleBackColor = true;
            this.btn_Up.Click += new System.EventHandler(this.btn_Up_Click);
            // 
            // btn_Down
            // 
            this.btn_Down.Location = new System.Drawing.Point(614, 6);
            this.btn_Down.Name = "btn_Down";
            this.btn_Down.Size = new System.Drawing.Size(30, 24);
            this.btn_Down.TabIndex = 13;
            this.btn_Down.Text = "↓";
            this.btn_Down.UseVisualStyleBackColor = true;
            this.btn_Down.Click += new System.EventHandler(this.btn_Down_Click);
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
            this.Controls.Add(this.btn_Down);
            this.Controls.Add(this.btn_Up);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.commandListEditor);
            this.Controls.Add(this.cmb_Block);
            this.Name = "ConditionalSetEditor";
            this.Size = new System.Drawing.Size(877, 682);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_Block;
        private CommandListEditor commandListEditor;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Up;
        private System.Windows.Forms.Button btn_Down;
    }
}
