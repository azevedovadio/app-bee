using AppBeeWebAPI.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AppBeeWebAPI.Board
{
    public class ProjectBoard
    {
        private List<Project> projects;

        public ProjectBoard()
        {
            Update("projetos.txt");
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = "./",
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "projetos.txt"
            };

            watcher.Changed += OnChanged;

            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1000);
            Update(e.FullPath);
        }

        private void Update(string filePath)
        {
            var file = new StreamReader(filePath);
            string line = null;

            projects = new List<Project>();

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    var info = line.Split(';');
                    projects.Add(new Project() { Name = info[0], Id = int.Parse(info[1].Trim()) });
                }
                catch
                {

                }
            }
        }

        public List<Project> GetProjects()
        {
            return projects;
        }
    }
}
