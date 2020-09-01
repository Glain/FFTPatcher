namespace EntryEdit
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_BattleConditionals = new System.Windows.Forms.TabPage();
            this.tabPage_WorldConditionals = new System.Windows.Forms.TabPage();
            this.tabPage_Events = new System.Windows.Forms.TabPage();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.menuItem_File = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionalSetsEditor1 = new EntryEdit.Editors.ConditionalSetsEditor();
            this.conditionalSetsEditor2 = new EntryEdit.Editors.ConditionalSetsEditor();
            this.eventEditor1 = new EntryEdit.Editors.EventsEditor();
            this.tabControl.SuspendLayout();
            this.tabPage_BattleConditionals.SuspendLayout();
            this.tabPage_WorldConditionals.SuspendLayout();
            this.tabPage_Events.SuspendLayout();
            this.menuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage_BattleConditionals);
            this.tabControl.Controls.Add(this.tabPage_WorldConditionals);
            this.tabControl.Controls.Add(this.tabPage_Events);
            this.tabControl.Location = new System.Drawing.Point(11, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(678, 458);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage_BattleConditionals
            // 
            this.tabPage_BattleConditionals.Controls.Add(this.conditionalSetsEditor1);
            this.tabPage_BattleConditionals.Location = new System.Drawing.Point(4, 22);
            this.tabPage_BattleConditionals.Name = "tabPage_BattleConditionals";
            this.tabPage_BattleConditionals.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_BattleConditionals.Size = new System.Drawing.Size(670, 432);
            this.tabPage_BattleConditionals.TabIndex = 0;
            this.tabPage_BattleConditionals.Text = "Battle Conditionals";
            this.tabPage_BattleConditionals.UseVisualStyleBackColor = true;
            // 
            // tabPage_WorldConditionals
            // 
            this.tabPage_WorldConditionals.Controls.Add(this.conditionalSetsEditor2);
            this.tabPage_WorldConditionals.Location = new System.Drawing.Point(4, 22);
            this.tabPage_WorldConditionals.Name = "tabPage_WorldConditionals";
            this.tabPage_WorldConditionals.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_WorldConditionals.Size = new System.Drawing.Size(670, 432);
            this.tabPage_WorldConditionals.TabIndex = 1;
            this.tabPage_WorldConditionals.Text = "World Conditionals";
            this.tabPage_WorldConditionals.UseVisualStyleBackColor = true;
            // 
            // tabPage_Events
            // 
            this.tabPage_Events.Controls.Add(this.eventEditor1);
            this.tabPage_Events.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Events.Name = "tabPage_Events";
            this.tabPage_Events.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Events.Size = new System.Drawing.Size(670, 432);
            this.tabPage_Events.TabIndex = 2;
            this.tabPage_Events.Text = "Events";
            this.tabPage_Events.UseVisualStyleBackColor = true;
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_File});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(700, 24);
            this.menuBar.TabIndex = 1;
            this.menuBar.Text = "menuStrip1";
            // 
            // menuItem_File
            // 
            this.menuItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_Exit});
            this.menuItem_File.Name = "menuItem_File";
            this.menuItem_File.Size = new System.Drawing.Size(37, 20);
            this.menuItem_File.Text = "File";
            // 
            // menuItem_Exit
            // 
            this.menuItem_Exit.Name = "menuItem_Exit";
            this.menuItem_Exit.Size = new System.Drawing.Size(92, 22);
            this.menuItem_Exit.Text = "Exit";
            this.menuItem_Exit.Click += new System.EventHandler(this.menuItem_Exit_Click);
            // 
            // conditionalSetsEditor1
            // 
            this.conditionalSetsEditor1.Location = new System.Drawing.Point(10, 10);
            this.conditionalSetsEditor1.Name = "conditionalSetsEditor1";
            this.conditionalSetsEditor1.Size = new System.Drawing.Size(650, 410);
            this.conditionalSetsEditor1.TabIndex = 0;
            // 
            // conditionalSetsEditor2
            // 
            this.conditionalSetsEditor2.Location = new System.Drawing.Point(10, 10);
            this.conditionalSetsEditor2.Name = "conditionalSetsEditor2";
            this.conditionalSetsEditor2.Size = new System.Drawing.Size(650, 410);
            this.conditionalSetsEditor2.TabIndex = 1;
            // 
            // eventEditor1
            // 
            this.eventEditor1.Location = new System.Drawing.Point(10, 10);
            this.eventEditor1.Name = "eventEditor1";
            this.eventEditor1.Size = new System.Drawing.Size(650, 410);
            this.eventEditor1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 497);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuBar);
            this.MainMenuStrip = this.menuBar;
            this.Name = "MainForm";
            this.Text = "Entry Edit";
            this.tabControl.ResumeLayout(false);
            this.tabPage_BattleConditionals.ResumeLayout(false);
            this.tabPage_WorldConditionals.ResumeLayout(false);
            this.tabPage_Events.ResumeLayout(false);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_BattleConditionals;
        private System.Windows.Forms.TabPage tabPage_WorldConditionals;
        private System.Windows.Forms.TabPage tabPage_Events;
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem menuItem_File;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Exit;
        private Editors.ConditionalSetsEditor conditionalSetsEditor1;
        private Editors.ConditionalSetsEditor conditionalSetsEditor2;
        private Editors.EventsEditor eventEditor1;
    }
}

