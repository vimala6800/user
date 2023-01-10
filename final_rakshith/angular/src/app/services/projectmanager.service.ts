import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ProjectManager } from '../models/projectmanager.model';
import { ProjectManagerAdd, UserAdd } from '../models/projectmanageradd.model';

@Injectable({
  providedIn: 'root'
})

export class ProjectmanagerService {
  baseApiUrl: string = environment.base_url;
  constructor(private http: HttpClient) { }
  getProjectManagers(): Observable<ProjectManager[]> {
    return this.http.get<ProjectManager[]>(this.baseApiUrl + '/api/ProjectManager');
  }
  addUser(addUserRequest: UserAdd): Observable<UserAdd> {
    addUserRequest.id = 'c70d820c-61be-4a2b-9b28-de8590192366';

    return this.http.post<UserAdd>(this.baseApiUrl + '/api/ProjectManager?role=projectmanager', addUserRequest);

  }
  addEmployee(addProjectManagerRequest: ProjectManagerAdd): Observable<ProjectManagerAdd> {
    addProjectManagerRequest.projectManagerID = 'c80d820c-61be-4a2b-9b28-de8590192366';
/*    addProjectManagerRequest.pmPhoto = '9030';
*/    addProjectManagerRequest.pmUserID = 'c80d820c-61be-4a2b-9b28-de8589292366';



    return this.http.post<ProjectManagerAdd>(this.baseApiUrl + '/api/ProjectManager/AddPM', addProjectManagerRequest);
  }
  getEmployee(id: string): Observable<ProjectManager> {
    return this.http.get<ProjectManager>(this.baseApiUrl + '/api/ProjectManager/' + id);
  }
  updateEmployee(id: string, updateEmployeeRequest: ProjectManager): Observable<ProjectManager> {
    return this.http.put<ProjectManager>(this.baseApiUrl + '/api/ProjectManager/' + id, updateEmployeeRequest);



  }
  //  getpmskills(): Observable<ProjectManagerSkillView[]>   
  //   {  
  //       return this.http.get<ProjectManagerSkillView[]>(this.baseApiUrl+'/api/ProjectManager/skill');
  //   } 
  public getpmskills = (): Observable<any> => {
    return this.http.get(this.baseApiUrl + '/api/ProjectManager/skill');
  }

  deleteEmployee(id: string): Observable<ProjectManager> {
    return this.http.delete<ProjectManager>(this.baseApiUrl + '/api/ProjectManager/' + id);
  }

}
