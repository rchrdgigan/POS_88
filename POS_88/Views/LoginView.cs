using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using POS_88.Database;

namespace POS_88.Views
{
    public partial class LoginView : Form
    {
        private const uint EmSetCueBanner = 0x1501;
        public string AuthenticatedUserName { get; private set; }
        public string AuthenticatedName { get; private set; }
        public string AuthenticatedUsertype { get; private set; }


        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr handle, uint message, IntPtr showWhenFocused, string cueBannerText);

        public LoginView()
        {
            InitializeComponent();
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
            btnClose.Click += delegate
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
        }


        private void ApplyTheme()
        {
            // topbar gradients
            lblModalTitle.BackColor = Color.Transparent;
            POSTheme.ApplyTopbar(panel1);

            // button close
            btnMinimize.BackColor = POSTheme.Navy;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;

            // button close
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

        private void Login()
        {
            string username = bunifuTxtUser.Text.Trim();
            string password = bunifuTxtPass.Text;

            lblUsernameRequired.Visible = string.IsNullOrWhiteSpace(username);
            lblPasswordRequired.Visible = string.IsNullOrEmpty(password);
            if (string.IsNullOrWhiteSpace(username))
            {
                panelUser.Height = 90;
            }
            else
            {
                panelUser.Height = 72;
            }
            if (string.IsNullOrEmpty(password))
            {
                panelPass.Height = 105;
            }
            else
            {
                panelPass.Height = 105;
            }

            if (lblUsernameRequired.Visible || lblPasswordRequired.Visible)
            {
                if (lblUsernameRequired.Visible)
                    bunifuTxtUser.Focus();
                else
                    bunifuTxtPass.Focus();

                return;
            }

            try
            {
                using (POSDBEntities context = new POSDBEntities())
                {
                    user account = context.users.SingleOrDefault(candidate =>
                        candidate.usernm == username && candidate.password == password);

                    if (account == null || !string.Equals(account.usertype, "ADMIN", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Invalid username or password.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        bunifuTxtPass.Focus();
                        return;
                    }

                    AuthenticatedUserName = account.usernm;
                    AuthenticatedName = account.nameuser;
                    AuthenticatedUsertype = account.usertype;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to sign in because the database could not be reached. Check the connection and try again.",
                    "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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