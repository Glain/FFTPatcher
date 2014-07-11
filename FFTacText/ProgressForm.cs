using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PatcherLib.Utilities;
using PatcherLib;

namespace FFTPatcher.TextEditor
{
    internal partial class ProgressForm : Form
    {
        public enum Task
        {
            IsDteNeeded,
            CalculateDte,
            GeneratePatch,
            ApplyingPatches
        }

        public enum TaskState
        {
            NotStarted,
            Starting,
            Done
        }

        internal struct FileProgress
        {
            public ISerializableFile File { get; set; }
            public Task Task { get; set; }
            public TaskState State { get; set; }
            public int BytesLeft { get; set; }
        }

        public ProgressForm()
        {
            InitializeComponent();
            treeView1.BeforeCheck += new TreeViewCancelEventHandler( treeView1_Before );
            treeView1.BeforeCollapse += new TreeViewCancelEventHandler( treeView1_Before );
            treeView1.BeforeExpand += new TreeViewCancelEventHandler( treeView1_Before );
            treeView1.BeforeSelect += new TreeViewCancelEventHandler( treeView1_Before );
            cancelButton.Click += new EventHandler( cancelButton_Click );
            treeView1.Enabled = false;
        }

        BackgroundWorker worker;
        void cancelButton_Click( object sender, EventArgs e )
        {
            if ( worker != null )
            {
                worker.CancelAsync();
                cancelButton.Text = "Canceling...";
                cancelButton.Enabled = false;
            }
        }

        void treeView1_Before( object sender, TreeViewCancelEventArgs e )
        {
            e.Cancel = e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse;
        }

        TreeNode patchnode;
        Dictionary<ISerializableFile, TreeNode> treenodes;
        Dictionary<ISerializableFile, IDictionary<Task, TreeNode>> subnodes;
        internal void BuildNodes( IList<ISerializableFile> files )
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            treenodes = new Dictionary<ISerializableFile, TreeNode>( files.Count );
            subnodes = new Dictionary<ISerializableFile, IDictionary<Task, TreeNode>>( files.Count );

            foreach ( var file in files )
            {
                subnodes[file] = new Dictionary<Task, TreeNode>( 3 );
                TreeNode node = treeView1.Nodes.Add( file.DisplayName );
                treenodes[file] = node;
                subnodes[file][Task.IsDteNeeded] = node.Nodes.Add( "Determine if DTE necessary" );
                subnodes[file][Task.CalculateDte] = node.Nodes.Add( "Calculate DTE" );
                subnodes[file][Task.GeneratePatch] = node.Nodes.Add( "Generating patches" );
            }

            patchnode = treeView1.Nodes.Add( "Applying patches" );
            treeView1.EndUpdate();
        }



        public void DoWork( IWin32Window parent, BackgroundWorker worker, FFTText.PatchIsoArgs args )
        {
            this.worker = worker;
            worker.ProgressChanged += new ProgressChangedEventHandler( worker_ProgressChanged );
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler( worker_RunWorkerCompleted );
            worker.RunWorkerAsync( args );
            this.ShowDialog( parent );
        }

        void worker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            if ( !e.Cancelled && e.Error == null )
            {
                MyMessageBox.Show( this, "Success", "Success", MessageBoxButtons.OK );
            }
            else if ( e.Error != null )
            {
                MyMessageBox.Show( this, e.Error.Message, "Error", MessageBoxButtons.OK );
            }
            else if ( e.Cancelled )
            {
                MyMessageBox.Show( this, "Cancelled", "Cancelled", MessageBoxButtons.OK );
            }
            worker.ProgressChanged -= worker_ProgressChanged;
            worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
            this.Close();
        }

        void worker_ProgressChanged( object sender, ProgressChangedEventArgs e )
        {
            FileProgress state = (FileProgress)e.UserState;
            MethodInvoker mi =
                delegate()
                {
                    TreeNode childNode = state.File != null ? subnodes[state.File][state.Task] : patchnode;
                    IDictionary<Task, TreeNode> grandchildNode = state.File != null ? subnodes[state.File] : new Dictionary<Task, TreeNode>();
                    if ( state.State == TaskState.Starting )
                    {
                        childNode.EnsureVisible();
                        treeView1.SelectedNode = childNode;
                        childNode.NodeFont = new Font( childNode.NodeFont ?? treeView1.Font, FontStyle.Italic );
                        if ( childNode == patchnode )
                        {
                            cancelButton.Enabled = false;
                        }
                        else
                        {
                            childNode.Parent.EnsureVisible();
                        }

                        if (state.Task == Task.CalculateDte && state.BytesLeft >= 0)
                        {
                            childNode.Text = string.Format( "Calculating DTE - {0} bytes left", state.BytesLeft );
                        }
                    }
                    else if ( state.State == TaskState.Done )
                    {
                        childNode.Checked = true;
                        treeView1.SelectedNode = childNode;
                        childNode.EnsureVisible();
                        childNode.NodeFont = new Font( childNode.NodeFont ?? treeView1.Font, ( childNode.NodeFont ?? treeView1.Font ).Style & ~FontStyle.Italic );
                        if ( childNode != patchnode )
                        {
                            childNode.Parent.EnsureVisible();
                        }
                        if (state.Task == Task.CalculateDte)
                        {
                            childNode.Text = "Calculating DTE - 0 bytes left";
                        }
                    }

                    if ( state.Task == Task.IsDteNeeded && state.State == TaskState.Done )
                    {
                        if ( !state.File.IsDteNeeded() )
                        {
                            TreeNode dteNode = subnodes[state.File][Task.CalculateDte];
                            dteNode.NodeFont = new Font( dteNode.NodeFont ?? treeView1.Font, FontStyle.Strikeout );
                            dteNode.Checked = true;
                        }
                    }

                    bool done = true;
                    grandchildNode.ForEach( kvp => done = done && kvp.Value.Checked );
                    if ( done && grandchildNode.Count != 0 )
                    {
                        TreeNode thisNode = treenodes[state.File];
                        treeView1.SelectedNode = thisNode;
                        thisNode.Checked = true;
                        thisNode.Collapse();
                    }
                    else if ( done && childNode == patchnode )
                    {
                        patchnode.Checked = true;
                    }
                };
            if ( this.InvokeRequired )
            {
                this.Invoke( mi );
            }
            else
            {
                mi();
            }
        }
    }
}
