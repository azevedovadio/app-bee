import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../services/project.service';
import { TaskService } from '../../services/task.service';
import { Project } from '../../models/project.model';
import { Task } from '../../models/task.model';
import { FormControl, FormBuilder, FormGroup} from '@angular/forms';
import { Observable} from 'rxjs';
import { map, startWith} from 'rxjs/operators';
import { ActivityService } from '../../services/activity.service';
import { Activity } from '../../models/activity.model';


@Component({
    templateUrl: 'home.component.html',
    styleUrls: ['home.component.scss']
})
export class HomeComponent implements OnInit {

    public projects: Project[];
    public tasks: Task[];

    public selectedProject: Project;
    public selectedTask: Task;
    public currentActivity: any;

    public projectName: string;
    public taskName: string;

    public filteredProjects: Observable<Project[]>;
    public filteredTasks: Observable<Task[]>;

    public form: FormGroup;

    constructor(private fb: FormBuilder, 
      private projectService: ProjectService, 
      private taskService: TaskService,
      private activityService: ActivityService) {
        this.projectService.get().subscribe(_ => this.projects = _);
        this.taskService.get().subscribe(_ => this.tasks = _);
    }

    ngOnInit() {
      this.form = this.fb.group({
        project: null,
        task: null
      });

      this.filteredProjects = this.form.get('project').valueChanges
        .pipe(
          startWith(''),
          map(value => this._filterProjects(value))
        );

      this.filteredTasks = this.form.get('task').valueChanges
        .pipe(
          startWith(''),
          map(value => this._filterTasks(value))
        );
    }

    public onProjectChanged(event: any) : void {
      this.selectedProject = event.option.value;
      this.projectName = event.option.value.name;
    }

    public onTaskChanged(event: any) : void {
      this.selectedTask = event.option.value;
      this.taskName = event.option.value.description;
    }

    private _filterProjects(value: string): Project[] {      
      this.form.get('task').reset();
      if (this.projects && typeof value === 'string' && value.trim() !== '') {
        const filterValue = value.toLowerCase();        
        return this.projects.filter(option => option.name.toLowerCase().includes(filterValue));
      }
      return [];
    }

    private _filterTasks(value: string): Task[] {
      if (this.tasks && typeof value === 'string' && value.trim() !== '' && this.selectedProject) {
        const filterValue = value.toLowerCase();
        return this.tasks.filter(option => option.projectId === this.selectedProject.id && option.description.toLowerCase().includes(filterValue));
      }
      return [];
    }

    public isDisabled(): boolean {
      return !this.selectedProject || !this.selectedTask || !this.selectedProject.id || !this.selectedTask.id;
    }

    public taskOpened(){
      this.selectedTask = undefined;
    }

    public taskClosed() {
      if(!this.selectedTask){
        this.taskName = '';
      }
    }

    public projectOpened(){
      this.selectedProject = undefined;
    }

    public projectClosed() {
      if(!this.selectedProject){
        this.projectName = '';
      }
    }

    public displayProject(project: Project){
      return project ?  project.name : project;
    }
    
    public displayTask(task: Task){
      return task ? task.description : task;
    }

    public start(){
       let activity: Activity = {
         projectId: this.selectedProject.id,
         taskId: this.selectedTask.id,
         startDate: new Date()
       }

       this.activityService.post(activity).then(e => {

        this.currentActivity = {
         projectName: this.projects.find(_ => _.id == e.projectId).name,
         taskName: this.tasks.find(_ => _.id == e.taskId).description,
         startDate: activity.startDate
       };

       
      });
    }
}
