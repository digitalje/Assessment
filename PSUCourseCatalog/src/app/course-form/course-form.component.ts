import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder, Form } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbTypeahead } from '@ng-bootstrap/ng-bootstrap';
import { Observable, from, Subject, merge} from 'rxjs';
import {debounceTime, distinctUntilChanged, filter, map} from 'rxjs/operators';

import { ICourseProfessor } from '../_models/iCourseProfessor';
import { CourseService } from '../_services/course.service';
import { AlertifyService } from '../_services/alertify.service';
import { IProfessor } from '../_models/iProfessor';

// Course Form Component: Used to add/edit/view the details of a course
@Component({
  selector: 'app-course-form',
  templateUrl: './course-form.component.html',
  styleUrls: ['./course-form.component.css']
})
export class CourseFormComponent implements OnInit {
  id;
  course: ICourseProfessor;
  courseForm: FormGroup;
  professors: IProfessor[];
  professorNames: string [];

  // Used for be the TypeAhead controls
  @ViewChild('instance') instance: NgbTypeahead;
  focus$ = new Subject<string>();
  click$ = new Subject<string>();

  constructor(private fb: FormBuilder
            , private route: ActivatedRoute
            , private router: Router
            , private courseService: CourseService
            , private alertify: AlertifyService) {

      // Get the course id that was in the QueryString
      this.route.params.subscribe( params => {
        if (params['id']) {
          this.id = params['id'];
        }
      });
    }

  ngOnInit() {
    // Create the course form (using reactive forms)
    this.createCourseForm();

    // Get the list of professors for the typeahead.
    this.getProfessors();

    // If we have a course id, get the details for the course
    if (this.id) {
      this.getCourse(this.id);
    }
  }

  // <summary>
  // Gets the list of professors and populates the professors object array and the professors name string array
  // </summary>
  getProfessors() {
    this.courseService.getProfessors().subscribe( (professors: IProfessor[]) => {
      this.professors = professors;
      this.professorNames = this.professors.map(p => p.professorName);
    });
  }

  // <summary>
  // Method used to create the course form. Creates the form controls, sets the validators, and default values
  // </summary>
  createCourseForm() {
    this.courseForm = this.fb.group({
      courseName: ['', Validators.required],
      professorName: [''],
      professorEmail: ['', Validators.email],
      roomNumber: [''],
      sunday: [false],
      monday: [false],
      tuesday: [false],
      wednesday: [false],
      thursday: [false],
      friday: [false],
      saturday: [false]
    });
  }

  // <summary>
  // Helper method to get the value from a form group control
  // </summary>
  getValue(prop: string) {
    return this.courseForm.get(prop).value;
  }

  // <summary>
  // Helper method to get the errors from a form group control
  // </summary>
  getErrors(prop: string) {
    return this.courseForm.get(prop).errors;
  }

  // <summary>
  // Validator method used to determine if at least one day is selected.
  // </summary>
  validDays() {

    // If the form hasn't been touched, don't worry about validating the days yet.
    if (this.courseForm.untouched) {
      return true;
    }

    // Check if at least one day is selected, if it is return true, else false.
    if (this.getValue('sunday') === true
      || this.getValue('monday') === true
      || this.getValue('tuesday') === true
      || this.getValue('wednesday') === true
      || this.getValue('thursday') === true
      || this.getValue('friday') === true
      || this.getValue('saturday') === true) {
        return true;
    } else {
      return false;
    }
  }

  // <summary>
  // Method used to determine if the Save button should be disabled or not. If the form is not valid,
  // the save button should be disabled.
  // </summary>
  canSave() {
    if (this.validDays() && this.courseForm.valid) {
      return true;
    } else {
      return false;
    }
  }

  // <summary>
  // Method used to get the course details for a given course.
  // </summary>
  // <param name="id">The id of the course to retrieve</param>
  getCourse(id) {
    this.courseService.getCourse(id).subscribe((course: ICourseProfessor) => {
      this.course = course;
      this.courseForm.patchValue({courseName: this.course.courseName
                                , professorName: this.course.professorName
                                , professorEmail: this.course.professorEmail
                                , roomNumber: this.course.roomNumber
                                , sunday: this.course.sunday
                                , monday: this.course.monday
                                , tuesday: this.course.tuesday
                                , wednesday: this.course.wednesday
                                , thursday: this.course.thursday
                                , friday: this.course.friday
                                , saturday: this.course.saturday
                                });
      });
  }

  // <summary>
  // Method used to save the course to the database
  // </summary>
  submitCourse() {
    // Check that the form is valid.
    if (this.courseForm.valid) {
      // Assign the form values to the course model
      this.course = Object.assign({}, this.courseForm.value);

      // If we had a course id, we will be doing an update
      if (this.id) {
        this.course.id = this.id;
        this.courseService.updateCourse(this.course).subscribe(() => {
          // Display the success message and navigate to the course list.
          this.alertify.success('The course was saved');
          this.router.navigate(['/']);
        }, error => {
          // Display the error
          this.alertify.error(error.error);
        });
      } else {
        // If there was no course id, we will be doing an add.
        this.courseService.addCourse(this.course).subscribe(() => {
          // Display the success message and navigate to the course list.
          this.alertify.success('The course was added');
          this.router.navigate(['/']);
        }, error => {
          // Display the error
          this.alertify.error(error.error);
        });
    }
    }
  }

  // <summary>
  // Method used by the typeahead to filter the dropdown to the matching entries.
  // </summary>
  search = (text$: Observable<string>) => {
    const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
    const clicksWithClosedPopup$ = this.click$.pipe(filter(() => !this.instance.isPopupOpen()));
    const inputFocus$ = this.focus$;

    return merge(debouncedText$, inputFocus$, clicksWithClosedPopup$).pipe(
      map(term => (term === '' ? this.professorNames
        : this.professorNames.filter(v => v.toLowerCase().indexOf(term.toLowerCase()) > -1)).slice(0, 10))
    );
  }
}
