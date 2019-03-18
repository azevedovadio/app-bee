using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBeeWebAPI.Model
{
    public class Task
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Assignee { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
