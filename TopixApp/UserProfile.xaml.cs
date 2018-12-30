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
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Page
    {
        private MainWindow mainWindow;
        private DBConnect dbConnect;
        private int profileID;
        private bool isLoggedInUser;

        // Will change parameters to accept a userID and reference to parent window (so we can change the current page and do other methods in main class)
        public UserProfile(int userProfileID, MainWindow mainWin)
        {
            mainWindow = mainWin;
            dbConnect = mainWindow.GetConnection();
            
            // Obtain information from database/server/data file using correlated userID
            User userInfo = dbConnect.GetUserInfo(userProfileID);

            InitializeComponent();

            profileID = userProfileID;
            UserName.Content = userInfo.GetFullName();
            
            UserAvatar.Source = new BitmapImage(new Uri(userInfo.Image));

            // Collapse bio section if no bio is found
            if(userInfo.Bio == string.Empty)
                BioBlock.Visibility = Visibility.Collapsed;
            else
                BioBlock.Text = userInfo.Bio;

            // Change what the profile button displays and keep track if logged in user
            if(isLoggedInUser = profileID == mainWindow.GetCurrentUserID())
                ProfileButton.Content = "Edit Profile";
            else if(dbConnect.CheckFriends(mainWindow.GetCurrentUserID(), profileID))
                ProfileButton.Content = "Unfollow";
            else
                ProfileButton.Content = "Follow";

            // Initialize all usercontrols
            UserTopixList.Content = new TopicListDisplay(dbConnect.GetTopics(profileID), mainWindow); // Use list of topicID's received from database info
            UserFriendList.Content = new UserListDisplay(dbConnect.GetFriends(profileID), mainWindow); // Use list of userID's received from database info
            UserEventList.Content = new EventListDisplay(new List<int>(){0,1,2,3,4}, profileID, mainWindow); // Use list of eventID's received from database info
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if(isLoggedInUser)
            {
                // Open edit profile
            }
            else if(ProfileButton.Content.Equals("Unfollow"))
            {
                dbConnect.RemoveFriend(mainWindow.GetCurrentUserID(),profileID);
                ProfileButton.Content = "Follow";
            }
            else
            {
                dbConnect.AddFriend(mainWindow.GetCurrentUserID(),profileID);
                ProfileButton.Content = "Unfollow";
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            GlobalMethodResource.HorizontalScrollEvent(sender,e);
        }
    }
}
