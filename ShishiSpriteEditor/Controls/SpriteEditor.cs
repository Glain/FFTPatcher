using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PatcherLib.Utilities;
using FFTPatcher.SpriteEditor.DataTypes;

namespace FFTPatcher.SpriteEditor
{
    public partial class SpriteEditor : UserControl
    {
        public Sprite Sprite { get; private set;}

        public AbstractSprite AbstractSprite { get; private set; }
        bool ignoreChanges = true;
        private Zoom zoom;

        private int paletteIndex = 0;
        public int PaletteIndex
        {
            get { return paletteIndex; }
        }

        public bool ImportExport8Bpp 
        {
            get { return chkImportExport8bpp.Checked; }
        }

        private Stream iso;

        public SpriteEditor()
        {
            InitializeComponent();
            var s = new List<SpriteType>((SpriteType[])Enum.GetValues(typeof(SpriteType)));
            //s.Remove(SpriteType.RUKA);

            shpComboBox.DataSource = s.ToArray();
            seqComboBox.DataSource = Enum.GetValues(typeof(SpriteType));
            paletteSelector.SelectedIndex = 0;
            //pictureBox1.MinimumSize = Frame.DefaultFrameSize + pictureBox1.Padding.Size;
            //animationViewer1.MinimumSize = Frame.DefaultFrameSize + animationViewer1.Padding.Size + new Size( 0, 40 );
            numericUpDown2.ValueChanged += numericUpDown2_SelectedIndexChanged;
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);

            spriteViewer1.SpriteDragEnter += sprite_DragEnter;
            spriteViewer1.SpriteDragDrop += sprite_DragDrop;

            InitZoomComboBox();
            UpdateZoom();
        }

        public void BindTo(Sprite sprite, IList<int> sharedSPRs, Stream iso)
        {
            ignoreChanges = true;
            this.iso = iso;
            Sprite = sprite;
            ReloadSprite();

            if (sharedSPRs.Count > 0)
            {
                StringBuilder sb = new StringBuilder("WARNING: This sprite shares an SPR with: ");
                foreach (int i in sharedSPRs)
                {
                    sb.AppendFormat("0x{0:X2} ", i + 1);
                }
                sharedLabel.Text = sb.ToString();
                sharedLabel.Visible = true;
            }
            else
            {
                sharedLabel.Visible = false;
            }

            Enabled = true;
            ignoreChanges = false;
        }

        private void UpdateCheckBoxesEtc(CharacterSprite charSprite)
        {
            if (charSprite == null)
            {
                shpComboBox.Enabled = false;
                shpComboBox.SelectedItem = null;
                seqComboBox.Enabled = false;
                seqComboBox.SelectedItem = null;
                flyingCheckbox.Enabled = false;
                flyingCheckbox.Checked = false;
                flagsCheckedListBox.Enabled = false;
                flagsCheckedListBox.BeginUpdate();
                for (int i = 0; i < flagsCheckedListBox.Items.Count; i++)
                {
                    flagsCheckedListBox.SetItemChecked(i, false);
                }
                flagsCheckedListBox.EndUpdate();
            }
            else
            {
                shpComboBox.Enabled = true;
                shpComboBox.SelectedItem = charSprite.SHP;
                seqComboBox.Enabled = true;
                seqComboBox.SelectedItem = charSprite.SEQ;
                flyingCheckbox.Enabled = true;
                flyingCheckbox.Checked = charSprite.Flying;
                flagsCheckedListBox.Enabled = true;
                var flags = new bool[] { 
                    charSprite.Flag1, charSprite.Flag2, charSprite.Flag3, charSprite.Flag4,
                    charSprite.Flag5, charSprite.Flag6, charSprite.Flag7, charSprite.Flag8 };
                flagsCheckedListBox.BeginUpdate();
                for (int i = 0; i < flagsCheckedListBox.Items.Count; i++)
                {
                    flagsCheckedListBox.SetItemChecked(i, flags[i]);
                }
                flagsCheckedListBox.EndUpdate();
            }
        }

        public void ReloadSprite(bool updateAnimationTab = true)
        {
            bool oldIgnoreChanges = ignoreChanges;
            ignoreChanges = true;
            AbstractSprite = Sprite.GetAbstractSpriteFromIso( iso );
            spriteViewer1.Sprite = AbstractSprite;

            UpdateCheckBoxesEtc(Sprite as CharacterSprite);
            UpdateShapes(Sprite as CharacterSprite,Sprite);

            if (updateAnimationTab)
                UpdateAnimationTab(Sprite as CharacterSprite,Sprite);
            
            maxSizeLabel.Visible = true;
            maxSizeLabel.Text = string.Format("Max SPR size:" + Environment.NewLine + "{0} bytes", Sprite.Size);
            ignoreChanges = oldIgnoreChanges;
        }

        private Shape currentShape;
        private void UpdateShapes(CharacterSprite charSprite,Sprite sprite)
        {
            bool oldIgnoreChanges = ignoreChanges;
            ignoreChanges = true;
            if (charSprite == null )
            {
                tabControl1.Enabled = false;
                if ( sprite.ToString() == "WEP1" || sprite.ToString() == "WEP2" ||
                     sprite.ToString() == "EFF1" || sprite.ToString() == "EFF2")
                {
                     tabControl1.Enabled = true;
                     if (sprite.ToString() == "WEP1")
                     {
                         currentShape = Shape.Shapes[SpriteType.WEP1];
                     }
                     else if (sprite.ToString() == "WEP2")
                     {
                         currentShape = Shape.Shapes[SpriteType.WEP2];
                     }
                     else if (sprite.ToString() == "EFF1")
                     {
                         currentShape = Shape.Shapes[SpriteType.EFF1];
                     }
                     else if (sprite.ToString() == "EFF2")
                     {
                         currentShape = Shape.Shapes[SpriteType.EFF2];
                     }
                     numericUpDown1.Maximum = currentShape.Frames.Count - 1;
                     UpdatePictureBox();
            
                }
            }
            else
            {
          
                tabControl1.Enabled = true;
                if (Shape.Shapes.ContainsKey(charSprite.SHP))
                {
                    numericUpDown1.Enabled = true;
                    pictureBox1.Enabled = true;

                    currentShape = Shape.Shapes[charSprite.SHP];
                    //numericUpDown1.Value = 1;
                    numericUpDown1.Maximum = currentShape.Frames.Count - 1;
                }
                else
                {
                    numericUpDown1.Enabled = false;
                    pictureBox1.Enabled = false;
                    currentShape = null;
                }
                UpdatePictureBox();
            }
            ignoreChanges = oldIgnoreChanges;
        }

        private void InitZoomComboBox()
        {
            for (int mult = 1; mult < 17; mult++)
            {
                cmbZoom.Items.Add(new Zoom(mult, ((mult * 100).ToString() + "%")));
            }

            cmbZoom.SelectedIndex = 0;
        }

        private void UpdateZoom()
        {
            zoom = (Zoom)cmbZoom.SelectedItem;
        }

        private void UpdateImages()
        {
            UpdatePictureBox();

            if (currentSequences != null)
                UpdateAnimation();
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == animationTabPage && Sequence.Sequences.ContainsKey((SpriteType)seqComboBox.SelectedItem))
            {
                numericUpDown2_SelectedIndexChanged(null, EventArgs.Empty);
                animationViewer1.Play();
                spriteViewer1.HighlightTiles(new Tile[0]);
            }
            else
            {
                animationViewer1.Pause();
                UpdatePictureBox();
            }
        }

        private void cmbZoom_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateZoom();
            UpdateImages();
        }

        private void PaletteChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                spriteViewer1.SetPalette(paletteSelector.SelectedIndex, portraitCheckbox.Checked ? (paletteSelector.SelectedIndex % 8 + 8) : paletteSelector.SelectedIndex);
                paletteIndex = paletteSelector.SelectedIndex;
                UpdatePictureBox();
                UpdateAnimation();
            }
        }

        private void shpComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( !ignoreChanges )
            {
                (Sprite as CharacterSprite).SetSHP( iso, (SpriteType)shpComboBox.SelectedItem );
                ReloadSprite();
                UpdateAnimationTab(Sprite as CharacterSprite,Sprite);
            }
        }

        private void seqComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( !ignoreChanges )
            {
                (Sprite as CharacterSprite).SetSEQ(iso, (SpriteType)seqComboBox.SelectedItem);
                UpdateAnimationTab(Sprite as CharacterSprite,Sprite);
            }
        }

        private void flyingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
                (Sprite as CharacterSprite).SetFlying(iso, flyingCheckbox.Checked);
        }

        private void flagsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!ignoreChanges)
                (Sprite as CharacterSprite).SetFlag(iso, e.Index, e.NewValue == CheckState.Checked);
        }

        private void UpdatePictureBox()
        {
            Image i = pictureBox1.Image;
            if (currentShape != null)
            {
                Frame currentFrame = currentShape.Frames[(int)numericUpDown1.Value];
                //pictureBox1.Image = currentFrame.GetFrame(Sprite.GetAbstractSpriteFromIso(iso), paletteIndex);

                Bitmap image = currentFrame.GetFrame(Sprite.GetAbstractSpriteFromIso(iso), paletteIndex);
                pictureBox1.Image = zoom.GetZoomedBitmap(image);

                if (tabControl1.SelectedTab == framesTabPage)
                {
                    spriteViewer1.HighlightTiles(currentFrame.Tiles);
                }
                else
                {
                    spriteViewer1.HighlightTiles(new Tile[0]);
                }
            }
            else
            {
                pictureBox1.Image = null;
            }

            if (i != null)
            {
                i.Dispose();
            }
        }

        void numericUpDown2_SelectedIndexChanged( object sender, EventArgs e )
        {
            UpdateAnimation();
        }

        private IList<Sequence> currentSequences;

        private void UpdateAnimation()
        {
            Sequence seq = currentSequences[(int)numericUpDown2.Value];
            IList<Bitmap> bmps;
            IList<double> delays;
            animationViewer1.SetSize(zoom.Multiplier);
            seq.BuildAnimation(spriteViewer1.Sprite, out bmps, out delays, paletteIndex, zoom);
            animationViewer1.ShowAnimation(bmps, delays, tabControl1.SelectedTab == animationTabPage);
        }

        private void UpdateAnimationTab(CharacterSprite charSprite, Sprite sprite)
        {
            if(sprite.ToString() == "WEP1")
            {
                seqComboBox.SelectedItem = seqComboBox.Items[7];
            }
            else if (sprite.ToString() == "WEP2")
            {
                seqComboBox.SelectedItem = seqComboBox.Items[8];
            }
            else if (sprite.ToString() == "EFF1")
            {
                seqComboBox.SelectedItem = seqComboBox.Items[9];
            }
            else if (sprite.ToString() == "EFF2")
            {
                seqComboBox.SelectedItem = seqComboBox.Items[10];
            }
            
            if (charSprite != null &&
                seqComboBox.SelectedItem != null)
            {
                if (Sequence.Sequences.ContainsKey((SpriteType)seqComboBox.SelectedItem))
                {
                    IList<Sequence> sequences = Sequence.Sequences[(SpriteType)seqComboBox.SelectedItem];
                    currentSequences = sequences;
                    numericUpDown2.Minimum = 0;
                    numericUpDown2.Maximum = sequences.Count - 1;
                    numericUpDown2.Value = 0;
                    numericUpDown2.Enabled = true;
                    animationViewer1.Enabled = true;
                }
                else
                {
                    animationViewer1.Pause();
                    animationViewer1.Enabled = false;
                    numericUpDown2.Enabled = false;
                }
            }
        }

        private void numericUpDown1_ValueChanged( object sender, EventArgs e )
        {
            if ( !ignoreChanges && currentShape != null )
            {
                UpdatePictureBox();
            }
        }

    }
}
