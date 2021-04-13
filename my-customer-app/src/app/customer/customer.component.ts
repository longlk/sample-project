import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../customer.service';
import {Customer} from '../customer';

import { MatPaginator } from '@angular/material/paginator';
import { ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit,AfterViewInit {
  customer : Customer;
  customers: Customer[];
  isShow: boolean = false;
  public datasource = new MatTableDataSource<Customer>();
  displayedColumns: string[] = ['CustomerId','title','FirstName','MiddleName','LastName'];

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private customerService: CustomerService) { }
 
  ngAfterViewInit(): void {
    this.datasource.paginator = this.paginator;
  }

  ngOnInit(): void {
   // this.getCustomer(10);
   // this.getCustomers();

  }

  getCustomer(id:number):void{
   
    this.customerService.getCustomer(id).subscribe(
      response  => {
        console.log('response =',response);
        this.customer = response;
      }
      );
    
  }
  getCustomers():void{
    this.isShow = true;
    this.customerService.getCustomers().subscribe(
      response  => {
        console.log('response list=',response);
        this.datasource.data = response as Customer[];
      }
      );
  }
}


