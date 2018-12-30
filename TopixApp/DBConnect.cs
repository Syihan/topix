using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TopixApp
{
    public class DBConnect
    {
        // Currently following https://www.codeproject.com/Articles/837599/Using-Csharp-to-connect-to-and-query-from-a-SQL-da for executing commands/etc
        SqlConnection connection;
        
        public DBConnect()
        {
            StartConnection();
        }

        public void StartConnection()
        {
            // Will need to change the Attach DbFilename to dynamically select the database security file in "..\TopixApp\TopixApp\"
            // John Connection String = Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\John\Desktop\TopixApp\TopixApp\TopixDatabase.mdf;Integrated Security=True
            // Syihan Connection String = Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Syihan\Google Drive\West Virginia University\'17 - '18 Senior Year\CS 430 - Advanced Software Engineering\TopixDesktop\TopixApp\TopixDatabase.mdf;Integrated Security=True
            connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Syihan\Google Drive\West Virginia University\'17 - '18 Senior Year\CS 430 - Advanced Software Engineering\TopixDesktop\TopixApp\TopixDatabase.mdf;Integrated Security=True");
        }

        public void CloseConnection()
        {
            connection.Dispose();
        }

        public User GetUserInfo(int userID)
        {
            User user = new User();

            using(SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.UserData WHERE Id = " + userID, connection))
            {
                connection.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            user.ID = reader.GetInt32(reader.GetOrdinal("Id"));
                            user.Firstname = reader.GetString(reader.GetOrdinal("FirstName"));
                            user.State = reader.GetString(reader.GetOrdinal("State"));
                            user.City = reader.GetString(reader.GetOrdinal("City"));
                            user.Email = reader.GetString(reader.GetOrdinal("Email"));

                            // Only recording index for nullable values to avoid calling GetOrdinal multiple times
                            // Normally will use GetOrdinal within reader.GetString(x), etc.
                            int lastNameIndex = reader.GetOrdinal("LastName");
                            int userBio = reader.GetOrdinal("Bio");
                            
                            // For nullable columns, check if null
                            if(!reader.IsDBNull(lastNameIndex))
                                user.Lastname = reader.GetString(lastNameIndex);
                            if (!reader.IsDBNull(userBio))
                                user.Bio = reader.GetString(userBio);
                        }
                    }
                }
                connection.Close();
            }

            return user;
        }

        public Topic GetTopicInfo(int topicID)
        {
            Topic topic = new Topic();

            using(SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.TopicData WHERE Id = " + topicID, connection))
            {
                connection.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            topic.ID = reader.GetInt32(reader.GetOrdinal("Id"));
                            topic.Name = reader.GetString(reader.GetOrdinal("Name"));
                            topic.Bio = reader.GetString(reader.GetOrdinal("Bio"));
                        }
                    }
                }
                connection.Close();
            }

            return topic;
        }

        public int LoginUser(string email, string password)
        {
            int userID = -1; // Default ID

            using(SqlCommand cmd = new SqlCommand("SELECT Id FROM dbo.LoginTable WHERE Email = '" + email + "' AND Password = '" + password + "'", connection))
            {
                connection.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        reader.Read(); // Read entry
                        userID = reader.GetInt32(reader.GetOrdinal("Id")); // Get data
                    }
                }
                connection.Close();
            }

            return userID;
        }

        public bool UserExists(string email)
        {
            bool result = false;

            using(SqlCommand cmd = new SqlCommand("SELECT Id FROM dbo.LoginTable WHERE Email = '" + email + "'", connection))
            {
                connection.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Row was found -> user exists
                    if(reader.HasRows)
                        result = true;
                }
                connection.Close();
            }

            return result;
        }

        public int CreateUser(string first, string last, string email, string password, string state, string city, string bio)
        {
            // Add information to UserData table
            using(SqlCommand cmd = new SqlCommand("INSERT INTO dbo.UserData (FirstName,LastName,State,City,Bio,Email) VALUES('" + first + "','" + last + "','" + state + "','" + city + "','" + bio + "','" + email + "')", connection))
            {
                connection.Open();
                
                cmd.ExecuteNonQuery();

                connection.Close();
            }

            int userID = -1;
            // Get the user ID from the inserted user
            using(SqlCommand cmd = new SqlCommand("SELECT Id FROM dbo.UserData WHERE Email = '" + email + "'", connection))
            {
                connection.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read(); // Read entry
                    userID = reader.GetInt32(reader.GetOrdinal("Id")); // Get data
                }
                connection.Close();
            }

            // Add information to LoginTable
            using(SqlCommand cmd = new SqlCommand("INSERT INTO dbo.LoginTable (Id,Email,Password) VALUES(" + userID + ",'" + email + "','" + password + "')", connection))
            {
                connection.Open();
                
                cmd.ExecuteNonQuery();

                connection.Close();
            }

            return userID;
        }

        public bool CheckFriends(int userID, int otherUserID)
        {
            // Checks to see if the provided user is following the other user

            bool result = false;

            using(SqlCommand cmd = new SqlCommand("SELECT UserID FROM dbo.UserFollow WHERE UserID = " + userID + " AND FollowerID = " + otherUserID, connection))
            {
                connection.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Row was found -> user/follower pair exists
                    if(reader.HasRows)
                        result = true;
                }
                connection.Close();
            }

            return result;
        }

        public bool CheckFollower(int topicID, int userID)
        {
            // Checks to see if the provided user follows the topic

            bool result = false;

            using(SqlCommand cmd = new SqlCommand("SELECT UserID FROM dbo.TopicFollow WHERE TopicID = " + topicID + " AND UserID = " + userID, connection))
            {
                connection.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Row was found -> topic/user pair exists
                    if(reader.HasRows)
                        result = true;
                }
                connection.Close();
            }

            return result;
        }

        public List<User> GetFriends(int userID)
        {
            List<int> friendIDs = new List<int>();

            // Get all user ID's that the provided user follows
            using(SqlCommand cmd = new SqlCommand("SELECT FollowerID FROM dbo.UserFollow WHERE UserID = " + userID, connection))
            {
                connection.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            friendIDs.Add(reader.GetInt32(reader.GetOrdinal("FollowerID"))); // Add the follower ID to the ID list
                        }
                    }
                }
                connection.Close();
            }

            // Get user data for all the follower ID's
            List<User> friends = new List<User>();

            foreach(int friendID in friendIDs)
                friends.Add(GetUserInfo(friendID));

            return friends;
        }

        public List<Topic> GetTopics(int userID)
        {
            List<int> topicIDs = new List<int>();

            // Get all topic ID's that the provided user follows
            using(SqlCommand cmd = new SqlCommand("SELECT TopicID FROM dbo.TopicFollow WHERE UserID = " + userID, connection))
            {
                connection.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            topicIDs.Add(reader.GetInt32(reader.GetOrdinal("TopicID"))); // Add the topic ID to the ID list
                        }
                    }
                }
                connection.Close();
            }

            // Get topic data for all the topic ID's
            List<Topic> topics = new List<Topic>();

            foreach(int topicID in topicIDs)
                topics.Add(GetTopicInfo(topicID));

            return topics;
        }

        public List<User> GetFollowers(int topicID, int userID)
        {
            User currentUser = GetUserInfo(userID); // Get user info so we can read what their location is at
            
            List<int> followerIDs = new List<int>();

            // Get all user ID's that follow the provided topic in the same area that the current user is in
            using(SqlCommand cmd = new SqlCommand("SELECT UserID FROM dbo.TopicFollow WHERE TopicID = " + topicID + " AND State = '" + currentUser.State + "' AND City = '" + currentUser.City + "'", connection))
            {
                connection.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            followerIDs.Add(reader.GetInt32(reader.GetOrdinal("UserID"))); // Add the user ID to the ID list
                        }
                    }
                }
                connection.Close();
            }

            // Get user data for all the follower ID's
            List<User> followers = new List<User>();

            foreach(int followerID in followerIDs)
                followers.Add(GetUserInfo(followerID));

            return followers;
        }

        public void AddFriend(int userID, int otherUserID)
        {
            using(SqlCommand cmd = new SqlCommand("INSERT INTO dbo.UserFollow (UserID,FollowerID) VALUES(" + userID + "," + otherUserID + ")", connection))
            {
                connection.Open();
                
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void RemoveFriend(int userID, int otherUserID)
        {
            using(SqlCommand cmd = new SqlCommand("DELETE FROM dbo.UserFollow WHERE UserID = " + userID + " AND FollowerID = " + otherUserID, connection))
            {
                connection.Open();
                
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void AddTopic(int userID, int topicID)
        {
            User currentUser = GetUserInfo(userID);

            using(SqlCommand cmd = new SqlCommand("INSERT INTO dbo.TopicFollow (TopicID,UserID,State,City) VALUES(" + topicID + "," + userID + ",'" + currentUser.State + "','" + currentUser.City + "')", connection))
            {
                connection.Open();
                
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void RemoveTopic(int userID, int topicID)
        {
            using(SqlCommand cmd = new SqlCommand("DELETE FROM dbo.TopicFollow WHERE TopicID = " + topicID + " AND UserID = " + userID, connection))
            {
                connection.Open();
                
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void UpdateUser(int userID, string first, string last, string state, string city, string bio)
        {
            // Update UserData Table
            using(SqlCommand cmd = new SqlCommand("UPDATE dbo.UserData SET FirstName = '" + first + "', LastName = '" + last + "', State = '" + state + "', City = '" + city + "', Bio = '" + bio + "' WHERE Id = " + userID, connection))
            {
                connection.Open();
                
                cmd.ExecuteNonQuery();

                connection.Close();
            }

            // Update location for all topics that the user follows
            using(SqlCommand cmd = new SqlCommand("UPDATE dbo.TopicFollow SET State = '" + state + "', City = '" + city + "' WHERE UserID = " + userID, connection))
            {
                connection.Open();
                
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
