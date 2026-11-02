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
            this._itemcode = itemcode;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            if (keyData == Keys.Return)
            {
                this.Close();
                //int num = (int)new frmAddItem("", "INSERT", "NOTFOUND", "").ShowDialog();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

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
            //int num = (int)new frmAddItem("", "INSERT", "NOTFOUND", "").ShowDialog();
        }


    }
}
