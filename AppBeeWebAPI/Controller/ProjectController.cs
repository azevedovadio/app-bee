using AppBeeWebAPI.Board;
using AppBeeWebAPI.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace AppBeeWebAPI.Controller
{

    public class ProjectController : ApiController
    {
        private ProjectBoard projectBoard;

        public ProjectController(ProjectBoard board)
        {
            projectBoard = board;
        }

        public IEnumerable<Project> Get()
        {
            return projectBoard.GetProjects();
        }
    }
}
