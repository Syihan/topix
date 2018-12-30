using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace TopixApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentUserID;
        private DBConnect dbConnect;

        public MainWindow()
        {
            currentUserID = 0;
            dbConnect = new DBConnect();

            InitializeComponent();

            Login();
            ProfileTabImage.Source = new BitmapImage(new Uri(GlobalMethodResource.ProjectDirectory + @"\Profile Images\" + currentUserID + ".png"));
            ContentDisplay.Content = new UserProfile(currentUserID,this);
        }

        private void Login()
        {
            this.Hide(); // Hide the main window

            LoginWindow login = new LoginWindow(dbConnect);
            if(login.ShowDialog() == true)
            {
                currentUserID = login.GetID(); // Get the ID stored in the login window
            }
            else
            {
                // Close the application, login did not return true (login only returns when logged in, account created, or window closed)
                this.Close();
                return; // I'm returning just in case
            }


            this.Show(); // Re-open the main window
        }

        public DBConnect GetConnection()
        {
            return dbConnect;
        }

        private void OpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenu("slideOut");
        }

        private void CloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenu("slideIn");
        }

        private void ProfileNav_Click(object sender, RoutedEventArgs e)
        {
            ContentDisplay.Content = new UserProfile(currentUserID,this);
            CloseMenu_Click(null,null);
        }

        private void HubNav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TopixNav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EventNav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchNav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MessageNav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsNav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogoutNav_Click(object sender, RoutedEventArgs e)
        {
            Login();
            CloseMenu_Click(null,null);

            ProfileTabImage.Source = new BitmapImage(new Uri(GlobalMethodResource.ProjectDirectory + @"\Profile Images\" + currentUserID + ".png"));
            ContentDisplay.Content = new UserProfile(currentUserID,this);
        }

        private void ToggleMenu(string sbType)
        {
            // Code based on http://www.codescratcher.com/wpf/sliding-panel-in-wpf/

            Storyboard sb = Resources[sbType] as Storyboard;
            sb.Begin(navBar);

            if(sbType.Equals("slideOut"))
            {
                OpenButton.Visibility = Visibility.Hidden;
                CloseButton.Visibility = Visibility.Visible;
            }
            else
            {
                OpenButton.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Hidden;
            }
        }

        public int GetCurrentUserID()
        {
            return currentUserID;
        }
    }
}
