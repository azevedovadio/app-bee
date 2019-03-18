using System;

namespace AppBeeWebAPI.Model
{
    public class Activity
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreationDate { get; set; }
        public int Status { get; set; }
    }
}
