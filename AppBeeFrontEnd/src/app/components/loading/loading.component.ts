import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { setTimeout } from 'timers';

@Component({
    templateUrl: 'loading.component.html',
    styleUrls: ['loading.component.scss']
})
export class LoadingComponent {

    private info: string;
    private attempts = 0;
    private showErrorMessage: boolean;

    constructor(private router: Router) {
        this.info = 'Loading settings...';
        setTimeout(() => this.load(), 1000);
    }

    public load(): void {
        this.router.navigateByUrl('/home');
    }
}
