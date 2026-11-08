using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_88.Views
{
    /// <summary>Dim full-screen cover with a "Please wait" text. Built in code, no designer needed.</summary>
    public sealed class LoadingOverlay : Form
    {
        private readonly Label _label;

        public LoadingOverlay(Form owner, string text = "Processing payment...")
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            BackColor = Color.Black;
            Opacity = 0.6;
            Bounds = owner.Bounds;

            _label = new Label
            {
                Text = text,
                ForeColor = Color.White,
                Font = new Font("Century Gothic", 16f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            Controls.Add(_label);
        }

        /// <summary>Hide the text and keep only the dim backdrop (used while the receipt is showing).</summary>
        public void BackdropOnly()
        {
            _label.Visible = false;
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }   // don't steal focus from the dialogs
        }
    }
}