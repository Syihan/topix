using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopixApp
{
    public class Topic
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get { return GlobalMethodResource.ProjectDirectory + @"\Topic Images\" + ID + ".png"; } }

        public Topic()
        {
            ID = -1;
            Name = string.Empty;
            Bio = string.Empty;
        }

        public Topic(int topicID, string topicName, string topicBio)
        {
            ID = topicID;
            Name = topicName;
            Bio = topicBio;
        }

    }
}
