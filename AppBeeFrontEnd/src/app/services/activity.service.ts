import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Activity } from '../models/activity.model';
import { Observable } from 'rxjs';

@Injectable()
export class ActivityService {

    constructor(private http: HttpClient) { }

    public get(): Observable<Activity[]> {
        return this.http.get<Activity[]>('http://localhost:12345/api/activity');
    }

    public post(activity: Activity) : Promise<Activity> {
        return this.http.post<Activity>('http://localhost:12345/api/activity', activity)
            .toPromise();
    }

}
