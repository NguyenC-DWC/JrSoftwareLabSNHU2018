using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Financing
{
    /// <summary>
    /// Interaction logic for loginWindow.xaml
    /// </summary>
    public partial class login : Window
    {
        // Creates Thread.
        public BackgroundWorker bw = new BackgroundWorker();

        // Constructor, Initializes thread.
        public login()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            String userName = txtBxuserName.Text.ToLower();
            string pass = passBxPassword.Password;

            // Hides "Forgot password?", and shows "loading ...".
            lblfrgtPass.Visibility = Visibility.Hidden;
            lblLoading.Visibility = Visibility.Visible;

            // Enables hourglass cursor while calidating credentials.
            Mouse.OverrideCursor = Cursors.Wait;

            // Disables the username, password, and login button 
            // while validating already entered credentials.
            txtBxuserName.IsEnabled = false;
            passBxPassword.IsEnabled = false;
            btnLogin.IsEnabled = false;

            // Validates user credentials.
            if (userName == "admin" && pass == "admin")
            {
                if (bw.IsBusy == false)
                {
                    bw.RunWorkerAsync();
                }
            }
            else
            {
                // Hides "loading ...", and shows "Forgot password?".
                lblLoading.Visibility = Visibility.Hidden;
                lblfrgtPass.Visibility = Visibility.Visible;
                lblfrgtPass.Content = "Forgot Password ?";

                // Re-enables username and password fields and sets them to red.
                txtBxuserName.IsEnabled = true;
                txtBxuserName.BorderBrush = Brushes.Red;
                passBxPassword.IsEnabled = true;
                passBxPassword.BorderBrush = Brushes.Red;
                btnLogin.IsEnabled = true;

                // Disables hourglass cursor.
                Mouse.OverrideCursor = null;
            }
        }

        // Has the window wait after entering credentials.
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            //Thread.Sleep(2000);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                lblfrgtPass.Visibility = Visibility.Visible;
                lblfrgtPass.Content = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                lblfrgtPass.Visibility = Visibility.Visible;
                lblfrgtPass.Content = "Error: " + e.Error.Message;
            }
            else
            {
                lblLoading.Visibility = Visibility.Hidden;
                lblfrgtPass.Visibility = Visibility.Visible;
                lblfrgtPass.Content = "LOGIN SUCCESSFUL";
                Mouse.OverrideCursor = null;
                txtBxuserName.IsEnabled = true;
                passBxPassword.IsEnabled = true;
                btnLogin.IsEnabled = true;
                var newW = new MainWindow();
                newW.Show();

                this.Close();
            }
        }

        // Currently unneeded
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        // Exit button closes window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        // Lets the window be dragged.
        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Returns border to white if text has changed after incorrectly entering username
        private void txtBxuserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBxuserName.BorderBrush = Brushes.White;
        }

        // Returns border to white if text has changed 
        // after incorrectly entering password
        private void passBxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passBxPassword.BorderBrush = Brushes.White;
        }

        // If enter is pressed moves to password field.
        private void txtBxuserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
                txtBxuserName.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        // If enter is pressed moves to button.
        private void passBxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
                btnLogin.Focus();
        }
    }
}