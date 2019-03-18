using AppBeeWebAPI.Model;
using System.Collections.Generic;
using System.IO;

namespace AppBeeWebAPI.Board
{
    public class TaskBoard
    {
        private List<Task> tasks;

        public TaskBoard()
        {
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = "./",
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "taks.csv"
            };

            watcher.Changed += OnChanged;

            watcher.EnableRaisingEvents = true;

            Update("taks.csv");
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {   
            Update(e.FullPath);
        }

        private void Update(string fullPath)
        {
            try
            {
                var file = new StreamReader(fullPath);
                string line = null;

                tasks = new List<Task>();

                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        var info = line.Split(';');
                        tasks.Add(new Task() { Id = int.Parse(info[0]), ProjectId = int.Parse(info[1]), Assignee = info[2], Description = info[3], IsCompleted = int.Parse(info[4]) == 1 });
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }

        public List<Task> GetTasks()
        {
            return tasks;
        }
    }
}
