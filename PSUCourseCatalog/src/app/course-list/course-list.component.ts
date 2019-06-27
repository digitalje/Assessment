import { Component, OnInit } from '@angular/core';
import { ICourseProfessor } from '../_models/iCourseProfessor';
import { CourseService } from '../_services/course.service';
import { AlertifyService } from '../_services/alertify.service';
import { stringify } from '@angular/core/src/util';

// Course List Component: Used to display the list of courses
@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
  courses: ICourseProfessor[];

  constructor(private courseService: CourseService
            , private alertify: AlertifyService) { }

  ngOnInit() {
    // Get the list of courses
    this.courseService.getCourses().subscribe((courses: ICourseProfessor[]) => {
      this.courses = courses;
    });
  }

  // <summary>
  // Method used to delete a course
  // </summary>
  // <param name="id">The id of the course to delete</param>
  deleteCourse(id: number) {
    // Confirm the delete with the user
    this.alertify.confirm('Are you sure you want to remove the course?', () => {
      this.courseService.deleteCourse(id).subscribe(
        () => {
          // If we successfully removed the course, remove it from our local list and display the message to the user
          this.courses.splice(this.courses.findIndex(c => c.id === id), 1);
          this.alertify.success('The course was removed');
        },
        error => {
          // Display an error to the user
          this.alertify.error(error.error);
        });
    });
  }
}
