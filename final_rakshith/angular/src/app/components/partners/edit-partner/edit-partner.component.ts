import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { CountryMaster, Partner, SkillMaster } from '../../../models/partner.model';
import { PartnersService } from '../../../services/partners.service';

@Component({
  selector: 'app-edit-partner',
  templateUrl: './edit-partner.component.html',
  styleUrls: ['./edit-partner.component.css']
})
export class EditPartnerComponent implements OnInit {
  myform!: FormGroup;
  url = './images/placeholder.jpg';
  countries: CountryMaster[];
  skills: SkillMaster[]

  
  partnerDetails: Partner = {
    partnerID: '',
    partnerName: '',
    location: '',
    countryID: '',
    registeredDate: '',
    spocUserID: '',
    address1: '',
    billingAddress1: '',
    partnerImage: '',
    partnerStatus: 0,
    SkillID:'',
    

  }

  constructor(private route: ActivatedRoute, private partnerService: PartnersService, private router: Router) { }
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
    this.route.paramMap.subscribe({

      next: (params) => {
        const id = params.get('partnerID');
        if (id) {
          //call api
          this.partnerService.getPartnerDetails(id).subscribe({
            next: (response) => {
              this.partnerDetails = response
              
            }
          })
        }
      }
    })
    //throw new Error('Method not implemented.');
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
  updateEmployee() {
    this.partnerService.updateEmployee(this.partnerDetails.partnerID, this.partnerDetails).
      subscribe({
        next: (response) => {
          console.log(response);
          this.router.navigate(['/partner'])
        }
      });
  }
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
