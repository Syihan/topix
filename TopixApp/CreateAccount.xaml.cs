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
using System.Windows.Shapes;

namespace TopixApp
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        DBConnect dbConnect;
        public int newUserID { get; set; }

        public CreateAccount(DBConnect connection)
        {
            dbConnect = connection;
            newUserID = -1;

            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if necessary fields are not empty
            if(FirstNameInput.Text != string.Empty && EmailInput.Text != string.Empty && PasswordInput.Text != string.Empty && PasswordInput.Text.Equals(ConfirmPasswordInput.Text))
            {
                // All necessary inputs are entered

                // Check if user doesn't already exist
                if(!dbConnect.UserExists(EmailInput.Text))
                {
                    // Insert user information into user data
                    newUserID = dbConnect.CreateUser(FirstNameInput.Text, LastNameInput.Text, EmailInput.Text, PasswordInput.Text, StateInput.Text, CityInput.Text, BioInput.Text);

                    // Save default icon image to Profile Images (size 125x125)
                    GlobalMethodResource.SaveAvatar(GlobalMethodResource.ProjectDirectory + @"\Placeholder Images\profile.png",newUserID);

                    // Add to default topic for demonstration
                    dbConnect.AddTopic(newUserID, 4);

                    this.DialogResult = true; // Set result of window to true
                    this.Close();
                }
                
                // User already exists
                else
                {
                    MessageBox.Show("Email provided is already used. Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("One of the required fields is empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to cancel?","Cancel",MessageBoxButton.YesNo,MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                this.Close();
        }
    }
}
