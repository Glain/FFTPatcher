﻿using System;
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
        private static readonly string[] PaletteSpriteNames = {             
            "Sprite #1",
            "Sprite #2",
            "Sprite #3",
            "Sprite #4",
            "Sprite #5",
            "Sprite #6",
            "Sprite #7",
            "Sprite #8",
            "Portrait #1",
            "Portrait #2",
            "Portrait #3",
            "Portrait #4",
            "Portrait #5",
            "Portrait #6",
            "Portrait #7",
            "Portrait #8"};

        private static readonly string[] PaletteGenericNames = {             
            "Palette #1",
            "Palette #2",
            "Palette #3",
            "Palette #4",
            "Palette #5",
            "Palette #6",
            "Palette #7",
            "Palette #8",
            "Palette #9",
            "Palette #10",
            "Palette #11",
            "Palette #12",
            "Palette #13",
            "Palette #14",
            "Palette #15",
            "Palette #16"};

        public Sprite Sprite { get; private set;}

        public AbstractSprite AbstractSprite { get; private set; }
        bool ignoreChanges = true;
        private Zoom zoom;

        private int paletteIndex = 0;
        public int PaletteIndex
        {
            get { return paletteIndex; }
        }

        private bool isUsingGenericNames = false;

        public bool ImportExport8Bpp 
        {
            get { return chkImportExport8bpp.Checked; }
        }

        private bool hasDataChange = false;
        private Stream iso;

        public SpriteEditor()
        {
            InitializeComponent();
            var s = new List<SpriteType>((SpriteType[])Enum.GetValues(typeof(SpriteType)));
            //s.Remove(SpriteType.RUKA);

            shpComboBox.DataSource = s.ToArray();
            seqComboBox.DataSource = Enum.GetValues(typeof(SpriteType));

            byte[] bytesArray = new byte[256];
            for (int index = 0; index < 256; index++)
            {
                bytesArray[index] = (byte)index;
            }
            cmb_Height.DataSource = bytesArray;

            paletteSelector.SelectedIndex = 0;
            //pictureBox1.MinimumSize = Frame.DefaultFrameSize + pictureBox1.Padding.Size;
            //animationViewer1.MinimumSize = Frame.DefaultFrameSize + animationViewer1.Padding.Size + new Size( 0, 40 );
            numericUpDown2.ValueChanged += numericUpDown2_SelectedIndexChanged;
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);

            spriteViewer1.SpriteDragEnter += sprite_DragEnter;
            spriteViewer1.SpriteDragDrop += sprite_DragDrop;

            pictureBox1.MouseWheel += pictureBox1_MouseWheel;
            animationViewer1.AnimationZoomScroll += animationViewer1_ZoomScroll;

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
                    sb.AppendFormat("0x{0:X2} ", i);
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

        public void UpdatePaletteDropdownNames(bool useGenericNames)
        {
            if (isUsingGenericNames != useGenericNames)
            {
                isUsingGenericNames = useGenericNames;
                UpdatePaletteDropdownNames(useGenericNames ? PaletteGenericNames : PaletteSpriteNames);
            }
        }

        private void UpdatePaletteDropdownNames(string[] names)
        {
            int selectedIndex = paletteSelector.SelectedIndex;
            ignoreChanges = true;

            paletteSelector.Items.Clear();
            paletteSelector.Items.AddRange(names);

            paletteSelector.SelectedIndex = selectedIndex;
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
                cmb_Height.Enabled = false;
                cmb_Height.SelectedItem = null;
            }
            else
            {
                shpComboBox.Enabled = true;
                shpComboBox.SelectedItem = charSprite.SHP;
                seqComboBox.Enabled = true;
                seqComboBox.SelectedItem = charSprite.SEQ;
                flyingCheckbox.Enabled = true;
                flyingCheckbox.Checked = charSprite.Flying;
                cmb_Height.Enabled = true;
                cmb_Height.SelectedItem = charSprite.Height;
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
                (Sprite as CharacterSprite).SetSHP( iso, (SpriteType)shpComboBox.SelectedItem, hasDataChange );
                hasDataChange = true;
                ReloadSprite();
                UpdateAnimationTab(Sprite as CharacterSprite,Sprite);
            }
        }

        private void seqComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( !ignoreChanges )
            {
                (Sprite as CharacterSprite).SetSEQ(iso, (SpriteType)seqComboBox.SelectedItem, hasDataChange);
                hasDataChange = true;
                UpdateAnimationTab(Sprite as CharacterSprite,Sprite);
            }
        }

        private void flyingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                (Sprite as CharacterSprite).SetFlying(iso, flyingCheckbox.Checked, hasDataChange);
                hasDataChange = true;
            }
        }

        private void cmb_Height_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                (Sprite as CharacterSprite).SetHeight(iso, (byte)cmb_Height.SelectedItem, hasDataChange);
                hasDataChange = true;
            }
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
                seqComboBox.SelectedItem = SpriteType.WEP1;         //seqComboBox.Items[7];
            }
            else if (sprite.ToString() == "WEP2")
            {
                seqComboBox.SelectedItem = SpriteType.WEP2;         //seqComboBox.Items[8];
            }
            else if (sprite.ToString() == "EFF1")
            {
                seqComboBox.SelectedItem = SpriteType.EFF1;         //seqComboBox.Items[9];
            }
            else if (sprite.ToString() == "EFF2")
            {
                seqComboBox.SelectedItem = SpriteType.EFF2;         //seqComboBox.Items[10];
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

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.Delta > 0)
                {
                    if (cmbZoom.SelectedIndex > 0)
                        cmbZoom.SelectedIndex--;
                }
                else
                {
                    if (cmbZoom.SelectedIndex < (cmbZoom.Items.Count - 1))
                        cmbZoom.SelectedIndex++;
                }

                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        private void animationViewer1_ZoomScroll(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (cmbZoom.SelectedIndex > 0)
                    cmbZoom.SelectedIndex--;
            }
            else
            {
                if (cmbZoom.SelectedIndex < (cmbZoom.Items.Count - 1))
                    cmbZoom.SelectedIndex++;
            }
        }
    }
}
