
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace POS_88
{
    public static class POSTheme
    {
        // Central color palette
        public static readonly Color Background =
            Color.FromArgb(240, 242, 245);

        public static readonly Color Navy =
            Color.FromArgb(15, 23, 42);

        public static readonly Color NavyMiddle =
            Color.FromArgb(25, 52, 91);

        public static readonly Color NavyDark =
            Color.FromArgb(8, 15, 32);

        public static readonly Color Accent =
            Color.DodgerBlue;

        public static readonly Color Text =
            Color.FromArgb(255, 255, 255);

        public static readonly Color MutedText =
            Color.FromArgb(184, 199, 223);

        public static readonly Color Border =
            Color.FromArgb(38, 54, 83);

        public static readonly Color Danger =
            Color.FromArgb(190, 45, 55);

        public static readonly Color ButtonHover =
            Color.FromArgb(35, 55, 85);

        // Vertical gradient: sidebar
        public static void ApplySidebar(Panel panel)
        {
            ApplyGradient(panel, LinearGradientMode.Vertical);
        }

        // Horizontal gradient: topbar
        public static void ApplyTopbar(Panel panel)
        {
            ApplyGradient(panel, LinearGradientMode.Horizontal);
        }

        // Shared gradient implementation
        public static void ApplyGradient(
            Panel panel,
            LinearGradientMode direction)
        {
            if (panel == null)
                return;

            // Avoid registering the same Paint handler twice.
            panel.Paint -= PanelGradient_Paint;

            panel.Tag = direction;
            panel.Paint += PanelGradient_Paint;

            panel.BackColor = Navy;
            panel.Invalidate();
        }

        public static void ApplyCardBunifu(Bunifu.Framework.UI.BunifuCards card)
        {
            ApplyBunifuCardGradient(card, LinearGradientMode.Horizontal);
        }
        public static void ApplyBunifuCardGradient(
            Bunifu.Framework.UI.BunifuCards card,
            LinearGradientMode direction)
        {
            if (card == null)
                return;

            // Avoid registering the same Paint handler twice.
            card.Paint -= PanelGradient_Paint;

            card.Tag = direction;
            card.Paint += PanelGradient_Paint;

            card.BackColor = Navy;
            card.Invalidate();
        }

        private static void PanelGradient_Paint(
            object sender,
            PaintEventArgs e)
        {
            Panel panel = sender as Panel;

            if (panel == null ||
                panel.ClientSize.Width <= 0 ||
                panel.ClientSize.Height <= 0)
                return;

            LinearGradientMode direction =
                panel.Tag is LinearGradientMode
                    ? (LinearGradientMode)panel.Tag
                    : LinearGradientMode.Vertical;

            using (LinearGradientBrush brush =
                new LinearGradientBrush(
                    panel.ClientRectangle,
                    Navy,
                    NavyDark,
                    direction))
            {
                ColorBlend blend = new ColorBlend();

                blend.Colors = new Color[]
                {
                    Navy,
                    NavyMiddle,
                    NavyDark
                };

                blend.Positions = new float[]
                {
                    0f, 0.5f, 1f
                };

                brush.InterpolationColors = blend;

                e.Graphics.FillRectangle(
                    brush, panel.ClientRectangle);
            }
        }

        // Apply consistent styling to navigation buttons.
        public static void StyleNavigationButton(Button button)
        {
            if (button == null)
                return;

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ButtonHover;
            button.FlatAppearance.MouseDownBackColor = Accent;

            button.BackColor = Navy;
            button.ForeColor = Text;
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
        }

        // Highlight the selected navigation button.
        public static void SetActiveButton(
            Button selectedButton,
            Button[] allButtons)
        {
            if (allButtons == null)
                return;

            foreach (Button button in allButtons)
            {
                if (button == null)
                    continue;

                button.BackColor = Navy;
                button.ForeColor = Text;
                button.FlatAppearance.MouseOverBackColor =
                    ButtonHover;
            }

            if (selectedButton != null)
            {
                selectedButton.BackColor = NavyMiddle;
                selectedButton.ForeColor = Color.White;
                selectedButton.FlatAppearance.MouseOverBackColor =
                    NavyMiddle;
            }
        }

        // Standard form/modal background.
        public static void StyleModal(Form form)
        {
            if (form == null)
                return;

            form.BackColor = Background;
            form.ForeColor = Color.FromArgb(30, 41, 59);
            form.Font = new Font(
                "Century Gothic", 9F, FontStyle.Regular);
        }

        // Standard modal title bar panel.
        public static void StyleModalHeader(Panel panel)
        {
            ApplyTopbar(panel);
        }

        // Standard modal buttons.
        public static void StylePrimaryButton(Button button)
        {
            if (button == null)
                return;

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(30, 110, 220);

            button.BackColor = Accent;
            button.ForeColor = Color.White;
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
        }

        public static void StyleCancelButton(Button button)
        {
            if (button == null)
                return;

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Border;
            button.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(225, 230, 238);

            button.BackColor = Color.White;
            button.ForeColor = Color.FromArgb(51, 65, 85);
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
        }

        public static void ShowSmoothFadeDialog(Form dialog, Form owner)
        {
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.Opacity = 0;

            bool isFadingOut = false;

            // Fade In
            dialog.Shown += (s, e) =>
            {
                Timer fadeIn = new Timer { Interval = 1 };

                fadeIn.Tick += (ts, te) =>
                {
                    if (dialog.IsDisposed || isFadingOut)
                    {
                        fadeIn.Stop();
                        fadeIn.Dispose();
                        return;
                    }

                    if (dialog.Opacity < 0.95)
                    {
                        dialog.Opacity += 0.08;
                    }
                    else
                    {
                        dialog.Opacity = 1;
                        fadeIn.Stop();
                        fadeIn.Dispose();
                    }
                };

                fadeIn.Start();
            };

            // Fade Out
            dialog.FormClosing += (s, e) =>
            {
                // Already fading, or fade finished -> let the close go through
                if (isFadingOut)
                {
                    e.Cancel = dialog.Opacity > 0;
                    return;
                }

                // Skip animation if the app/Windows is shutting down
                if (e.CloseReason == CloseReason.ApplicationExitCall ||
                    e.CloseReason == CloseReason.WindowsShutDown ||
                    e.CloseReason == CloseReason.TaskManagerClosing)
                {
                    return;
                }

                // Start fade out: cancel this close, close again when done
                e.Cancel = true;
                isFadingOut = true;

                // Preserve the DialogResult, since cancelling the close resets it
                DialogResult result = dialog.DialogResult;

                Timer fadeOut = new Timer { Interval = 1 };

                fadeOut.Tick += (ts, te) =>
                {
                    if (dialog.IsDisposed)
                    {
                        fadeOut.Stop();
                        fadeOut.Dispose();
                        return;
                    }

                    if (dialog.Opacity > 0.05)
                    {
                        dialog.Opacity -= 0.08;
                    }
                    else
                    {
                        dialog.Opacity = 0;
                        fadeOut.Stop();
                        fadeOut.Dispose();

                        dialog.DialogResult = result; // restore result
                        dialog.Close();               // now actually close
                    }
                };

                fadeOut.Start();
            };

            dialog.ShowDialog(owner);
        }

        public static void ShowSmoothZoomDialog(Form dialog, Form owner)
        {
            const int DurationMs = 180;      // total animation time
            const double StartScale = 0.92;  // how small it starts / ends

            Size fullSize = dialog.Size;
            Point center = new Point(
                owner.Left + owner.Width / 2,
                owner.Top + owner.Height / 2);

            bool closing = false;
            bool allowClose = false;
            DialogResult pendingResult = DialogResult.None;

            double progress = 0;             // 0 = small/transparent, 1 = full size/opaque
            double fromProgress = 0, toProgress = 1;
            var sw = new Stopwatch();

            var timer = new Timer { Interval = 10 };

            // Apply a given progress value to the form
            Action<double> apply = p =>
            {
                double scale = StartScale + (1 - StartScale) * p;
                int w = (int)Math.Round(fullSize.Width * scale);
                int h = (int)Math.Round(fullSize.Height * scale);

                // Single call = one layout pass, no flicker
                dialog.SetBounds(center.X - w / 2, center.Y - h / 2, w, h);
                dialog.Opacity = p;
            };

            Action<double, double> startAnimation = (from, to) =>
            {
                fromProgress = from;
                toProgress = to;
                sw.Restart();
                timer.Start();
            };

            dialog.StartPosition = FormStartPosition.Manual;
            dialog.Opacity = 0;
            apply(0);

            dialog.Shown += (s, e) => startAnimation(0, 1);

            dialog.FormClosing += (s, e) =>
            {
                if (allowClose) return;

                e.Cancel = true;
                if (closing) return;

                closing = true;
                pendingResult = dialog.DialogResult;   // cancelling resets this, so save it
                startAnimation(progress, 0);
            };

            timer.Tick += (s, e) =>
            {
                if (dialog.IsDisposed) { timer.Stop(); return; }

                double t = Math.Min(1.0, sw.ElapsedMilliseconds / (double)DurationMs);

                // Ease-out when opening (fast -> slow), ease-in when closing (slow -> fast)
                double eased = closing
                    ? t * t * t
                    : 1 - Math.Pow(1 - t, 3);

                progress = fromProgress + (toProgress - fromProgress) * eased;
                apply(progress);

                if (t >= 1.0)
                {
                    timer.Stop();

                    if (closing)
                    {
                        allowClose = true;
                        dialog.DialogResult = pendingResult;  // restore the result
                        dialog.Close();
                    }
                }
            };

            dialog.FormClosed += (s, e) => timer.Dispose();

            dialog.ShowDialog(owner);
        }
    }
}
