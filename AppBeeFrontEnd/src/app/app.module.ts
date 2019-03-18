import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {FormsModule, ReactiveFormsModule} from '@angular/forms';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';

import { HomeComponent } from './components/home/home.component';
import { LoadingComponent } from './components/loading/loading.component';

import { SharedModule } from './shared.module';
import { ProjectService } from './services/project.service';
import { TaskService } from './services/task.service';
import { MaterialModule } from './material.module';



@NgModule({
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        SharedModule,
        MaterialModule,
        FormsModule,
        ReactiveFormsModule,
        BrowserAnimationsModule
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        LoadingComponent
    ],
    bootstrap: [AppComponent],
    providers: [
        ProjectService,
        TaskService
    ]

})
export class AppModule { }
