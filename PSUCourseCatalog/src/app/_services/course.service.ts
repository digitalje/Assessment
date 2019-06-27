import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ICourseProfessor } from '../_models/iCourseProfessor';
import { IProfessor } from '../_models/iProfessor';

// Service used to access the Web API methods
@Injectable({
  providedIn: 'root'
})
export class CourseService {
  // Get the url from the environment.ts file
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // <summary>
  // Gets the list of professors
  // </summary>
  getProfessors(): Observable<IProfessor[]> {
    return this.http.get<IProfessor[]>(this.baseUrl + '/professor');
  }

  // <summary>
  // Gets the list of courses
  // </summary>
  getCourses(): Observable<ICourseProfessor[]> {
    return this.http.get<ICourseProfessor[]>(this.baseUrl + '/course');
  }

  // <summary>
  // Gets the course details for a given course
  // </summary>
  // <param name="id">The id of the course to retrieve</param>
  getCourse(id: number): Observable<ICourseProfessor> {
    return this.http.get<ICourseProfessor>(this.baseUrl + '/course/' + id);
  }

  // <summary>
  // Deletes a given course
  // </summary>
  // <param name="id">The id of the course to delete</param>
  deleteCourse(id: number) {
    return this.http.delete(this.baseUrl + '/course/' + id);
  }

  // <summary>
  // Updates the course details for the given course
  // </summary>
  // <param name="course">The course object that will be updated</param>
  updateCourse(course: ICourseProfessor) {
    return this.http.put(this.baseUrl + '/course/', course);
  }

  // <summary>
  // Adds the given course
  // </summary>
  // <param name="course">The course object that will be added</param>
  addCourse(course: ICourseProfessor) {
    return this.http.post(this.baseUrl + '/course/', course);
  }
}
