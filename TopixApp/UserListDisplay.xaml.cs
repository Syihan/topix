using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace TopixApp
{
    /// <summary>
    /// Interaction logic for UserListDisplay.xaml
    /// </summary>
    public partial class UserListDisplay : UserControl
    {
        private MainWindow mainWindow;

        public UserListDisplay(List<User> userList, MainWindow mainWin)
        {
            // Save instance of the main window
            mainWindow = mainWin;

            InitializeComponent();

            // Load the user items into the ItemsControl
            UserDisplay.ItemsSource = userList;

            if(userList.Count == 0)
                NoneFound.Visibility = Visibility.Visible;
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            // Get userID from the tag assigned to the button
            int userID = (int)((System.Windows.Controls.Button)sender).Tag;

            // Set the main window display to the chosen user profile
            mainWindow.ContentDisplay.Content = new UserProfile(userID, mainWindow);
        }
    }
}
