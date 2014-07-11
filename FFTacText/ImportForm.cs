using System;
using System.Windows.Forms;

namespace FFTPatcher.TextEditor
{
    public partial class ImportForm : Form
    {
        public string IsoFileName { get; private set; }
        public string TblFileName { get; private set; }

        public ImportForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            IsoFileName = null;
            TblFileName = null;
            base.OnShown(e);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void isoBrowseButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ISO files (*.iso, *.bin, *.img)|*.iso;*.bin;*.img";
            openFileDialog1.FileName = isoTextBox.Text;
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                isoTextBox.Text = openFileDialog1.FileName;
                IsoFileName = openFileDialog1.FileName;
                okButton.Enabled = IsoFileName != null && TblFileName != null;
            }
        }

        private void tblBrowseButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "TBL files (*.tbl)|*.tbl";
            openFileDialog1.FileName = tblTextBox.Text;
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                tblTextBox.Text = openFileDialog1.FileName;
                TblFileName = openFileDialog1.FileName;
                okButton.Enabled = IsoFileName != null && TblFileName != null;
            }
        }
    }
}
