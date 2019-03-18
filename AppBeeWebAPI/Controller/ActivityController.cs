using AppBeeWebAPI.Board;
using AppBeeWebAPI.Model;
using System;
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

        public void Post(Activity activity)
        {
            activityBoard.Add(activity);
           
        }

        public IEnumerable<Activity> Get()
        {
            return activityBoard.GetActivities();
        }
    }
}
