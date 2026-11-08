using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Forms;

using POS_88.Database;
using POS_88.Persistence;

namespace POS_88.Views
{
    public partial class PickItemView : Form
    {
        private UnitOfWorkPOS _unitOfWork;
        private POSImplementation _posImplement;
        private ImageList imgList;
        private CashierView _frmCashier;

        public PickItemView()
        {
            InitializeComponent();
            this._posImplement = new POSImplementation();
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this.imgList = new ImageList();
        }

        public PickItemView(CashierView frm)
        {
            this.InitializeComponent();
            this.ApplyPOSTheme();
            this._posImplement = new POSImplementation();
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this.imgList = new ImageList();
            this._frmCashier = frm;

            btnClose.Click += btnClose_Click;
            btnSearch.Click += btnSearch_Click;
            btnClearFilter.Click += btnReload_Click;
        }

        public void ApplyPOSTheme()
        {
            btnClose.BackColor = POSTheme.Danger;
        }

        public void LoadItem()
        {
            this.imgList = new ImageList();
            this.imgList.ImageSize = new Size(120, 100);
            IEnumerable<inventory> all = this._unitOfWork.Inventory.GetAll();
            setting setting = this._unitOfWork.Setting.Find((Expression<Func<setting, bool>>)(x => x.settingsid == 1)).FirstOrDefault<setting>();
            int num = 0;
            foreach (inventory inventory in all)
            {
                string str = Path.Combine(setting.imagePath, inventory.itemcode + ".jpg");
                if (File.Exists(str))
                {
                    FileInfo fileInfo1 = new FileInfo(str);
                    FileInfo fileInfo2 = new FileInfo(str);
                    this.imgList.Images.Add(Image.FromFile(str));
                    this.lvwItems.LargeImageList = this.imgList;
                    this.lvwItems.Items.Add(new ListViewItem()
                    {
                        ImageIndex = num,
                        Name = inventory.itemcode,
                        Text = inventory.itemname,
                        Tag = (object)inventory.itemcode
                    });
                    ++num;
                }
            }
        }

        private void PickItemView_Load(object sender, EventArgs e)
        {
            this.Width = this.bunifuCards1.Width + this.bunifuCards2.Width + 30;
            this.Height = this.bunifuCards1.Height + 30;
            this.LoadCategory();
            this.LoadItem();
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void btnSearch_Click(object sender, EventArgs e) => this.SearchItem();

        public void SearchItem()
        {
            this.lvwItems.Items.Clear();
            this.imgList.Images.Clear();
            setting setting = this._unitOfWork.Setting.Find((Expression<Func<setting, bool>>)(x => x.settingsid == 1)).FirstOrDefault<setting>();
            IEnumerable<spSearchByItem_Result> searchByItemResults = this._posImplement.SearchItem("SEARCH PICK", this.txtSearch.Text);
            int num = 0;
            foreach (spSearchByItem_Result searchByItemResult in searchByItemResults)
            {
                string str = Path.Combine(setting.imagePath, searchByItemResult.ITEM_CODE + ".jpg");
                if (File.Exists(str))
                {
                    FileInfo fileInfo1 = new FileInfo(str);
                    FileInfo fileInfo2 = new FileInfo(str);
                    this.imgList.Images.Add(Image.FromFile(str));
                    this.lvwItems.LargeImageList = this.imgList;
                    this.lvwItems.Items.Add(new ListViewItem()
                    {
                        ImageIndex = num,
                        Name = searchByItemResult.ITEM_CODE,
                        Text = searchByItemResult.NAME,
                        Tag = (object)searchByItemResult.ITEM_CODE
                    });
                    ++num;
                }
            }
        }


        public void LoadCategory()
        {
            IEnumerable<category> all = this._unitOfWork.Category.GetAll();
            this.lstCategory.Items.Clear();
            this.lstCategory.DataSource = (object)all;
            this.lstCategory.DisplayMember = "categoryname";
            this.lstCategory.ValueMember = "categoryid";
        }

        private void lvwItems_MouseClick(object sender, MouseEventArgs e)
        {
            this._frmCashier.AddItem(this.lvwItems.FocusedItem.Name);
        }

        public void SearchItemByCategory(string strCategory)
        {
            this.lvwItems.Items.Clear();
            this.imgList.Images.Clear();
            setting setting = this._unitOfWork.Setting.Find((Expression<Func<setting, bool>>)(x => x.settingsid == 1)).FirstOrDefault<setting>();
            IEnumerable<spSearchByItem_Result> searchByItemResults = this._posImplement.SearchItem("BY CATEGORY", strCategory);
            int num = 0;
            foreach (spSearchByItem_Result searchByItemResult in searchByItemResults)
            {
                string str = Path.Combine(setting.imagePath, searchByItemResult.ITEM_CODE + ".jpg");
                if (File.Exists(str))
                {
                    FileInfo fileInfo1 = new FileInfo(str);
                    FileInfo fileInfo2 = new FileInfo(str);
                    this.imgList.Images.Add(Image.FromFile(str));
                    this.lvwItems.LargeImageList = this.imgList;
                    this.lvwItems.Items.Add(new ListViewItem()
                    {
                        ImageIndex = num,
                        Name = searchByItemResult.ITEM_CODE,
                        Text = searchByItemResult.NAME,
                        Tag = (object)searchByItemResult.ITEM_CODE
                    });
                    ++num;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Return)
                this.SearchItem();
            if (keyData == Keys.Escape)
                this.Close();
            if (keyData == (Keys.F | Keys.Control))
                this.txtSearch.Focus();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            this.txtSearch.Clear();
            this.SearchItem();
            this.txtSearch.Focus();
        }

        private void lstCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strCategory = this.lstCategory.SelectedValue.ToString();
            if (strCategory == "ALL")
                this.SearchItem();
            else
                this.SearchItemByCategory(strCategory);
        }
    }
}
