import { Component, OnInit } from '@angular/core';
import { Partner, PartnerModel } from '../../../models/partner.model';
import { PartnersService } from '../../../services/partners.service';

@Component({
  selector: 'app-partners-list',
  templateUrl: './partners-list.component.html',
  styleUrls: ['./partners-list.component.css']
})
export class PartnersListComponent implements OnInit {
  isDesc: boolean = false;
  column: string = 'partnerID';

  searchText: any;
  partners: PartnerModel[] = [];
  totalLength: any;
  noOfRows = 5;
  page: number = 1;

  


  constructor(private partnersService: PartnersService) { }
  ngOnInit(): void {


    this.partnersService.getAllPartners()
      .subscribe({
        next: (partners) => {
          this.partners = partners;
          console.log(partners);
        },
        error: (response) => {
          console.log(response);
        }
      })


  }
  sort(property) {
    this.isDesc = !this.isDesc; //change the direction    
    this.column = property;
    let direction = this.isDesc ? 1 : -1;



    this.partners.sort(function (a, b) {
      if (a[property] < b[property]) {
        return -1 * direction;
      }
      else if (a[property] > b[property]) {
        return 1 * direction;
      }
      else {
        return 0;
      }
    });
  };
}
