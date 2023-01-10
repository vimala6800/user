import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AddPartner, AddSPOC } from '../models/add-partner.model';
import { Partner, PartnerModel } from '../models/partner.model'

@Injectable({
  providedIn: 'root'
})
export class PartnersService {
  base_url: string = environment.base_url;
  addUserRequest: AddSPOC;

  constructor(private http: HttpClient) { }
  getAllPartners(): Observable<PartnerModel[]> {
   return this.http.get<PartnerModel[]>(this.base_url + '/api/partners');

  }
  getPartnerDetails(id: string): Observable<Partner> {
    return this.http.get<Partner>(this.base_url + '/api/partners/' + id);
  } 
  public GetAllCountry = (): Observable<any> => {
    return this.http.get(this.base_url + '/api/partners/GetAllCountries');
      

  }
  public GetAllSkills = (): Observable<any> => {
    return this.http.get(this.base_url + '/api/partners/GetAllSkills');
  }  
  //getPartner(id: string): Observable<Partner> {
  //  return this.http.get<Partner>(this.base_url + '/api/partners/' + id)
  //}
  //getOnePartner(id): Observable<Partner[]> {
  //  return this.http.get<Partner[]>(this.base_url + id)
  //}
  addUser(addUserRequest: AddSPOC): Observable<AddSPOC> {
    addUserRequest.id = 'c70d820c-20cd-2a22-2b21-de8590192366';

    return this.http.post<AddSPOC>(this.base_url + '/api/partners?role=partner', addUserRequest);
  }
  setUserValue(addUserRequest: AddSPOC) {
    this.addUserRequest = addUserRequest;
  }
  getUserValue() {
    return this.addUserRequest;
  }
  //set UserValue(addUserRequest: AddSPOC) {
  //  this.addUserRequest = addUserRequest;
  //}
  //get UserValue() {
  //  return this.addUserRequest;
  //}

  updateEmployee(id: string, updateEmployeeRequest: Partner): Observable<Partner> {
    return this.http.put<Partner>(this.base_url + '/api/partners/' + id, updateEmployeeRequest);

  }

  //create_spoc(addSpocRequest: AddPartner): Observable<AddPartner> {
    

  //  return this.http.post<AddPartner>(this.base_url + '/api/partners/addSPOC', addSpocRequest)

      
  //}  
  addPartner(addpartnerRequest: AddPartner): Observable<AddPartner> {
    addpartnerRequest.partnerID = 'c80d820c-61be-4a2b-9b28-de8590192366';
    addpartnerRequest.partnerImage = '9030';
    addpartnerRequest.spocUserID = 'c80d820c-61be-4a2b-9b28-de8589292366';
    //addpartnerRequest.countryID = 'b82803c7-10aa-4c53-3295-08dae7020c6a';


    /*addProjectManagerRequest.created = '00-00-0000';
    addProjectManagerRequest.createdBy = '00000000-0000-0000-0000-000000000000';
    addProjectManagerRequest.lastModified = '00-00-0000';
    addProjectManagerRequest.lastModifiedBy = '00000000-0000-0000-0000-000000000000';*/
    return this.http.post<AddPartner>(this.base_url + '/api/partners/AddPartner', addpartnerRequest);
  }

  deletePartner(id: string): Observable<Partner> {
    return this.http.delete<Partner>(this.base_url + '/api/partners/' + id);
  }
  //delete(id: string): Observable<Partner> {
  //  return this.http.delete<Partner>(`${this.base_url}/${id}`);
  //}
}
