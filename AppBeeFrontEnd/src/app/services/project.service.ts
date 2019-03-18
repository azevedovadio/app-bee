import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Project } from '../models/project.model';
import { Observable } from 'rxjs';

@Injectable()
export class ProjectService {

    constructor(private http: HttpClient) { }

    public get(): Observable<Project[]> {
        return this.http.get<Project[]>('http://localhost:12345/api/project');
    }

}
