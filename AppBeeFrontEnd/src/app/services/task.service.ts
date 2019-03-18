import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Task } from '../models/task.model';
import { Observable } from 'rxjs';

@Injectable()
export class TaskService {

    constructor(private http: HttpClient) { }

    public get(): Observable<Task[]> {
        return this.http.get<Task[]>('http://localhost:12345/api/task');
    }

}
