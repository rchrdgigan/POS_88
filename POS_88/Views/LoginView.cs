using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using POS_88.Database;
using POS_88.Presenters;
// TODO: add the namespaces where these forms live in POS_88:
// frmCashier, Form1, frmPrintZReading, frmCashierReport, frmAddItem

namespace POS_88.Views
{
    public partial class LoginView : Form
    {
        private const uint EmSetCueBanner = 0x1501;

        // ---- values carried over from frmVerify ----
        private readonly string _userNm;
        private readonly string _itemcode;
        private readonly string _actionType;
        private readonly Form _frmCalled;
        private readonly MainView _form1;
        private readonly CashierView _frmcash;
        private readonly bool _exitAppOnCancel;
        private bool _completed;

        public string AuthenticatedUserName { get; private set; }
        public string AuthenticatedName { get; private set; }
        public string AuthenticatedUsertype { get; private set; }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr handle, uint message, IntPtr showWhenFocused, string cueBannerText);

        // Default: behaves exactly like your current LoginView (admin only, returns DialogResult)
        public LoginView() : this("LOGIN", false) { }

        public LoginView(string usernm, string itemcode, CashierView frm, string actionType)
            : this(actionType, ShouldExit(actionType))
        {
            _itemcode = itemcode;
            _userNm = usernm;
            _frmcash = frm;
        }

        public LoginView(string usernm, Form frm, string actionType)
            : this(actionType, ShouldExit(actionType))
        {
            _userNm = usernm;
            _frmCalled = frm;
        }

        public LoginView(string usernm, CashierView frm, string actionType)
            : this(actionType, ShouldExit(actionType))
        {
            _userNm = usernm;
            _frmCalled = frm;
            _frmcash = frm;
        }

        public LoginView(string usernm, MainView frm, string actionType)
            : this(actionType, ShouldExit(actionType))
        {
            _userNm = usernm;
            _frmCalled = frm;
            _form1 = frm;
        }

        private LoginView(string actionType, bool exitAppOnCancel)
        {
            InitializeComponent();
            _actionType = (actionType ?? "LOGIN").Trim().ToUpperInvariant();
            _exitAppOnCancel = exitAppOnCancel;

            ApplyTheme();
            panelPass.Height = 105;
            panelUser.Height = 72;
            bunifuTxtPass.HintText = string.Empty;
            bunifuTxtPass.isPassword = true;
            ApplyAsteriskPasswordMask(bunifuTxtPass);
            KeyPreview = true;
            lblUsernameRequired.Visible = false;
            lblPasswordRequired.Visible = false;

            btnLogin.Click += LoginButton_Click;
            btnClearFilter.Click += ClearButton_Click;
            btnMinimize.Click += delegate { WindowState = FormWindowState.Minimized; };
            btnClose.Click += delegate { CancelAndExit(); };

            Load += delegate { bunifuTxtUser.Focus(); };
            FormClosed += LoginView_FormClosed;
        }

        // LOGIN and CASHIER close the whole app when cancelled (same as frmVerify.Exit)
        private static bool ShouldExit(string actionType)
        {
            string a = (actionType ?? string.Empty).Trim().ToUpperInvariant();
            return a == "LOGIN" || a == "CASHIER";
        }

        private void ApplyTheme()
        {
            lblModalTitle.BackColor = Color.Transparent;
            POSTheme.ApplyTopbar(panel1);

            btnMinimize.BackColor = POSTheme.Navy;
            btnMinimize.ForeColor = Color.White;
            btnMinimize.UseVisualStyleBackColor = false;

            btnClose.BackColor = POSTheme.Danger;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;
        }

        private void ApplyAsteriskPasswordMask(Control control)
        {
            foreach (Control child in control.Controls)
            {
                TextBox textBox = child as TextBox;
                if (textBox != null)
                {
                    textBox.Text = string.Empty;
                    textBox.UseSystemPasswordChar = false;
                    textBox.PasswordChar = '*';
                    SendMessage(textBox.Handle, EmSetCueBanner, new IntPtr(1), "Enter password");
                    return;
                }
                ApplyAsteriskPasswordMask(child);
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            bunifuTxtUser.Text = string.Empty;
            bunifuTxtPass.Text = string.Empty;
            lblUsernameRequired.Visible = false;
            lblPasswordRequired.Visible = false;
            panelPass.Height = 105;
            panelUser.Height = 72;
            bunifuTxtUser.Focus();
        }

        private void CancelAndExit()
        {
            if (_exitAppOnCancel)
            {
                Application.Exit();
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        // If the form is closed any other way (Alt+F4, etc.) before a successful
        // login, the app exits for LOGIN/CASHIER - same as frmVerify_FormClosed.
        private void LoginView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_exitAppOnCancel && !_completed)
                Application.Exit();
        }

        private void Login()
        {
            string username = bunifuTxtUser.Text.Trim();
            string password = bunifuTxtPass.Text;

            // ---- field validation ----
            lblUsernameRequired.Visible = string.IsNullOrWhiteSpace(username);
            lblPasswordRequired.Visible = string.IsNullOrEmpty(password);
            panelUser.Height = lblUsernameRequired.Visible ? 90 : 72;
            panelPass.Height = 105;

            if (lblUsernameRequired.Visible || lblPasswordRequired.Visible)
            {
                if (lblUsernameRequired.Visible) bunifuTxtUser.Focus();
                else bunifuTxtPass.Focus();
                return;
            }

            // ---- authenticate ----
            string accountUser, accountName, accountType;
            try
            {
                using (POSDBEntities context = new POSDBEntities())
                {
                    user account = context.users.FirstOrDefault(candidate =>
                        candidate.usernm == username && candidate.password == password);

                    if (account == null)
                    {
                        MessageBox.Show("Incorrect username or password.", "Access Denied",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        bunifuTxtPass.Text = string.Empty;
                        bunifuTxtPass.Focus();
                        return;
                    }

                    accountUser = account.usernm;
                    accountName = account.nameuser;
                    accountType = account.usertype;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to sign in because the database could not be reached. Check the connection and try again.",
                    "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isAdmin = string.Equals(accountType, "ADMIN", StringComparison.OrdinalIgnoreCase);

            // ---- action-based authorization (from frmVerify) ----
            switch (_actionType)
            {
                case "LOGIN":
                    if (!isAdmin) { Deny(); return; }
                    if (_form1 != null) _form1._userName = accountUser;
                    Succeed(accountUser, accountName, accountType);
                    break;

                //case "VOID SALE":
                //    if (!isAdmin) { Deny(); return; }
                //    if (_frmcash != null) _frmcash.UnlockVoid();
                //    Succeed(accountUser, accountName, accountType);
                //    break;

                case "Z-READING":
                    if (!isAdmin) { Deny(); return; }
                    Succeed(accountUser, accountName, accountType);
                    //using (var z = new frmPrintZReading()) z.ShowDialog();
                    break;

                //case "RECENT SALES":          // any valid user
                //    Succeed(accountUser, accountName, accountType);
                //    using (var r = new frmCashierReport(accountUser)) r.ShowDialog();
                //    break;

                case "CASHIER":               // any valid user
                    if (_frmcash != null) _frmcash.userNm = accountUser;
                    Succeed(accountUser, accountName, accountType);
                    break;

                case "CASHIER EDIT ITEM":
                    if (!isAdmin) { Deny(); return; }
                    //    // form stays open, same as the old behavior
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

                        using (AddOrEditItemView editItemView = new AddOrEditItemView())
                        {
                            new AddOrEditItemPresenter(
                                editItemView,
                                _userNm,
                                _itemcode,
                                null); //(optional) 
                            editItemView.StartPosition = FormStartPosition.CenterParent;
                            POSTheme.ShowSmoothFadeDialog(editItemView, modalOwner);
                        }
                    }
                    //new frmAddItem(_itemcode, "UPDATE", _frmcash, "CASHIER", _userNm).Show();
                    break;

                default:
                    Deny();
                    break;
            }
        }

        private void Deny()
        {
            MessageBox.Show("Access Denied. This action requires an administrator account.", "Access Denied",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            bunifuTxtPass.Text = string.Empty;
            bunifuTxtPass.Focus();
        }

        private void Succeed(string usernm, string name, string usertype)
        {
            AuthenticatedUserName = usernm;
            AuthenticatedName = name;
            AuthenticatedUsertype = usertype;
            _completed = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                Login();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}