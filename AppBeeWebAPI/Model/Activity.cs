using System;

namespace AppBeeWebAPI.Model
{
    public class Activity
    {
        public Guid? id { get; set; } 
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
