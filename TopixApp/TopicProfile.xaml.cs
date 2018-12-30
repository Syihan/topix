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
    /// Interaction logic for TopicProfile.xaml
    /// </summary>
    public partial class TopicProfile : Page
    {
        private MainWindow mainWindow;
        private DBConnect dbConnect;
        private int userID;
        private int topicID;
        //private List<Event> events;

        public TopicProfile(int tID, MainWindow mainWin)
        {
            mainWindow = mainWin;
            dbConnect = mainWindow.GetConnection();
            userID = mainWindow.GetCurrentUserID();
            topicID = tID;

            Topic topic = dbConnect.GetTopicInfo(topicID);

            InitializeComponent();

            TopicName.Content = topic.Name;
            TopicAvatar.Source = new BitmapImage(new Uri(topic.Image));
            TopicBio.Text = topic.Bio;

            List<User> followers = dbConnect.GetFollowers(topicID,userID);
            LocalUserCount.Content = followers.Count + " Local Users";

            if(dbConnect.CheckFollower(topicID,userID))
                TopicButton.Content = "Unfollow";
            else
                TopicButton.Content = "Follow";

            TopicFollowerList.Content = new UserListDisplay(followers, mainWindow); // Use list of userID's received from database info
            TopicEventList.Content = new EventListDisplay(new List<int>(){0,1,2,3,4}, userID, mainWindow); // Use list of eventID's received from database info
        }

        private void TopicButton_Click(object sender, RoutedEventArgs e)
        {
            if(TopicButton.Content.Equals("Unfollow"))
            {
                dbConnect.RemoveTopic(userID,topicID);
                TopicButton.Content = "Follow";
            }
            else
            {
                dbConnect.AddTopic(userID,topicID);
                TopicButton.Content = "Unfollow";
            }

            // Update follower list
            List<User> followers = dbConnect.GetFollowers(topicID,userID);
            LocalUserCount.Content = followers.Count + " Local Users";
            TopicFollowerList.Content = new UserListDisplay(followers, mainWindow); // Use list of userID's received from database info
        }
    }
}
