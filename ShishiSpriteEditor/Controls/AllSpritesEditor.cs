using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FFTPatcher.SpriteEditor
{
    public partial class AllSpritesEditor : UserControl
    {
        public AllSprites Sprites { get; private set; }
        private Stream iso;

        public Sprite CurrentSprite
        {
            get { return spriteEditor1.Sprite; }
        }

        public int PaletteIndex
        {
            get { return spriteEditor1.PaletteIndex; }
        }

        public bool ImportExport8Bpp
        {
            get { return spriteEditor1.ImportExport8Bpp; }
        }

        public void BindTo(AllSprites allSprites, Stream iso)
        {
            if (allSprites == null)
                throw new ArgumentNullException("allSprites");
            if (iso == null)
                throw new ArgumentNullException("iso");
            if (!iso.CanRead || !iso.CanWrite || !iso.CanSeek)
                throw new InvalidOperationException("iso doesn't support reading, writing, and seeking");

            this.Sprites = allSprites;
            this.iso = iso;
            Enabled = true;
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            for (int i = 0; i < allSprites.Count; i++)
            {
                comboBox1.Items.Add(Sprites[i]);
            }
            comboBox1.SelectedIndex = 0;
            comboBox1.EndUpdate();
        }

        public AllSpritesEditor()
        {
            InitializeComponent();
            Enabled = false;
        }

        public void ReloadCurrentSprite(bool updateAnimationTab = true)
        {
            if (CurrentSprite != null)
            {
                spriteEditor1.ReloadSprite(updateAnimationTab);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sprite sprite = (sender as ComboBox).SelectedItem as Sprite;
            if (sprite != null)
            {
                spriteEditor1.BindTo(sprite, Sprites.SharedSPRs[sprite], iso);
            }
        }
    }
}
