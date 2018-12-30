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
    /// Interaction logic for EventListDisplay.xaml
    /// </summary>
    public partial class EventListDisplay : UserControl
    {
        private MainWindow mainWindow;

        private struct EventInfo
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public int RSVP { get; set; }

            public EventInfo(int eventID, string eventName, string eventImage, int eventRSVP)
            {
                ID = eventID;
                Name = eventName;
                Image = eventImage;
                RSVP = eventRSVP;
            }
        }

        public EventListDisplay(List<int> eventList, int userID, MainWindow mainWin)
        {
            mainWindow = mainWin;

            InitializeComponent();

            EventsDisplay.ItemsSource = LoadEventInfo(eventList,userID);
        }

        private void EventInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EventRSVP_Click(object sender, RoutedEventArgs e)
        {

        }

        private List<EventInfo> LoadEventInfo(List<int> eventList, int userID)
        {
            List<EventInfo> info = new List<EventInfo>();

            foreach(int eventID in eventList)
            {
                // Get topic info from database/server/etc


                // Need to check if userID is contained in RSVP's
                // Figure out way to change RSVP/Cancel button appropriately

                // Add topic info to list
                info.Add(new EventInfo(eventID, "Event #" + eventID, @"/Placeholder Images/ratioTest.jpg", 50));
            }

            return info;
        }
    }
}
