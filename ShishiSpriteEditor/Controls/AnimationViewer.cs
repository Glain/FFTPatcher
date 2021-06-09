using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Bimixual.Animation;

namespace FFTPatcher.SpriteEditor
{
    public partial class AnimationViewer : UserControl
    {
        FpsTimer fpsTimer;
        DrawManager drawManager;
        SpriteManager spriteManager;

        private int origWidth, origHeight;

        public AnimationViewer()
        {
            InitializeComponent();
            playButton.Enabled = false;
            trackBar1.Enabled = false;
            origWidth = control1.Width;
            origHeight = control1.Height;

            control1.MouseWheel += control1_MouseWheel;
        }

        public void SetSize(int zoomMultiplier)
        {
            control1.Width = origWidth * zoomMultiplier;
            control1.Height = origHeight * zoomMultiplier;
        }

        protected override void OnHandleDestroyed( EventArgs e )
        {
            base.OnHandleDestroyed( e );
        }

        private void Go()
        {
            if (drawManager != null)
            {
                drawManager.DrawGame();
            }
        }

        JustSitThereSprite sprite;
        FlipBook flipBook;
        public void ShowAnimation(IList<Bitmap> bitmaps, IList<double> delays, bool startPlaying)
        {
            fpsTimer = new FpsTimer(60);

            LoopControl.FpsTimer = fpsTimer;
            LoopControl.SetAndStartAction(control1, Go);
            drawManager = null;
            var myDrawManager = new DrawManager(control1, fpsTimer);

            if (bitmaps.Count != delays.Count)
                throw new ArgumentException("must have same number of bitmaps as delays");
            IList<Bitmap> oldBitmaps = null;
            if (flipBook != null )
            {
                oldBitmaps = flipBook.Bitmaps;
            }

            spriteManager = new SpriteManager(fpsTimer);
            sprite = new JustSitThereSprite(new Point(0, 0));

            if (flipBook != null)
            {
                flipBook.FrameChanged -= flipBook_FrameChanged;
            }

            flipBook = new FlipBook(bitmaps, delays);
            flipBook.Loop = true;
            flipBook.Paused = true;
            flipBook.FrameChanged += new EventHandler(flipBook_FrameChanged);
            sprite.AddAlteration(flipBook);
            spriteManager.AddObject(sprite);
            myDrawManager.AddDrawable(spriteManager);
            playButton.Enabled = true;

            drawManager = myDrawManager;

            trackBar1.Minimum = 0;
            trackBar1.Maximum = bitmaps.Count - 1;
            trackBar1.Value = 0;

            if (startPlaying)
                Play();

            if (oldBitmaps != null)
            {
                foreach (Bitmap b in oldBitmaps)
                {
                    if (b != null)
                        b.Dispose();
                }
            }
        }

        void flipBook_FrameChanged(object sender, EventArgs e)
        {
            if (flipBook != null)
                trackBar1.Value = flipBook.CurrentFrame;
        }

        public void ShowAnimation(IList<Bitmap> bitmaps, double delay, bool startPlaying)
        {
            double[] delays = new double[bitmaps.Count];
            for (int i = 0; i < bitmaps.Count; i++)
                delays[i] = delay;
            ShowAnimation(bitmaps, delays, startPlaying);
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            if (flipBook != null)
                flipBook.BackOneFrame();
        }

        private void forwardButton_Click(object sender, EventArgs e)
        {
            if (flipBook != null)
                flipBook.ForwardOneFrame();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (flipBook != null)
                flipBook.SetFrame(trackBar1.Value);
        }

        public void Pause()
        {
            pauseButton.Enabled = false;
            playButton.Enabled = true;
            forwardButton.Enabled = true;
            backButton.Enabled = true;
            trackBar1.Enabled = true;
            if (flipBook != null)
                flipBook.Pause();
        }

        public void Play()
        {
            playButton.Enabled = false;
            pauseButton.Enabled = true;
            forwardButton.Enabled = false;
            backButton.Enabled = false;
            trackBar1.Enabled = false;
            if (flipBook != null)
                flipBook.Unpause();
        }

        public event EventHandler<MouseEventArgs> AnimationZoomScroll;
        protected virtual void OnAnimationZoomScroll(MouseEventArgs e)
        {
            EventHandler<MouseEventArgs> handler = AnimationZoomScroll;
            if (handler != null)
                handler(this, e);
        }
        private void control1_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                OnAnimationZoomScroll(e);
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }
    }
}
