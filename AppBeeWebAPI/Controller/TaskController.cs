using AppBeeWebAPI.Board;
using AppBeeWebAPI.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace AppBeeWebAPI.Controller
{
    public class TaskController: ApiController
    {
        private TaskBoard taskBoard;

        public TaskController(TaskBoard board)
        {
            taskBoard = board;
        }

        public IEnumerable<Task> Get()
        {
            return taskBoard.GetTasks();
        }
    }
}
