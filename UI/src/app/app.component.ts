import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'UI';
  data: any;
  constructor(private http: HttpClient) {
    this.http.get('https://localhost:5001/api/Users').subscribe({
      next: (response) => {
        this.data = response;
        console.log('Request Successful');
      },
      error: (error) => {
        console.log('Request Failed');
        console.log(error);
      },
      complete: () => {
        console.log('Request Completed');
      },
    });
  }
}
