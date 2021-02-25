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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_BattleConditionals = new System.Windows.Forms.TabPage();
            this.battleConditionalSetsEditor = new EntryEdit.Editors.ConditionalSetsEditor();
            this.tabPage_WorldConditionals = new System.Windows.Forms.TabPage();
            this.worldConditionalSetsEditor = new EntryEdit.Editors.ConditionalSetsEditor();
            this.tabPage_Events = new System.Windows.Forms.TabPage();
            this.eventsEditor = new EntryEdit.Editors.EventsEditor();
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
            this.menuItem_CopyCommands = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_PasteCommands = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_CopyEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_PasteEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Edit_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_UseTrimmedDefaults = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_SetDefaults = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_RestoreDefaults = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Edit_Separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_ClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_DeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_ReloadAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_View = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_CheckSize = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Patch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_PatchISO = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_LoadISO = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Patch_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_PatchPSXSaveState = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_LoadPSXSaveState = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_About = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_SavePatchXML = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
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
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_File,
            this.menuItem_Edit,
            this.menuItem_View,
            this.menuItem_Patch,
            this.menuItem_About});
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
            this.menuItem_SavePatchXML,
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
            this.menuItem_NewPatch.Size = new System.Drawing.Size(167, 22);
            this.menuItem_NewPatch.Text = "New Patch";
            this.menuItem_NewPatch.Click += new System.EventHandler(this.menuItem_NewPatch_Click);
            // 
            // menuItem_File_Separator1
            // 
            this.menuItem_File_Separator1.Name = "menuItem_File_Separator1";
            this.menuItem_File_Separator1.Size = new System.Drawing.Size(164, 6);
            // 
            // menuItem_LoadPatch
            // 
            this.menuItem_LoadPatch.Name = "menuItem_LoadPatch";
            this.menuItem_LoadPatch.Size = new System.Drawing.Size(167, 22);
            this.menuItem_LoadPatch.Text = "Load Patch...";
            this.menuItem_LoadPatch.Click += new System.EventHandler(this.menuItem_LoadPatch_Click);
            // 
            // menuItem_SavePatch
            // 
            this.menuItem_SavePatch.Enabled = false;
            this.menuItem_SavePatch.Name = "menuItem_SavePatch";
            this.menuItem_SavePatch.Size = new System.Drawing.Size(167, 22);
            this.menuItem_SavePatch.Text = "Save Patch...";
            this.menuItem_SavePatch.Click += new System.EventHandler(this.menuItem_SavePatch_Click);
            // 
            // menuItem_File_Separator2
            // 
            this.menuItem_File_Separator2.Name = "menuItem_File_Separator2";
            this.menuItem_File_Separator2.Size = new System.Drawing.Size(164, 6);
            // 
            // menuItem_LoadScript
            // 
            this.menuItem_LoadScript.Enabled = false;
            this.menuItem_LoadScript.Name = "menuItem_LoadScript";
            this.menuItem_LoadScript.Size = new System.Drawing.Size(167, 22);
            this.menuItem_LoadScript.Text = "Load Script...";
            this.menuItem_LoadScript.Click += new System.EventHandler(this.menuItem_LoadScript_Click);
            // 
            // menuItem_SaveScript
            // 
            this.menuItem_SaveScript.Enabled = false;
            this.menuItem_SaveScript.Name = "menuItem_SaveScript";
            this.menuItem_SaveScript.Size = new System.Drawing.Size(167, 22);
            this.menuItem_SaveScript.Text = "Save Script...";
            this.menuItem_SaveScript.Click += new System.EventHandler(this.menuItem_SaveScript_Click);
            // 
            // menuItem_File_Separator3
            // 
            this.menuItem_File_Separator3.Name = "menuItem_File_Separator3";
            this.menuItem_File_Separator3.Size = new System.Drawing.Size(164, 6);
            // 
            // menuItem_LoadAllScripts
            // 
            this.menuItem_LoadAllScripts.Enabled = false;
            this.menuItem_LoadAllScripts.Name = "menuItem_LoadAllScripts";
            this.menuItem_LoadAllScripts.Size = new System.Drawing.Size(167, 22);
            this.menuItem_LoadAllScripts.Text = "Load All Scripts...";
            this.menuItem_LoadAllScripts.Click += new System.EventHandler(this.menuItem_LoadAllScripts_Click);
            // 
            // menuItem_SaveAllScripts
            // 
            this.menuItem_SaveAllScripts.Enabled = false;
            this.menuItem_SaveAllScripts.Name = "menuItem_SaveAllScripts";
            this.menuItem_SaveAllScripts.Size = new System.Drawing.Size(167, 22);
            this.menuItem_SaveAllScripts.Text = "Save All Scripts...";
            this.menuItem_SaveAllScripts.Click += new System.EventHandler(this.menuItem_SaveAllScripts_Click);
            // 
            // menuItem_File_Separator4
            // 
            this.menuItem_File_Separator4.Name = "menuItem_File_Separator4";
            this.menuItem_File_Separator4.Size = new System.Drawing.Size(164, 6);
            // 
            // menuItem_Exit
            // 
            this.menuItem_Exit.Name = "menuItem_Exit";
            this.menuItem_Exit.Size = new System.Drawing.Size(167, 22);
            this.menuItem_Exit.Text = "Exit";
            this.menuItem_Exit.Click += new System.EventHandler(this.menuItem_Exit_Click);
            // 
            // menuItem_Edit
            // 
            this.menuItem_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_CopyCommands,
            this.menuItem_PasteCommands,
            this.menuItem_CopyEntry,
            this.menuItem_PasteEntry,
            this.menuItem_Edit_Separator1,
            this.menuItem_UseTrimmedDefaults,
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
            // menuItem_CopyCommands
            // 
            this.menuItem_CopyCommands.Name = "menuItem_CopyCommands";
            this.menuItem_CopyCommands.Size = new System.Drawing.Size(220, 22);
            this.menuItem_CopyCommands.Text = "Copy Commands";
            this.menuItem_CopyCommands.Click += new System.EventHandler(this.menuItem_CopyCommands_Click);
            // 
            // menuItem_PasteCommands
            // 
            this.menuItem_PasteCommands.Name = "menuItem_PasteCommands";
            this.menuItem_PasteCommands.Size = new System.Drawing.Size(220, 22);
            this.menuItem_PasteCommands.Text = "Paste Commands";
            this.menuItem_PasteCommands.Click += new System.EventHandler(this.menuItem_PasteCommands_Click);
            // 
            // menuItem_CopyEntry
            // 
            this.menuItem_CopyEntry.Name = "menuItem_CopyEntry";
            this.menuItem_CopyEntry.Size = new System.Drawing.Size(220, 22);
            this.menuItem_CopyEntry.Text = "Copy Entry";
            this.menuItem_CopyEntry.Click += new System.EventHandler(this.menuItem_CopyEntry_Click);
            // 
            // menuItem_PasteEntry
            // 
            this.menuItem_PasteEntry.Name = "menuItem_PasteEntry";
            this.menuItem_PasteEntry.Size = new System.Drawing.Size(220, 22);
            this.menuItem_PasteEntry.Text = "Paste Entry";
            this.menuItem_PasteEntry.Click += new System.EventHandler(this.menuItem_PasteEntry_Click);
            // 
            // menuItem_Edit_Separator1
            // 
            this.menuItem_Edit_Separator1.Name = "menuItem_Edit_Separator1";
            this.menuItem_Edit_Separator1.Size = new System.Drawing.Size(217, 6);
            // 
            // menuItem_UseTrimmedDefaults
            // 
            this.menuItem_UseTrimmedDefaults.Name = "menuItem_UseTrimmedDefaults";
            this.menuItem_UseTrimmedDefaults.Size = new System.Drawing.Size(220, 22);
            this.menuItem_UseTrimmedDefaults.Text = "Use Trimmed Defaults";
            this.menuItem_UseTrimmedDefaults.Click += new System.EventHandler(this.menuItem_UseTrimmedDefaults_Click);
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
            // menuItem_Patch
            // 
            this.menuItem_Patch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_PatchISO,
            this.menuItem_LoadISO,
            this.menuItem_Patch_Separator1,
            this.menuItem_PatchPSXSaveState,
            this.menuItem_LoadPSXSaveState});
            this.menuItem_Patch.Name = "menuItem_Patch";
            this.menuItem_Patch.Size = new System.Drawing.Size(49, 20);
            this.menuItem_Patch.Text = "Patch";
            // 
            // menuItem_PatchISO
            // 
            this.menuItem_PatchISO.Enabled = false;
            this.menuItem_PatchISO.Name = "menuItem_PatchISO";
            this.menuItem_PatchISO.Size = new System.Drawing.Size(192, 22);
            this.menuItem_PatchISO.Text = "Patch PSX ISO...";
            this.menuItem_PatchISO.Click += new System.EventHandler(this.menuItem_PatchISO_Click);
            // 
            // menuItem_LoadISO
            // 
            this.menuItem_LoadISO.Name = "menuItem_LoadISO";
            this.menuItem_LoadISO.Size = new System.Drawing.Size(192, 22);
            this.menuItem_LoadISO.Text = "Load PSX ISO...";
            this.menuItem_LoadISO.Click += new System.EventHandler(this.menuItem_LoadISO_Click);
            // 
            // menuItem_Patch_Separator1
            // 
            this.menuItem_Patch_Separator1.Name = "menuItem_Patch_Separator1";
            this.menuItem_Patch_Separator1.Size = new System.Drawing.Size(189, 6);
            // 
            // menuItem_PatchPSXSaveState
            // 
            this.menuItem_PatchPSXSaveState.Enabled = false;
            this.menuItem_PatchPSXSaveState.Name = "menuItem_PatchPSXSaveState";
            this.menuItem_PatchPSXSaveState.Size = new System.Drawing.Size(192, 22);
            this.menuItem_PatchPSXSaveState.Text = "Patch pSX Save State...";
            this.menuItem_PatchPSXSaveState.Click += new System.EventHandler(this.menuItem_PatchPSXSaveState_Click);
            // 
            // menuItem_LoadPSXSaveState
            // 
            this.menuItem_LoadPSXSaveState.Enabled = false;
            this.menuItem_LoadPSXSaveState.Name = "menuItem_LoadPSXSaveState";
            this.menuItem_LoadPSXSaveState.Size = new System.Drawing.Size(192, 22);
            this.menuItem_LoadPSXSaveState.Text = "Load pSX Save State...";
            this.menuItem_LoadPSXSaveState.Click += new System.EventHandler(this.menuItem_LoadPSXSaveState_Click);
            // 
            // menuItem_About
            // 
            this.menuItem_About.Name = "menuItem_About";
            this.menuItem_About.Size = new System.Drawing.Size(61, 20);
            this.menuItem_About.Text = "About...";
            this.menuItem_About.Click += new System.EventHandler(this.menuItem_About_Click);
            // 
            // menuItem_SavePatchXML
            // 
            this.menuItem_SavePatchXML.Enabled = false;
            this.menuItem_SavePatchXML.Name = "menuItem_SavePatchXML";
            this.menuItem_SavePatchXML.Size = new System.Drawing.Size(167, 22);
            this.menuItem_SavePatchXML.Text = "Save Patch XML...";
            this.menuItem_SavePatchXML.Visible = false;
            this.menuItem_SavePatchXML.Click += new System.EventHandler(this.menuItem_SavePatchXML_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 821);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuBar;
            this.Name = "MainForm";
            this.Text = "EntryEdit";
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
        private System.Windows.Forms.ToolStripMenuItem menuItem_CopyEntry;
        private System.Windows.Forms.ToolStripMenuItem menuItem_PasteEntry;
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
        private System.Windows.Forms.ToolStripMenuItem menuItem_Patch;
        private System.Windows.Forms.ToolStripMenuItem menuItem_PatchISO;
        private System.Windows.Forms.ToolStripMenuItem menuItem_LoadISO;
        private System.Windows.Forms.ToolStripSeparator menuItem_Patch_Separator1;
        private System.Windows.Forms.ToolStripMenuItem menuItem_PatchPSXSaveState;
        private System.Windows.Forms.ToolStripMenuItem menuItem_LoadPSXSaveState;
        private System.Windows.Forms.ToolStripMenuItem menuItem_About;
        private System.Windows.Forms.ToolStripMenuItem menuItem_CopyCommands;
        private System.Windows.Forms.ToolStripMenuItem menuItem_PasteCommands;
        private System.Windows.Forms.ToolStripMenuItem menuItem_UseTrimmedDefaults;
        private System.Windows.Forms.ToolStripMenuItem menuItem_SavePatchXML;
    }
}

