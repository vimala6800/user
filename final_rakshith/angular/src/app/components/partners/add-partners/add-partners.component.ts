import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'oidc-client';

import { AddPartner, AddSPOC } from '../../../models/add-partner.model';
import { CountryMaster, SkillMaster } from '../../../models/partner.model';
import { PartnersService } from '../../../services/partners.service';


@Component({
  selector: 'app-add-partners',
  templateUrl: './add-partners.component.html',
  styleUrls: ['./add-partners.component.css']
})
export class AddPartnersComponent implements OnInit {
  myform!: FormGroup;

  url = './images/placeholder.jpg';
  countries: CountryMaster[];
  selectedCountry: number;
  skills: SkillMaster[]

  //dropdownList = [];
  //selectedItems = [];
  //dropdownSettings = {};
  adduserId: any;
  constructor(private partnerService: PartnersService, private route: ActivatedRoute, private router: Router) {

  }
 
  addpartnerRequest: AddPartner = {
      partnerID: '',
      partnerName: '',
      location: '',
      countryID: '',
      registeredDate: undefined,
      spocUserID: '',
      partnerStatus: 0,
      address1: '',
      billingAddress1: '',
      partnerImage: '',
      skillID:''
  }
  addUserRequest: AddSPOC = {
    id: '',
    userName: '',
    email: ''
  }
  ngOnInit(): void {
    this.myform = new FormGroup({
      "name": new FormControl(null, Validators.required),
      "location": new FormControl(null, Validators.required),
      "Country": new FormControl(null, Validators.required),
      "regdate": new FormControl(null, Validators.required),
      "spocname": new FormControl(null, Validators.required),
      "spocemail": new FormControl(null, [Validators.required,
      Validators.email
      ]),
      "status": new FormControl(null, Validators.required),
      "Skill": new FormControl(null, Validators.required),
      "address1": new FormControl(null, Validators.required),
      "baddress": new FormControl(null, Validators.required)
    })



    //this.dropdownList = [
    //  this.partnerService.GetAllCountry()
    //    .subscribe({
    //      next: (countries) => {
    //        this.countries = countries;
    //        console.log(countries);
    //      },
    //      error: (response) => {
    //        console.log(response);
    //      }
    //    })

    //];
    //this.dropdownSettings: IDropdownSettings = {
    //  singleSelection: false,
    //  idField: 'item_id',
    //  textField: 'item_text',
    //  selectAllText: 'Select All',
    //  unSelectAllText: 'UnSelect All',
    //  itemsShowLimit: 3,
    //  allowSearchFilter: true
    //};
    this.loadData();
  
    this.partnerService.GetAllCountry()
      .subscribe({
        next: (countries) => {
          this.countries = countries;
          console.log(countries);
        },
        error: (response) => {
          console.log(response);
        }
      })
    this.partnerService.GetAllSkills()
      .subscribe({
        next: (skills) => {
          this.skills = skills;
          console.log(skills);
        },
        error: (response) => {
          console.log(response);
        }
      })
  }
  
  loadData() {

    console.log('userValue',this.partnerService.getUserValue);

  }
 
  addPartner() {
    //this.partnerService.GetAllCountry();
    this.partnerService.addUser(this.addUserRequest)

      .subscribe({
        next: (users) => {
          console.log(users);
          //this.addpartnerRequest.spocUserID = users.id;

          //this.router.navigate(['projectmanagers']);
          this.partnerService.setUserValue(this.addUserRequest);
        }
      });
    console.log(this.addUserRequest);

    //console.log(this.addpartnerRequest.spocUserID);
    this.partnerService.addPartner(this.addpartnerRequest)

      .subscribe({

        next: (partners) => {
          //spocuserid: this.addUserRequest.id;
          console.log(partners);
          this.router.navigate(['/partner']);
        }
      });
    console.log(this.addpartnerRequest);
  }
 
  cancel() {
    // Simply navigate back to reminders view
    this.router.navigate(['../'], { relativeTo: this.route }); // Go up to parent route     
  }
  //addEmployee() {


  //  this.spocService.create_spoc(this.add)
  //    .subscribe({
  //      next: (employee) => {
  //        this.router.navigate([''])
  //          ;
  //      }


  //    })
  //}
  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event: any) => { // called once readAsDataURL is completed
        console.log(event);
        this.url = event.target.result;
      }
    }

    
  }

}
