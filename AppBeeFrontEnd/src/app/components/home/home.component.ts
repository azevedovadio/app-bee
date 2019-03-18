import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../services/project.service';
import { TaskService } from '../../services/task.service';
import { Project } from '../../models/project.model';
import { Task } from '../../models/task.model';
import {FormControl, FormBuilder, FormGroup} from '@angular/forms';
import {Observable} from 'rxjs';
import {map, startWith} from 'rxjs/operators';


@Component({
    templateUrl: 'home.component.html',
    styleUrls: ['home.component.scss']
})
export class HomeComponent {

    public projects: Project[];
    public tasks: Task[];

    public projectsControl = new FormControl();
    public tasksControl = new FormControl();

    public filteredProjects: Observable<Project[]>;
    public filteredTasks: Observable<Task[]>;

    form: FormGroup;

    constructor(private fb: FormBuilder, private projectService: ProjectService, private taskService: TaskService) {
        this.projectService.get().subscribe(_ => this.projects = _);
        this.taskService.get().subscribe(_ => this.tasks = _);
        this.projectService.get().subscribe(_ => console.log(_));
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

    private _filterProjects(value: string): Project[] {

      console.log(value);

      if(!value) {
        return [];
      }

      const filterValue = value.toLowerCase();

      if (!this.projects) {
        return [];
      }
      let result = this.projects.filter(option => option.name.toLowerCase().includes(filterValue));
      console.log(result);
      return result;
    }

    private _filterTasks(value: string): Task[] {
      console.log(value);

      if(!value) {
        return [];
      }

      const filterValue = value.toLowerCase();
      console.log(filterValue);
      console.log('testee');
      if (!this.tasks) {
        return [];
      }
      return this.tasks.filter(option => option.description.toLowerCase().includes(filterValue));
    }

    public displayFnProject(user: Project) {
      console.log(user);
      if (user) { return user.name; }
    }

    public displayFnTask(user: Task) {
      console.log(user);
      if (user) { return user.description; }
    }
}
