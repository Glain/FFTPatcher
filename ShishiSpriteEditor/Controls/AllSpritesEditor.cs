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

            spriteEditor1.SpriteDragEnter += sprite_DragEnter;
            spriteEditor1.SpriteDragDrop += sprite_DragDrop;
        }

        public void ReloadCurrentSprite(bool updateAnimationTab = true)
        {
            if (CurrentSprite != null)
            {
                spriteEditor1.ReloadSprite(updateAnimationTab);
            }
        }

        public event DragEventHandler SpriteDragEnter;
        protected virtual void OnSpriteDragEnter(DragEventArgs e)
        {
            DragEventHandler handler = SpriteDragEnter;
            if (handler != null)
                handler(this, e);
        }
        private void sprite_DragEnter(object sender, DragEventArgs e)
        {
            OnSpriteDragEnter(e);
        }

        public event DragEventHandler SpriteDragDrop;
        protected virtual void OnSpriteDragDrop(DragEventArgs e)
        {
            DragEventHandler handler = SpriteDragDrop;
            if (handler != null)
                handler(this, e);
        }
        private void sprite_DragDrop(object sender, DragEventArgs e)
        {
            OnSpriteDragDrop(e);
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
