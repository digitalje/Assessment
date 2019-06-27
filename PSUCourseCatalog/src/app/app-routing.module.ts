import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CourseListComponent } from './course-list/course-list.component';
import { CourseFormComponent } from './course-form/course-form.component';

const routes: Routes = [
  { path: '', component: CourseListComponent} // Default path is the course list
  , { path: 'course', component: CourseFormComponent } // Used to acceess the course form, same componenent is used for adds and updates
  , { path: '**', redirectTo: '', pathMatch: 'full'} // Invalid paths will take you to the course list (or default path
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
