using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

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

            List<object> imageList = new List<object>();
            for ( int i = 0; i < images.ListCount; i++ )
            {
                var l = images[i];
                for ( int j = 0; j < l.Count; j++ )
                {
                    if ( j == l.Count - 1 && i != images.ListCount - 1 )
                    {
                        imageList.Add( new SeparatorComboBox.SeparatorItem( l[j] ) );
                    }
                    else
                    {
                        imageList.Add( l[j] );
                    }
                }
            }

            comboBox1.DataSource = imageList;
            comboBox1.EndUpdate();
            this.iso = iso;
            Enabled = true;
            ignoreChanges = false;

            comboBox1.SelectedIndex = 0;
            RefreshPictureBox();
            SetupPaletteDropdown();
        }

        public AbstractImage GetImageFromComboBoxItem()
        {
            AbstractImage data = comboBox1.SelectedItem as AbstractImage;
            if (data == null)
            {
                data = (comboBox1.SelectedItem as SeparatorComboBox.SeparatorItem).Data as AbstractImage;
            }

            return data;
        }

        public string GetCurrentImageFileFilter()
        {
            AbstractImage im = GetImageFromComboBoxItem();
            return im.FilenameFilter;
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
                abIm.WriteImageToIso( iso, im );
            }
            RefreshPictureBox();
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
                    ddl_Palette.Items.Add(index);
                }
                ddl_Palette.SelectedIndex = im.CurrentPalette;
                pnl_Palette.Visible = true;
            }
            else
            {
                pnl_Palette.Visible = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( !ignoreChanges )
            {
                SetupPaletteDropdown();
                RefreshPictureBox();
            }
        }

        private void panel1_DragEnter( object sender, DragEventArgs e )
        {
            if (e.Data.GetDataPresent( DataFormats.FileDrop ) )
            {
                string[] files = (string[])e.Data.GetData( DataFormats.FileDrop );
                if (files.Length == 1 && System.IO.File.Exists( files[0] ))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panel1_DragDrop( object sender, DragEventArgs e )
        {
            if (e.Data.GetDataPresent( DataFormats.FileDrop ))
            {
                string[] paths = (string[])e.Data.GetData( DataFormats.FileDrop );
                if (paths.Length == 1 && System.IO.File.Exists(paths[0]))
                {
                    LoadToCurrentImage( paths[0] );
                }
            }
        }

        private void ddl_Palette_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbstractImage im = GetImageFromComboBoxItem();
            im.CurrentPalette = (int)ddl_Palette.SelectedItem;
            RefreshPictureBox(true);
        }
    }
}
