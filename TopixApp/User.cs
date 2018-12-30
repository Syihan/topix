using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopixApp
{
    public class User
    {
        // Set up all data contained in a user object
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Bio { get; set; }
        public string State{ get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Image { get { return GlobalMethodResource.ProjectDirectory + @"\Profile Images\" + ID + ".png"; } }
        public string FullName { get { return GetFullName(); } }


        public User()
        {
            // Initialize all values to empty/default
            ID = -1;
            Firstname = string.Empty;
            Lastname = string.Empty;
            Bio = string.Empty;
            State = string.Empty;
            City = string.Empty;
            Email = string.Empty;
        }

        public string GetFullName()
        {
            if(Lastname == string.Empty)
                return Firstname;

            else
                return Firstname + " " + Lastname;
        }
    }
}
