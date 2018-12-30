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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TopixApp
{
    /// <summary>
    /// Interaction logic for TopicListDisplay.xaml
    /// </summary>
    public partial class TopicListDisplay : UserControl
    {
        private MainWindow mainWindow;

        public TopicListDisplay(List<Topic> topicList, MainWindow mainWin)
        {
            // Save instance of the main window
            mainWindow = mainWin;

            InitializeComponent();

            // Load the user items into the ItemsControl
            TopicDisplay.ItemsSource = topicList;

            if(topicList.Count == 0)
                NoneFound.Visibility = Visibility.Visible;
        }

        private void TopicButton_Click(object sender, RoutedEventArgs e)
        {
            // Get userID from the tag assigned to the button
            int topicID = (int)((System.Windows.Controls.Button)sender).Tag;

            // Set the main window display to the chosen user profile
            mainWindow.ContentDisplay.Content = new TopicProfile(topicID, mainWindow);
        }
    }
}
