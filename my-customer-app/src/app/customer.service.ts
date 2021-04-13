import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Observable,of} from 'rxjs';
import {Customer} from './customer';
@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(private http: HttpClient) { }

  private customerUrl = 'https://localhost:5001/customer/';
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  getCustomer(id:number): Observable<Customer>{
    return this.http.get<Customer>(this.customerUrl + JSON.stringify(id) );
    
  }
  getCustomers(): Observable<Customer[]>{
    return this.http.get<Customer[]>(this.customerUrl );
    
  }
}
