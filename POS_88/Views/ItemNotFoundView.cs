using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using POS_88.Presenters;

namespace POS_88.Views
{
    public partial class ItemNotFoundView : Form
    {
        private string _itemcode;
        private SpeechSynthesizer speechSynthesizerObj;

        public ItemNotFoundView()
        {
            InitializeComponent();
        }
        public ItemNotFoundView(string itemcode)
        {
            this.InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Fuchsia;
            TransparencyKey = Color.Fuchsia;
            StartPosition = FormStartPosition.CenterParent;

            this._itemcode = itemcode;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            if (keyData == Keys.Return)
            {
                this.Close();
                Form modalOwner = this.MdiParent ?? this;
                using (Form backdrop = new Form())
                {
                    backdrop.FormBorderStyle = FormBorderStyle.None;
                    backdrop.StartPosition = FormStartPosition.Manual;
                    backdrop.ShowInTaskbar = false;
                    backdrop.BackColor = Color.Black;
                    backdrop.Opacity = 0.45;
                    backdrop.Bounds = modalOwner.Bounds;
                    backdrop.Show(modalOwner);
                    backdrop.BringToFront();

                    using (AddOrEditItemView addItemView = new AddOrEditItemView())
                    {
                        new AddOrEditItemPresenter(
                            addItemView,
                            null,
                            null,
                            () => SearchEvent?.Invoke(this, EventArgs.Empty),
                            true);
                        addItemView.StartPosition = FormStartPosition.CenterParent;
                        POSTheme.ShowSmoothFadeDialog(addItemView, modalOwner);
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public event EventHandler SearchEvent;

        private void ItemNotFoundView_Load(object sender, EventArgs e)
        {
            this.lblItemCode.Text = this._itemcode;
            this.speechSynthesizerObj = new SpeechSynthesizer();
            this.speechSynthesizerObj = new SpeechSynthesizer();
            this.speechSynthesizerObj.SpeakAsync("Item Not Found!");
        }

        private void btnEnter_Click(object sender, EventArgs e) => this.Close();

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form modalOwner = this.MdiParent ?? this;
            using (Form backdrop = new Form())
            {
                backdrop.FormBorderStyle = FormBorderStyle.None;
                backdrop.StartPosition = FormStartPosition.Manual;
                backdrop.ShowInTaskbar = false;
                backdrop.BackColor = Color.Black;
                backdrop.Opacity = 0.45;
                backdrop.Bounds = modalOwner.Bounds;
                backdrop.Show(modalOwner);
                backdrop.BringToFront();

                using (AddOrEditItemView addItemView = new AddOrEditItemView())
                {
                    new AddOrEditItemPresenter(
                        addItemView,
                        null,
                        null,
                        () => SearchEvent?.Invoke(this, EventArgs.Empty));
                    addItemView.StartPosition = FormStartPosition.CenterParent;
                    POSTheme.ShowSmoothFadeDialog(addItemView, modalOwner);
                }
            }
        }


    }
}
