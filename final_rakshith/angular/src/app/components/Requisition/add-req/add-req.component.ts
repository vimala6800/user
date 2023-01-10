import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AddRequisition } from '../../../models/requisition/addrequisition';
import { PartnersService } from '../../../services/partners.service';
import { ProjectmanagerService } from '../../../services/projectmanager.service';
import { RequisitionService } from '../../../services/requisition.service';
import { RequisitionClient } from '../../../web-api-client';
import {HttpClient } from '@angular/common/http'

@Component({
  selector: 'app-add-req',
  templateUrl: './add-req.component.html',
  styleUrls: ['./add-req.component.scss']
})
export class AddReqComponent implements OnInit{
  disabledNumber: number;
  disabledDate: Date;
  Countries: any[];
  ProjectManager: any[];
  selectedPM: string;
  selectedSkill: string;
  Skills: any[];
  Departments: any[];
  selectedDepartment: string;
  selectedCurrency: string;
  Currency: any[];
  today: string;
  comments: string;
  selectedMandatory: string;
  selectedExperience: number;
  selectedSalesPerson: string;
  selectedStatus: number;
  reqcode: string;
  uploadedFile: File;
  fileName: string;
  partners: any[];

  skillDataArray: any[] = [];

  addRequisitionRequest: AddRequisition = {
    requisitionID: '',
    requisitionCode: '',
    potentialNumber: '',
    complexity: '',
    requisitionDate: '',
    deadlineDate: '',
    clientName: '',
    clientCountreyID: '',
    projectType: '',
    salesPersonUserID: '',
    projectManagerID: '',
    requisitionStatus: 0,
    expectedStartDate: '',
    estimatedBudget: 0,
    departmentID: '',
    currencyID: '',
    projectDescription: '',
    jobDescription: '',
    duration: 0,
    durationUnits: '',
    shiftTimings: '',
    noOfResources: 0,
    openPositions: 0,
    keyDescription: '',
    preferredEducation: '',
    minExperience: 0,
    maxExperience: 0,
    jdFileName: '',
    skillID: '',
    mandatory: '',
    experience: 0,
    comments: '',
    partnerID: ''
  }
  skilName: string;

  constructor(
    private listsClient: RequisitionClient, private projectmanagerservice: ProjectmanagerService ,private route: ActivatedRoute, private requisitionService: RequisitionService, private router: Router, private partnerService: PartnersService, private http: HttpClient) { }
  ngOnInit(): void {

    



    const currentDate = new Date();
    this.today = currentDate.toISOString().substring(0, 10);


    this.generateDisabledNumber();
    this.getDisabledDate();
    this.getallcountries();
    this.getProjectManagers();
    this.getskills();
    this.getDepartments();
    this.getAllCurrency();
    this.getAllPartners();
  }

  generateDisabledNumber() {
    this.disabledNumber = Math.floor(Math.random() * 10000) + Date.now();
  }
  getDisabledDate() {
    this.disabledDate = new Date();
  }
  getallcountries() {
    this.requisitionService.GetAllCountry()
      .subscribe({
        next: (Countriess) => {
          this.Countries = Countriess;
          console.log(Countriess);
        },
        error: (response) => {
          console.log(response);
        }
      });

  }
  getProjectManagers() {
    this.projectmanagerservice.getProjectManagers()
      .subscribe({
        next: (ProjectMan) => {
          this.ProjectManager = ProjectMan;
          console.log(ProjectMan);
        },
        error: (response) => {
          console.log(response);
        }
      });
  }

  getskills() {
    this.requisitionService.getAllSkil()
      .subscribe({
        next: (Skill) => {
          this.Skills = Skill;
          console.log(this.Skills);
        },
        error: (response) => {
          console.log(response);
        }
      });
  }

  getDepartments() {
    this.requisitionService.getAllDepartments()
      .subscribe({
        next: (Department) => {
          this.Departments = Department;
          console.log(this.Departments);
        },
        error: (response) => {
          console.log(response);
        }
      });
  }

  getAllCurrency() {
    this.requisitionService.getAllCurrency()
      .subscribe({
        next: (Curreency) => {
          this.Currency = Curreency;
          console.log(this.Currency);
        },
        error: (response) => {
          console.log(response);
        }
      });
  }


  getAllPartners() {

    this.partnerService.getAllPartners()
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
  


  addRequisition(addRequisitionRequest: AddRequisition) {
    addRequisitionRequest.requisitionID = '2cf429e8-da21-4295-b2d9-f8f4d8cf0cef';
    addRequisitionRequest.skillID = this.skillDataArray[0].skillID
    addRequisitionRequest.mandatory = this.skillDataArray[0].mandatory
    addRequisitionRequest.experience = this.skillDataArray[0].experience
    addRequisitionRequest.comments = this.skillDataArray[0].comments 
    this.http.post<AddRequisition>('https://localhost:7184/api/Requisition', addRequisitionRequest)
    
      .subscribe(response => {
        console.log(response);
      }, error => {
        console.log(error);

      });

  }



  onSubmit() {
    let splitone: string[] = [];
    let dropdown = this.selectedSkill
    splitone = dropdown.split('_')
    let desired: string = splitone[1];
    let desSkillID: string = splitone[0];
    

    let split2: string[] = [];
    let dropdown2 = this.selectedMandatory
    split2 = dropdown2.split('_');
    let desired2: string = split2[1];
    let desired2num: string=split2[0]


    //this.requisitionService.addRequisition(this.addRequisitionRequest)
    //  .subscribe({
    //    next: (requisition) => {
    //      console.log(requisition);
    //    }
    //  })

    this.skillDataArray.push({
      skillName: desired,
      skillID: desSkillID,
      mandatorydisp: desired2,
      mandatory: desired2num,
      experience: this.selectedExperience,
      comments: this.comments,
    });
  }

 
}

