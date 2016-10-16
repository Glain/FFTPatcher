using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FFTPatcher.SpriteEditor.DataTypes.OtherImages;

namespace FFTPatcher.SpriteEditor
{
    public partial class AllOtherImagesEditor : UserControl
    {
        public AllOtherImagesEditor()
        {
            InitializeComponent();
        }

        private bool ignoreChanges = false;
        private System.IO.Stream iso = null;

        public AllOtherImages AllOtherImages { get; private set; }

        public void BindTo( AllOtherImages images, System.IO.Stream iso )
        {
            ignoreChanges = true;
            AllOtherImages = images;
            comboBox1.SelectedIndex = -1;
            comboBox1.BeginUpdate();
            comboBox1.DataSource = images.Images;
            comboBox1.EndUpdate();

            this.iso = iso;
            Enabled = true;
            ignoreChanges = false;

            comboBox1.SelectedIndex = 0;

            SetupEntryDropdown();
            SetupPaletteDropdown();
            RefreshPictureBox();
        }

        public AbstractImageList GetImageListFromComboBoxItem()
        {
            return (AbstractImageList)comboBox1.SelectedItem;
        }

        public AbstractImage GetImageFromComboBoxItem()
        {
            AbstractImageList list = GetImageListFromComboBoxItem();
            return (list.Count == 1) ? list[0] : list[ddl_Entry.SelectedIndex];
        }

        public string GetCurrentImageFileFilter()
        {
            AbstractImage im = GetImageFromComboBoxItem();
            return im.FilenameFilter;
        }

        public string GetCurrentImageInputFileFilter()
        {
            AbstractImage im = GetImageFromComboBoxItem();
            return im.InputFilenameFilter;
        }

        public string GetCurrentOriginalFilename()
        {
            return GetImageFromComboBoxItem().OriginalFilename;
        }

        public void SaveCurrentImage( string path )
        {
            AbstractImage im = GetImageFromComboBoxItem();
            using ( System.IO.Stream s = System.IO.File.Open( path, System.IO.FileMode.Create ) )
            {
                im.SaveImage( iso, s );
            }
        }

        public void LoadToCurrentImage( string path )
        {
            using (Image im = Image.FromFile( path ))
            {
                AbstractImage abIm = GetImageFromComboBoxItem();
                abIm.ImportFilename = path;
                abIm.WriteImageToIso( iso, im );
            }
            RefreshPictureBox();
        }

        public bool ImportEntireFile(string path)
        {
            AbstractImage abIm = GetImageFromComboBoxItem();
            bool result = abIm.ImportEntireFile(iso, path);
            RefreshPictureBox(true);
            return result;
        }

        public bool ExportEntireFile(string path)
        {
            AbstractImage abIm = GetImageFromComboBoxItem();

            if (abIm.Filesize > 0)
                return abIm.ExportEntireFile(iso, path, true);
            else
                return false;
        }

        private void DestroyPictureBoxImage()
        {
            //if ( pictureBox1.Image != null )
            //{
            //    var im = pictureBox1.Image;
            //    pictureBox1.Image = null;
            //    im.Dispose();
            //}
        }

        private void AssignNewPictureBoxImage(Image newImage)
        {
            DestroyPictureBoxImage();
            pictureBox1.Image = newImage;
            panel1.SuspendLayout();
            panel1.BackColor = Color.Black;
            if ( pictureBox1.Width < panel1.ClientSize.Width && pictureBox1.Height < panel1.ClientSize.Height )
            {
                pictureBox1.BorderStyle = BorderStyle.Fixed3D;
                pictureBox1.Location = new Point( 
                    ( panel1.ClientRectangle.Width - pictureBox1.Width ) / 2 + panel1.ClientRectangle.X, 
                    ( panel1.ClientRectangle.Height - pictureBox1.Height ) / 2 + panel1.ClientRectangle.Y );
            }
            else
            {
                pictureBox1.BorderStyle = BorderStyle.None;
                pictureBox1.Location = panel1.ClientRectangle.Location;
            }
            panel1.ResumeLayout();
        }

        private void RefreshPictureBox(bool forceReload = false)
        {
            AbstractImage im = GetImageFromComboBoxItem();
            AssignNewPictureBoxImage( im.GetImageFromIso( iso, forceReload ) );
            imageSizeLabel.Text = string.Format( "Image dimensions: {0}x{1}", im.Width, im.Height );
        }

        private void SetupEntryDropdown(AbstractImageList list = null)
        {
            if (list == null)
            {
                list = GetImageListFromComboBoxItem();
            }

            int listCount = list.Count;
            if (listCount > 1)
            {
                ddl_Entry.Items.Clear();
                for (int index = 0; index < listCount; index++)
                {
                    string addend = (list[index].Name == null) ? "" : (" - " + list[index].Name);
                    ddl_Entry.Items.Add((index + 1) + addend);
                }
                ddl_Entry.SelectedIndex = 0;

                ddl_Entry.Visible = true;
                lbl_Entry.Visible = true;
            }
            else
            {
                ddl_Entry.Visible = false;
                lbl_Entry.Visible = false;
            }
        }

        private void SetupPaletteDropdown(AbstractImage im = null)
        {
            if (im == null)
            {
                im = GetImageFromComboBoxItem();
            }

            if (im.PaletteCount > 0)
            {
                ddl_Palette.Items.Clear();
                for (int index = 0; index < im.PaletteCount; index++)
                {
                    ddl_Palette.Items.Add((index + 1));
                }
                ddl_Palette.SelectedIndex = im.CurrentPalette;

                pnl_Palette.Visible = true;
            }
            else
            {
                pnl_Palette.Visible = false;
            }
        }

        private void Setup8bppCheckbox(AbstractImage im = null)
        {
            if (im == null)
            {
                im = GetImageFromComboBoxItem();
            }

            if (im.CanSelectPalette())
            {
                chk_8bpp.Checked = ((ISelectablePalette4bppImage)im).ImportExport8bpp;
                chk_8bpp.Visible = true;
            }
            else
            {
                chk_8bpp.Visible = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( !ignoreChanges )
            {
                SetupEntryDropdown();
                SetupPaletteDropdown();
                Setup8bppCheckbox();
                RefreshPictureBox();
            }
        }

        private void ddl_Palette_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbstractImage im = GetImageFromComboBoxItem();
            im.CurrentPalette = ddl_Palette.SelectedIndex;
            RefreshPictureBox(true);
        }

        private void ddl_Entry_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbstractImage im = GetImageFromComboBoxItem();
            SetupPaletteDropdown();
            Setup8bppCheckbox();
            RefreshPictureBox(true);
        }

        private void chk_8bpp_CheckedChanged(object sender, EventArgs e)
        {
            AbstractImage im = GetImageFromComboBoxItem();
            if (im.CanSelectPalette())
            {
                ((ISelectablePalette4bppImage)im).ImportExport8bpp = chk_8bpp.Checked;

                if (chk_8bpp.Checked)
                {
                    im.CurrentPalette = Math.Max(0, im.CurrentPalette - 15);
                    int upperIndexBound = Math.Max(1, im.PaletteCount - 15);

                    ddl_Palette.Items.Clear();
                    for (int index = 0; index < upperIndexBound; index++)
                    {
                        ddl_Palette.Items.Add((index + 1));
                    }
                    ddl_Palette.SelectedIndex = im.CurrentPalette;

                    RefreshPictureBox(true);
                }
                else
                {
                    SetupPaletteDropdown();
                }
            }
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && System.IO.File.Exists(files[0]))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (paths.Length == 1 && System.IO.File.Exists(paths[0]))
                {
                    LoadToCurrentImage(paths[0]);
                }
            }
        }
    }
}
