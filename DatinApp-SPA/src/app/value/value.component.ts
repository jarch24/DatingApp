import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {

  values: any;
  query: string;
  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getValues();
  }
  getValues() {
    this.http.get('http://localhost:5000/api/values/get').subscribe(response => {
      this.values = response;
    }, error => {
      console.log(error);
    });
  }
  getValuesByName(query: string) {
      console.log(query);
      this.http.get('http://localhost:5000/api/values/getValues/' + query).subscribe(response => {
        this.values = response;
      },
      error => {
        console.log(error);
      });
    }
}
