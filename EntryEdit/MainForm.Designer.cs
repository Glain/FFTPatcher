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
            this.menuItem_NewPatch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_File_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_LoadPatch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_SavePatch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_File_Separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_LoadScript = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_SaveScript = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_File_Separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_LoadAllScripts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_SaveAllScripts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_File_Separator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Edit_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_SetDefaults = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_RestoreDefaults = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_View = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_CheckSize = new System.Windows.Forms.ToolStripMenuItem();
            this.battleConditionalSetsEditor = new EntryEdit.Editors.ConditionalSetsEditor();
            this.worldConditionalSetsEditor = new EntryEdit.Editors.ConditionalSetsEditor();
            this.eventsEditor = new EntryEdit.Editors.EventsEditor();
            this.menuItem_Edit_Separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_ClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_DeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_ReloadAll = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tabControl.Location = new System.Drawing.Point(0, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1000, 791);
            this.tabControl.TabIndex = 0;
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(tabControl_Selecting);
            // 
            // tabPage_BattleConditionals
            // 
            this.tabPage_BattleConditionals.Controls.Add(this.battleConditionalSetsEditor);
            this.tabPage_BattleConditionals.Location = new System.Drawing.Point(4, 22);
            this.tabPage_BattleConditionals.Name = "tabPage_BattleConditionals";
            this.tabPage_BattleConditionals.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_BattleConditionals.Size = new System.Drawing.Size(992, 765);
            this.tabPage_BattleConditionals.TabIndex = 0;
            this.tabPage_BattleConditionals.Text = "Battle Conditionals";
            this.tabPage_BattleConditionals.UseVisualStyleBackColor = true;
            // 
            // tabPage_WorldConditionals
            // 
            this.tabPage_WorldConditionals.Controls.Add(this.worldConditionalSetsEditor);
            this.tabPage_WorldConditionals.Location = new System.Drawing.Point(4, 22);
            this.tabPage_WorldConditionals.Name = "tabPage_WorldConditionals";
            this.tabPage_WorldConditionals.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_WorldConditionals.Size = new System.Drawing.Size(992, 765);
            this.tabPage_WorldConditionals.TabIndex = 1;
            this.tabPage_WorldConditionals.Text = "World Conditionals";
            this.tabPage_WorldConditionals.UseVisualStyleBackColor = true;
            // 
            // tabPage_Events
            // 
            this.tabPage_Events.Controls.Add(this.eventsEditor);
            this.tabPage_Events.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Events.Name = "tabPage_Events";
            this.tabPage_Events.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Events.Size = new System.Drawing.Size(992, 765);
            this.tabPage_Events.TabIndex = 2;
            this.tabPage_Events.Text = "Events";
            this.tabPage_Events.UseVisualStyleBackColor = true;
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_File,
            this.menuItem_Edit,
            this.menuItem_View});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(1004, 24);
            this.menuBar.TabIndex = 1;
            this.menuBar.Text = "menuStrip1";
            // 
            // menuItem_File
            // 
            this.menuItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_NewPatch,
            this.menuItem_File_Separator1,
            this.menuItem_LoadPatch,
            this.menuItem_SavePatch,
            this.menuItem_File_Separator2,
            this.menuItem_LoadScript,
            this.menuItem_SaveScript,
            this.menuItem_File_Separator3,
            this.menuItem_LoadAllScripts,
            this.menuItem_SaveAllScripts,
            this.menuItem_File_Separator4,
            this.menuItem_Exit});
            this.menuItem_File.Name = "menuItem_File";
            this.menuItem_File.Size = new System.Drawing.Size(37, 20);
            this.menuItem_File.Text = "File";
            // 
            // menuItem_NewPatch
            // 
            this.menuItem_NewPatch.Name = "menuItem_NewPatch";
            this.menuItem_NewPatch.Size = new System.Drawing.Size(164, 22);
            this.menuItem_NewPatch.Text = "New Patch";
            this.menuItem_NewPatch.Click += new System.EventHandler(this.menuItem_NewPatch_Click);
            // 
            // menuItem_File_Separator1
            // 
            this.menuItem_File_Separator1.Name = "menuItem_File_Separator1";
            this.menuItem_File_Separator1.Size = new System.Drawing.Size(161, 6);
            // 
            // menuItem_LoadPatch
            // 
            this.menuItem_LoadPatch.Name = "menuItem_LoadPatch";
            this.menuItem_LoadPatch.Size = new System.Drawing.Size(164, 22);
            this.menuItem_LoadPatch.Text = "Load Patch...";
            this.menuItem_LoadPatch.Click += new System.EventHandler(this.menuItem_LoadPatch_Click);
            // 
            // menuItem_SavePatch
            // 
            this.menuItem_SavePatch.Enabled = false;
            this.menuItem_SavePatch.Name = "menuItem_SavePatch";
            this.menuItem_SavePatch.Size = new System.Drawing.Size(164, 22);
            this.menuItem_SavePatch.Text = "Save Patch...";
            this.menuItem_SavePatch.Click += new System.EventHandler(this.menuItem_SavePatch_Click);
            // 
            // menuItem_File_Separator2
            // 
            this.menuItem_File_Separator2.Name = "menuItem_File_Separator2";
            this.menuItem_File_Separator2.Size = new System.Drawing.Size(161, 6);
            // 
            // menuItem_LoadScript
            // 
            this.menuItem_LoadScript.Enabled = false;
            this.menuItem_LoadScript.Name = "menuItem_LoadScript";
            this.menuItem_LoadScript.Size = new System.Drawing.Size(164, 22);
            this.menuItem_LoadScript.Text = "Load Script...";
            this.menuItem_LoadScript.Click += new System.EventHandler(this.menuItem_LoadScript_Click);
            // 
            // menuItem_SaveScript
            // 
            this.menuItem_SaveScript.Enabled = false;
            this.menuItem_SaveScript.Name = "menuItem_SaveScript";
            this.menuItem_SaveScript.Size = new System.Drawing.Size(164, 22);
            this.menuItem_SaveScript.Text = "Save Script...";
            this.menuItem_SaveScript.Click += new System.EventHandler(this.menuItem_SaveScript_Click);
            // 
            // menuItem_File_Separator3
            // 
            this.menuItem_File_Separator3.Name = "menuItem_File_Separator3";
            this.menuItem_File_Separator3.Size = new System.Drawing.Size(161, 6);
            // 
            // menuItem_LoadAllScripts
            // 
            this.menuItem_LoadAllScripts.Enabled = false;
            this.menuItem_LoadAllScripts.Name = "menuItem_LoadAllScripts";
            this.menuItem_LoadAllScripts.Size = new System.Drawing.Size(164, 22);
            this.menuItem_LoadAllScripts.Text = "Load All Scripts...";
            this.menuItem_LoadAllScripts.Click += new System.EventHandler(this.menuItem_LoadAllScripts_Click);
            // 
            // menuItem_SaveAllScripts
            // 
            this.menuItem_SaveAllScripts.Enabled = false;
            this.menuItem_SaveAllScripts.Name = "menuItem_SaveAllScripts";
            this.menuItem_SaveAllScripts.Size = new System.Drawing.Size(164, 22);
            this.menuItem_SaveAllScripts.Text = "Save All Scripts...";
            this.menuItem_SaveAllScripts.Click += new System.EventHandler(this.menuItem_SaveAllScripts_Click);
            // 
            // menuItem_File_Separator4
            // 
            this.menuItem_File_Separator4.Name = "menuItem_File_Separator4";
            this.menuItem_File_Separator4.Size = new System.Drawing.Size(161, 6);
            // 
            // menuItem_Exit
            // 
            this.menuItem_Exit.Name = "menuItem_Exit";
            this.menuItem_Exit.Size = new System.Drawing.Size(164, 22);
            this.menuItem_Exit.Text = "Exit";
            this.menuItem_Exit.Click += new System.EventHandler(this.menuItem_Exit_Click);
            // 
            // menuItem_Edit
            // 
            this.menuItem_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_Copy,
            this.menuItem_Paste,
            this.menuItem_Edit_Separator1,
            this.menuItem_SetDefaults,
            this.menuItem_RestoreDefaults,
            this.menuItem_Edit_Separator2,
            this.menuItem_ClearAll,
            this.menuItem_DeleteAll,
            this.menuItem_ReloadAll});
            this.menuItem_Edit.Enabled = false;
            this.menuItem_Edit.Name = "menuItem_Edit";
            this.menuItem_Edit.Size = new System.Drawing.Size(39, 20);
            this.menuItem_Edit.Text = "Edit";
            // 
            // menuItem_Copy
            // 
            this.menuItem_Copy.Name = "menuItem_Copy";
            this.menuItem_Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuItem_Copy.Size = new System.Drawing.Size(220, 22);
            this.menuItem_Copy.Text = "Copy Entry";
            this.menuItem_Copy.Click += new System.EventHandler(this.menuItem_Copy_Click);
            // 
            // menuItem_Paste
            // 
            this.menuItem_Paste.Name = "menuItem_Paste";
            this.menuItem_Paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.menuItem_Paste.Size = new System.Drawing.Size(220, 22);
            this.menuItem_Paste.Text = "Paste Entry";
            this.menuItem_Paste.Click += new System.EventHandler(this.menuItem_Paste_Click);
            // 
            // menuItem_Edit_Separator1
            // 
            this.menuItem_Edit_Separator1.Name = "menuItem_Edit_Separator1";
            this.menuItem_Edit_Separator1.Size = new System.Drawing.Size(217, 6);
            // 
            // menuItem_SetDefaults
            // 
            this.menuItem_SetDefaults.Name = "menuItem_SetDefaults";
            this.menuItem_SetDefaults.Size = new System.Drawing.Size(220, 22);
            this.menuItem_SetDefaults.Text = "Set Current Data as Defaults";
            this.menuItem_SetDefaults.Click += new System.EventHandler(this.menuItem_SetDefaults_Click);
            // 
            // menuItem_RestoreDefaults
            // 
            this.menuItem_RestoreDefaults.Name = "menuItem_RestoreDefaults";
            this.menuItem_RestoreDefaults.Size = new System.Drawing.Size(220, 22);
            this.menuItem_RestoreDefaults.Text = "Restore Defaults";
            this.menuItem_RestoreDefaults.Click += new System.EventHandler(this.menuItem_RestoreDefaults_Click);
            // 
            // menuItem_View
            // 
            this.menuItem_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_CheckSize});
            this.menuItem_View.Enabled = false;
            this.menuItem_View.Name = "menuItem_View";
            this.menuItem_View.Size = new System.Drawing.Size(44, 20);
            this.menuItem_View.Text = "View";
            // 
            // menuItem_CheckSize
            // 
            this.menuItem_CheckSize.Name = "menuItem_CheckSize";
            this.menuItem_CheckSize.Size = new System.Drawing.Size(130, 22);
            this.menuItem_CheckSize.Text = "Check Size";
            this.menuItem_CheckSize.Click += new System.EventHandler(this.menuItem_CheckSize_Click);
            // 
            // battleConditionalSetsEditor
            // 
            this.battleConditionalSetsEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.battleConditionalSetsEditor.Location = new System.Drawing.Point(4, 4);
            this.battleConditionalSetsEditor.Name = "battleConditionalSetsEditor";
            this.battleConditionalSetsEditor.Size = new System.Drawing.Size(987, 762);
            this.battleConditionalSetsEditor.TabIndex = 0;
            // 
            // worldConditionalSetsEditor
            // 
            this.worldConditionalSetsEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.worldConditionalSetsEditor.Location = new System.Drawing.Point(4, 4);
            this.worldConditionalSetsEditor.Name = "worldConditionalSetsEditor";
            this.worldConditionalSetsEditor.Size = new System.Drawing.Size(987, 762);
            this.worldConditionalSetsEditor.TabIndex = 1;
            // 
            // eventsEditor
            // 
            this.eventsEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventsEditor.Location = new System.Drawing.Point(4, 4);
            this.eventsEditor.Name = "eventsEditor";
            this.eventsEditor.Size = new System.Drawing.Size(987, 762);
            this.eventsEditor.TabIndex = 0;
            // 
            // menuItem_Edit_Separator2
            // 
            this.menuItem_Edit_Separator2.Name = "menuItem_Edit_Separator2";
            this.menuItem_Edit_Separator2.Size = new System.Drawing.Size(217, 6);
            // 
            // menuItem_ClearAll
            // 
            this.menuItem_ClearAll.Name = "menuItem_ClearAll";
            this.menuItem_ClearAll.Size = new System.Drawing.Size(220, 22);
            this.menuItem_ClearAll.Text = "Clear All";
            this.menuItem_ClearAll.Click += new System.EventHandler(this.menuItem_ClearAll_Click);
            // 
            // menuItem_DeleteAll
            // 
            this.menuItem_DeleteAll.Name = "menuItem_DeleteAll";
            this.menuItem_DeleteAll.Size = new System.Drawing.Size(220, 22);
            this.menuItem_DeleteAll.Text = "Delete All";
            this.menuItem_DeleteAll.Click += new System.EventHandler(this.menuItem_DeleteAll_Click);
            // 
            // menuItem_ReloadAll
            // 
            this.menuItem_ReloadAll.Name = "menuItem_ReloadAll";
            this.menuItem_ReloadAll.Size = new System.Drawing.Size(220, 22);
            this.menuItem_ReloadAll.Text = "Reload All";
            this.menuItem_ReloadAll.Click += new System.EventHandler(this.menuItem_ReloadAll_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 821);
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
        private Editors.ConditionalSetsEditor battleConditionalSetsEditor;
        private Editors.ConditionalSetsEditor worldConditionalSetsEditor;
        private Editors.EventsEditor eventsEditor;
        private System.Windows.Forms.ToolStripMenuItem menuItem_NewPatch;
        private System.Windows.Forms.ToolStripSeparator menuItem_File_Separator1;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Edit;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Copy;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Paste;
        private System.Windows.Forms.ToolStripSeparator menuItem_Edit_Separator1;
        private System.Windows.Forms.ToolStripMenuItem menuItem_SetDefaults;
        private System.Windows.Forms.ToolStripMenuItem menuItem_RestoreDefaults;
        private System.Windows.Forms.ToolStripMenuItem menuItem_SaveScript;
        private System.Windows.Forms.ToolStripSeparator menuItem_File_Separator2;
        private System.Windows.Forms.ToolStripMenuItem menuItem_LoadScript;
        private System.Windows.Forms.ToolStripMenuItem menuItem_View;
        private System.Windows.Forms.ToolStripMenuItem menuItem_CheckSize;
        private System.Windows.Forms.ToolStripMenuItem menuItem_LoadPatch;
        private System.Windows.Forms.ToolStripMenuItem menuItem_SavePatch;
        private System.Windows.Forms.ToolStripSeparator menuItem_File_Separator3;
        private System.Windows.Forms.ToolStripMenuItem menuItem_LoadAllScripts;
        private System.Windows.Forms.ToolStripMenuItem menuItem_SaveAllScripts;
        private System.Windows.Forms.ToolStripSeparator menuItem_File_Separator4;
        private System.Windows.Forms.ToolStripSeparator menuItem_Edit_Separator2;
        private System.Windows.Forms.ToolStripMenuItem menuItem_ClearAll;
        private System.Windows.Forms.ToolStripMenuItem menuItem_DeleteAll;
        private System.Windows.Forms.ToolStripMenuItem menuItem_ReloadAll;
    }
}

