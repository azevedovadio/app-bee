using AppBeeWebAPI.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AppBeeWebAPI.Board
{
    public class ActivityBoard
    {
        private List<Activity> activities = new List<Activity>();

        public ActivityBoard()
        {
            try
            {
                var content = File.ReadAllText("activities.json");
                activities = JsonConvert.DeserializeObject<Activity[]>(content).ToList();
            }
            catch
            {

            }
        }

        public void Add(Activity activity)
        {
            try
            {
                activities.Add(activity);

                File.WriteAllText("activities.json", JsonConvert.SerializeObject(activities));
            }
            catch
            {

            }
        }

        public List<Activity> GetActivities()
        {
            return activities;
        }
    }
}
