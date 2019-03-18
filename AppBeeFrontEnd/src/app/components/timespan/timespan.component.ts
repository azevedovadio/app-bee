import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { interval } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

@Component({
    selector: 'timespan',
    template: '<span>{{counter | async}}</span>'
})
export class TimespanComponent {

    @Input() start: Date;

    public counter: Observable<string>;

    constructor() {
        this.counter = interval(500).pipe(
            map(() => {
                let pad = (num: number): string => {
                    num = Math.floor(num);
                    return (num < 10 ? '0' : '') + num.toFixed(0);
                }

                let time = (new Date().getTime() - this.start.getTime()) / 1000;
                time = time % (60 * 60);
                return pad(time / 60) + ':' + pad(time % 60);
            }));
    }
}
