using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace POS_88.Views
{
    public sealed class SkeletonLoader : Control
    {
        private readonly Timer animationTimer;
        private int highlightPosition = -160;

        public SkeletonLoader()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.White;

            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            this.animationTimer = new Timer();
            this.animationTimer.Interval = 25;

            this.animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            this.highlightPosition += 12;

            if (this.highlightPosition > this.Width + 160)
            {
                this.highlightPosition = -160;
            }

            this.Invalidate();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (this.Visible)
            {
                this.highlightPosition = -160;
                this.animationTimer.Start();
            }
            else
            {
                this.animationTimer.Stop();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            using (SolidBrush baseBrush =
                new SolidBrush(Color.FromArgb(232, 234, 237)))
            {
                using (LinearGradientBrush highlightBrush =
                    new LinearGradientBrush(
                        new Rectangle(
                            this.highlightPosition - 80,
                            0,
                            160,
                            this.Height),
                        Color.FromArgb(232, 234, 237),
                        Color.FromArgb(250, 250, 250),
                        LinearGradientMode.Horizontal))
                {
                    for (int row = 0; row < this.Height; row += 42)
                    {
                        int rowHeight = Math.Min(
                            34,
                            this.Height - row - 4);

                        Rectangle rowBounds = new Rectangle(
                            4,
                            row + 4,
                            Math.Max(0, this.Width - 8),
                            rowHeight);

                        e.Graphics.FillRectangle(
                            baseBrush,
                            rowBounds);

                        e.Graphics.FillRectangle(
                            highlightBrush,
                            rowBounds);
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.animationTimer != null)
                {
                    this.animationTimer.Stop();
                    this.animationTimer.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
