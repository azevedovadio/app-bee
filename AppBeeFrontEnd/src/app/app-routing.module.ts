import { NgModule, Type } from '@angular/core';
import { RouterModule, Routes, Route } from '@angular/router';
import { APP_BASE_HREF } from '@angular/common';
import { HomeComponent } from './components/home/home.component';
import { LoadingComponent } from './components/loading/loading.component';

const appRoutes: Routes = [
    {
        path: 'home',
        component: HomeComponent,
    },
    {
        path: 'loading',
        component: LoadingComponent
    },

    { path: '', redirectTo: '/loading', pathMatch: 'full' },

];

@NgModule({
    imports: [
        RouterModule.forRoot(
            appRoutes,
            { enableTracing: false, useHash: true } // Enable for debugging purposes only
        )
    ],
    exports: [
        RouterModule
    ],
    providers: [{ provide: APP_BASE_HREF, useValue: '/' }]
})
export class AppRoutingModule { }
