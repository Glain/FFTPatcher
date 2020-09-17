namespace EntryEdit.Editors
{
    partial class CommandListEditor
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
            this.btn_Add = new System.Windows.Forms.Button();
            this.tlp_Commands = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Page_Prev = new System.Windows.Forms.Button();
            this.btn_Page_Next = new System.Windows.Forms.Button();
            this.spinner_Page = new System.Windows.Forms.NumericUpDown();
            this.btn_Page_First = new System.Windows.Forms.Button();
            this.btn_Page_Last = new System.Windows.Forms.Button();
            this.btn_CheckAll = new System.Windows.Forms.Button();
            this.btn_UncheckAll = new System.Windows.Forms.Button();
            this.btn_ToggleAll = new System.Windows.Forms.Button();
            this.btn_Up = new System.Windows.Forms.Button();
            this.btn_Down = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Page)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(294, 1);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(30, 24);
            this.btn_Add.TabIndex = 0;
            this.btn_Add.Text = "+";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // tlp_Commands
            // 
            this.tlp_Commands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlp_Commands.AutoScroll = true;
            this.tlp_Commands.ColumnCount = 1;
            this.tlp_Commands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 874F));
            this.tlp_Commands.Location = new System.Drawing.Point(0, 27);
            this.tlp_Commands.Name = "tlp_Commands";
            this.tlp_Commands.Size = new System.Drawing.Size(874, 523);
            this.tlp_Commands.TabIndex = 1;
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(327, 1);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(30, 24);
            this.btn_Delete.TabIndex = 2;
            this.btn_Delete.Text = "-";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Page_Prev
            // 
            this.btn_Page_Prev.Location = new System.Drawing.Point(33, 1);
            this.btn_Page_Prev.Name = "btn_Page_Prev";
            this.btn_Page_Prev.Size = new System.Drawing.Size(30, 24);
            this.btn_Page_Prev.TabIndex = 3;
            this.btn_Page_Prev.Text = "<";
            this.btn_Page_Prev.UseVisualStyleBackColor = true;
            this.btn_Page_Prev.Click += new System.EventHandler(this.btn_Page_Prev_Click);
            // 
            // btn_Page_Next
            // 
            this.btn_Page_Next.Location = new System.Drawing.Point(110, 1);
            this.btn_Page_Next.Name = "btn_Page_Next";
            this.btn_Page_Next.Size = new System.Drawing.Size(30, 24);
            this.btn_Page_Next.TabIndex = 4;
            this.btn_Page_Next.Text = ">";
            this.btn_Page_Next.UseVisualStyleBackColor = true;
            this.btn_Page_Next.Click += new System.EventHandler(this.btn_Page_Next_Click);
            // 
            // spinner_Page
            // 
            this.spinner_Page.Location = new System.Drawing.Point(68, 3);
            this.spinner_Page.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinner_Page.Name = "spinner_Page";
            this.spinner_Page.Size = new System.Drawing.Size(37, 20);
            this.spinner_Page.TabIndex = 5;
            this.spinner_Page.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinner_Page.ValueChanged += new System.EventHandler(this.spinner_Page_ValueChanged);
            // 
            // btn_Page_First
            // 
            this.btn_Page_First.Location = new System.Drawing.Point(0, 1);
            this.btn_Page_First.Name = "btn_Page_First";
            this.btn_Page_First.Size = new System.Drawing.Size(30, 24);
            this.btn_Page_First.TabIndex = 6;
            this.btn_Page_First.Text = "<<";
            this.btn_Page_First.UseVisualStyleBackColor = true;
            this.btn_Page_First.Click += new System.EventHandler(this.btn_Page_First_Click);
            // 
            // btn_Page_Last
            // 
            this.btn_Page_Last.Location = new System.Drawing.Point(143, 1);
            this.btn_Page_Last.Name = "btn_Page_Last";
            this.btn_Page_Last.Size = new System.Drawing.Size(30, 24);
            this.btn_Page_Last.TabIndex = 7;
            this.btn_Page_Last.Text = ">>";
            this.btn_Page_Last.UseVisualStyleBackColor = true;
            this.btn_Page_Last.Click += new System.EventHandler(this.btn_Page_Last_Click);
            // 
            // btn_CheckAll
            // 
            this.btn_CheckAll.Location = new System.Drawing.Point(400, 1);
            this.btn_CheckAll.Name = "btn_CheckAll";
            this.btn_CheckAll.Size = new System.Drawing.Size(77, 24);
            this.btn_CheckAll.TabIndex = 8;
            this.btn_CheckAll.Text = "Check All";
            this.btn_CheckAll.UseVisualStyleBackColor = true;
            this.btn_CheckAll.Click += new System.EventHandler(this.btn_CheckAll_Click);
            // 
            // btn_UncheckAll
            // 
            this.btn_UncheckAll.Location = new System.Drawing.Point(480, 1);
            this.btn_UncheckAll.Name = "btn_UncheckAll";
            this.btn_UncheckAll.Size = new System.Drawing.Size(77, 24);
            this.btn_UncheckAll.TabIndex = 9;
            this.btn_UncheckAll.Text = "Uncheck All";
            this.btn_UncheckAll.UseVisualStyleBackColor = true;
            this.btn_UncheckAll.Click += new System.EventHandler(this.btn_UncheckAll_Click);
            // 
            // btn_ToggleAll
            // 
            this.btn_ToggleAll.Location = new System.Drawing.Point(560, 1);
            this.btn_ToggleAll.Name = "btn_ToggleAll";
            this.btn_ToggleAll.Size = new System.Drawing.Size(77, 24);
            this.btn_ToggleAll.TabIndex = 10;
            this.btn_ToggleAll.Text = "Toggle All";
            this.btn_ToggleAll.UseVisualStyleBackColor = true;
            this.btn_ToggleAll.Click += new System.EventHandler(this.btn_ToggleAll_Click);
            // 
            // btn_Up
            // 
            this.btn_Up.Location = new System.Drawing.Point(202, 1);
            this.btn_Up.Name = "btn_Up";
            this.btn_Up.Size = new System.Drawing.Size(30, 24);
            this.btn_Up.TabIndex = 11;
            this.btn_Up.Text = "↑";
            this.btn_Up.UseVisualStyleBackColor = true;
            this.btn_Up.Click += new System.EventHandler(this.btn_Up_Click);
            // 
            // btn_Down
            // 
            this.btn_Down.Location = new System.Drawing.Point(235, 1);
            this.btn_Down.Name = "btn_Down";
            this.btn_Down.Size = new System.Drawing.Size(30, 24);
            this.btn_Down.TabIndex = 12;
            this.btn_Down.Text = "↓";
            this.btn_Down.UseVisualStyleBackColor = true;
            this.btn_Down.Click += new System.EventHandler(this.btn_Down_Click);
            // 
            // CommandListEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_Down);
            this.Controls.Add(this.btn_Up);
            this.Controls.Add(this.btn_ToggleAll);
            this.Controls.Add(this.btn_UncheckAll);
            this.Controls.Add(this.btn_CheckAll);
            this.Controls.Add(this.btn_Page_Last);
            this.Controls.Add(this.btn_Page_First);
            this.Controls.Add(this.spinner_Page);
            this.Controls.Add(this.btn_Page_Next);
            this.Controls.Add(this.btn_Page_Prev);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.tlp_Commands);
            this.Controls.Add(this.btn_Add);
            this.Name = "CommandListEditor";
            this.Size = new System.Drawing.Size(876, 553);
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Page)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.TableLayoutPanel tlp_Commands;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Page_Prev;
        private System.Windows.Forms.Button btn_Page_Next;
        private System.Windows.Forms.NumericUpDown spinner_Page;
        private System.Windows.Forms.Button btn_Page_First;
        private System.Windows.Forms.Button btn_Page_Last;
        private System.Windows.Forms.Button btn_CheckAll;
        private System.Windows.Forms.Button btn_UncheckAll;
        private System.Windows.Forms.Button btn_ToggleAll;
        private System.Windows.Forms.Button btn_Up;
        private System.Windows.Forms.Button btn_Down;
    }
}
