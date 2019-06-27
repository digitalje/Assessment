import { Injectable } from '@angular/core';
declare let alertify: any;

// Service created for easy of use of the Alertify dialog box
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  // <summary>
  // Used to display a confirmation box to the user
  // </summary>
  // <param name="message">The message to display to the user</param>
  // <param name="okCallback">Pointer to the function that should be called when the user clicks OK</param>
  confirm(message: string, okCallback: () => any) {
    alertify.confirm('Please Confirm', message, function() {
        okCallback();
      },
      () => {});
  }

  // <summary>
  // Used to display a Success message to the user
  // </summary>
  // <param name="message">The message to display to the user</param>
  success(message: string) {
    alertify.success(message);
  }

  // <summary>
  // Used to display an Errpr message to the user
  // </summary>
  // <param name="message">The message to display to the user</param>
  error(message: string) {
    alertify.error(message);
  }

  // <summary>
  // Used to display a Warning message to the user
  // </summary>
  // <param name="message">The message to display to the user</param>
  warning(message: string) {
    alertify.warning(message);
  }

  // <summary>
  // Used to display a Notification message to the user
  // </summary>
  // <param name="message">The message to display to the user</param>
  message(message: string) {
    alertify.message(message);
  }
}
