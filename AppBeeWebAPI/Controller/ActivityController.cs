using AppBeeWebAPI.Board;
using AppBeeWebAPI.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;

namespace AppBeeWebAPI.Controller
{
    public class ActivityController: ApiController
    {
        private ActivityBoard activityBoard;

        public ActivityController(ActivityBoard board)
        {
            activityBoard = board;
        }

        public Activity Post(Activity activity)
        {
            if(activity.id == null)
            {
                activity.id = Guid.NewGuid();
            }

            var current = activityBoard.GetActivities().FirstOrDefault(_ => _.EndDate == null);
            if(current != null)
            {
                current.EndDate = DateTime.Now;
            }

            activityBoard.Add(activity);

            return activity;
           
        }

        public IEnumerable<Activity> Get()
        {
            return activityBoard.GetActivities();
        }
    }
}
