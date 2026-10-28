using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using POS_88.Presenters;
using POS_88.Views;

namespace POS_88
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainView mainView = new MainView();
            MainPresenter mainPresenter = new MainPresenter(mainView);
            mainView.Show();

            DialogResult loginResult;
            string authenticatedUserName;
            string authenticatedUsertype;
            string authenticatedName;

            using (Form backdrop = new Form())
            {
                backdrop.FormBorderStyle = FormBorderStyle.None;
                backdrop.StartPosition = FormStartPosition.Manual;
                backdrop.ShowInTaskbar = false;
                backdrop.BackColor = System.Drawing.Color.Black;
                backdrop.Opacity = 0.45;
                backdrop.Bounds = mainView.Bounds;
                backdrop.Show(mainView);
                backdrop.BringToFront();

                using (LoginView loginView = new LoginView())
                {
                    loginResult = loginView.ShowDialog(mainView);
                    authenticatedUserName = loginView.AuthenticatedUserName;
                    authenticatedName = loginView.AuthenticatedName;
                    authenticatedUsertype = loginView.AuthenticatedUsertype;

                }
            }

            if (loginResult != DialogResult.OK)
            {
                mainView.Close();
                return;
            }

            mainView.AuthenticatedUserName = authenticatedUserName;
            mainView.AuthenticatedUsertype = authenticatedUsertype;
            mainView.AuthenticatedName = authenticatedName;

            Application.Run(mainView);
        }
    }
}
